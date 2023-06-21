using BNG;
using Containers;
using Installers;
using Interfaces;
using Substances;
using Tasks;
using UnityEngine;
using Zenject;

namespace Machines
{
    public class StirringMachineCntrl : MonoBehaviour, IMachine
    {
        [SerializeField] private SnapZone _snapZone;
        private TasksCntrl _tasksCntrl;
        public bool _isEnter;
        public bool _isStart;

        private SubstancesCntrl _substancesCntrl;
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus, SubstancesCntrl substancesCntrl)
        {
            _signalBus = signalBus;
            _substancesCntrl = substancesCntrl;
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
            var enterIntoMachineSignal = new EnterIntoMachineSignal()
            {
                ContainersType = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().ContainerType,
                MachinesType = MachinesTypes.StirringMachine
            };
            _signalBus.Fire(enterIntoMachineSignal);
            _isEnter = true;
        }
        
        public void OnStartWork()
        {
            if (!_isEnter || _isStart)
            {
                return;
            }
            _isStart = true;
            
            var temp = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>();
            _substancesCntrl.StirSubstance(temp);
            StartStirringAnimation();
            var startMachineWorkSignal = new StartMachineWorkSignal()
            {
                MachinesType = MachinesTypes.StirringMachine
            };
            _signalBus.Fire(startMachineWorkSignal);
        }
        public void OnFinishWork()
        {
            if (!_isStart)
            {
                return;
            }
            _isStart = false;
            StopStirringAnimation();
            var finishMashineWorkSignal = new FinishMashineWorkSignal()
            {
                MachinesType = MachinesTypes.StirringMachine,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>().GetNextSubstance().SubstanceProperty
            };
            _signalBus.Fire(finishMashineWorkSignal);
        }

        private void StartStirringAnimation()
        {
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().AnchorCntrl.gameObject.GetComponent<Animator>().enabled = true;
            gameObject.GetComponent<Animator>().enabled = true;
        }
        
        private void StopStirringAnimation()
        {
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().AnchorCntrl.gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = false;
        }
    }
}
