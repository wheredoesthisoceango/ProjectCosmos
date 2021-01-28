using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GalaxySelector : MonoBehaviour
{
    public GameObject mainCam;

    public GameObject galaxySystem;

    public List<GameObject> starSystems = new List<GameObject>();

    private Camera_StarGalaxyTarget cameraController;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraController = mainCam.GetComponent<Camera_StarGalaxyTarget>();
    }

    // Update is called once per frame
    void Update()
    {        
        //var mouse = Mouse.current;
        //Vector3 mousePos = mouse.position.ReadValue();

        if (Keyboard.current.fKey.wasPressedThisFrame) {
            cameraController.SystemSelected = true;
            cameraController.Target = galaxySystem.transform.position;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast (ray, out RaycastHit hit)) 
            {
                MoveToSystem(hit.transform.name);                              
            }    
        }
    }

    void MoveToSystem(string systemName) {
        switch (systemName) {
            case "System1": 
                cameraController.SystemSelected = true;
                cameraController.Target = starSystems[0].transform.position;
                break;
            case "System2":
                cameraController.SystemSelected = true;
                cameraController.Target = starSystems[1].transform.position; 
                break;
            case "System3": 
                cameraController.SystemSelected = true;
                cameraController.Target = starSystems[2].transform.position;
                break;
            default:
                break;
        }
    }
}
