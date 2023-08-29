using System;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using Zenject;

public class DoorState : MonoBehaviour
{
        public int MinAngleToMove, MaxAngleToMove;
        public bool DoorIsOpen;

        Rigidbody rigid;
        public float angle;
        
        public bool DoorIsLocked = false;
        
        Vector3 currentRotation;
        float moveLockAmount, rotateAngles, ratio;
        private bool DoorIsHeld;
        private Grabber _leftGrabber, _rightGrabber;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(List<Grabber> grabbers, SignalBus signalBus)
        {
            foreach (var grabber in grabbers)
            {
                if(grabber.HandSide == ControllerHand.Left)
                {
                    _leftGrabber = grabber;
                }
                else
                {
                    _rightGrabber = grabber;
                }
            }
            _signalBus = signalBus;
        }

        void Start() 
        {
            rigid = GetComponent<Rigidbody>();
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (!InputBridge.Instance.RightTriggerDown && !InputBridge.Instance.LeftTriggerDown)
            {
                return;
            }

            var grabber = InputBridge.Instance.RightTriggerDown ? _rightGrabber : _leftGrabber;

            if (grabber.HeldGrabbable is null || grabber.HeldGrabbable.gameObject != gameObject)
            {
                return;
            }
        }

        

        private void CheckDoorState()
        {
            if (angle > MinAngleToMove && angle < MaxAngleToMove)
            {
                Debug.Log("move");
                DoorIsLocked = false;
                /*if (angle < MinAngleToMove+10)
                {
                    DoorIsOpen = false;
                    gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x,
                        MinAngleToMove, gameObject.transform.localEulerAngles.z);
                }

                if (angle > MaxAngleToMove)
                {
                    DoorIsOpen = true;
                    gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x,
                        MaxAngleToMove, gameObject.transform.localEulerAngles.z);
                }*/

            }
        }
        void Update() {

            currentRotation = transform.localEulerAngles;
            angle = Mathf.Floor(currentRotation.y);
            CheckDoorState();


            // Lock Door in place if closed and requires handle to be turned
            if(angle < 0.02f && DoorIsLocked) {
                // Check on detection mode
                if (rigid.collisionDetectionMode == CollisionDetectionMode.Continuous || rigid.collisionDetectionMode == CollisionDetectionMode.ContinuousDynamic) {
                    rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                }

                rigid.isKinematic = true;
            }
            else {
                // Check on detection mode
                if (rigid.collisionDetectionMode == CollisionDetectionMode.ContinuousSpeculative) {
                    rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                }

                rigid.isKinematic = false;
            }
        }
}
