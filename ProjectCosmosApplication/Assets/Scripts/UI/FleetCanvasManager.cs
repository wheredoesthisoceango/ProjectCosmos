using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
 using UnityEngine.EventSystems;

public class FleetCanvasManager : MonoBehaviour
{
    public GameObject cam;

    private float initDist;
    GraphicRaycaster raycaster;

    private Image fleetStatusIcon;

    private FleetUnit parentFleet;
    
    void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        parentFleet = transform.root.GetComponent<FleetUnit>();
        fleetStatusIcon = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = cam.transform.rotation;

        if (this.GetComponent<Canvas>().isActiveAndEnabled) {
            var dist = Vector3.Distance(this.transform.position, cam.transform.position);
            this.transform.localScale = Vector3.one * dist / initDist;

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
                    FleetSelected(true);
                }

                if (results.Count == 0 && fleetStatusIcon != null) {
                    FleetSelected(false);
                }
            }
        }
    }

    void FleetSelected(bool selected) {
        if (selected) {
            fleetStatusIcon.color = Color.green;
            parentFleet.isFleetSelected = true;    
        }
        else {            
            fleetStatusIcon.color = Color.white;
            parentFleet.isFleetSelected = false;        
        }
    }

    public void SetupIcon() {        
        initDist = Vector3.Distance(this.transform.position, cam.transform.position);
    }
}
