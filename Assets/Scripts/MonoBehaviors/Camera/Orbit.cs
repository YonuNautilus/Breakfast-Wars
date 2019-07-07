using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public bool lockCursor;
    public float mouseSensitivity = 10f;
    public Transform target;
    public float distFromTarget = 2f;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    void Start(){
        if (lockCursor){
            Cursor.lockState = CursorLockMode.Locked;
        }

        //player = GameObject.FindGameObjectWithTag("Player");
        //initOffset = new Vector3(player.position.x - this.transform.position.x,
        //    player.position.y - this.transform.position.y,
        //    player.position.z - this.transform.position.x);
        //offset = new Vector3(player.position.x, player.position.y + 8.0f, player.position.z + 7.0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //playerMoveDir = player.transform.position - playerPrevPos;
        //if (playerMoveDir != Vector3.zero)
        //{
        //    playerMoveDir.Normalize();
        //    transform.position = player.transform.position - playerMoveDir * distance;

        //    transform.position += new Vector3(0, 5f, 0);// required height

        //    transform.LookAt(player.transform.position);

        //    playerPrevPos = player.transform.position;
        //}


        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * camSensitivity, Vector3.up) * offset;
        //playerHead = player.transform.position;
        ////playerHead.y += initOffset.y;
        //transform.position = player.position + initOffset + offset;
        //transform.LookAt(playerHead);
    }
}