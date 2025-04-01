using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;


[CustomEditor(typeof(FieldOfView))]

public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI(){
        FieldOfView fov = (FieldOfView)target;
        Vector3 heightOffset = fov.GetHeightOffset();
        Handles.color = Color.white;
        Handles.DrawWireArc(heightOffset, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(heightOffset, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(heightOffset, fov.transform.position + viewAngle02 * fov.radius);

        if(fov.canSeePlayer){
            Handles.color = Color.green;
            Handles.DrawLine(heightOffset, fov.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees){
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

//reference: https://www.youtube.com/watch?v=j1-OyLo77ss
