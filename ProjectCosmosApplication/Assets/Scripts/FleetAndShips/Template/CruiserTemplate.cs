using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiserTemplate : ShipTemplate
{
    public CruiserTemplate() {
    }
    
    void Awake() {
        powerGeneratorMaxSize = 2;
        maxComponentPower = 10;
        componentScalingFactor = 10;

        float strength = fieldComponent.baseStrength * componentScalingFactor;

        fieldComponent.shipClassStrength = strength;
        armorComponent.shipClassStrength = strength;
        weaponComponent.shipClassStrength = strength;

        fieldComponent.currentStrength = strength;
        armorComponent.currentStrength = strength;
        weaponComponent.currentStrength = strength;
    }

}
