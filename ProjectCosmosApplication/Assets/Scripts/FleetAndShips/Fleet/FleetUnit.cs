using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetUnit : MonoBehaviour
{
    [SerializeField]
    private int timeToMaxTurnSpeed = 6;
    [SerializeField]
    private int timeToMaxAcceleration = 18;
    private float timeAtButtonDown;
    private float timeAtButtonUp;
    private float rotTimeRunning;
    private float transTimeRunning;
    private bool isPlayerFleet;
    private bool inCombat;
    private bool isAlive;

    private EasingFunction.Ease rotEase;
    private EasingFunction.Function rotEaseFunc;
    private EasingFunction.Ease transEase;
    private EasingFunction.Function transEaseFunc;
    private Vector3 fleetTargetPosition;
    private List<GameObject> fleetUnitShips = new List<GameObject>();
    private GameObject targetFleetUnitGO;

    private int TimeToMaxTurnSpeed { get => timeToMaxTurnSpeed; set => timeToMaxTurnSpeed = value; }
    private int TimeToMaxAcceleration { get => timeToMaxAcceleration; set => timeToMaxAcceleration = value; }
    private Vector3 FleetTargetPosition { get => fleetTargetPosition; set => fleetTargetPosition = value; }
    private float TimeAtButtonDown { get => timeAtButtonDown; set => timeAtButtonDown = value; }
    private float TimeAtButtonUp { get => timeAtButtonUp; set => timeAtButtonUp = value; }
    private float RotTimeRunning { get => rotTimeRunning; set => rotTimeRunning = value; }
    private float TransTimeRunning { get => transTimeRunning; set => transTimeRunning = value; }
    private bool IsPlayerFleet { get => isPlayerFleet; set => isPlayerFleet = value; }
    private bool InCombat { get => inCombat; set => inCombat = value; }
    private bool IsAlive { get => isAlive; set => isAlive = value; }
    private List<GameObject> FleetUnitShips { get => fleetUnitShips; set => fleetUnitShips = value; }
    private GameObject TargetFleetUnitGO { get => targetFleetUnitGO; set => targetFleetUnitGO = value; }
    private EasingFunction.Ease RotEase { get => rotEase; set => rotEase = value; }
    private EasingFunction.Function RotEaseFunc { get => rotEaseFunc; set => rotEaseFunc = value; }
    private EasingFunction.Ease TransEase { get => transEase; set => transEase = value; }
    private EasingFunction.Function TransEaseFunc { get => transEaseFunc; set => transEaseFunc = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (this.tag == "Player") {
            IsPlayerFleet = true;
        }

        FleetTargetPosition = transform.position;

        RotEase = EasingFunction.Ease.EaseInOutQuad;
        RotEaseFunc = EasingFunction.GetEasingFunction(RotEase);

        TransEase = EasingFunction.Ease.EaseInOutCubic;
        TransEaseFunc = EasingFunction.GetEasingFunction(TransEase);

        foreach (Transform child in transform) {
            FleetUnitShips.Add(child.gameObject);
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
        if (!inCombat && targetFleetUnitGO != null && targetFleetUnitGO.tag == "Hostile") {
            inCombat = true;
            fleetTargetPosition = transform.position;
            foreach (GameObject ship in fleetUnitShips) {
                var combatPosition = GetPositionNearEnemy(ship, targetFleetUnitGO);
                ship.GetComponent<ShipTemplate>().CombatTargetPosition = combatPosition;
            }
        }
        
        // target is dead, exiting combat
        if (targetFleetUnitGO == null && inCombat) {
            print("out of combat?");
            inCombat = false;
        }

        if (isPlayerFleet && !inCombat) {
            if (Input.GetMouseButtonDown(1)) {
                TimeAtButtonDown = Time.fixedTime;
            }
            if (Input.GetMouseButtonUp(1)) {
                TimeAtButtonUp = Time.fixedTime;
            }

            float buttonClickTime = TimeAtButtonUp - TimeAtButtonDown;
            if (buttonClickTime > 0 && buttonClickTime < 0.25) {    
                TimeAtButtonDown = 0;
                TimeAtButtonUp = 0;

                RotTimeRunning = 0;
                TransTimeRunning = 0;

                FleetTargetPosition = GetTargetPosition();
            }

            if (Vector3.Distance(transform.position, FleetTargetPosition) > 0.1f) {
                RotateToTarget(this.gameObject, FleetTargetPosition, timeToMaxTurnSpeed);
                MoveToTarget(this.gameObject, FleetTargetPosition, timeToMaxAcceleration);
            }
        }

        if (isPlayerFleet && inCombat)
        {
            foreach (GameObject ship in fleetUnitShips) {
                //Debug.DrawLine (ship.transform.position, ship.transform.position + ship.GetComponent<ShipTemplate>().CombatTargetPosition * 10, Color.red, Mathf.Infinity);
                if (Vector3.Distance(ship.transform.position, ship.GetComponent<ShipTemplate>().CombatTargetPosition) > 0.1f) {
                    //print("move to combat pos dist: " + Vector3.Distance(ship.transform.position, ship.GetComponent<ShipTemplate>().CombatTargetPosition));
                    RotateToTarget(ship, targetFleetUnitGO.transform.position, 15);
                    MoveToTarget(ship, ship.GetComponent<ShipTemplate>().CombatTargetPosition, 18);
                }
                else {
                    ship.GetComponent<ShipTemplate>().DoDamage(targetFleetUnitGO.GetComponent<FleetUnit>());
                    //SendDamage(TargetFleetUnitGO);                    
                }
            }
        }
    }

    private Vector3 GetPositionNearEnemy(GameObject shipToMove, GameObject targetFleet) {
        var targetDirection = (targetFleet.transform.position - shipToMove.transform.position).normalized;
        var targetDist = Vector3.Distance(shipToMove.transform.position, targetFleet.transform.position);
        
        
        var travelDist = targetDist - shipToMove.GetComponent<ShipTemplate>().WeaponComponent.FiringRange;
        if (travelDist > 0) {
            targetDirection *= travelDist;
        }
        targetDirection += shipToMove.transform.position;
        Debug.DrawLine (shipToMove.transform.position, targetDirection, Color.red, Mathf.Infinity);
        //print("target vector" + targetDirection);
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.position = targetDirection;

        return targetDirection;
    }

    private void CheckFleetStatus() {
        List<GameObject> shipsToRemove = new List<GameObject>();

        foreach (GameObject ship in FleetUnitShips) {
            if (!ship.GetComponent<ShipTemplate>().IsAlive) {
                shipsToRemove.Add(ship);
            }
        }

        foreach (GameObject ship in shipsToRemove) {
            FleetUnitShips.Remove(ship);
            Destroy(ship);
        }
        
        if (FleetUnitShips.Count == 0) {
            Destroy(gameObject);
            print("fleet unit dead");
        }
    }

    public void ReceiveDamage(int damageSent, int shipToDamage) {
        var ship = FleetUnitShips[shipToDamage].GetComponent<ShipTemplate>();
        if (ship.IsAlive) {
            ship.ProcessDamage(damageSent);
        }
    }

    public void SendDamage(GameObject targetFleet) {    
        foreach (GameObject ship in FleetUnitShips) {
            ship.GetComponent<ShipTemplate>().DoDamage(targetFleet.GetComponent<FleetUnit>());
        }    
    }

    GameObject CheckForEnemyFleet() {        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, FleetUnitShips[0].GetComponent<ShipTemplate>().SensorComponent.CurrentSensorResolution);
        float distToFleet = 10000;
        GameObject targetFleetUnitGO = null;
        if (hitColliders.Length > 0) {
            foreach (Collider col in hitColliders) {
                if (col.tag != "GalacticPlane" && col.tag != this.tag) {
                    float d = Vector3.Distance(transform.position, col.transform.position);
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

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance)) {
            hitPoint = ray.GetPoint(distance);
        }
        return hitPoint;
    }

    void RotateToTarget(GameObject objToRotate, Vector3 target, int timeToMaxTurnSpeed) {        
        var targetRotation = Quaternion.LookRotation(target - objToRotate.transform.position);
        var rotationEase = 0f;

        if (RotTimeRunning < timeToMaxTurnSpeed) {    
            rotationEase = RotEaseFunc(0, 1, RotTimeRunning / timeToMaxTurnSpeed);
            RotTimeRunning += Time.deltaTime;
        }

        objToRotate.transform.rotation = Quaternion.Lerp(objToRotate.transform.rotation, targetRotation, rotationEase);
    }

    void MoveToTarget(GameObject objToMove, Vector3 target, int timeToMaxAcceleration) {
        var translationEase = 0f;
        
        if (TransTimeRunning < timeToMaxAcceleration) {    
            translationEase = TransEaseFunc(0, 1, TransTimeRunning / timeToMaxAcceleration);
            TransTimeRunning += Time.deltaTime;
        }

        objToMove.transform.position = Vector3.Lerp(objToMove.transform.position, target, translationEase);  //new Vector3(x, y, z);
    }
}
