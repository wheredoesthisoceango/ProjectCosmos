using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorComponent : ShipComponent
{    
    private int currentArmorDurability;
    private int armorDurability;

    public int CurrentArmorDurability { get => currentArmorDurability; set => currentArmorDurability = value; }
    public int ArmorDurability { get => armorDurability; set => armorDurability = value; }

    internal ArmorComponent() {
        currentArmorDurability = armorDurability = 100;
    }
}
