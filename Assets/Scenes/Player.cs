using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Start is called before the first frame update

    private Animator anim;
    private CharacterController controller;
    public float moveSpeed = 20.0f;
    public float turnSpeed = 60.0f;
    public Vector3 moveDirection = Vector3.zero;
    public float gravity = 20.0f;

    void Start() {
        anim = gameObject.GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKey("w")||Input.GetKey("up")){
		    anim.SetInteger("Anim", 1);
	    } else {
		    anim.SetInteger("Anim", 0);
	    }

        if(controller.isGrounded){
            moveDirection = transform.forward * Input.GetAxis("Vertical") * moveSpeed;
        }

        float turn = Input.GetAxis("Horizontal");
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
        controller.Move(moveDirection * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;
    }
}
