using System;
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
        [SerializeField]
        private SnapZone _snapZone;

        private TasksCntrl _tasksCntrl;
        public bool _isEnter;
        public bool _isStart;
        [Inject]
        public void Construct(TasksCntrl tasksCntrl)
        {
            _tasksCntrl = tasksCntrl;
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
            Debug.Log( "Start " +_snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().Substance.SubParams.SubName);
            _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().StirringSubstance();
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
            _tasksCntrl.CheckFinishMachineWork(MachinesTypes.StirringMachine, _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().Substance);
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
