using System.Collections.Generic;
using BNG;
using Substances;
using Tasks;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class TransferSubstanceContainer : DisplaySubstance
    {
        private bool _isAgain;
        private Grabber _leftGrabber, _rightGrabber;
        protected TasksCntrl _tasksCntrl;
        protected SubstancesCntrl _substancesCntrl;

        [Inject]
        public void Construct(List<Grabber> grabbers, TasksCntrl tasksCntrl, SubstancesCntrl substancesCntrl)
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
            _substancesCntrl = substancesCntrl;
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
            if (CurrentSubstance is null)
            {
                Debug.Log("spoon");
                //если это ложка
                if (ContainerType != ContainersTypes.SpoonContainer && ContainerType != ContainersTypes.DozatorContainer)
                {
                    return;
                }
                if (triggerGameObject.GetComponent<BaseContainer>().CurrentSubstance is null)
                {
                    return;
                }
                checkAdd = AddSubstance(triggerGameObject.GetComponent<BaseContainer>().CurrentSubstance);
                if (!checkAdd) return;
                checkRemove = triggerGameObject.GetComponent<TransferSubstanceContainer>().RemoveSubstance(MaxVolume);
                if(!checkRemove) return;
                
                _tasksCntrl.CheckTransferSubstance(this, triggerGameObject.GetComponent<BaseContainer>(), CurrentSubstance);
                return;
            }
        
            checkAdd = triggerGameObject.GetComponent<TransferSubstanceContainer>().AddSubstance(CurrentSubstance);
            if (!checkAdd) return;
            checkRemove = RemoveSubstance(triggerGameObject.GetComponent<BaseContainer>().MaxVolume);
            if(!checkRemove) return;
            
            _tasksCntrl.CheckTransferSubstance(GetComponent<BaseContainer>(), triggerGameObject.GetComponent<BaseContainer>(), triggerGameObject.GetComponent<BaseContainer>().CurrentSubstance);
        }
        
        protected virtual bool AddSubstance(SubstanceSplit substance)
        {
            if (substance?.SubstanceProperty is null)
            {
                return false;
            }
            CurrentSubstance = _substancesCntrl.AddSubstance(CurrentSubstance, MaxVolume);
            UpdateDisplaySubstance();
            return true;
        }

        protected virtual bool RemoveSubstance(float volumeToRemove)
        {
            if (CurrentSubstance?.SubstanceProperty is null)
            {
                return false;
            }
            
            CurrentSubstance = _substancesCntrl.RemoveSubstance(CurrentSubstance, volumeToRemove);
            UpdateDisplaySubstance();
            return true;
        }
    }
}
