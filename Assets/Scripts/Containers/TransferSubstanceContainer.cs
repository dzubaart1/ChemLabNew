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

        [Inject]
        public void Construct(List<Grabber> grabbers, TasksCntrl tasksCntrl)
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
                //если это ложка
                if (ContainerType != ContainersTypes.SpoonContainer && ContainerType != ContainersTypes.DozatorContainer)
                {
                    return;
                }
                if (triggerGameObject.GetComponent<BaseContainer>().Substance is null)
                { 
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
