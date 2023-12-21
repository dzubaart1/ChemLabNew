using BNG;
using Containers;
using Installers;
using Interfaces;
using Substances;
using UnityEngine;
using Zenject;

namespace Machines
{
    public class StirringMachineCntrl : MonoBehaviour, IMachine
    {
        [SerializeField] private SnapZone _snapZone;

        private bool _isEnter;
        private bool _isStart;

        private SubstancesCntrl _substancesCntrl;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus, SubstancesCntrl substancesCntrl)
        {
            _signalBus = signalBus;
            _substancesCntrl = substancesCntrl;
            _signalBus.Subscribe<LoadSignal>(StopStirringAnimation);
        }

        private void Update()
        {
            if (_snapZone.HeldItem is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().AnchorCntrl is null)
            {
                _isEnter = false;
                return;
            }

            if (!_isEnter)
            {
                OnEnterObject();
            }
        }

        public void OnEnterObject()
        {
            _isEnter = true;

            _signalBus.Fire(new MachineWorkSignal()
            {
                ContainersType = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().ContainerType,
                MachinesType = MachinesTypes.StirringMachine,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().GetNextSubstance()?.SubstanceProperty
            });
        }
        
        public void StartWork()
        {
            if (!_isEnter || _isStart)
            {
                _signalBus.Fire(new EndGameSignal());
                return;
            }

            StartStirringAnimation();
            
            _isStart = true;
            _snapZone.CanRemoveItem = false;

            _signalBus.Fire(new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.StirringMachine,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>().GetNextSubstance().SubstanceProperty
            });
        }

        public void FinishWork()
        {
            if (!_isStart)
            {
                _signalBus.Fire(new EndGameSignal());
                return;
            }

            StopStirringAnimation();
            
            _isStart = false;
            _snapZone.CanRemoveItem = true;

            _substancesCntrl.StirSubstance(_snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>());

            _signalBus.Fire(new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.StirringMachine,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>().GetNextSubstance().SubstanceProperty
            });
        }

        public void StartStirringAnimation()
        {
            _isStart = true;

            gameObject.GetComponent<AudioSource>().Play();
            if (_snapZone.HeldItem is null)
            {
                return;
            }
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().AnchorCntrl.StartAnimate();
        }
        
        public void StopStirringAnimation()
        {
            _isStart = false;

            gameObject.GetComponent<AudioSource>().Stop();
            if (_snapZone.HeldItem is null)
            {
                return;
            }
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().AnchorCntrl.FinishAnimate();
        }
    }
}
