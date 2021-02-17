using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleet : MonoBehaviour
{
    private List<GameObject> ships = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in transform) {
            ships.Add(child.gameObject);
        }
    }

    void Update()
    {
        
    }
}
