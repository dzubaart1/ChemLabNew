using BNG;
using Containers;
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
        [Inject]
        public void Construct(TasksCntrl tasksCntrl, SubstancesCntrl substancesCntrl)
        {
            _tasksCntrl = tasksCntrl;
            _substancesCntrl = substancesCntrl;
        }

        private void Update()
        {
            if (_snapZone.HeldItem is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().Anchor is null)
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
            _tasksCntrl.CheckEnteringIntoMachine(MachinesTypes.StirringMachine,
                _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().ContainerType);
            _isEnter = true;
        }
        
        public void OnStartWork()
        {
            if (!_isEnter || _isStart)
            {
                return;
            }
            _isStart = true;
            var temp = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Peek();
            _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Clear();
            _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Push(_substancesCntrl.StirSubstance(temp));
            _snapZone.HeldItem.gameObject.GetComponent<DisplaySubstance>().UpdateDisplaySubstance();

            StartStirringAnimation();
            _tasksCntrl.CheckStartMachineWork(MachinesTypes.StirringMachine);
        }
        public void OnFinishWork()
        {
            if (!_isStart)
            {
                return;
            }
            _isStart = false;
            StopStirringAnimation();
            Debug.Log("1" + _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Peek().SubstanceProperty.SubName);
            _tasksCntrl.CheckFinishMachineWork(MachinesTypes.StirringMachine, _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Peek().SubstanceProperty);
        }

        private void StartStirringAnimation()
        {
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().Anchor.gameObject.GetComponent<Animator>().enabled = true;
            gameObject.GetComponent<Animator>().enabled = true;
        }
        
        private void StopStirringAnimation()
        {
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().Anchor.gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = false;
        }
    }
}
