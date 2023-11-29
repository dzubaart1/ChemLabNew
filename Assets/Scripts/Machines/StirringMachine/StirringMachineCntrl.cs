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
            var enterIntoMachineSignal = new MachineWorkSignal()
            {
                ContainersType = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().ContainerType,
                MachinesType = MachinesTypes.StirringMachine,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().GetNextSubstance()?.SubstanceProperty
            };
            _signalBus.Fire(enterIntoMachineSignal);
            _isEnter = true;
        }
        
        public void OnStartWork()
        {
            if (!_isEnter || _isStart)
            {
                var errorMachineWorkSignal = new MachineWorkSignal()
                {
                    MachinesType = MachinesTypes.StirringMachine,
                };
                _signalBus.Fire(errorMachineWorkSignal);
                return;
            }
            StartStirringAnimation();
            
            _isStart = true;

            _snapZone.HeldItem.gameObject.GetComponent<Grabbable>().enabled = false;
            var startMachineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.StirringMachine,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>().GetNextSubstance().SubstanceProperty
            };
            _signalBus.Fire(startMachineWorkSignal);
            
            var temp = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>();
            _substancesCntrl.StirSubstance(temp);

        }
        public void OnFinishWork()
        {
            if (!_isStart)
            {
                return;
            }
            StopStirringAnimation();
            
            _isStart = false;
            
            _snapZone.HeldItem.gameObject.GetComponent<Grabbable>().enabled = true;
            var finishMashineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.StirringMachine,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>().GetNextSubstance().SubstanceProperty
            };
            _signalBus.Fire(finishMashineWorkSignal);
        }

        private void StartStirringAnimation()
        {
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().AnchorCntrl.StartAnimate();
            gameObject.GetComponent<AudioSource>().Play();
        }
        
        private void StopStirringAnimation()
        {
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().AnchorCntrl.FinishAnimate();
            gameObject.GetComponent<AudioSource>().Stop();
        }
    }
}
