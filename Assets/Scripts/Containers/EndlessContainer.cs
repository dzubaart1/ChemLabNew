using Substances;
using Tasks;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class EndlessContainer : TransferSubstanceContainer
    {
        [SerializeField] private SubstancePropertyBase _substanceParams;
        private TasksCntrl _tasksCntrl;
        
        [Inject]
        public void Construct(TasksCntrl tasksCntrl)
        {
            _tasksCntrl = tasksCntrl;
        }
        private void Start()
        {
            UpdateDisplaySubstance();
            _tasksCntrl.Notify += CheckTasks;
            CheckTasks();
        }

        protected override bool AddSubstance(SubstanceSplit substance)
        {
            return false;
        }

        protected override bool RemoveSubstance(float maxVolume)
        {
            if (CurrentSubstance == null && IsEnable())
            {
                return false;
            }

            CurrentSubstance = _substancesCntrl.RemoveSubstance(CurrentSubstance, maxVolume);
            CheckTasks();
            return true;
        }

        public void CheckTasks()
        {
            if (_tasksCntrl.CurrentTask().SubstancesParams is null) return;
            if (!_substanceParams.SubName.Equals(_tasksCntrl.CurrentTask().SubstancesParams.SubName)) return;
            
            var substance = new SubstanceSplit(_substanceParams, _tasksCntrl.CurrentTask().Weight);
            CurrentSubstance = substance;
        }
    }
}
