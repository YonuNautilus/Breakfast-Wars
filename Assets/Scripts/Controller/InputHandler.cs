using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class InputHandler : MonoBehaviour {
        float vertical;
        float horizontal;
        float delta;

        StateManager states;
        CameraManager camManager;
        void Start() {
            states = GetComponent<StateManager>();
            states.Init();

            camManager = CameraManager.singleton;
            camManager.Init(this.transform);
        }

        void FixedUpdate() {
            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
        }

        void Update() {
            delta = Time.deltaTime;
            camManager.Tick(delta);
        }

        void GetInput() {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }


        void UpdateStates() {
            states.horizontal = horizontal;
            states.vertical = vertical;

            Vector3 v = states.vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;
            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);

            states.FixedTick(Time.deltaTime);
        }
    }
}
