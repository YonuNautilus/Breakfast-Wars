using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Start is called before the first frame update

    private Animator anim;
    private CharacterController controller;
    public float moveSpeed = 20.0f;
    public float turnSpeed = 60.0f;
    public float mouseSensitivity = 2.0f;
    public Vector3 moveDirection = Vector3.zero;
    public float gravity = 20.0f;

    private Vector3 prevMouse;

    void Start() {
        prevMouse = Input.mousePosition;
        anim = gameObject.GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(1)){
            Globals.camLock = true;
            Cursor.lockState = CursorLockMode.Locked;
        } else if (Input.GetMouseButtonUp(1)) {
            Globals.camLock = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKey("w")||Input.GetKey("up")){
		    anim.SetInteger("Anim", 1);
	    } else {
		    anim.SetInteger("Anim", 0);
	    }

        if(controller.isGrounded){
            moveDirection = transform.forward * Input.GetAxis("Vertical") * moveSpeed;
        }

        float lookVert = mouseSensitivity * Input.GetAxis("Mouse Y");

        float turn = Input.GetAxis("Horizontal");
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
        controller.Move(moveDirection * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;
    }
}
