using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class StateManager : MonoBehaviour {
        public float vertical;
        public float horizontal;
        public float moveAmount;
        public Vector3 moveDir;

        public GameObject activeModel;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rb;

        public float delta;

        public void Init() {
            SetupAnimator();
            rb = GetComponent<Rigidbody>();
        }

        void SetupAnimator() {
            if(activeModel == null) {
                anim = GetComponentInChildren<Animator>();
                if(anim == null) {
                    Debug.Log("No model found");
                }
                else {
                    activeModel = anim.gameObject;
                }
            }

            if (anim == null)
                anim = activeModel.GetComponent<Animator>();

            anim.applyRootMotion = false;
        }

        public void FixedTick(float d) {
            delta = d;

            rb.velocity = moveDir;
            Debug.Log(moveDir);
        }
    }
}

