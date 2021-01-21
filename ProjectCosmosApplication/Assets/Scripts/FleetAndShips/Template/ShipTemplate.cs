using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTemplate : MonoBehaviour
{
    internal int powerGeneratorMaxSize;

    internal int maxComponentPower;

    internal int componentScalingFactor;

    internal ShipComponent fieldComponent = new FieldComponent();
    internal ShipComponent armorComponent = new ArmorComponent();
    internal ShipComponent weaponComponent = new WeaponComponent();
    internal ShipComponent sensorComponent = new SensorComponent();
    internal ShipComponent engineComponent = new EngineComponent();
    internal ShipComponent powerCoreComponent = new PowerCoreComponent();
    internal ShipComponent dComputeComponent = new DComputeComponent();
    internal ShipComponent cComputeComponent = new CComputeComponent();

    void Start()
    {

    }

    void Update()
    {
        
    }
}
