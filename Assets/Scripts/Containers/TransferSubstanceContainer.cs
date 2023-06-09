using System.Collections.Generic;
using BNG;
using Canvases;
using Installers;
using Substances;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class TransferSubstanceContainer : SubstanceContainer
    {
        private bool _isAgain;
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
            if (CurrentCountSubstances == 0)
            {
                Debug.Log("spoon");
                //если это ложка
                if (ContainerType != ContainersTypes.SpoonContainer && ContainerType != ContainersTypes.DozatorContainer)
                {
                    return;
                }
                if (triggerGameObject.GetComponent<SubstanceContainer>().CurrentCountSubstances == 0)
                {
                    return;
                }
                
                checkAdd = AddSubstance(triggerGameObject.GetComponent<SubstanceContainer>().GetNextSubstance());
                if (!checkAdd) return;
                checkRemove = triggerGameObject.GetComponent<TransferSubstanceContainer>().RemoveSubstance(MaxVolume);
                if(!checkRemove) return;

                var transferSubstanceSignal1 = new TransferSubstanceSignal()
                {
                    From = triggerGameObject.GetComponent<BaseContainer>().ContainerType,
                    To = ContainerType,
                    TranserProperty = GetNextSubstance().SubstanceProperty
                };
                _signalBus.Fire(transferSubstanceSignal1);
                return;
            }
        
            checkAdd = triggerGameObject.GetComponent<TransferSubstanceContainer>().AddSubstance(GetNextSubstance());
            if (!checkAdd) return;
            checkRemove = RemoveSubstance(triggerGameObject.GetComponent<TransferSubstanceContainer>().MaxVolume);
            if(!checkRemove) return;
            var transferSubstanceSignal2 = new TransferSubstanceSignal()
            {
                From = ContainerType,
                To = triggerGameObject.GetComponent<TransferSubstanceContainer>().ContainerType,
                TranserProperty = triggerGameObject.GetComponent<TransferSubstanceContainer>().GetNextSubstance().SubstanceProperty
            };
            _signalBus.Fire(transferSubstanceSignal2);
        }
    }
}
