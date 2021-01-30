using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetIcon : MonoBehaviour
{
    public GameObject cam;

    private float initDist;

    // Start is called before the first frame update
    void Start()
    {
        initDist = Vector3.Distance(this.transform.position, cam.transform.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.rotation = cam.transform.rotation;

        //var dist = Vector3.Distance(this.transform.position, cam.transform.position);
        //this.transform.localScale = Vector3.one * dist / initDist;
    }
}
