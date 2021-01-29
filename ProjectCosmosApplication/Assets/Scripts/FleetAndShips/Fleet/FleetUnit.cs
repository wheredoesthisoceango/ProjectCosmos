using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FleetUnit : MonoBehaviour
{
    private float timeAtButtonDown;
    private float timeAtButtonUp;
    private bool isPlayerFleet;
    private bool inCombat;
    private bool isAlive;
    private List<GameObject> fleetUnitShips = new List<GameObject>();
    private GameObject targetFleetUnitGO;
    public float thrust;
    public float rotationSpeed;
    public float stoppingDist;

    private Vector3 fleetTargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (this.tag == "Player") {
            isPlayerFleet = true;
        }

        foreach (Transform child in transform) {
            fleetUnitShips.Add(child.gameObject);
        }

        fleetTargetPosition = transform.position;
        foreach (GameObject ship in fleetUnitShips) {
            ship.GetComponent<ShipTemplate>().TargetPosition = ship.transform.position;
            ship.GetComponent<Rigidbody>().isKinematic = true;            
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckFleetStatus();

        // check if enemy in range
        if (!inCombat) {
            targetFleetUnitGO = CheckForEnemyFleet();
        }

        // target found, entering combat
        if (targetFleetUnitGO != null) {
            inCombat = true;
            transform.GetComponent<Rigidbody>().isKinematic = true;
            foreach (GameObject ship in fleetUnitShips) {
                ship.GetComponent<Rigidbody>().isKinematic = false;

                float dist = 10000;
                GameObject closestEnemyShip = null;
                foreach (GameObject enemyShip in targetFleetUnitGO.GetComponent<FleetUnit>().fleetUnitShips) {
                    float newDist = Vector3.Distance(ship.transform.position, enemyShip.transform.position);
                    if (newDist < dist) {
                        dist = newDist;
                        closestEnemyShip = enemyShip;
                    }
                }

                var combatPosition = GetPositionNearEnemy(ship, closestEnemyShip);
                MoveAndRotateToTarget(ship, combatPosition);
                if (Vector3.Distance(ship.transform.position, combatPosition) < 10) {
                    ship.GetComponent<ShipTemplate>().DoDamage(targetFleetUnitGO.GetComponent<FleetUnit>());
                }
            }
        }
        
        // target is dead, exiting combat
        if (targetFleetUnitGO == null && inCombat) {
            transform.GetComponent<Rigidbody>().isKinematic = false;
            foreach (GameObject ship in fleetUnitShips) {
                ship.GetComponent<Rigidbody>().isKinematic = true;
            }
            inCombat = false;
        }

        // if player and out of combat, get player input
        if (isPlayerFleet && !inCombat) {
            fleetTargetPosition = GetPlayerInput();
            MoveAndRotateToTarget(this.gameObject, fleetTargetPosition);
        }
    }

    private Vector3 GetPlayerInput() {
        Vector3 targetPos = fleetTargetPosition;

        if (Mouse.current.rightButton.wasPressedThisFrame) {
            timeAtButtonDown = Time.fixedTime;
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame) {
            timeAtButtonUp = Time.fixedTime;
        }

        float buttonClickTime = timeAtButtonUp - timeAtButtonDown;
        if (buttonClickTime > 0 && buttonClickTime < 0.25f) {    
            timeAtButtonDown = 0;
            timeAtButtonUp = 0;

            targetPos = GetTargetPosition();
        }
        return targetPos;      
    }

    private Vector3 GetPositionNearEnemy(GameObject shipToMove, GameObject targetShip) {
        var targetDirection = (targetShip.transform.position - shipToMove.transform.position).normalized;
        var targetDist = Vector3.Distance(shipToMove.transform.position, targetShip.transform.position);        
        
        var travelDist = targetDist - shipToMove.GetComponent<ShipTemplate>().WeaponComponent.FiringRange;
        if (travelDist > 0) {
            targetDirection *= travelDist;
        }
        targetDirection += shipToMove.transform.position;

        return targetDirection;
    }

    private void CheckFleetStatus() {
        List<GameObject> shipsToRemove = new List<GameObject>();

        foreach (GameObject ship in fleetUnitShips) {
            if (!ship.GetComponent<ShipTemplate>().IsAlive) {
                shipsToRemove.Add(ship);
            }
        }

        foreach (GameObject ship in shipsToRemove) {
            fleetUnitShips.Remove(ship);
            Destroy(ship);
        }
        
        if (fleetUnitShips.Count == 0) {
            Destroy(gameObject);
            print("fleet unit dead");
        }
    }

    public void ReceiveDamage(int damageSent, int shipToDamage) {
        var ship = fleetUnitShips[shipToDamage].GetComponent<ShipTemplate>();
        if (ship.IsAlive) {
            ship.ProcessDamage(damageSent);
        }
    }

    /*public void SendDamage(GameObject targetFleet) {    
        foreach (GameObject ship in FleetUnitShips) {
            ship.GetComponent<ShipTemplate>().DoDamage(targetFleet.GetComponent<FleetUnit>());
        }    
    }*/

    GameObject CheckForEnemyFleet() {        
        // using the first ship in the fleet for their sensor range... will need to change
        Collider[] hitColliders = Physics.OverlapSphere(fleetUnitShips[0].transform.position, fleetUnitShips[0].GetComponent<ShipTemplate>().SensorComponent.CurrentSensorResolution);
        float distToFleet = 10000;
        GameObject targetFleetUnitGO = null;
        if (hitColliders.Length > 0) {
            foreach (Collider col in hitColliders) {
                if (col.tag != "GalacticPlane" && col.tag != this.tag) {
                    float d = Vector3.Distance(fleetUnitShips[0].transform.position, col.transform.position);
                    if (d < distToFleet) {
                        distToFleet = d;
                        targetFleetUnitGO = col.gameObject;
                    }
                }
            }
        }
        return targetFleetUnitGO;
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

    void MoveAndRotateToTarget(GameObject objToMove, Vector3 target) {
        Rigidbody rb = objToMove.GetComponent<Rigidbody>();

        var dist = Vector3.Distance(objToMove.transform.position, target);
        var rot = objToMove.transform.InverseTransformPoint(target).x;
        
        if (dist > stoppingDist) {
            rb.AddRelativeForce(Vector3.forward * thrust);
        }            
        else if (dist < stoppingDist && rb.velocity.magnitude > 0) {
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
