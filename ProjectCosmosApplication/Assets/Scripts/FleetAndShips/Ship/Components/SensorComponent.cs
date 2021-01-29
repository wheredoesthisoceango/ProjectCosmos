using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorComponent : ShipComponent
{
    private float sensorResolution;
    private float currentSensorResolution;

    public float SensorResolution { get => sensorResolution; set => sensorResolution = value; }
    public float CurrentSensorResolution { get => currentSensorResolution; set => currentSensorResolution = value; }

    public SensorComponent() {
        sensorResolution = currentSensorResolution = 12;
    }

}
