/* ====================================
 * Credit goes to puppeteer from the Unity Forums
 * Source: https://forum.unity.com/threads/rts-camera-script.72045/
 * ==================================== */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float ScrollSpeed = 15;
    public float ScrollEdge = .01f;

    float PanSpeed = 10;

    public Vector2 ZoomRange = new Vector2(-5, 5);
    public float CurrentZoom = 0;
    public float ZoomZpeed = 1;
    public float ZoomRotation = 1;

    private Vector3 InitPos;
    private Vector3 InitRotation;



    void Start()
    {
        InitPos = transform.position;
        InitRotation = transform.eulerAngles;
    }

    void Update()
    {
        //PAN
        if (Input.GetKey("mouse 2"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * PanSpeed * (Input.mousePosition.x - Screen.width * .5f) / (Screen.width * .5f), Space.World);
            transform.Translate(Vector3.forward * Time.deltaTime * PanSpeed * (Input.mousePosition.y - Screen.height * .5f) / (Screen.height * .5f), Space.World);
        }
        else
        {
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge))
            {
                transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
            }
            else if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge)
            {
                transform.Translate(Vector3.right * Time.deltaTime * -ScrollSpeed, Space.World);
            }

            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
            }
            else if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * -ScrollSpeed, Space.World);
            }
        }

        //ZOOM IN/OUT

        CurrentZoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 1000 * ZoomZpeed;

        CurrentZoom = Mathf.Clamp(CurrentZoom, ZoomRange.x, ZoomRange.y);

        transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y - (InitPos.y + CurrentZoom)) * .1f, transform.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x - (transform.eulerAngles.x - (InitRotation.x + CurrentZoom * ZoomRotation)) * .1f, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
