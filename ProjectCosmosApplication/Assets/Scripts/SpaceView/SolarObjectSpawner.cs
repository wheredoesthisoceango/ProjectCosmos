using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarObjectSpawner : MonoBehaviour
{
    public GameObject[] planets;
    public GameObject sun;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject planet in planets) {
            DrawCircle orbitDistance = planet.GetComponent<DrawCircle>();
            Vector3 sunPos = sun.transform.position;
            sunPos.z += orbitDistance.orbitRadius;
            GameObject newPlanet = Instantiate(planet, sunPos, Quaternion.identity);
            newPlanet.transform.RotateAround(sun.transform.position, Vector3.up, RandFloat());
            newPlanet.GetComponent<Orbit>().target = sun;
            newPlanet.GetComponent<DrawCircle>().orbitTarget = sun.transform;
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
