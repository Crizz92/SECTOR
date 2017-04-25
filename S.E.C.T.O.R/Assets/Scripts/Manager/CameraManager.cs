using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using System.Collections;



public class CameraManager : MonoBehaviour {


    public struct CameraModifierData
    {
        public float _second;
        public float _degree;
        public Vector3 _offset;
    }

    protected Camera _camera;
    [SerializeField]
    private float forcedAspectRatioWidth = 16;
    [SerializeField]
    private float forcedAspectRatioHeight = 10;

    [SerializeField]
    public Transform _target;
    protected Vector2 _positionToFollow = Vector2.zero;
    [SerializeField]
    public Vector3 _offset;
    protected Vector3 _velocity = Vector3.zero;
    [SerializeField]
    protected float speed = 2.0f;


    protected virtual void Start ()
    {

        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = forcedAspectRatioWidth / forcedAspectRatioHeight;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        _camera = GetComponent<Camera>();
	}

    #if UNITY_EDITOR
    [CustomEditor(typeof(CameraManager))]
    public class CameraManagerEditor : Editor
    {
        private CameraManager _cameraManager
        {
            get
            {
                return target as CameraManager;
            }
        }
        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                _cameraManager._target = EditorGUILayout.ObjectField("Target", _cameraManager._target, typeof(Transform), true) as Transform;
                _cameraManager._offset = EditorGUILayout.Vector3Field("Offset from target :", _cameraManager._offset);
                _cameraManager.speed = EditorGUILayout.FloatField("speed :", _cameraManager.speed);

                if(_cameraManager._target)
                {
                    _cameraManager.transform.position = _cameraManager._target.position + _cameraManager._offset;
                }


                if (GUI.changed)
                {
                    EditorUtility.SetDirty(_cameraManager);
                }
            }

        }

    }
    #endif

    // Update is called once per frame
	protected virtual void LateUpdate ()
    {
        if(_target)
        {
            _positionToFollow = _target.position;
        }
        Vector3 specificPosition = (Vector3)_positionToFollow + _offset;
        transform.position = Vector3.Lerp(transform.position, specificPosition, speed);
    }

    public virtual void RotateCamera(float degree, float second)
    {
        Debug.Log("RotateCamera");
        CameraModifierData cameraModifierData = new CameraModifierData();
        cameraModifierData._degree = degree;
        cameraModifierData._second = second;
        StopCoroutine("SmoothCameraRotation");
        StartCoroutine("SmoothCameraRotation", cameraModifierData);
    }
    IEnumerator SmoothCameraRotation(CameraModifierData cameraModifierData)
    {
        
        Quaternion initialeRotation = transform.rotation;
        Quaternion finalRotation = initialeRotation * Quaternion.Euler(Vector3.forward * cameraModifierData._degree);
        float ratio = 0;

        while (ratio != 1)
        {
            ratio += Time.deltaTime / cameraModifierData._second;
            transform.rotation = Quaternion.Lerp(initialeRotation, finalRotation, ratio);
            yield return null;
        }
    }

    public virtual void OffSetLerp(Vector3 offset, float second)
    {
        CameraModifierData cameraModifierData = new CameraModifierData();
        cameraModifierData._offset = offset;
        cameraModifierData._second = second;
        StopCoroutine("SmoothOffSetLerp");
        StartCoroutine("SmoothOffSetLerp", cameraModifierData);
    }
    IEnumerator SmoothOffSetLerp(CameraModifierData cameraModifierData)
    {
        Vector3 initialOffSet = _offset;
        Vector3 finalOffSet = cameraModifierData._offset;
        float ratio = 0;

        while (ratio != 1)
        {
            ratio += Time.deltaTime / cameraModifierData._second;
            _offset = Vector3.Lerp(initialOffSet, finalOffSet, ratio);
            yield return null;
        }
    }

}
