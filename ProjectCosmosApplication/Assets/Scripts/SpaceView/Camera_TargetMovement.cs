using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera_TargetMovement : MonoBehaviour
{
    public bool targetExists = true;

    [SerializeField]
	Transform focus = default;

	[SerializeField, Range(1f, 300f)]
	float distance = 250f;

    [SerializeField, Min(0f)]
	float focusRadius = 1f;

    [SerializeField, Range(0f, 1f)]
	float focusCentering = 0.5f;

    [SerializeField, Range(1f, 360f)]
	float rotationSpeed = 90f;

    [SerializeField, Range(1f, 360f)]
	float zoomSpeed = 50f;

    [SerializeField, Range(-89f, 89f)]
	float minVerticalAngle = -30f, maxVerticalAngle = 60f;

    [SerializeField, Range(100f, 300f)]
	float minDistance = 100f, maxDistance = 300f;

    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    Vector3 focusPoint;
    Vector2 orbitAngles = new Vector2(45f, 0f);

    bool acceptInput;

    public bool AcceptInput { get => acceptInput; set => acceptInput = value; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void Awake () {
		focusPoint = focus.position;
		transform.localRotation = Quaternion.Euler(orbitAngles);
	}

    void LateUpdate () {
        UpdateFocusPoint();
        Quaternion lookRotation;
        if (ManualRotation()) {
            ConstrainAngles();
            lookRotation = Quaternion.Euler(orbitAngles);
        }
        else {
            lookRotation = transform.localRotation;
        }
        ManualZoom();  
        ConstrainDistance();
		Vector3 lookDirection = lookRotation * Vector3.forward;
		Vector3 lookPosition = focusPoint - lookDirection * distance;
		transform.SetPositionAndRotation(lookPosition, lookRotation);
	}

    void UpdateFocusPoint () {
		Vector3 targetPoint = focus.position;
        if (focusRadius > 0f) {
			float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
			if (distance > 0.01f && focusCentering > 0f) {
				t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
			}
			if (distance > focusRadius) {
				t = Mathf.Min(t, focusRadius / distance);
			}
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
		}
		else {
		    focusPoint = targetPoint;
        }
	}

    void OnValidate () {
		if (maxVerticalAngle < minVerticalAngle) {
			maxVerticalAngle = minVerticalAngle;
		}
	}

    void ConstrainAngles () {
		orbitAngles.x =	Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

		if (orbitAngles.y < 0f) {
			orbitAngles.y += 360f;
		}
		else if (orbitAngles.y >= 360f) {
			orbitAngles.y -= 360f;
		}
	}

    void ConstrainDistance () {
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    bool ManualRotation () {    
        Vector2 input = Vector2.zero;    
        if (Mouse.current.rightButton.isPressed) 
            input = Mouse.current.delta.ReadValue().normalized;

        var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(input.magnitude);
        var x = input.y * mouseSensitivityFactor * -0.5f;
        var y = input.x * mouseSensitivityFactor;
		const float e = 0.1f;
		if (input.x < -e || input.x > e || input.y < -e || input.y > e) {
			orbitAngles += rotationSpeed * Time.unscaledDeltaTime * new Vector2(x,y);
            return true;
		}
        return false;
	}

    void ManualZoom() {
        Vector2 input = Mouse.current.scroll.ReadValue();
        distance -= input.y * zoomSpeed * Time.unscaledDeltaTime;
    }
}
