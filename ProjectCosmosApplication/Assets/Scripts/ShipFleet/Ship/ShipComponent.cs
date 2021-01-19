using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComponent
{
    public int requiredPower;
    public int currentPower;

    public ShipComponent() {
        requiredPower = 1;
        currentPower = 0;
    }
}
