using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_StarGalaxyTarget : MonoBehaviour
{
    public Transform target;

    float x;
    float y;
    float z;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;      
        //print(target.position.x + " " + target.position.y + " " + target.position.z);  
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) < 0.1) {
            this.GetComponent<Camera_SimpleController>().enabled = true;
        }
        else {
            this.GetComponent<Camera_SimpleController>().enabled = false;
        }

        var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / 0.2f) * Time.deltaTime);

        x = Mathf.Lerp(x, target.position.x, positionLerpPct);
        y = Mathf.Lerp(y, target.position.y, positionLerpPct);
        z = Mathf.Lerp(z, target.position.z, positionLerpPct);

        transform.position = new Vector3(x, y, z);

    }
}
