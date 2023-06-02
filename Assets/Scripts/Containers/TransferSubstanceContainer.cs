using System.Collections.Generic;
using BNG;
using Installers;
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
        protected SignalBus _signalBus;
        
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

                var transferSubstanceSignal1 = new TransferSubstanceSignal()
                {
                    From = triggerGameObject.GetComponent<BaseContainer>().ContainerType,
                    To = ContainerType,
                    TranserProperty = CurrentSubstancesList.Peek().SubstanceProperty
                };
                _signalBus.Fire(transferSubstanceSignal1);
                return;
            }
        
            checkAdd = triggerGameObject.GetComponent<TransferSubstanceContainer>().AddSubstance(this);
            if (!checkAdd) return;
            checkRemove = RemoveSubstance(triggerGameObject.GetComponent<TransferSubstanceContainer>().MaxVolume);
            if(!checkRemove) return;
            var transferSubstanceSignal = new TransferSubstanceSignal()
            {
                From = ContainerType,
                To = triggerGameObject.GetComponent<TransferSubstanceContainer>().ContainerType,
                TranserProperty = triggerGameObject.GetComponent<TransferSubstanceContainer>().CurrentSubstancesList.Peek().SubstanceProperty
            };
            _signalBus.Fire(transferSubstanceSignal);
        }
    }
}
