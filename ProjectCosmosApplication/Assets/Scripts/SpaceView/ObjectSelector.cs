using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ObjectSelector : MonoBehaviour
{
    PhysicsRaycaster raycaster;

    private Transform fleetUnitSelected;

    void Awake()
    {
        raycaster = GetComponent<PhysicsRaycaster>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Set up the new Pointer Event
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            pointerData.position = Mouse.current.position.ReadValue();
            raycaster.Raycast(pointerData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                //Debug.Log(result.gameObject.tag + " selected");
                if (result.gameObject.tag == "Ship") {
                    fleetUnitSelected = result.gameObject.transform.root;
                    fleetUnitSelected.BroadcastMessage("FleetSelected", true);
                }
            }

            if (results.Count == 0 && fleetUnitSelected != null) {
                fleetUnitSelected.BroadcastMessage("FleetSelected", false);                
            }
        }
    }
}
