using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
	[SerializeField] Vector3 rotation;
	[SerializeField] float rotationSpeed = 0;
	[SerializeField] bool randomize;
	
	[SerializeField] float minSpeed;
	[SerializeField] float maxSpeed;

	// Use this for initialization
	void Start () {
		if(randomize) {
			rotation = new Vector3(RandFloat(), RandFloat(), RandFloat());
			rotationSpeed = Random.Range(minSpeed,maxSpeed);
		}
	}
	
	float RandFloat() {
		return Random.Range(0f,1.01f);
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		this.transform.Rotate(rotation, rotationSpeed * Time.deltaTime);
	}
}
