using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[AddComponentMenu("Control Script/MouseLook")] // add to the Unity editor's component menu
public class MouseLook : MonoBehaviour
{
    // enum to choose rotation axis in the Unity editor
    public enum RotationAxes
    {
        MouseXAndY = 0,     // yaw and pitch
        MouseX = 1,         // yaw only
        MouseY = 2          // pitch only
    }
    // rotation axis
    public RotationAxes axes = RotationAxes.MouseX;

    // rotation sensitivity
    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    // min and max pitch angles
    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    // for the pitch
    private float verticalRot = 0;

    //enable/disable looking
    public bool canLook;

    // Start is called before the first frame update
    void Start()
    {
        // make the rigid body not change rotation
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.freezeRotation = true;
        }
        canLook = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (!canLook) return;
        // yaw only
        if (axes == RotationAxes.MouseX)
        {
            // rotate about y-axis
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        }
        // pitch only
        else if (axes == RotationAxes.MouseY)
        {
            // change in pitch
            verticalRot -= Input.GetAxis("Mouse Y") * sensitivityVert;
            verticalRot = Mathf.Clamp(verticalRot, minimumVert, maximumVert);

            // get current yaw
            float horizontalRot = transform.localEulerAngles.y;

            // set the pitch, yaw remains the same
            transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);
        }
        // yaw and pitch
        else
        {
            // change in pitch
            verticalRot -= Input.GetAxis("Mouse Y") * sensitivityVert;
            verticalRot = Mathf.Clamp(verticalRot, minimumVert, maximumVert);

            // change in yaw
            float delta = Input.GetAxis("Mouse X") * sensitivityHor;
            float horizontalRot = transform.localEulerAngles.y + delta;

            // set pitch and yaw
            transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);
        }
    }
}
