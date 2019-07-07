using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRigidBody : MonoBehaviour {
    private Animator anim;

    private string moveInputAxis = "Vertical";
    private string turnInputAxis = "Horizontal";

    public float rotationRate = 360;    //degrees per second
    public float moveSpeed = 10;
    public float mouseSensitivity = 2.0f;

    private Rigidbody rb;
    private Vector3 prevMouse;

    #region Monobehavior API

    private void Start() {
        prevMouse = Input.mousePosition;
        anim = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
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
        ApplyInput(moveAxis, turnAxis);
    }

    private void ApplyInput(float moveInput, float turnInput) {
        move(moveInput);
        turn(turnInput);
    }

    private void move(float input) {
        rb.AddForce(transform.forward * input * moveSpeed, ForceMode.Impulse);
    }

    private void turn(float input) {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
        //rb.AddTorque(transform.up * 0, ForceMode.VelocityChange);
        Quaternion tempRot = new Quaternion(0, transform.rotation.y, 0, 1);
        transform.SetPositionAndRotation(transform.position, tempRot);
    }

    #endregion
}
