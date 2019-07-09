using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class Helper : MonoBehaviour {
        [Range(0, 1)]
        public float vertical;

        public string animName;
        public bool playAnim;

        public string[] oh_attacks;
        public string jumpAction;

        public bool doJump;
        public bool pickupItem;
        public bool pickupWeapon;
        public bool poke;
        public bool interacting;
        public int weaponType;

        public bool enableRootMotion;

        Animator anim;

        void Start() {
            anim = GetComponent<Animator>();
        }

        void Update() {
            enableRootMotion = !anim.GetBool("canMove");
            anim.applyRootMotion = enableRootMotion;

            interacting = anim.GetBool("interacting");

            if (enableRootMotion)
                return;

            if (!interacting && pickupItem) {
                anim.Play("Right_Hand_Grab");
                pickupItem = false;
            }

            if (!interacting && pickupWeapon) {
                anim.Play("Upper_Body_Grab");
                pickupWeapon= false;
            }

            if(!interacting && poke) {
                anim.Play("Left_Arm_Poke");
                poke = false;
            }

            if (interacting) {
                playAnim = false;
                vertical = Mathf.Clamp(vertical, 0, 0.5f);
            }

            if (doJump) {
                anim.Play("Jump_Charge");
                doJump = false;
            }

            anim.SetInteger("weaponType", weaponType);

            if (playAnim) {
                string targetAnim = "";
                switch (weaponType) {
                    case 0:
                        //targetAnim = jumpAction;
                        vertical = 0;
                        break;
                    case 1:
                        int r = Random.Range(0, oh_attacks.Length);
                        targetAnim = oh_attacks[r];
                        break;
                }

                anim.CrossFade(targetAnim, 0.2f);

                //anim.SetBool("canMove", false);
                //enableRootMotion = true;

                playAnim = false;
            }
            anim.SetFloat("vertical", vertical);

            doJump = false;
        }
    }
}
