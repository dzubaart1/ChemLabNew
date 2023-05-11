using Tasks;
using UnityEngine;
using Zenject;
using Containers;
using BNG;

namespace Machines
{
    public class ScannerMachineCntrl : MonoBehaviour
    {
        [SerializeField] private GameObject _scannerCanvas;
        [SerializeField] private SnapZone _snapZone;
        private TasksCntrl _tasksCntrl;
        public bool _isEnter;
        private bool _isStart;
        [Inject]
        public void Construct(TasksCntrl tasksCntrl)
        {
            _tasksCntrl = tasksCntrl;
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
            _tasksCntrl.CheckEnteringIntoMachine(MachinesTypes.ScannerMachine,
                _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().ContainerType);
            _isEnter = true;
        }
        
        public void OnStartWork()
        {
            if (!_isEnter ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType != ContainersTypes.PetriContainer)
                return;
            _scannerCanvas.SetActive(true);
            _tasksCntrl.CheckStartMachineWork(MachinesTypes.ScannerMachine);
        }
        public void OnFinishWork()
        {
            _scannerCanvas.SetActive(false);
            _tasksCntrl.CheckStartMachineWork(MachinesTypes.ScannerMachine);
        }
    }
}

