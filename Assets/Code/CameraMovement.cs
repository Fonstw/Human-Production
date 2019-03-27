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

    public float cameraTurnSpeed = .1f;
    private float oldMousePosition;

    public LayerMask groundLayer;
    public float heightAboveGround;

    //zoom
    public float zoomSpeed = 3;
    public int maxHeight = 60;
    public int minHeight = 1;

    void Start()
    {
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
                if (Input.GetAxis("Horizontal") > 0 || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge)) { transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed); }
            }
            if(Physics.Raycast(new Vector3(transform.position.x-rayOffset, transform.position.y, transform.position.z), Vector3.down*100, Mathf.Infinity, groundLayer)){
                //Debug.Log("Left");
                if (Input.GetAxis("Horizontal") < 0 || Input.mousePosition.x <= Screen.width * ScrollEdge) { transform.Translate(Vector3.right * Time.deltaTime * -ScrollSpeed); }
            }
            if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z+rayOffset), Vector3.down*100, Mathf.Infinity, groundLayer)){
                //Debug.Log("Forward");
                if (Input.GetAxis("Vertical") > 0 || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge)) { transform.Translate(Vector3.forward * Time.deltaTime * ScrollSpeed); }
            }
            if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z-rayOffset), Vector3.down*100, Mathf.Infinity, groundLayer)){
                //Debug.Log("Back");
                if (Input.GetAxis("Vertical") < 0 || Input.mousePosition.y <= Screen.height * ScrollEdge) { transform.Translate(Vector3.forward * Time.deltaTime * -ScrollSpeed); }
            }
            return;
        }
        //End van code (PS: als je precies in een hoek komt... you're fucked)


        //PAN
        if (Input.GetAxis("Horizontal") > 0 || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge))
            transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed);
        else if (Input.GetAxis("Horizontal") < 0 || Input.mousePosition.x <= Screen.width * ScrollEdge)
            transform.Translate(Vector3.right * Time.deltaTime * -ScrollSpeed);

        if (Input.GetAxis("Vertical") > 0 || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge))
            transform.Translate(Vector3.forward * Time.deltaTime * ScrollSpeed);
        else if (Input.GetAxis("Vertical") < 0 || Input.mousePosition.y <= Screen.height * ScrollEdge)
            transform.Translate(Vector3.forward * Time.deltaTime * -ScrollSpeed);

        //ZOOM IN/OUT
        //Debug.Log("Mouse Scroll Y: " + Input.mouseScrollDelta.y);
        if(Input.GetAxis("Zoom") < 0 && transform.localRotation.x >= -0.22){
            heightAboveGround -= zoomSpeed * Time.deltaTime;
            transform.RotateAround(transform.position, transform.right, -(zoomSpeed*1.3f)*Time.deltaTime);
        } else if(Input.GetAxis("Zoom") > 0 && transform.localRotation.x <= 0.42){
            heightAboveGround += zoomSpeed * Time.deltaTime;
            transform.RotateAround(transform.position, transform.right, (zoomSpeed*1.3f)*Time.deltaTime);
        }

        if(heightAboveGround < minHeight){
            heightAboveGround = minHeight;
        }
        if(heightAboveGround > maxHeight){
            heightAboveGround = maxHeight;
        }
        
        //ROTATE
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            float difference = Input.mousePosition.x - oldMousePosition;

            transform.eulerAngles -= new Vector3(0, -difference * cameraTurnSpeed, 0);

            oldMousePosition = Input.mousePosition.x;
        }
        else
            oldMousePosition = Input.mousePosition.x;
    }
}
