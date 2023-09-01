 using System;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using Zenject;

public class DoorState : MonoBehaviour
{
        public int MinAngleToMove, MaxAngleToMove;
        public bool DoorIsOpen;

        private float angle;
        
        Vector3 currentRotation;
        float moveLockAmount, rotateAngles, ratio;
        private bool DoorIsHeld;
        private Grabber _leftGrabber, _rightGrabber;
        private SignalBus _signalBus;
        
        Rigidbody rigid;
        private Vector3 startPos;
        public Vector3 closeRot;
        public Vector3 openRot;

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
            startPos = transform.position;
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
            
            if (!(_rightGrabber.HeldGrabbable is null) && _rightGrabber.HeldGrabbable.gameObject.CompareTag("handle") ||
                    !(_leftGrabber.HeldGrabbable is null) && _leftGrabber.HeldGrabbable.gameObject.CompareTag("handle"))
            {
                UnlockTheDoor();
                return;
            }

            
            if (angle > MinAngleToMove && angle < MaxAngleToMove)
            {
                UnlockTheDoor();
            }
            else
            {
                LockTheDoor();
            }
        }
        void Update() {
            currentRotation = transform.localEulerAngles;
            angle = Mathf.Floor(currentRotation.y);
            CheckDoorState();
        }

        private void LockTheDoor()
        {
            GetComponent<DoorHelper>().DoorIsLocked = true;
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log(MaxAngleToMove);
            if (angle >= MaxAngleToMove)
            {
                transform.eulerAngles = closeRot;
                DoorIsOpen = false;
            }
            else
            {
                transform.eulerAngles = openRot;
                DoorIsOpen = true;
            }
            
            transform.position = startPos;
        }
        private void UnlockTheDoor()
        {
            GetComponent<DoorHelper>().DoorIsLocked = false;
            rigid.constraints = RigidbodyConstraints.None;
        }
}
