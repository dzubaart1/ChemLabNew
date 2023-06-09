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
        }

        private void Start()
        {
            _signalBus.Subscribe<CheckTasksSignal>(CheckTask);
        }

        public void CheckTask(CheckTasksSignal checkTasksSignal)
        {
            if (checkTasksSignal.CurrentTask.DozatorDoze == 0)
            {
                return;
            }

            _currentDoze = checkTasksSignal.CurrentTask.DozatorDoze;
        }
        public string GetDoze()
        {
            _baseContainer.MaxVolume = _currentDoze;
            _signalBus.Fire(new StartMachineWorkSignal(){MachinesType = MachinesTypes.DozatorMachine});
            return _currentDoze.ToString(CultureInfo.InvariantCulture);
        }
    }
}