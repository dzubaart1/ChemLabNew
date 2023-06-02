using UnityEngine;
using Zenject;
using Containers;
using BNG;
using Installers;
using Substances;

namespace Machines
{
    public class DryBoxCntrl : MonoBehaviour
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
        }
        
        private void Update()
        {
            if (_snapZone.HeldItem is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType != ContainersTypes.PetriContainer)
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
            _signalBus.Fire(new EnterIntoMachineSignal(){ContainersType = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().ContainerType, MachinesType = MachinesTypes.DryBoxMachine});
            _isEnter = true;
        }
        
        public void OnStartWork()
        {
            if (!_isEnter ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType != ContainersTypes.PetriContainer)
                return;

            var temp = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>();
            _substancesCntrl.DrySubstance(temp);
            temp.UpdateDisplaySubstance();
            
            StartMachineWorkSignal startMachineWorkSignal = new StartMachineWorkSignal()
            {
                MachinesType = MachinesTypes.DryBoxMachine
            };
            _signalBus.Fire(startMachineWorkSignal);
        }
        public void OnFinishWork()
        {
            /*FinishMashineWorkSignal finishMashineWorkSignal = new FinishMashineWorkSignal()
            {
                MachinesType = MachinesTypes.DryBoxMachine,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>()
                    .CurrentSubstancesList.Peek().SubstanceProperty
            };
            _signalBus.Fire(finishMashineWorkSignal); */
        }
    }
}


