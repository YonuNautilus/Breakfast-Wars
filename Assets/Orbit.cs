using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float camSensitivity = 4.0f;
    public GameObject player;

    float distance = 10;
    Vector3 playerPrevPos, playerMoveDir;

    public static Vector3 initOffset = new Vector3(0.0f, 12.17f, -15.6f);
    public static Vector3 playerHead;

    private Vector3 offset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //initOffset = new Vector3(player.position.x - this.transform.position.x,
        //    player.position.y - this.transform.position.y,
        //    player.position.z - this.transform.position.x);
        //offset = new Vector3(player.position.x, player.position.y + 8.0f, player.position.z + 7.0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        playerMoveDir = player.transform.position - playerPrevPos;
        if (playerMoveDir != Vector3.zero)
        {
            playerMoveDir.Normalize();
            transform.position = player.transform.position - playerMoveDir * distance;

            transform.position += new Vector3(0, 5f, 0);// required height

            transform.LookAt(player.transform.position);

            playerPrevPos = player.transform.position;
        }


        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * camSensitivity, Vector3.up) * offset;
        //playerHead = player.transform.position;
        ////playerHead.y += initOffset.y;
        //transform.position = player.position + initOffset + offset;
        //transform.LookAt(playerHead);
    }
}