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
    //public int MinAngleToMove, MaxAngleToMove;
    public bool DoorIsOpen = false;

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

    public int MinL, MaxL, MinR, MaxR;
    public int MinAngle, MaxAngle;
    public bool IsLeftSide;
    private DoorTypes _handleDTR, _handleDTL = DoorTypes.None;
    
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
    void Update() {
        currentRotation = transform.localEulerAngles;
        angle = Mathf.Floor(currentRotation.y);
        if(angle >= 180) {
            _addAngle = angle - 180;
            MinAngle = MinR;
            MaxAngle = MaxR;
            IsLeftSide = false;
        }
        else {
            _addAngle = 180 - angle;
            MinAngle = MinL;
            MaxAngle = MaxL;
            IsLeftSide = true;
        }
        CheckDoorState();
    }
    
    private void CheckDoorState()
    {
        if (!(_rightGrabber.HeldGrabbable is null) && !(_rightGrabber.HeldGrabbable.gameObject.GetComponent<HandleHelper>() is null) && !(_rightGrabber.HeldGrabbable.gameObject.GetComponent<HandleHelper>().ParentRigid.gameObject
                .GetComponent<DoorState>() is null))
        {
            _handleDTR = _rightGrabber.HeldGrabbable.gameObject.GetComponent<HandleHelper>().ParentRigid.gameObject
                .GetComponent<DoorState>().DoorType;
        }

        if (!(_leftGrabber.HeldGrabbable is null) && !(_leftGrabber.HeldGrabbable.gameObject.GetComponent<HandleHelper>() is null) && !(_leftGrabber.HeldGrabbable.gameObject.GetComponent<HandleHelper>().ParentRigid.gameObject
                .GetComponent<DoorState>() is null))
        {
            _handleDTL = _leftGrabber.HeldGrabbable.gameObject.GetComponent<HandleHelper>().ParentRigid.gameObject
                .GetComponent<DoorState>().DoorType;
        }
        if (!(_rightGrabber.HeldGrabbable is null) && _rightGrabber.HeldGrabbable.gameObject.CompareTag("handle") && _handleDTR == DoorType ||
            !(_leftGrabber.HeldGrabbable is null) && _leftGrabber.HeldGrabbable.gameObject.CompareTag("handle") && _handleDTL == DoorType)
        {
            UnlockTheDoor();
            return;
        }

        if (_addAngle >= MinAngle && _addAngle <= MaxAngle)
        {
            UnlockTheDoor();
        }
        else
        {
            LockTheDoor();
        }
        
    }
    

    private void LockTheDoor()
    {
        
        GetComponent<DoorHelper>().DoorIsLocked = true;
        rigid.constraints = RigidbodyConstraints.FreezeAll;
        
        if (IsLeftSide)
        {
            if (_addAngle <= MinAngle)
            {
                CloseDoor();
            }
            else if (_addAngle >= MaxAngle)
            {
                OpenDoor();
            }
        }
        else
        {
            if (_addAngle >= MinAngle)
            {
                CloseDoor();
            }
            else if (_addAngle <= MaxAngle)
            {
                OpenDoor();
            }
        }
        transform.position = startPos;
    }
    private void UnlockTheDoor()
    {
        DoorIsOpen = true;
        GetComponent<DoorHelper>().DoorIsLocked = false;
        rigid.constraints = RigidbodyConstraints.None;
    }

    private void CloseDoor()
    {
        transform.localEulerAngles = closeRot;
        if (!DoorIsOpen)
            return;
        DoorIsOpen = false;
        _signalBus.Fire(new DoorWorkSignal()
            {
                DoorType = DoorType,
                IsOpen = false
            });
        Debug.Log("DOOR IS " + DoorIsOpen);
    }

    private void OpenDoor()
    {
        transform.localEulerAngles = openRot;
        if (DoorIsOpen)
            return;
        DoorIsOpen = true;
        _signalBus.Fire(new DoorWorkSignal()
            {
                DoorType = DoorType,
                IsOpen = true
            });
        Debug.Log("DOOR IS " + DoorIsOpen);
    }
}
