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
        //[SerializeField] private VideoPlayer _videoPlayer;
        private SignalBus _signalBus;
        public bool _isEnter;
        private bool _isStart;
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
                MachinesType = MachinesTypes.ScannerMachine
            };
            _signalBus.Fire(enterIntoMachineSignal);
            _isEnter = true;
        }
        
        public void OnStartWork()
        {
            if (!_isEnter ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType != ContainersTypes.PetriContainer)
                return;
            _scannerCanvas.SetActive(true);
            //_videoPlayer.Play();
            MachineWorkSignal startMachineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.ScannerMachine
            }; 
            _signalBus.Fire(startMachineWorkSignal);
        }
        public void OnFinishWork()
        {
            _scannerCanvas.SetActive(false);
            //_videoPlayer.Stop();
            MachineWorkSignal finishMashineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.ScannerMachine,
                SubstancePropertyBase = _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().GetNextSubstance().SubstanceProperty
            };
            _signalBus.Fire(finishMashineWorkSignal);
        }
    }
}

