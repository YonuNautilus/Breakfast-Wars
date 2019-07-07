using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTransform : MonoBehaviour {

    private string moveInputAxis = "Vertical";
    private string turnInputAxis = "Horizontal";

    public float rotationRate = 360;    //degrees per second

    public float moveSpeed = 2;

    #region Monobehavior API

    // Update is called once per frame
    void Update() {
        float moveAxis = Input.GetAxis(moveInputAxis);
        float turnAxis = Input.GetAxis(turnInputAxis);
        ApplyInput(moveAxis, turnAxis);
    }

    private void ApplyInput(float moveInput, float turnInput) {
        move(moveInput);
        turn(turnInput);
    }

    private void move(float input) {
        transform.Translate(Vector3.forward * input * moveSpeed);
    }

    private void turn(float input) {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
    }

    #endregion
}
