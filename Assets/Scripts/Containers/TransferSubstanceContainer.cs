using System;
using System.Collections.Generic;
using BNG;
using Tasks;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class TransferSubstanceContainer : BaseContainer
    {
        private bool _isAgain;
        private Grabber _leftGrabber, _rightGrabber;
        private TasksCntrl _tasksCntrl;
        private GameObject _player;
        private bool _canvasState = false;
        Vector3 newDirection;

        [Inject]
        public void Construct(List<Grabber> grabbers, TasksCntrl tasksCntrl, GameObject rigInst)
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
            _tasksCntrl = tasksCntrl;
            _player = rigInst;
        }
        private void Update()
        {
            if (IsNull(GetComponent<BaseContainer>().HiddenCanvas))
            {
                return;
            }
            GameObject _hiddenCanvas = GetComponent<BaseContainer>().HiddenCanvas;
            Vector3 offset = new Vector3(90, 0, 90);
            newDirection = Vector3.RotateTowards(_hiddenCanvas.transform.forward, _player.transform.position - _hiddenCanvas.transform.position,0.01f,5);
            _hiddenCanvas.transform.rotation = Quaternion.LookRotation(newDirection + offset);
            //_hiddenCanvas.transform.position += _hiddenCanvas.transform.forward * 0.01f;
            if (_rightGrabber.HeldGrabbable is null || _rightGrabber.HeldGrabbable.gameObject != gameObject)
            {
                _canvasState = false;
                _hiddenCanvas.SetActive(_canvasState); 
                return;
            }
            if (!InputBridge.Instance.AButtonDown)
            {
                return;
            }
            _canvasState = !_canvasState;
            _hiddenCanvas.SetActive(_canvasState);
        }

        
        
        private void OnTriggerStay(Collider other)
        {
            if (_isAgain)
            {
                return;
            }

            if (!other.GetComponent<TransferSubstanceContainer>())
            {
                return;
            }
            
            if (!InputBridge.Instance.RightTriggerDown && !InputBridge.Instance.LeftTriggerDown)
            {
                return;
            }

            var grabber = InputBridge.Instance.RightTriggerDown ? _rightGrabber : _leftGrabber;

            if (grabber.HeldGrabbable is null || grabber.HeldGrabbable.gameObject != gameObject)
            {
                return;
            }
            Transfer(other.gameObject);
            _isAgain = true;
        }

        private void OnTriggerExit(Collider other)
        {
            _isAgain = false;
        }

        private void Transfer(GameObject triggerGameObject)
        {
            bool temp;
            if (Substance is null)
            {
                Debug.Log("spoon");
                //если это ложка
                if (ContainerType != ContainersTypes.SpoonContainer && ContainerType != ContainersTypes.DozatorContainer)
                {
                    return;
                }
                if (triggerGameObject.GetComponent<BaseContainer>().Substance is null)
                { 
                    Debug.Log("2");
                    return;
                }
                temp = AddSubstance(triggerGameObject.GetComponent<BaseContainer>().Substance);
                if (!temp) return;
                triggerGameObject.GetComponent<TransferSubstanceContainer>().RemoveSubstance(MaxVolume);
                _tasksCntrl.CheckTransferSubstance(this, triggerGameObject.GetComponent<BaseContainer>(), Substance);
                return;
            }
        
            temp = triggerGameObject.GetComponent<TransferSubstanceContainer>().AddSubstance(Substance);
            if (!temp) return;
            RemoveSubstance(triggerGameObject.GetComponent<BaseContainer>().MaxVolume);
            _tasksCntrl.CheckTransferSubstance(GetComponent<BaseContainer>(), triggerGameObject.GetComponent<BaseContainer>(), triggerGameObject.GetComponent<BaseContainer>().Substance);
        }
    }
}
