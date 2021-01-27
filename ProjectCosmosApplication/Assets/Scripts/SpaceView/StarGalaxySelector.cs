using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGalaxySelector : MonoBehaviour
{
    public GameObject camController;

    public GameObject representedStar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown() 
    {
        //print("moving to " + representedStar);
        camController.GetComponent<Camera_StarGalaxyTarget>().target = representedStar.transform;
        camController.GetComponent<Camera_StarGalaxyTarget>().systemSelected = true;
    }
}
