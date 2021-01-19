using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiserTemplate : ShipTemplate
{
    ShipComponent fields = new FieldComponent();
    public CruiserTemplate() {
        powerGeneratorMaxSize = 2;
    }
}
