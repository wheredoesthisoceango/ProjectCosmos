using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera_MainController : MonoBehaviour
{
    bool freeEnabled;

    // Start is called before the first frame update
    void Start()
    {
        EnableTargetCamControl();
        freeEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame) {
            if (freeEnabled) {
                EnableTargetCamControl();
                freeEnabled = false;
            }
            else {
                EnableFreeCamControl();
                freeEnabled = true;
            }
        }

        // if wasd keyboard movement detected, use free cam

        // if user double clicks on space object, enable target cam
    }

    void EnableFreeCamControl() {   
        transform.GetComponent<Camera_FreeMovement>().enabled = true;
        transform.GetComponent<Camera_TargetMovement>().enabled = false;        
    }

    void EnableTargetCamControl() {
        transform.GetComponent<Camera_FreeMovement>().enabled = false;
        transform.GetComponent<Camera_TargetMovement>().enabled = true;
    }
}
