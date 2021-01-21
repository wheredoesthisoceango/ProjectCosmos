using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetUnit : MonoBehaviour
{
    public GameObject cruiser;

    private Vector3 target;

    float timeAtButtonDown;
    float timeAtButtonUp;

    float x;
    float y;
    float z;

    float rotationEase = 0.01f;

    float translationEase = 1;

    float timeRunning;

    EasingFunction.Ease ease;
    EasingFunction.Function func;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        x = target.x;
        y = target.y;
        z = target.z;

        ease = EasingFunction.Ease.EaseInOutSine;
        func = EasingFunction.GetEasingFunction(ease);
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

            timeRunning = 0;

            GetTargetPosition();
        }

        if (Vector3.Distance(transform.position, target) > 0.1f) {
            RotateToTarget();
            //MoveToTarget();
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
            target = hitPoint;
            //print(hitPoint);
        }
    }

    void RotateToTarget() {        
        var targetRotation = Quaternion.LookRotation(target - transform.position);

        if (timeRunning < 1) {    
            rotationEase = func(0, 1, timeRunning / 1);
            timeRunning += Time.deltaTime;
            print(rotationEase);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationEase);
    }

    void MoveToTarget() {
        //var positionLerpPct = 0.01f; //1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / 0.2f) * Time.deltaTime);

        //x = Mathf.Lerp(x, target.x, value);
        //y = Mathf.Lerp(y, target.y, value);
        //z = Mathf.Lerp(z, target.z, value);

        transform.position = Vector3.Lerp(transform.position, target, translationEase / 100);  //new Vector3(x, y, z);
    }
}
