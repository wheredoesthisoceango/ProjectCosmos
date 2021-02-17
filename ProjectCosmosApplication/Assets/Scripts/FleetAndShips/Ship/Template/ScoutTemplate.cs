using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutTemplate : ShipTemplate
{
    public ScoutTemplate() {
        PowerGeneratorMaxSize = 1;
        MaxComponentPower = 5;
        ShipClassCompScalingFactor = 2;

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
