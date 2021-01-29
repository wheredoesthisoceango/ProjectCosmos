using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FleetUnitMovement : MonoBehaviour
{
    public float thrust;
    //public float maxVelocity;
    public int range;
    public float rotationSpeed;
    public GameObject target;
    
    private List<GameObject> fleetShips = new List<GameObject>();
    private float rotTimeRunning;
    private EasingFunction.Function rotEaseFunc;
    private float timeAtButtonDown;
    private float timeAtButtonUp;
    private Vector3 targetPosition;

    private void Start() {
        foreach (Transform child in transform) {
            fleetShips.Add(child.gameObject);
        }
        
        rotEaseFunc = EasingFunction.GetEasingFunction(EasingFunction.Ease.EaseInOutQuad);
    }

    private void Update() {
        /*if (Mouse.current.rightButton.wasPressedThisFrame) {
            timeAtButtonDown = Time.fixedTime;
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame) {
            timeAtButtonUp = Time.fixedTime;
        }

        float buttonClickTime = timeAtButtonUp - timeAtButtonDown;
        if (buttonClickTime > 0 && buttonClickTime < 0.25) {    
            timeAtButtonDown = 0;
            timeAtButtonUp = 0;

            rotTimeRunning = 0;

            targetPosition = GetTargetPosition();
        }*/


        foreach (GameObject ship in fleetShips) {
            Rigidbody rb = ship.GetComponent<Rigidbody>();

            var dist = Vector3.Distance(ship.transform.position, target.transform.position);
            var rot = AngleDir(ship.transform, target.transform);
            
            if (dist > range) {
                rb.AddRelativeForce(Vector3.forward * thrust, ForceMode.Force);
            }            
            else if (dist < range && rb.velocity.magnitude > 0) {
                if (rb.velocity.magnitude <= 0.1f) {
                    rb.velocity = Vector3.zero;
                }
                else {
                    rb.velocity = rb.velocity * 0.99f;
                }
            }
            
            if (rot < -1 || rot > 1) {
                if (rot < 0)
                    rot = -1 * Mathf.Log(-1 * rot);
                else
                    rot = Mathf.Log(rot);

                rb.AddRelativeTorque(transform.up * rot * rotationSpeed);
            }
            else {
                if (rb.angularVelocity.magnitude <= 0.1f) {
                    rb.angularVelocity = Vector3.zero;
                }
                else {
                    rb.angularVelocity = rb.angularVelocity * 0.5f;
                }
            }
        }
    }


    private float AngleDir(Transform objectFacing, Transform objectToTest) {
        Vector3 localPos = objectFacing.InverseTransformPoint(objectToTest.position);
        return localPos.x;
    }
    
    Vector3 GetTargetPosition() {
        Plane plane = new Plane(Vector3.up, 0);
        Vector3 hitPoint = Vector3.zero;
        float distance;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (plane.Raycast(ray, out distance)) {
            hitPoint = ray.GetPoint(distance);
        }
        return hitPoint;
    }
}
