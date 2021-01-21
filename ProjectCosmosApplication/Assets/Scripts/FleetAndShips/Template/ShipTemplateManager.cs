using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTemplateManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    public ShipTemplate GetCruiser() {
        return new CruiserTemplate();
    }
    
    public ShipTemplate GetScout() {
        return new ScoutTemplate();
    }
}
