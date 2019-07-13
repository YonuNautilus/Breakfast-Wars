using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class StateManager : MonoBehaviour {
        [Header("Init")]
        public GameObject activeModel;

        [Header("Inputs")]
        public float vertical;
        public float horizontal;
        public float moveAmount;
        public Vector3 moveDir;
        public bool rt, rb, lt, lb, a, b, y, x;

        [Header("Stats")]
        public float moveSpeed = 5;
        public float runSpeed = 10f;
        public float rotateSpeed = 5;
        public float toGround = 0.75f;
        public float thruGround = 0.1f;


        [Header("States")]
        public bool run;
        public bool onGround;
        public bool lockon;
        public bool inAction;
        public bool canMove;
        public bool isTwoHanded;
        public WeaponType wt = WeaponType.UNARMED;
        public enum WeaponType {
            UNARMED = 0,
            ONEHANDED = 1,
            TWOHANDED = 2

        }

        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rigbod;
        [HideInInspector]
        public AnimatorHook a_hook;

        [HideInInspector]
        private Vector3[] origins = new Vector3[5];
        [HideInInspector]
        private Vector3 boxColliderOffset;
        [HideInInspector]
        private Collider bc;

        [HideInInspector]
        public float delta;
        [HideInInspector]
        public LayerMask ignoreLayers;

        float _actionDelay;

        public void Init() {
            SetupAnimator();
            rigbod = GetComponent<Rigidbody>();
            rigbod.angularDrag = 999;
            rigbod.drag = 4;
            rigbod.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            a_hook = activeModel.AddComponent<AnimatorHook>();
            a_hook.Init(this);

            gameObject.layer = 8;
            ignoreLayers = ~(1 << 9);

            anim.SetBool("onGround", true);

            bc = gameObject.GetComponent<Collider>();
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

            DetectAction();

            if (inAction){
                anim.applyRootMotion = true;

                _actionDelay += delta;
                if(_actionDelay > 0.3f) {
                    inAction = false;
                    _actionDelay = 0;
                }
                else return;
            }

            canMove = anim.GetBool("canMove");

            if (!canMove)
                return;

            anim.applyRootMotion = false;

            rigbod.drag = (moveAmount > 0 || !onGround) ? 0 : 4;

            float targetSpeed = (run) ? runSpeed: moveSpeed;

            if(onGround)
                rigbod.velocity = moveDir * (targetSpeed * moveAmount);

            if (run)
                lockon = false;

            if (!lockon) {
                Vector3 targetDir = moveDir;
                targetDir.y = 0;
                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * moveAmount * rotateSpeed);
                transform.rotation = targetRotation;
            }

            HandleMovementAnimations();
        }

        public void DetectAction() {
            if (!canMove) return;

            if (!rb && !rt && !lb && !lt) return;

            string targetAnim = null;

            if (rb) targetAnim = "Right_Hand_Swing_Charge";
            if (rt) targetAnim = "Right_Hand_Swing_Pre";

            if (string.IsNullOrEmpty(targetAnim)) return;

            canMove = false;
            inAction = true;
            anim.CrossFade(targetAnim, 0.2f);
            ///rigbod.velocity = Vector3.zero;
        }

        public void Tick(float d) {
            delta = d;
            onGround = OnGround();
            anim.SetBool("onGround", onGround);
        }

        void HandleMovementAnimations() {
            anim.SetBool("run", run);
            anim.SetFloat("vertical", moveAmount, 0.4f, delta);

        }

        public bool OnGround() {
            bool r = false;

            Vector3 origin = transform.position + (Vector3.up * toGround);
            Vector3 dir = -Vector3.up;

            float dis = toGround + thruGround;

            RaycastHit hit;

            int amm = 0;
            float yavg = 0;
            foreach (Vector3 v in GetOrigins(origin)){
                Debug.DrawRay(v, dir * (dis - thruGround), Color.green, 0.1f, false);
                Debug.DrawRay(new Vector3(v.x, v.y - toGround, v.z), dir * (thruGround), Color.red, 0.1f, false);
                if (Physics.Raycast(v, dir, out hit, dis, ignoreLayers)) {
                    r = true;
                    yavg += hit.point.y;
                    amm++;
                }
            }
            if(amm > 0) {
                Vector3 targetposition = new Vector3(transform.position.x, yavg / amm, transform.position.z);
                transform.position = targetposition;
            }

            return r;
        }

        private Vector3[] GetOrigins(Vector3 origin) {
            Vector3[] origins = new Vector3[7];

            bc = gameObject.GetComponent<Collider>();

            BoxCollider nbc = gameObject.GetComponent<BoxCollider>();

            Quaternion relative = transform.localRotation;

            if(nbc != null) {
                origins[0] = origin;
                origins[1] = origin + (relative * new Vector3(nbc.size.x / 2, 0, nbc.size.z / 2));    //front right
                origins[2] = origin + (relative * new Vector3(nbc.size.x / 2, 0, -nbc.size.z / 2));
                origins[3] = origin + (relative * new Vector3(-nbc.size.x / 2, 0, -nbc.size.z / 2));
                origins[4] = origin + (relative * new Vector3(-nbc.size.x / 2, 0, nbc.size.z / 2));

                origins[5] = origin + (relative * new Vector3(0, 0, nbc.size.z / 2));
                origins[6] = origin + (relative * new Vector3(0, 0, -nbc.size.z / 2));
            }
            else {
                origins = new Vector3[1];
                origins[0] = origin;
            }

            return origins;
        }

        public void HandleTwoHanded() {
            anim.SetInteger("isTwoHanded", (int)wt);
        }
    }
}

