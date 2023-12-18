using UnityEngine;
using Zenject;
using Containers;
using BNG;
using Installers;

namespace Machines
{
    public class ScannerMachineCntrl : MonoBehaviour
    {
        [SerializeField] private GameObject _scannerCanvas;
        [SerializeField] private SnapZone _snapZone;
        private SignalBus _signalBus;
        public bool _isEnter;
        private bool _isStart;
        public bool _buttonIsOn;
        [Inject]
        public void Construct(SignalBus signalBus)
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
            MachineWorkSignal enterIntoMachineSignal = new MachineWorkSignal()
            {
                ContainersType = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().ContainerType,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().GetNextSubstance().SubstanceProperty,
                MachinesType = MachinesTypes.ScannerMachine
            };
            _signalBus.Fire(enterIntoMachineSignal);
            _isEnter = true;
        }
        
        public void OnStartWork()
        {
            if (!_buttonIsOn || _snapZone.HeldItem is null)
            {
                return;
            }
            MachineWorkSignal startMachineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.ScannerMachine
            }; 
            _signalBus.Fire(startMachineWorkSignal);
            
            if (!_isEnter ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType != ContainersTypes.PetriContainer)
                return;
            _scannerCanvas.SetActive(true);
        }
        public void OnFinishWork()
        {
            MachineWorkSignal finishMashineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.ScannerMachine,
                SubstancePropertyBase = _snapZone.HeldItem?.gameObject.GetComponent<MixContainer>().GetNextSubstance().SubstanceProperty
            };
            _signalBus.Fire(finishMashineWorkSignal);
            _scannerCanvas.SetActive(false);
        }

        
    }
}

