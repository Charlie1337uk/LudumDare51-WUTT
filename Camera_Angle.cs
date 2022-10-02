using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Angle : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private Quaternion quatY;
    [SerializeField] private Quaternion quatX;
    public GameObject _rot;
    public GameObject Arm;
    [SerializeField] private float eZ;
    public void FixedUpdate()
    {
        x = variableJoystick.Vertical * speed * -1;
        y = variableJoystick.Horizontal * speed;
        eZ = Arm.transform.rotation.z;
        if (eZ < -0.38f) { if (x < 0) { x = 0; } }
        if (eZ > 0.1f) { if (x > 0) { x = 0; } }

        quatY.eulerAngles = new Vector3(0, y ,0);
        quatX.eulerAngles = new Vector3(0, 0, x);        
        _rot.transform.localRotation *= quatY;
        Arm.transform.localRotation *= quatX;        
    }
}
