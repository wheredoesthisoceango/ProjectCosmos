using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetUnit : MonoBehaviour
{
    public GameObject cruiser;

    public int timeToMaxTurnSpeed = 6;
    public int timeToMaxAcceleration = 18;

    public int sensorRange = 25;

    private Vector3 target;

    float timeAtButtonDown;
    float timeAtButtonUp;

    float rotTimeRunning;
    float transTimeRunning;

    Collider sphereCollider;

    EasingFunction.Ease rotEase;
    EasingFunction.Function rotEaseFunc;
    EasingFunction.Ease transEase;
    EasingFunction.Function transEaseFunc;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;

        rotEase = EasingFunction.Ease.EaseInOutQuad;
        rotEaseFunc = EasingFunction.GetEasingFunction(rotEase);

        transEase = EasingFunction.Ease.EaseInOutCubic;
        transEaseFunc = EasingFunction.GetEasingFunction(transEase);
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

            rotTimeRunning = 0;
            transTimeRunning = 0;

            GetTargetPosition();
        }

        if (Vector3.Distance(transform.position, target) > 0.1f) {
            RotateToTarget();
            MoveToTarget();
        }

        // check if enemy in range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sensorRange);
        if (hitColliders.Length > 0) {
            foreach (Collider col in hitColliders) {
                if (col.tag != "GalacticPlane" && col.tag != this.tag) {
                    print(col.name);
                }
            }
        }

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
        }
    }

    void RotateToTarget() {        
        var targetRotation = Quaternion.LookRotation(target - transform.position);
        var rotationEase = 0f;

        if (rotTimeRunning < timeToMaxTurnSpeed) {    
            rotationEase = rotEaseFunc(0, 1, rotTimeRunning / timeToMaxTurnSpeed);
            rotTimeRunning += Time.deltaTime;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationEase);
    }

    void MoveToTarget() {
        var translationEase = 0f;
        
        if (transTimeRunning < timeToMaxAcceleration) {    
            translationEase = transEaseFunc(0, 1, transTimeRunning / timeToMaxAcceleration);
            transTimeRunning += Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, target, translationEase);  //new Vector3(x, y, z);
    }
}
