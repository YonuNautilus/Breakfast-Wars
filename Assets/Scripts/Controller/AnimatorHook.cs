using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class AnimatorHook : MonoBehaviour {
        Animator anim;
        StateManager states;

        public float rm_multiplier; //root motion multiplier
        bool rolling;
        float roll_t;   //roll curve time index
        
        public void Init(StateManager st) {
            states = st;
            anim = st.anim;
        }

        public void InitForRoll() {
            rolling = true;
            roll_t = 0;
        }

        public void CloseRoll() {
            if (!rolling)
                return;

            rm_multiplier = 1;
            roll_t = 0;
            rolling = false;
        }

        private void OnAnimatorMove() {
            if (states.canMove) return; //If canMove is true, it means the player controls the movement, so there is no root motion to acount for, so we return;

            states.rigbod.drag = 0;

            if (rm_multiplier == 0)
                rm_multiplier = 1;

            if (!rolling) {
                Vector3 delta = anim.deltaPosition;
                delta.y = 0;
                Vector3 v = (delta * rm_multiplier) / states.delta;
                states.rigbod.velocity = v;
            }
            else {
                //Sample the curve,  create new vector v1 (forwards unit vector * the curve value at roll_t)
                roll_t += Time.deltaTime / 1.15f;

                if(roll_t > 1) {
                    roll_t = 1;
                }

                float zVal = states.roll_curve.Evaluate(roll_t);
                Vector3 v1 = Vector3.forward * zVal;
                Vector3 relative = transform.TransformDirection(v1);
                Vector3 v2 = (relative * rm_multiplier) / states.delta;
                states.rigbod.velocity = v2;
            }
        }
    }
}

