using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorComponent : ShipComponent
{
    private float sensorStength;

    public float SensorStength { get => sensorStength; set => sensorStength = value; }

    public SensorComponent() {
        sensorStength = 10;
    }

}
