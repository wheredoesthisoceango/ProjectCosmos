using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComponent
{
    public int requiredPower;
    public int currentPower;
    public float baseStrength;
    public float currentStrength;
    public float shipClassStrength;
    public int researchVersion;

    public ShipComponent() {
        requiredPower = 1;
        currentPower = 0;
        researchVersion = 1;
        baseStrength = 5;
        currentStrength = baseStrength;
    }
}
