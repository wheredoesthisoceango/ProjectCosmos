using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //determines z movement limit
    public int systemLength = 100;    
    //determines x movement limit
    public int systemWidth = 100;
    public float panSpeed = 20f;
    public float rotSpeed = 10f;
    public float zoomSpeed = 50f;
    public int zoomDistance;
    public int currentZoom = 0;
    public float borderWidth = 10f;
    public bool edgeScrolling = true;
    public Camera cam;

    private float mouseX, mouseY;
    
    private int maxZoom = 6;
    private Vector3 newCamPos = Vector3.zero;
    private bool zoomInAction = false;
    
    // Start is called before the first frame update
    void Start() {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        Movement();
        Rotation();
        Zoom();
    }

    void Movement() {
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;

        forward.y = 0;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        if (pos.z < systemWidth && (Input.GetKey("w") || edgeScrolling == true && Input.mousePosition.y >= Screen.height - borderWidth)) {
            pos += forward * panSpeed * Time.deltaTime;
        }
        if (pos.z > -systemWidth && (Input.GetKey("s") || edgeScrolling == true && Input.mousePosition.y <= borderWidth)) {
            pos -= forward * panSpeed * Time.deltaTime;
        }

        if (pos.x < systemLength && (Input.GetKey("d") || edgeScrolling == true && Input.mousePosition.x >= Screen.width - borderWidth)) {
            pos += right * panSpeed * Time.deltaTime;
        }

        if (pos.x > -systemLength && (Input.GetKey("a") || edgeScrolling == true && Input.mousePosition.x <= borderWidth)) {
            pos -= right * panSpeed * Time.deltaTime;
        }
        transform.position = pos;
    }

    void Rotation() {
        if (Input.GetMouseButton(1)) {
            mouseX += Input.GetAxis("Mouse X") * rotSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * rotSpeed;
            mouseY = Mathf.Clamp(mouseY, -30, 45);
            transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        }
    }

    void Zoom() {
        Vector3 camPos = cam.transform.position;

        //print(Input.GetAxis("Mouse ScrollWheel"));

        //zoom in
        if (!zoomInAction && Input.GetAxis("Mouse ScrollWheel") > 0f && currentZoom > 0) {
            currentZoom -= 1;
            camPos += cam.transform.forward * zoomDistance;
            newCamPos = camPos;
            zoomInAction = true;
        }
        //zoom out
        if (!zoomInAction && Input.GetAxis("Mouse ScrollWheel") < 0f && currentZoom < maxZoom) {
            currentZoom += 1;
            camPos -= cam.transform.forward * zoomDistance;
            newCamPos = camPos;
            zoomInAction = true;
        }

        if (newCamPos != Vector3.zero) {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, newCamPos, zoomSpeed * Time.deltaTime);

            if (Vector3.Distance(cam.transform.position, newCamPos) < 0.1f) {
                //print("current zoom " + currentZoom);
                newCamPos = Vector3.zero;
                zoomInAction = false;
            }
        }
        
        
        //cam.transform.position = camPos;

        /*Vector3 camPos = cam.transform.position;
        float distance = Vector3.Distance(transform.position, cam.transform.position);
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && distance > zoomMin)
        {
            camPos += cam.transform.forward * zoomSpeed * Time.deltaTime;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f && distance < zoomMax)
        {
            camPos -= cam.transform.forward * zoomSpeed * Time.deltaTime;
        }
        cam.transform.position = camPos;*/
    }
}
