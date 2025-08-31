using System.Numerics;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D _coll;

    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnityEngine.Vector2 exitDirection = (collision.transform.position - _coll.bounds.center).normalized;

            if (customInspectorObjects.swapCameras && customInspectorObjects.cameraFromRight != null && customInspectorObjects.cameraFromLeft != null)
            {
                // swap cameras
                CameraManager.instance.SwapCamera(customInspectorObjects.cameraFromRight, customInspectorObjects.cameraFromLeft, exitDirection);
            }

            if (customInspectorObjects.panCameraOnContact)
            {
                // pan the camera based on the pan direction in the inspector
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (customInspectorObjects.panCameraOnContact)
            {
                // pan the camera back to the starting position
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}

[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineCamera cameraFromLeft;
    [HideInInspector] public CinemachineCamera cameraFromRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}


#if UNITY_EDITOR

[CustomEditor(typeof(CameraControlTrigger))]
public class MyScriptEditor : Editor
{
    SerializedProperty customInspectorObjects;
    SerializedProperty swapCameras;
    SerializedProperty panCameraOnContact;
    SerializedProperty cameraFromRight;
    SerializedProperty cameraFromLeft;
    SerializedProperty panDirection;
    SerializedProperty panDistance;
    SerializedProperty panTime;

    private void OnEnable()
    {
        customInspectorObjects = serializedObject.FindProperty("customInspectorObjects");
        swapCameras = customInspectorObjects.FindPropertyRelative("swapCameras");
        panCameraOnContact = customInspectorObjects.FindPropertyRelative("panCameraOnContact");
        cameraFromRight = customInspectorObjects.FindPropertyRelative("cameraFromRight");
        cameraFromLeft = customInspectorObjects.FindPropertyRelative("cameraFromLeft");
        panDirection = customInspectorObjects.FindPropertyRelative("panDirection");
        panDistance = customInspectorObjects.FindPropertyRelative("panDistance");
        panTime = customInspectorObjects.FindPropertyRelative("panTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(swapCameras);
        EditorGUILayout.PropertyField(panCameraOnContact);

        if (swapCameras.boolValue)
        {
            EditorGUILayout.PropertyField(cameraFromLeft);
            EditorGUILayout.PropertyField(cameraFromRight);
        }

        if (panCameraOnContact.boolValue)
        {
            EditorGUILayout.PropertyField(panDirection);
            EditorGUILayout.PropertyField(panDistance);
            EditorGUILayout.PropertyField(panTime);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif