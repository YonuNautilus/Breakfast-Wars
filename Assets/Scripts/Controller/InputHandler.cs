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
            states.FixedTick(delta);
            camManager.Tick(delta);
        }

       
        void Update() {
            delta = Time.deltaTime;
            states.Tick(delta);
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
        }


        void UpdateStates() {
            states.horizontal = horizontal;
            states.vertical = vertical;

            Vector3 v = states.vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;
            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);

            states.rollInput = a_input;

            if (b_input) {
                //states.run = (states.moveAmount > 0);
            }
            else {
                //states.run = false;
            }

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

                camManager.lockonTarget = states.lockOnTarget.transform;
                camManager.lockon = states.lockon;
            }
            //states.FixedTick(Time.deltaTime);
        }
    }
}
