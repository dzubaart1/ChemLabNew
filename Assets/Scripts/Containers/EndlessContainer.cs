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

        public override bool AddSubstance(SubstanceContainer substance)
        {
            return false;
        }

        public override bool RemoveSubstance(float maxVolume)
        {
            if (CurrentSubstancesList.Count == 0 || !IsEnable())
            {
                return false;
            }

            var temp = CurrentSubstancesList.Peek();
            CurrentSubstancesList.Pop();
            var removingRes = _substancesCntrl.RemoveSubstance(temp, maxVolume);
            if (removingRes is not null)
            {
                CurrentSubstancesList.Push(removingRes);
            }
            return true;
        }

        public void CheckTasks(CheckTasksSignal checkTasksSignal)
        {
            if (checkTasksSignal.CurrentTask.SubstancesParams is null) return;
            if (!_substanceParams.SubName.Equals(checkTasksSignal.CurrentTask.SubstancesParams.SubName)) return;
            
            var substance = new Substance(_substanceParams, checkTasksSignal.CurrentTask.Weight);
            if (_substanceParams is SubstancePropertySplit substancePropertySplit)
            {
                var temp = _substancesCntrl.SplitSubstances(substance).ToArray();
                for (int i = temp.Length-1; i >= 0; i--)
                {
                    CurrentSubstancesList.Push(temp[i]);
                }
            }
            else
            {
                CurrentSubstancesList.Push(substance);
            }
            
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
