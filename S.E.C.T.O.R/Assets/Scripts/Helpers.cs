using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static class Helpers
{
    public class Pooling<T>
    {
        // array of the object in the pool
        private T[] _objectsPool;
        private int _nbrObjectInPool = 0;
        public int NbrObjectInPool
        {
            get { return _nbrObjectInPool; }
        }

        public void Initialize(int poolSize)
        {
            _objectsPool = new T[poolSize];
        }
        public T GetObject()
        {
            for(int i = 0; i < _objectsPool.Length; i++)
            {
                T currentObject = _objectsPool[i];
                if(!EqualityComparer<T>.Default.Equals(currentObject, default(T)))
                {
                    _objectsPool[i] = default(T);
                    return currentObject;
                }
            }
            return default(T);

        }
        public void Store(T objectToStore)
        {
            if (EqualityComparer<T>.Default.Equals(objectToStore, default(T))) return;
            for(int i = 0; i < _objectsPool.Length; i++)
            {
                T currentObject = _objectsPool[i];
                if(EqualityComparer<T>.Default.Equals(currentObject, default(T)))
                {
                    _objectsPool[i] = objectToStore;
                }
            }
        }
    }
    #region array management
    public static void AddToArray<T>(T[] array, T element)
    {
        if (element == null) return;
        for(int i = 0; i < array.Length; i++)
        {
            if(array[i] == null)
            {
                array[i] = element;
                return;
            }
        }
    }
    public static void AddArrayToArray<T>(T[] arrayReceiving, T[] arrayToAdd)
    {
        for(int toAddIndex = 0; toAddIndex < arrayToAdd.Length; toAddIndex++)
        {
            for(int receiverIndex = 0; receiverIndex < arrayReceiving.Length; receiverIndex++)
            {
                if(arrayReceiving[receiverIndex] == null)
                {
                    arrayReceiving[receiverIndex] = arrayToAdd[toAddIndex];
                    break;
                }
            }
        }
    }
    public static void RemoveFromArray<T>(T[] array, T element)
    {
        if (element == null) return;
        for(int i = 0; i < array.Length; i++)
        {
            if(array[i].Equals(element))
            {
                array[i] = default(T);
            }
        }
    }
    public static void RemoveArrayFromArray<T>(T[] arrayCleaned, T[] arrayToRemove)
    {
        for (int toRemoveIndex = 0; toRemoveIndex < arrayToRemove.Length; toRemoveIndex++)
        {
            for (int cleanedIndex = 0; cleanedIndex < arrayCleaned.Length; cleanedIndex++)
            {
                if (arrayCleaned[cleanedIndex].Equals(arrayToRemove))
                {
                    arrayCleaned[cleanedIndex] = default(T);
                    break;
                }
            }
        }
    }
    public static bool ArrayIsEmpty<T>(T[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if (array[i] != null) return false;
        }
        return true;
    }
    public static void ClearArray<T>(T[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            array[i] = default(T);
        }
    }
    #endregion
    public static float CalculateAngleOnX(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.x;
    }
    public static float CalculateAngleOnY(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.y;
    }
    public static float CalculateAngleOnZ(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

    public static bool CheckIfInRectangle(Vector2 point, Vector2 bottomLeft, Vector2 topRight)
    {
        if (point.x > bottomLeft.x && point.x < topRight.x && point.y > bottomLeft.y && point.y < topRight.y)
        {
            return true;
        }
        else return false;
    }
    public static Vector3 ClampInRectangle(Vector3 point, Vector2 bottomLeft, Vector2 topRight)
    {
        Vector3 pointModification = point;
        pointModification.x = Mathf.Clamp(pointModification.x, bottomLeft.x, topRight.x);
        pointModification.y = Mathf.Clamp(pointModification.y, bottomLeft.y, topRight.y);

        return pointModification;
    }
}
