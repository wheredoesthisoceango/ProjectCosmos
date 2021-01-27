using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiserTemplate : ShipTemplate
{
    internal CruiserTemplate() {
    }
    
    void Awake() {
        PowerGeneratorMaxSize = 2;
        MaxComponentPower = 10;
        ShipClassCompScalingFactor = 10;

        FieldComponent.FieldStrength *= ShipClassCompScalingFactor;
        ArmorComponent.ArmorDurability *= ShipClassCompScalingFactor;
        WeaponComponent.WeaponDamage *= ShipClassCompScalingFactor;
        SensorComponent.SensorResolution *= ShipClassCompScalingFactor;

        FieldComponent.CurrentFieldStrength = FieldComponent.FieldStrength;
        ArmorComponent.CurrentArmorDurability = ArmorComponent.ArmorDurability;
        WeaponComponent.CurrentWeaponDamage = WeaponComponent.WeaponDamage;
        SensorComponent.CurrentSensorResolution = SensorComponent.SensorResolution;
    }

}
