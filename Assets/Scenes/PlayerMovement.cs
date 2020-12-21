using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;

    private string moveInputAxis = "Vertical";
    private string turnInputAxis = "Horizontal";

    public float rotationRate = 360;    //degrees per second
    public float moveSpeed = 10;
    public float mouseSensitivity = 2.0f;

    private Rigidbody rb;
    private Vector3 prevMouse;

    private Vector3 boxCollideroffset;

    private Vector3 moveDirection;

    private Vector3[] origins = new Vector3[5];
    private BoxCollider bc;

    private Vector3 center;
    private Vector3 backLeftOrigin;
    private Vector3 backRightOrigin;
    private Vector3 frontLeftOrigin;
    private Vector3 frontRighttOrigin;

    // Start is called before the first frame update
    void Start(){

        prevMouse = Input.mousePosition;
        anim = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        bc = gameObject.GetComponent<BoxCollider>();
        boxCollideroffset = bc.center - transform.position;

        origins[0] = boxCollideroffset + new Vector3(0, -bc.size.y, 0);
        origins[1] = boxCollideroffset + new Vector3(bc.size.x, -bc.size.y, bc.size.z);
        origins[2] = boxCollideroffset + new Vector3(bc.size.x, -bc.size.y, -bc.size.z);
        origins[3] = boxCollideroffset + new Vector3(-bc.size.x, -bc.size.y, -bc.size.z);
        origins[4] = boxCollideroffset + new Vector3(-bc.size.x, -bc.size.y, bc.size.z);
        
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButtonDown(1)) {
            Globals.camLock = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetMouseButtonUp(1)) {
            Globals.camLock = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKey("w") || Input.GetKey("up")) {
            anim.SetInteger("Anim", 1);
        }
        else {
            anim.SetInteger("Anim", 0);
        }
        //if()
        float moveAxis = Input.GetAxis(moveInputAxis);
        float turnAxis = Input.GetAxis(turnInputAxis);
        
        Vector3 combinedInput = new Vector3(moveAxis, 0, turnAxis);
        moveDirection = transform.InverseTransformVector(new Vector3(combinedInput.normalized.x, 0, combinedInput.normalized.z));

        Debug.Log("COMBINED INPUT " + combinedInput);
        Debug.Log("MOVE DIRECTION " + moveDirection);
        
        //Quaternion rot = Quaternion.LookRotation(moveDirection);
        //transform.rotation = rot;

        Vector3 avgFoot = GetFootPos();
    }

    private void FixedUpdate() {
        rb.velocity = moveDirection * moveSpeed;
    }

    private Vector3 GetFootPos(){
        updateVertices();
        Vector3 loc = center;
        int num = 0;
        RaycastHit hitInfo;

        foreach(Vector3 v in origins){
            Debug.DrawRay(v, -Vector3.up, Color.red, 0.1f, false);
            if (Physics.Raycast(v, -Vector3.up, out hitInfo, 1.6f)) {
                loc += hitInfo.point;
                num++;
            }
        }
        try {
            return loc /= num;
        } catch(Exception ex) {
            Debug.Log(ex.Message);
            return new Vector3(0, 0, 0);
        }
    }

    private void updateVertices(){
        origins[0] = transform.position + boxCollideroffset + new Vector3(0, -bc.size.y/2, 0);
        origins[1] = transform.position + boxCollideroffset + new Vector3(bc.size.x/2, -bc.size.y/2, bc.size.z/2);
        origins[2] = transform.position + boxCollideroffset + new Vector3(bc.size.x/2, -bc.size.y/2, -bc.size.z/2);
        origins[3] = transform.position + boxCollideroffset + new Vector3(-bc.size.x/2, -bc.size.y/2, -bc.size.z/2);
        origins[4] = transform.position + boxCollideroffset + new Vector3(-bc.size.x/2, -bc.size.y/2, bc.size.z/2);
    }
}
