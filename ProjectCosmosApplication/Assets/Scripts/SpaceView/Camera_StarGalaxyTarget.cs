using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_StarGalaxyTarget : MonoBehaviour
{
    public GameObject camFocus;
    private Vector3 target;
    private bool systemSelected = true;
    private float x;
    private float y;
    private float z;

    public Vector3 Target { get => target; set => target = value; }
    public bool SystemSelected { get => systemSelected; set => systemSelected = value; }


    // Start is called before the first frame update
    void Start()
    {
        x = camFocus.transform.position.x;
        y = camFocus.transform.position.y;
        z = camFocus.transform.position.z;      
        //print(target.position.x + " " + target.position.y + " " + target.position.z);  
    }

    // Update is called once per frame
    void Update()
    {
        if (systemSelected && Vector3.Distance(target, camFocus.transform.position) > 0.1) {
            this.GetComponent<Camera_SimpleController>().enabled = false;

            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / 0.2f) * Time.deltaTime);

            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);

            camFocus.transform.position = new Vector3(x, y, z);
        }
        else if (systemSelected) {
            systemSelected = false;
            this.GetComponent<Camera_SimpleController>().enabled = true;
        }
    }
}
