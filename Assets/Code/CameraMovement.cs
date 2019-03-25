/* ====================================
 * Credit goes to puppeteer from the Unity Forums
 * Source: https://forum.unity.com/threads/rts-camera-script.72045/
 * ==================================== */

using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float ScrollSpeed = 15;
    public float ScrollEdge = .01f;

    public float PanSpeed = 50;

    // public Vector2 ZoomRange = new Vector2(-5, 5);
    // public float CurrentZoom = 0;
    // public float ZoomZpeed = 1;
    // public float ZoomRotation = 1;

    //private Vector3 InitPos;
    //private Vector3 InitRotation;

    public float cameraTurnSpeed = .1f;
    private float oldMousePosition;

    public LayerMask groundLayer;
    public float heightAboveGround;



    void Start()
    {
        //InitPos = transform.position;
        //InitRotation = transform.eulerAngles;

        oldMousePosition = Input.mousePosition.x;
    }

    void Update()
    {
        //SpecificAmountAboveGround
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer)){
            transform.position = new Vector3(transform.position.x, hit.point.y + heightAboveGround, transform.position.z);
        } else {
            int rayOffset = 5;
            Debug.DrawRay(new Vector3(transform.position.x+rayOffset, transform.position.y, transform.position.z), Vector3.down*100);
            Debug.DrawRay(new Vector3(transform.position.x-rayOffset, transform.position.y, transform.position.z), Vector3.down*100);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z+rayOffset), Vector3.down*100);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z-rayOffset), Vector3.down*100);

            if(Physics.Raycast(new Vector3(transform.position.x+rayOffset, transform.position.y, transform.position.z), Vector3.down*100, Mathf.Infinity, groundLayer)){
                //Debug.Log("Right");
                if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge)) { transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed); }
            }
            if(Physics.Raycast(new Vector3(transform.position.x-rayOffset, transform.position.y, transform.position.z), Vector3.down*100, Mathf.Infinity, groundLayer)){
                //Debug.Log("Left");
                if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge) { transform.Translate(Vector3.right * Time.deltaTime * -ScrollSpeed); }
            }
            if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z+rayOffset), Vector3.down*100, Mathf.Infinity, groundLayer)){
                //Debug.Log("Forward");
                if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge)) { transform.Translate(Vector3.forward * Time.deltaTime * ScrollSpeed); }
            }
            if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z-rayOffset), Vector3.down*100, Mathf.Infinity, groundLayer)){
                //Debug.Log("Back");
                if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge) { transform.Translate(Vector3.forward * Time.deltaTime * -ScrollSpeed); }
            }
            return;
        }
        //End van code (PS: als je precies in een hoek komt... you're fucked)


        //PAN
        if (Input.GetKey("mouse 2"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * PanSpeed * (Input.mousePosition.x - Screen.width * .5f) / (Screen.width * .5f));
            transform.Translate(Vector3.forward * Time.deltaTime * PanSpeed * (Input.mousePosition.y - Screen.height * .5f) / (Screen.height * .5f));
        }
        else
        {
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge))
                transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed);
            else if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge)
                transform.Translate(Vector3.right * Time.deltaTime * -ScrollSpeed);

            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge))
                transform.Translate(Vector3.forward * Time.deltaTime * ScrollSpeed);
            else if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge)
                transform.Translate(Vector3.forward * Time.deltaTime * -ScrollSpeed);
        }

        //ZOOM IN/OUT

        //CurrentZoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 1000 * ZoomZpeed;

        //CurrentZoom = Mathf.Clamp(CurrentZoom, ZoomRange.x, ZoomRange.y);

        //transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y - (InitPos.y + CurrentZoom)) * .1f, transform.position.z);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x - (transform.eulerAngles.x - (InitRotation.x + CurrentZoom * ZoomRotation)) * .1f, transform.eulerAngles.y, transform.eulerAngles.z);

        // Turn camera, added by me
        if (Input.GetMouseButton(1))
        {
            float difference = Input.mousePosition.x - oldMousePosition;

            transform.eulerAngles -= new Vector3(0, -difference * cameraTurnSpeed, 0);

            oldMousePosition = Input.mousePosition.x;
        }
        else
            oldMousePosition = Input.mousePosition.x;
    }
}
