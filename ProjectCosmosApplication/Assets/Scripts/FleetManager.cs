using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> fleets = new List<GameObject>();
    public List<GameObject> Fleets { get => fleets; set => fleets = value; }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
