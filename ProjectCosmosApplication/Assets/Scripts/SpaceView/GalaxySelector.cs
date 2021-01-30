using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GalaxySelector : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject camFocus;
    public GameObject galaxySystem;
    public List<GameObject> starSystems = new List<GameObject>();
    private Vector3 target;
    private bool systemSelected = true;
    private float x;
    private float y;
    private float z;
    
    // Start is called before the first frame update
    void Start()
    {
        //cameraController = mainCam.GetComponent<Camera_StarGalaxyTarget>();
        x = camFocus.transform.position.x;
        y = camFocus.transform.position.y;
        z = camFocus.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {        
        //var mouse = Mouse.current;
        //Vector3 mousePos = mouse.position.ReadValue();

        if (Keyboard.current.fKey.wasPressedThisFrame) {
            systemSelected = true;
            target = galaxySystem.transform.position;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast (ray, out RaycastHit hit)) 
            {
                MoveToSystem(hit.transform.name);                              
            }    
        }

        if (systemSelected && Vector3.Distance(target, camFocus.transform.position) > 0.1) {
            mainCam.GetComponent<Camera_SimpleController>().enabled = false;

            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / 0.2f) * Time.deltaTime);

            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);

            camFocus.transform.position = new Vector3(x, y, z);
        }
        else if (systemSelected) {
            systemSelected = false;
            mainCam.GetComponent<Camera_SimpleController>().enabled = true;
            SetupNewSystem();
        }
    }

    private void SetupNewSystem() {
        Collider[] hitColliders = Physics.OverlapSphere(camFocus.transform.position, 500);
        if (hitColliders.Length > 0) {
            foreach (Collider col in hitColliders) {
                if (col.tag == "Player" || col.tag == "Hostile") {
                    print("setup " + col.name);
                }
            }
        }
    }

    void MoveToSystem(string systemName) {
        switch (systemName) {
            case "System1": 
                systemSelected = true;
                target = starSystems[0].transform.position;
                break;
            case "System2":
                systemSelected = true;
                target = starSystems[1].transform.position; 
                break;
            case "System3": 
                systemSelected = true;
                target = starSystems[2].transform.position;
                break;
            default:
                break;
        }
    }
}
