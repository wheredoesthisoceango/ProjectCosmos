using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetUnit : MonoBehaviour
{
    List<ShipTemplate> ships = new List<ShipTemplate>();

    // Start is called before the first frame update
    void Start()
    {
        ShipTemplateManager shipTemplateManager = GetComponent<ShipTemplateManager>();

        ships.Add(shipTemplateManager.GetCruiser());
        ships.Add(shipTemplateManager.GetScout());

        print(ships[0].GetType());
        print(ships[1].GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
