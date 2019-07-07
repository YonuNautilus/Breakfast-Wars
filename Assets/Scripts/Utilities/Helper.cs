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

        public int weaponType;

        Animator anim;

        void Start() {
            anim = GetComponent<Animator>();
        }

        void Update() {
            anim.SetInteger("weaponType", weaponType);

            if (playAnim) {
                string targetAnim = "";
                switch (weaponType) {
                    case 0:

                        break;
                    case 1:
                        int r = Random.Range(0, oh_attacks.Length);
                        targetAnim = oh_attacks[r];
                        break;
                }

                vertical = 0;
                anim.CrossFade(targetAnim, 0.2f);
                playAnim = false;
            }
            anim.SetFloat("vertical", vertical);
        }
    }
}
