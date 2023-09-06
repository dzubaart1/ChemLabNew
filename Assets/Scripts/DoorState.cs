 using System;
using System.Collections.Generic;
using BNG;
 using DefaultNamespace;
 using Installers;
 using Machines;
 using UnityEngine;
using Zenject;

public class DoorState : MonoBehaviour
{
    public DoorTypes DoorType;
    public int MinAngleToMove, MaxAngleToMove;
    public bool DoorIsOpen;

    public float angle, _addAngle;
    
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
        if(angle >= 180) {
            _addAngle = angle - 360;
        }
        else {
            _addAngle = 360 - angle;
        }
        CheckDoorState();
    }

    private void LockTheDoor()
    {
        
        GetComponent<DoorHelper>().DoorIsLocked = true;
        rigid.constraints = RigidbodyConstraints.FreezeAll;
        if (angle >= MaxAngleToMove) //&& angle < _addAngle)
        {
            if (!DoorIsOpen)
                return;
            transform.localEulerAngles = closeRot;
            DoorIsOpen = false;
            if (DoorType != DoorTypes.ScannerDoor)
            {
                _signalBus.Fire(new DoorWorkSignal()
                {
                    DoorType = DoorType,
                    IsOpen = false
                });
            }
            
        }
        else
        {
            if (DoorIsOpen)
                return;
            Debug.Log("DOOR IS CLOSE" + angle + " " + MaxAngleToMove);
            transform.localEulerAngles = openRot;
            DoorIsOpen = true;
            /*if (DoorType == DoorTypes.DryBoxDoor)
            {
                _signalBus.Fire(new DoorWorkSignal()
                {
                    DoorType = DoorType,
                    IsOpen = true
                });
            }*/
            
        }
        
        transform.position = startPos;
    }
    private void UnlockTheDoor()
    {
        GetComponent<DoorHelper>().DoorIsLocked = false;
        rigid.constraints = RigidbodyConstraints.None;
    }
}
