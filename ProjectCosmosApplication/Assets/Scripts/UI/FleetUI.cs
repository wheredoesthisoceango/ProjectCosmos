using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetUI : MonoBehaviour
{
    public FleetManager fleetManager;
    private Canvas canvas;
    [SerializeField]
    private Text textField;

    void Start()
    {
        canvas = transform.GetComponent<Canvas>();        
    }

    void Update()
    {
        
    }
}
