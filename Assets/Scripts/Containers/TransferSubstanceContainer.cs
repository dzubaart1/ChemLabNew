using System.Collections.Generic;
using BNG;
using Substances;
using Tasks;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class TransferSubstanceContainer : SubstanceContainer
    {
        private bool _isAgain;
        private Grabber _leftGrabber, _rightGrabber;
        protected TasksCntrl _tasksCntrl;
        
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
            bool checkAdd, checkRemove;
            if (CurrentSubstancesList.Count == 0)
            {
                Debug.Log("spoon");
                //если это ложка
                if (ContainerType != ContainersTypes.SpoonContainer && ContainerType != ContainersTypes.DozatorContainer)
                {
                    return;
                }
                if (triggerGameObject.GetComponent<SubstanceContainer>().CurrentSubstancesList.Count == 0)
                {
                    return;
                }
                
                checkAdd = AddSubstance(triggerGameObject.GetComponent<SubstanceContainer>());
                if (!checkAdd) return;
                checkRemove = triggerGameObject.GetComponent<TransferSubstanceContainer>().RemoveSubstance(MaxVolume);
                if(!checkRemove) return;
                
                _tasksCntrl.CheckTransferSubstance(this, triggerGameObject.GetComponent<BaseContainer>(), CurrentSubstancesList.Peek().SubstanceProperty);
                return;
            }
        
            checkAdd = triggerGameObject.GetComponent<TransferSubstanceContainer>().AddSubstance(this);
            if (!checkAdd) return;
            checkRemove = RemoveSubstance(triggerGameObject.GetComponent<TransferSubstanceContainer>().MaxVolume);
            if(!checkRemove) return;
            
            _tasksCntrl.CheckTransferSubstance(GetComponent<BaseContainer>(), triggerGameObject.GetComponent<BaseContainer>(), triggerGameObject.GetComponent<SubstanceContainer>().CurrentSubstancesList.Peek().SubstanceProperty);
        }
    }
}
