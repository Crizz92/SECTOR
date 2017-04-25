using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public static class EntityManager {

    enum ECheckState
    {
        Entity = 0,
        InteractifElement,
        LightController,
    }
    private static List<Entity> _entityList = new List<Entity>();
    private static int _currentEntityIndex = 0;
    private static List<InteractifElement> _interactableEntityList = new List<InteractifElement>();
    private static int _currentInteractifElemIndex = 0;
    private static List<LightController> _lightsList = new List<LightController>();
    private static int _currentLightIndex = 0;
    private static InGameCamera _ingameCamera;
    [SerializeField]
    private static int _activationRange = 30;
    [SerializeField]
    private static int _analyzePerFrame = 30;
    private static bool _activated = false;


    private static int _currentIndex = 0;
    private static int _nbrEntityToCheck = 0;


    public static void Activate()
    {
        _activated = true;
    }
    public static void Deactivate()
    {
        _activated = false;
        Clear();
    }
    public static void Initialize ()
    {
        _ingameCamera = GameObject.FindObjectOfType<InGameCamera>();

        if(_ingameCamera)
        {
            _entityList = GameObject.FindObjectsOfType<Entity>().Cast<Entity>().ToList();
            foreach(Entity entity in _entityList)
            {
                entity.Initialize();
            }
            _interactableEntityList = GameObject.FindObjectsOfType<InteractifElement>().Cast<InteractifElement>().ToList();
            foreach(InteractifElement entity in _interactableEntityList)
            {
                entity.Initialize();
            }
            Light []lightList = GameObject.FindObjectsOfType<Light>();
            //foreach (Light light in lightList)
            //{
            //    if(!light.GetComponent<LightController>()) light.gameObject.AddComponent<LightController>();
            //}
            _lightsList = GameObject.FindObjectsOfType<LightController>().Cast<LightController>().ToList();
            foreach(LightController light in _lightsList)
            {
                light.Initialize();
            }


        }
	}
    public static void Clear()
    {
        _ingameCamera = null;
        _entityList.Clear();
        _interactableEntityList.Clear();
        _lightsList.Clear();
        _currentIndex = 0;
        _nbrEntityToCheck = 0;
    }
	
    public static void Update()
    {
        UpdateEntities();
        UpdateLights();
    }

    #region Entity Management
    public static Entity CreateEntity(Entity toCreate)
    {
        Entity entity = GameObject.Instantiate(toCreate) as Entity;
        if(entity) AddEntity(entity);
        return entity;
    }
    public static bool DestroyEntity(Entity toDestroy)
    {
        if (toDestroy)
        {
            RemoveEntity(toDestroy);
            GameObject.Destroy(toDestroy.gameObject);
            return true;
        }
        else return false;
    }
    private static void AddEntity(Entity entity)
    {
        if (!entity) return;
        else _entityList.Add(entity);
    }
    private static void RemoveEntity(Entity entity)
    {
        if (!entity) return;
        else _entityList.Remove(entity);
    }
    public static List<Entity> EntityInSphere(Vector3 center, float radius)
    {
        return EntityInSphere<Entity>(center, radius);
    }
    public static List<T> EntityInSphere<T>(Vector3 center, float radius) where T : class
    {
        return EntityInSphere<T>(center, radius, new List<EntityProperties>());
    }
    public static List<Entity> EntityInSphere(Vector3 center, float radius, EntityProperties property)
    {
        return EntityInSphere<Entity>(center, radius, property);
    }
    public static List<Entity> EntityInSphere(Vector3 center, float radius, List<EntityProperties> properties)
    {
        return EntityInSphere<Entity>(center, radius, properties);
    }
    public static List<T> EntityInSphere<T>(Vector3 center, float radius, EntityProperties property) where T : class
    {
        List<EntityProperties> properties = new List<EntityProperties>();
        properties.Add(property);
        return EntityInSphere<T>(center, radius, properties);
    }
    public static List<T> EntityInSphere<T>(Vector3 center, float radius, List<EntityProperties> properties) where T : class
    {
        List<T> entityFound = new List<T>();

        //Check all the entities referenced in the list
        for (int i = 0; i < _entityList.Count; i++)
        {
            Entity entity = _entityList[i];
            // if the entity exist and is of type T
            if (entity && entity is T)
            {
                // we now check if the entity is in the radius
                float distance = Vector3.Distance(entity.transform.position, center);
                if (distance > radius)
                {
                    continue;
                }
                // checking if the properties matchs
                bool containsProperty = true;
                foreach(EntityProperties currentProperty in properties)
                {
                    if(!entity.Properties.Contains(currentProperty))
                    {
                        containsProperty = false;
                        break;
                    }
                }
                // if properties not match, go to next entity
                if (!containsProperty) continue;
                // else we found an entity that match and add it to the list 
                else
                {
                    entityFound.Add(entity as T);
                }
            }
        }
        return entityFound;
    }
    public static bool EntityMatchProperty(Entity entity, EntityProperties property)
    {
        if (!entity) return false;
        foreach (EntityProperties entityProperty in entity.Properties)
        {
            if (entityProperty == property)
            {
                return true;
            }
        }
        return false;
    }
    public static bool EntityMatchProperties(Entity entity, List<EntityProperties> properties)
    {
        foreach(EntityProperties analysedProperty in properties)
        {
            bool propertyMatched = false;
            foreach(EntityProperties entityProperty in entity.Properties)
            {
                if (entityProperty == analysedProperty) propertyMatched = true;
            }
            if (!propertyMatched) return false;
        }
        return true;
    }
    #endregion
    #region IInteractable Management
    public static List<InteractifElement> InteractablesInRange(Drone drone)
    {
        List<InteractifElement> interactableInRange = new List<InteractifElement>();
        for(int i = 0; i < _interactableEntityList.Count; i++)
        {
            if(_interactableEntityList[i].Interactable 
                &&Vector3.Distance(drone.transform.position, _interactableEntityList[i].transform.position) < _interactableEntityList[i].InteractionRadius)
            {
                interactableInRange.Add(_interactableEntityList[i]);
            }
        }
        return interactableInRange;
    }
    public static InteractifElement InteractableInRange(Vector3 position)
    {
        InteractifElement nearestInteractive = null;
        float nearestDistance = Mathf.Infinity;
        for (int i = 0; i < _interactableEntityList.Count; i++)
        {
            if(_interactableEntityList[i])
            {
                float distanceFromDrone = Vector3.Distance(position, _interactableEntityList[i].transform.position);
                if (_interactableEntityList[i].Interactable
                    && distanceFromDrone < _interactableEntityList[i].InteractionRadius
                    && distanceFromDrone < nearestDistance)
                {
                    nearestInteractive = _interactableEntityList[i];
                }
            }
        }
        return nearestInteractive;
    }

    public static void ClearInteractifElement()
    {
        for(int i =0; i < _interactableEntityList.Count; i++)
        {
            if (_interactableEntityList[i])
            {
                _interactableEntityList[i].DestroyButtonIndicator();
            }
        }
    }
    #endregion
    public static int ActiveEntityCount()
    {
        int activeEntityCount = 0;
        foreach(Entity entity in _entityList)
        {
            if(entity && !entity.Sleeping)
            {
                activeEntityCount++;
            }
        }
        return activeEntityCount;
    }
    public static int ActiveDynamicLightCount()
    {
        int activeDynamicLightCount = 0;
        foreach(LightController light in _lightsList)
        {
            if(light && light.Enabled)
            {
                activeDynamicLightCount++;
            }
        }
        return activeDynamicLightCount;
    }
    public static int ActiveDynamicLightCastingShadowCount()
    {
        int castingShadowCount = 0;
        foreach (LightController light in _lightsList)
        {
            if (light && light.Enabled && light.CastingShadows)
            {
                castingShadowCount++;
            }
        }
        return castingShadowCount;
    }

    private static void UpdateEntities()
    {
        if(_activated)
        {
            int nbrEntityToCheck = _analyzePerFrame;
            if (nbrEntityToCheck > _entityList.Count) nbrEntityToCheck = _entityList.Count;
            while (nbrEntityToCheck > 0)
            {
                if (_currentEntityIndex >= _entityList.Count)
                {
                    _currentEntityIndex = 0;
                    break;
                }
                Entity entity = _entityList[_currentEntityIndex];
                if (entity && entity._sleepingWhenFar && entity.ManagerDependant)
                {
                    Vector3 center = _ingameCamera.InLevelPosition;
                    center.z = 0.0f;
                    Vector3 entityPos = entity.transform.position;
                    entityPos.z = 0.0f;

                    float distance = Vector3.Distance(center, entityPos);
                    if (distance > _activationRange)
                    {
                        entity.Sleep();
                    }
                    else
                    {
                        entity.WakeUp();
                    }
                }

                _currentEntityIndex++;
                nbrEntityToCheck--;
            }
        }
    }
    private static void UpdateInteractifElements()
    {

    }
    private static void UpdateLights()
    {
        if (_activated)
        {
            int nbrLightToCheck = _analyzePerFrame;
            if (nbrLightToCheck > _lightsList.Count) nbrLightToCheck = _lightsList.Count;
            while (nbrLightToCheck > 0)
            {
                if (_currentLightIndex >= _lightsList.Count)
                {
                    _currentLightIndex = 0;
                    break;
                }

                LightController lightController = _lightsList[_currentLightIndex];
                if (lightController && lightController.LightManagerDependant)
                {
                    Vector3 center = _ingameCamera.InLevelPosition;
                    center.z = 0.0f;
                    Vector3 entityPos = lightController.transform.position;
                    entityPos.z = 0.0f;

                    float distance = Vector3.Distance(center, entityPos);
                    if (distance > lightController.DisableDistance)
                    {
                        lightController.Disable();
                    }
                    else
                    {
                        lightController.Enable();
                    }
                }

                _currentLightIndex++;
                nbrLightToCheck--;
            }
        }
    }
}
