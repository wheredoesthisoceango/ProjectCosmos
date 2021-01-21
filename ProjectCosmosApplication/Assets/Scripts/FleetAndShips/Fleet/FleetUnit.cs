using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetUnit : MonoBehaviour
{
    public GameObject cruiser;

    private Vector3 targetPosition;

    float timeAtButtonDown;
    float timeAtButtonUp;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            timeAtButtonDown = Time.fixedTime;
        }
        if (Input.GetMouseButtonUp(1)) {
            timeAtButtonUp = Time.fixedTime;
        }

        float buttonClickTime = timeAtButtonUp - timeAtButtonDown;
        if (buttonClickTime > 0 && buttonClickTime < 0.25) {    
            timeAtButtonDown = 0;
            timeAtButtonUp = 0;

            GetTargetPosition();
        }

        if (Vector3.Distance(transform.position, targetPosition) > 10) {
            MoveToTarget();
        }

        // check if enemy in range

        // if not, then maintain holding pattern

        // else there is, approach enemy and engage in combat
    }

    void GetTargetPosition() {
        Plane plane = new Plane(Vector3.up, 0);
        float distance;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance)) {
            var hitPoint = ray.GetPoint(distance);
            print(hitPoint);
        }
    }

    void MoveToTarget() {
        
    }
}
