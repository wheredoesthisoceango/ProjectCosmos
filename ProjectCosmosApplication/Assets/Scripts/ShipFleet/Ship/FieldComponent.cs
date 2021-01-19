using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldComponent : ShipComponent
{
    float maxFieldStrength;
    float currentFieldStrength;
    
    public FieldComponent() {
        requiredPower = 0;
    }
}
