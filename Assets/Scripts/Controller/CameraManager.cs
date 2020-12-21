using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class CameraManager : MonoBehaviour {
        public static CameraManager singleton;

        public bool lockon;
        public bool controllerEnabled;
        public float followSpeed = 9;
        public float mouseSpeed = 2;
        public float controllerSpeed = 7;

        public Transform target;
        public EnemyTarget lockonTarget;
        public Transform lockonTransform;

        [HideInInspector]
        public Transform pivot;
        [HideInInspector]
        public Transform camTrans;
        StateManager states;

        float turnSmoothing = 0.1f;
        public float minAngle = -35;
        public float maxAngle = 35;
        float smoothX;
        float smoothY;
        float smoothXVelocity;
        float smoothYVelocity;

        public float lookAngle;
        public float tiltAngle;

        bool usedRightAxis;

        public void Init(StateManager sm) {
            states = sm;
            target = states.transform;
            camTrans = Camera.main.transform;
            pivot = camTrans.parent;
        }

        public void Tick(float d) {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float c_h = Input.GetAxis("RightAxis X");
            float c_v = Input.GetAxis("RightAxis Y");

            float targetSpeed = mouseSpeed;

            if (lockonTarget != null) {
                if (lockonTransform == null) {
                    lockonTransform = lockonTarget.GetTarget();
                    states.lockOnTransform = lockonTransform;
                }

                if (Mathf.Abs(c_h) > 0.6f) {
                    if (!usedRightAxis) {
                        lockonTransform = lockonTarget.GetTarget((c_h > 0));    //c_h > 0 means (if x axis input is pointing at the right)
                        states.lockOnTransform = lockonTransform;
                        usedRightAxis = true;
                    }
                        
                }
            }

            if (usedRightAxis) {
                if (Mathf.Abs(c_h) < 0.6f) {
                    usedRightAxis = false;
                }
            }

            if (c_h != 0 || c_v != 0) {
                h = c_h;
                v = c_v;
                targetSpeed = controllerSpeed;
            }

            FollowTarget(d);
            HandleRotations(d, v, h, targetSpeed);
        }

        void FollowTarget(float d) {
            float speed = d * followSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, d);
            //transform.position = targetPosition;
            transform.position = target.position;
        }

        void HandleRotations(float d, float v, float h, float targetSpeed) {
            if (turnSmoothing > 0) {
                smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXVelocity, turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYVelocity, turnSmoothing);
            }
            else {
                smoothX = h;
                smoothY = v;
            }

            tiltAngle -= smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

            if (lockon && lockonTarget != null) {

                Vector3 targetDir = lockonTransform.position - transform.position;
                targetDir.Normalize();
                //targetDir.y = 0;

                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;

                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, d * 9);
                lookAngle += transform.eulerAngles.y;

                return;
            }

            lookAngle += smoothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);

            tiltAngle -= smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);

            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
        }
        void Awake() {
            singleton = this;
        }
        // Update is called once per frame
        void Update() {

        }
    }
}