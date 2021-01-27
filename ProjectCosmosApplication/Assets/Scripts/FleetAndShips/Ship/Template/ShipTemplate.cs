using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTemplate : MonoBehaviour
{
    private int powerGeneratorMaxSize;

    private int maxComponentPower;

    private int shipClassCompScalingFactor;
    
    private bool isAlive;

    private FieldComponent fieldComponent = new FieldComponent();
    private ArmorComponent armorComponent = new ArmorComponent();
    private WeaponComponent weaponComponent = new WeaponComponent();
    private SensorComponent sensorComponent = new SensorComponent();
    private EngineComponent engineComponent = new EngineComponent();
    private PowerCoreComponent powerCoreComponent = new PowerCoreComponent();
    private DComputeComponent dComputeComponent = new DComputeComponent();
    private CComputeComponent cComputeComponent = new CComputeComponent();

    protected int PowerGeneratorMaxSize { get => powerGeneratorMaxSize; set => powerGeneratorMaxSize = value; }
    protected int MaxComponentPower { get => maxComponentPower; set => maxComponentPower = value; }
    protected int ShipClassCompScalingFactor { get => shipClassCompScalingFactor; set => shipClassCompScalingFactor = value; }
    protected FieldComponent FieldComponent { get => fieldComponent; set => fieldComponent = value; }
    protected ArmorComponent ArmorComponent { get => armorComponent; set => armorComponent = value; }
    protected WeaponComponent WeaponComponent { get => weaponComponent; set => weaponComponent = value; }
    protected SensorComponent SensorComponent { get => sensorComponent; set => sensorComponent = value; }
    protected EngineComponent EngineComponent { get => engineComponent; set => engineComponent = value; }
    protected PowerCoreComponent PowerCoreComponent { get => powerCoreComponent; set => powerCoreComponent = value; }
    protected DComputeComponent DComputeComponent { get => dComputeComponent; set => dComputeComponent = value; }
    protected CComputeComponent CComputeComponent { get => cComputeComponent; set => cComputeComponent = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }

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
        int weaponDamage = weaponComponent.CurrentWeaponDamage;
        if (weaponComponent.OffCooldown) {                
            print("pew pew " + this.name + " did " + weaponDamage + " damage");
            int randShip = 0; //Random.Range(0, targetFleet.GetComponent<FleetUnit>().getNumShipsInFleet());
            targetFleet.ReceiveDamage(weaponDamage, randShip);
            weaponComponent.OffCooldown = false;
        }
        else {
            weaponComponent.ReduceCoolDownTime();
        }
    }
}
