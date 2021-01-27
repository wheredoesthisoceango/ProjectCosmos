using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetUnit : MonoBehaviour
{
    [SerializeField]
    private int timeToMaxTurnSpeed = 6;
    [SerializeField]
    private int timeToMaxAcceleration = 18;
    [SerializeField]
    private int sensorRange = 25;
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
    private Vector3 playerSelectedTargetPosition;
    private List<GameObject> fleetUnitShips = new List<GameObject>();
    private GameObject targetFleetUnitGO;

    public int TimeToMaxTurnSpeed { get => timeToMaxTurnSpeed; set => timeToMaxTurnSpeed = value; }
    public int TimeToMaxAcceleration { get => timeToMaxAcceleration; set => timeToMaxAcceleration = value; }
    public int SensorRange { get => sensorRange; set => sensorRange = value; }
    public Vector3 PlayerSelectedTargetPosition { get => playerSelectedTargetPosition; set => playerSelectedTargetPosition = value; }
    public float TimeAtButtonDown { get => timeAtButtonDown; set => timeAtButtonDown = value; }
    public float TimeAtButtonUp { get => timeAtButtonUp; set => timeAtButtonUp = value; }
    public float RotTimeRunning { get => rotTimeRunning; set => rotTimeRunning = value; }
    public float TransTimeRunning { get => transTimeRunning; set => transTimeRunning = value; }
    public bool IsPlayerFleet { get => isPlayerFleet; set => isPlayerFleet = value; }
    public bool InCombat { get => inCombat; set => inCombat = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public List<GameObject> FleetUnitShips { get => fleetUnitShips; set => fleetUnitShips = value; }
    public GameObject TargetFleetUnitGO { get => targetFleetUnitGO; set => targetFleetUnitGO = value; }
    public EasingFunction.Ease RotEase { get => rotEase; set => rotEase = value; }
    public EasingFunction.Function RotEaseFunc { get => rotEaseFunc; set => rotEaseFunc = value; }
    public EasingFunction.Ease TransEase { get => transEase; set => transEase = value; }
    public EasingFunction.Function TransEaseFunc { get => transEaseFunc; set => transEaseFunc = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (this.tag == "Player") {
            IsPlayerFleet = true;
        }

        PlayerSelectedTargetPosition = transform.position;

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

        if (IsPlayerFleet && !InCombat) {
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

                PlayerSelectedTargetPosition = GetTargetPosition();
            }

            if (Vector3.Distance(transform.position, PlayerSelectedTargetPosition) > 0.1f) {
                RotateToTarget(PlayerSelectedTargetPosition);
                MoveToTarget(PlayerSelectedTargetPosition);
            }
        }

        // check if enemy in range
        if (!InCombat) {
            TargetFleetUnitGO = CheckForEnemyFleet();
        }
        if (TargetFleetUnitGO != null && TargetFleetUnitGO.tag == "Hostile") {
            InCombat = true;
            SendDamage(TargetFleetUnitGO);
            RotateToTarget(TargetFleetUnitGO.transform.position);
        }
        else if (TargetFleetUnitGO == null && InCombat) {
            //target gone, dead?
            print("out of combat?");
            InCombat = false;
        }

        // if not, then maintain holding pattern

        // else there is, approach enemy and engage in combat
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SensorRange);
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

    void RotateToTarget(Vector3 target) {        
        var targetRotation = Quaternion.LookRotation(target - transform.position);
        var rotationEase = 0f;

        if (RotTimeRunning < TimeToMaxTurnSpeed) {    
            rotationEase = RotEaseFunc(0, 1, RotTimeRunning / TimeToMaxTurnSpeed);
            RotTimeRunning += Time.deltaTime;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationEase);
    }

    void MoveToTarget(Vector3 target) {
        var translationEase = 0f;
        
        if (TransTimeRunning < TimeToMaxAcceleration) {    
            translationEase = TransEaseFunc(0, 1, TransTimeRunning / TimeToMaxAcceleration);
            TransTimeRunning += Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, PlayerSelectedTargetPosition, translationEase);  //new Vector3(x, y, z);
    }
}
