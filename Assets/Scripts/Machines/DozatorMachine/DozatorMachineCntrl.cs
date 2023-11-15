using System;
using System.Globalization;
using Containers;
using Installers;
using UnityEngine;
using Zenject;

namespace Machines.DozatorMachine
{
    public class DozatorMachineCntrl : MonoBehaviour
    {
        [SerializeField] private BaseContainer _baseContainer;
        private SignalBus _signalBus;
        private float _currentDoze;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CheckTasksSignal>(CheckTask);
        }

        private void CheckTask(CheckTasksSignal checkTasksSignal)
        {
            if (checkTasksSignal.CurrentTask.DozatorDoze == 0)
            {
                return;
            }

            _currentDoze = checkTasksSignal.CurrentTask.DozatorDoze;
        }

        public void SetDoze(float volume)
        {
            _baseContainer.MaxVolume = volume;
        }

        public float GetDoze()
        {
            return _baseContainer.MaxVolume;
        }
        public float GetDozeFromTask()
        {
            _baseContainer.MaxVolume = _currentDoze;
            _signalBus.Fire(new MachineWorkSignal(){MachinesType = MachinesTypes.DozatorMachine});
            return _currentDoze;
        }
    }
}