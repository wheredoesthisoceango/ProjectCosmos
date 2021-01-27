using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldComponent : ShipComponent
{
    private int currentFieldStrength;
    private int fieldStrength;

    public int CurrentFieldStrength { get => currentFieldStrength; set => currentFieldStrength = value; }
    public int FieldStrength { get => fieldStrength; set => fieldStrength = value; }

    internal FieldComponent() {
        //BaseCompValue = 50;

        currentFieldStrength = fieldStrength = 100;
    }
}
