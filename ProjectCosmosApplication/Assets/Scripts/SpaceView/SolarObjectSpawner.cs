using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarObjectSpawner : MonoBehaviour
{
    public GameObject[] planets;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject planet in planets) {
            DrawCircle orbitDistance = planet.GetComponent<DrawCircle>();
            Vector3 sunPos = transform.position;
            sunPos.z += orbitDistance.orbitRadius;
            GameObject newPlanet = Instantiate(planet, sunPos, Quaternion.identity);
            newPlanet.transform.RotateAround(transform.position, Vector3.up, RandFloat());
            newPlanet.GetComponent<DrawCircle>().orbitTarget = transform;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	float RandFloat() {
		return Random.Range(0f,50f);
	}
}
