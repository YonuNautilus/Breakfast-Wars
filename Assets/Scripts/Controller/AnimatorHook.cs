using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class AnimatorHook : MonoBehaviour {
        Animator anim;
        StateManager states;
        public void Init(StateManager st) {
            states = st;
            anim = st.anim;
        }

        private void OnAnimatorMove() {
            if (states.canMove) return; //If canMove is true, it means the player controls the movement, so there is no root motion to acount for, so we return;

            states.rigbod.drag = 0;
            float mulitplier = 1;

            Vector3 delta = anim.deltaPosition;
            delta.y = 0;
            Vector3 v = (delta * mulitplier) / states.delta;
            states.rigbod.velocity = v;
            
        }
    }
}

