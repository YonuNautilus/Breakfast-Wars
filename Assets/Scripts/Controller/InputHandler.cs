using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class InputHandler : MonoBehaviour {
        public float vertical;
        public float horizontal;
        public bool a_input;
        public bool b_input;
        public bool x_input;
        public bool y_input;
        public bool rb_input;
        public bool lb_input;
        public bool rt_input;
        public float rt_axis;
        public bool lt_input;
        public float lt_axis;

        public bool leftAxis_down;
        public bool rightAxis_down;

        float a_timer;  //b_timer for running ramp-up
        float rt_timer;
        float lt_timer;

        float delta;

        StateManager states;
        CameraManager camManager;

        void Start() {
            states = GetComponent<StateManager>();
            states.Init();

            camManager = CameraManager.singleton;
            camManager.Init(states);
        }

        void FixedUpdate() {
            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            states.FixedTick(delta);
            camManager.Tick(delta);
        }

       
        void Update() {
            delta = Time.deltaTime;
            states.Tick(delta);
            ResetInputsAndStates();
        }
        

        void GetInput() {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

            b_input = Input.GetButton("B");
            a_input = Input.GetButton("A");
            x_input = Input.GetButton("X");
            y_input = Input.GetButton("Y");


            rt_input = Input.GetButton("RT");
            rt_axis = Input.GetAxis("RT");
            rb_input = Input.GetButton("RB");
            if (rt_axis != -1) rt_input = true;

            lt_input = Input.GetButton("LT");
            lt_axis = Input.GetAxis("LT");
            lb_input = Input.GetButton("LB");
            if (lt_axis != -1) lt_input = true;

            rightAxis_down = Input.GetButtonUp("R3");
            leftAxis_down = Input.GetButtonUp("L3");

            if (a_input) {
                a_timer += delta;
            }
        }


        void UpdateStates() {
            states.horizontal = horizontal;
            states.vertical = vertical;

            Vector3 v = states.vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;
            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);

            if (a_input && a_timer > 0.5f) {
                states.run = (states.moveAmount > 0);
            }

            if (a_input == false && a_timer > 0 && a_timer < 0.5f)
                states.rollInput = true;
            

            states.b = b_input;
            states.a = a_input;
            states.x = x_input;
            states.y = y_input;
            states.rt = rt_input;
            states.lt = lt_input;
            states.rb = rb_input;
            states.lb = lb_input;

            if (y_input) {
                if(states.wt == StateManager.WeaponType.ONEHANDED) {
                    
                }
            }

            if (rightAxis_down) {
                states.lockon = !states.lockon;
                if (states.lockOnTarget == null)
                    states.lockon = false;

                camManager.lockonTarget = states.lockOnTarget;
                states.lockOnTransform = camManager.lockonTransform;
                camManager.lockon = states.lockon;
            }
            //states.FixedTick(Time.deltaTime);
        }

        void ResetInputsAndStates() {
            if (a_input == false)
                a_timer = 0;

            if (states.rollInput)
                states.rollInput = false;

            if (states.run)
                states.run = false;
        }
    }
}
