using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private float fixedDeltaTime;

    [SerializeField, Range(0, 2)] 
    private float adjustedTime = 1;

    void Start()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        Time.timeScale = adjustedTime;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
}
