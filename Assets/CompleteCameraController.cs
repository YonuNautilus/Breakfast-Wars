using UnityEngine;
using System.Collections;

public class CompleteCameraController : MonoBehaviour
{
    protected const float MAXDISTANCE = 30.0f;
    protected const float MINDISTANCE = 5.0f;
    public const float DEFAULTXANGLEDEG = 10.0f;
    protected const float DEFAULTXANGLE = DEFAULTXANGLEDEG * Mathf.Deg2Rad;

    public float mouseSensitivity = 0.5f;

    public GameObject player;       //Public variable to store a reference to the player game object

    private Vector3 target;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    private Vector3 dOffset;    //the offset that will be altered, so the initial can be saved

    private Vector3 targetOffset;

    protected float distance;

    private Vector3 prevMouse;

    float dx;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Calculate the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;

        targetOffset = new Vector3(0.0f, Mathf.Tan(DEFAULTXANGLE) * Mathf.Abs(offset.z), 0.0f);

        target = new Vector3(player.transform.position.x, player.transform.position.y + offset.y, player.transform.position.z) - targetOffset;
        //Calculate and store the offset value by getting the distance between the player's position and player focus-target.
        // = transform.position - target;

        distance = Vector3.Distance(player.transform.position, this.transform.position);
        prevMouse = Input.mousePosition;
}

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            distance += 1f;
            if (distance > MAXDISTANCE)
                distance = MAXDISTANCE;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            distance -= 1f;
            if (distance < MINDISTANCE)
                distance = MINDISTANCE;
        }

        updateTarget();

        //use distance as the magnitude, use the normal of the initial offset to set a new offset that is the distance magnitude away from the target NOT the player.
        dOffset = offset.normalized * distance;

        if (Input.GetMouseButtonDown(1))
        {
            prevMouse = Input.mousePosition;
        }

        if (Globals.camLock)
        {

            Vector3 mouseX = Input.mousePosition;

            //float dx = (mouseX.x - prevMouse.x);
            dx += mouseSensitivity * Input.GetAxis("Mouse X");

            dOffset = Quaternion.AngleAxis(dx, Vector3.up) * dOffset;
            //transform.position = player.transform.position + dOffset;

            transform.position = player.transform.position + player.transform.TransformDirection(dOffset);

            transform.RotateAround(target, Vector3.up, 0.1f * Mathf.Atan2(dx,distance));

            //prevMouse = Input.mousePosition;
        }
        else
        {
            dx = 0;
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = player.transform.position + player.transform.TransformDirection(dOffset);
        }

        transform.LookAt(target);
        //transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Tan((player.transform.position.x - transform.position.x) /(player.transform.position.z - transform.position.z)),Vector3.up);
    }

    void updateTarget()
    {
        target = new Vector3(player.transform.position.x, player.transform.position.y + offset.y - targetOffset.y, player.transform.position.z);
    }
}