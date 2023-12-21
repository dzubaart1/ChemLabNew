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
        private bool _isEnter;

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
            _isEnter = true;

            _signalBus.Fire(new MachineWorkSignal()
            {
                ContainersType = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().ContainerType,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().GetNextSubstance().SubstanceProperty,
                MachinesType = MachinesTypes.ScannerMachine
            });
        }
        
        public void OnStartWork()
        {
            if (!_isEnter)
            {
                return;
            }

            ShowResultCanvas();

            _signalBus.Fire(new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.ScannerMachine
            });
        }
        public void OnFinishWork()
        {
            HideResultCanvas();

            _signalBus.Fire(new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.ScannerMachine,
                SubstancePropertyBase = _snapZone.HeldItem?.gameObject.GetComponent<MixContainer>().GetNextSubstance().SubstanceProperty
            });
        }

        public void ShowResultCanvas()
        {
            _scannerCanvas.SetActive(true);
        }

        public void HideResultCanvas()
        {
            _scannerCanvas.SetActive(false);
        }
    }
}

