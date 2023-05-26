using Tasks;
using UnityEngine;
using Zenject;
using Containers;
using BNG;
using Substances;

namespace Machines
{
    public class DryBoxCntrl : MonoBehaviour
    {
        [SerializeField] private SnapZone _snapZone;
        private TasksCntrl _tasksCntrl;
        private bool _isEnter;
        private bool _isStart;

        private SubstancesCntrl _substancesCntrl;
        [Inject]
        public void Construct(TasksCntrl tasksCntrl, SubstancesCntrl substancesCntrl)
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
            _tasksCntrl.CheckEnteringIntoMachine(MachinesTypes.DryBoxMachine,
                _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().ContainerType);
            _isEnter = true;
        }
        
        public void OnStartWork()
        {
            if (!_isEnter ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType != ContainersTypes.PetriContainer)
                return;
            
            var temp = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Peek();
            _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Clear();
            _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Push(_substancesCntrl.DrySubstance(temp));
            _snapZone.HeldItem.gameObject.GetComponent<DisplaySubstance>().UpdateDisplaySubstance();
            _tasksCntrl.CheckStartMachineWork(MachinesTypes.DryBoxMachine);
        }
        public void OnFinishWork()
        {
            _tasksCntrl.CheckFinishMachineWork(MachinesTypes.DryBoxMachine, _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Peek().SubstanceProperty);
        }
    }
}


