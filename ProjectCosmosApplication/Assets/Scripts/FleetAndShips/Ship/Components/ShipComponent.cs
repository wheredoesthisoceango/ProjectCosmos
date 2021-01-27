using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComponent
{
    private int requiredPower;
    private int currentPower;
    //private int baseCompValue;
    //private int currentCompValue;
    //private int shipClassModdedValue;
    private int researchVersion;

    public int RequiredPower { get => requiredPower; set => requiredPower = value; }
    public int CurrentPower { get => currentPower; set => currentPower = value; }
    //public int BaseCompValue { get => baseCompValue; set => baseCompValue = value; }
    //public int CurrentCompValue { get => currentCompValue; set => currentCompValue = value; }
    //public int ShipClassModdedValue { get => shipClassModdedValue; set => shipClassModdedValue = value; }
    public int ResearchVersion { get => researchVersion; set => researchVersion = value; }

    public ShipComponent() {
        RequiredPower = 1;
        CurrentPower = 0;
        ResearchVersion = 1;
        //BaseCompValue = 5;
    }

}
