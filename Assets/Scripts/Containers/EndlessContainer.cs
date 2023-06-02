using Installers;
using Substances;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class EndlessContainer : TransferSubstanceContainer
    {
        [SerializeField] private SubstancePropertyBase _substanceParams;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Start()
        {
            _signalBus.Subscribe<CheckTasksSignal>(CheckTasks);
            UpdateDisplaySubstance();
        }

        public override bool AddSubstance(Substance substance)
        {
            return false;
        }

        public override bool RemoveSubstance(float maxVolume)
        {
            if (CurrentCountSubstances == 0 || !IsEnable())
            {
                return false;
            }
            
            _substancesCntrl.RemoveSubstance(this, maxVolume);
            return true;
        }

        public void CheckTasks(CheckTasksSignal checkTasksSignal)
        {
            if (checkTasksSignal.CurrentTask.SubstancesParams is null ||
                !_substanceParams.SubName.Equals(checkTasksSignal.CurrentTask.SubstancesParams.SubName))
            {
                return;
            }

            var substance = new Substance(_substanceParams, checkTasksSignal.CurrentTask.Weight);
            if (_substanceParams is SubstancePropertySplit)
            {
                var temp = _substancesCntrl.SplitSubstances(substance);
                UpdateSubstancesArray(temp);
            }
            AddSubstanceToArray(substance);
            UpdateDisplaySubstance();
        }
        
        protected override bool IsEnable()
        {
            if (_snapZone)
            {
                return _snapZone.HeldItem is null;
            }
            return true;
        }
    }
}
