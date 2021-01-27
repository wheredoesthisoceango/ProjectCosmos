using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxySelector : MonoBehaviour
{
    public GameObject camController;

    public GameObject representedPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        /*if (Input.GetKeyDown(KeyCode.F)) {
            print("return to galaxy view");
            print(representedPosition.transform);
            camController.GetComponent<Camera_StarGalaxyTarget>().target = representedPosition.transform;
        }*/
    }
}
