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
        private bool _isEnter;
        private bool _isStart;

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
            var enterIntoMachineSignal = new MachineWorkSignal()
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
            gameObject.GetComponent<AudioSource>().Play();
            var startMachineWorkSignal = new MachineWorkSignal()
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
            gameObject.GetComponent<AudioSource>().Stop();
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
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().gameObject.GetComponentInChildren<Animator>().enabled = true;
            //gameObject.GetComponent<Animator>().enabled = true;
        }
        
        private void StopStirringAnimation()
        {
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().AnchorCntrl.FinishAnimate();
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().gameObject.GetComponentInChildren<Animator>().enabled = false;
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().gameObject.GetComponentInChildren<Animator>()
                .gameObject.transform.localPosition = new Vector3(0, 0, 0);
            //gameObject.GetComponent<Animator>().enabled = false;
        }
    }
}
