using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTemplate : MonoBehaviour
{
    private int powerGeneratorMaxSize;

    private int maxComponentPower;

    private int shipClassCompScalingFactor;
    
    private bool isAlive;

    private Vector3 combatTargetPosition;

    private FieldComponent fieldComponent = new FieldComponent();
    private ArmorComponent armorComponent = new ArmorComponent();
    private WeaponComponent weaponComponent = new WeaponComponent();
    private SensorComponent sensorComponent = new SensorComponent();
    private EngineComponent engineComponent = new EngineComponent();
    private PowerCoreComponent powerCoreComponent = new PowerCoreComponent();
    private DComputeComponent dComputeComponent = new DComputeComponent();
    private CComputeComponent cComputeComponent = new CComputeComponent();

    public int PowerGeneratorMaxSize { get => powerGeneratorMaxSize; set => powerGeneratorMaxSize = value; }
    public int MaxComponentPower { get => maxComponentPower; set => maxComponentPower = value; }
    public int ShipClassCompScalingFactor { get => shipClassCompScalingFactor; set => shipClassCompScalingFactor = value; }
    public FieldComponent FieldComponent { get => fieldComponent; set => fieldComponent = value; }
    public ArmorComponent ArmorComponent { get => armorComponent; set => armorComponent = value; }
    public WeaponComponent WeaponComponent { get => weaponComponent; set => weaponComponent = value; }
    public SensorComponent SensorComponent { get => sensorComponent; set => sensorComponent = value; }
    public EngineComponent EngineComponent { get => engineComponent; set => engineComponent = value; }
    public PowerCoreComponent PowerCoreComponent { get => powerCoreComponent; set => powerCoreComponent = value; }
    public DComputeComponent DComputeComponent { get => dComputeComponent; set => dComputeComponent = value; }
    public CComputeComponent CComputeComponent { get => cComputeComponent; set => cComputeComponent = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public Vector3 CombatTargetPosition { get => combatTargetPosition; set => combatTargetPosition = value; }

    void Start()
    {
        IsAlive = true;
    }

    void Update()
    {
        
    }

    private int ProcessFieldDamage(int incomingDamage) {
        int remainingDamage = 0;

        var fieldValAfterDamage = fieldComponent.CurrentFieldStrength - incomingDamage;

        remainingDamage = Mathf.Max(0, -1 * fieldValAfterDamage);
        fieldComponent.CurrentFieldStrength = fieldValAfterDamage > 0 ? fieldValAfterDamage : 0;
        print("oof I took " + incomingDamage + " field damage, fields at " + fieldValAfterDamage);

        return remainingDamage;
    }

    private int ProcessArmorDamage(int incomingDamage) {
        int remainingDamage = 0;
        
        var armorValAfterDamage = armorComponent.CurrentArmorDurability - incomingDamage;

        remainingDamage = Mathf.Max(0, -1 * armorValAfterDamage);
        armorComponent.CurrentArmorDurability = armorValAfterDamage > 0 ? armorValAfterDamage : 0;
        print("oof I took " + incomingDamage + " armor damage, armor at " + armorValAfterDamage);

        return remainingDamage;
    }

    public void ProcessDamage(int incomingDamage) {
        int remainingDamage = incomingDamage;
        if (FieldComponent.CurrentFieldStrength > 0) {
            remainingDamage = ProcessFieldDamage(incomingDamage);
        }
        if (remainingDamage > 0 && ArmorComponent.CurrentArmorDurability > 0) {
            remainingDamage = ProcessArmorDamage(remainingDamage);
        }

        if (remainingDamage > 0) {
            IsAlive = false;
            print("oof i am dead");
        }
    }

    public void DoDamage(FleetUnit targetFleet) {
        if (weaponComponent.OffCooldown) {                
            print("pew pew " + this.name + " did " + weaponComponent.CurrentWeaponDamage + " damage");
            int randShip = 0; //Random.Range(0, targetFleet.GetComponent<FleetUnit>().getNumShipsInFleet());
            targetFleet.ReceiveDamage(weaponComponent.CurrentWeaponDamage, randShip);
            weaponComponent.OffCooldown = false;
        }
        else {
            weaponComponent.ReduceCoolDownTime();
        }
    }
}
