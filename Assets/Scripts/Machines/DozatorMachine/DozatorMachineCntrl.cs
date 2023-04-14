using System.Globalization;
using Containers;
using Tasks;
using UnityEngine;
using Zenject;

namespace Machines.DozatorMachine
{
    public class DozatorMachineCntrl : MonoBehaviour
    {
        [SerializeField] private BaseContainer _baseContainer;
        private TasksCntrl _tasksCntrl;
        [Inject]
        public void Construct(TasksCntrl tasksCntrl)
        {
            _tasksCntrl = tasksCntrl;
        }

        public string GetDoze()
        {
            if (_tasksCntrl.CurrentTask().DozatorDoze == 0)
            {
                return "0.00";
            }

            _baseContainer.MaxVolume = _tasksCntrl.CurrentTask().DozatorDoze;
            var text = _tasksCntrl.CurrentTask().DozatorDoze.ToString("0.00", CultureInfo.InvariantCulture);
            _tasksCntrl.CheckStartMachineWork(MachinesTypes.DozatorMachine);
            return text;
        }
    }
}