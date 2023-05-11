#nullable enable
using JetBrains.Annotations;
using Substances;
using Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Containers
{
    public class EndlessContainer : TransferSubstanceContainer
    {
        [SerializeField] private SubstanceParams _substanceParams;
        private TasksCntrl _tasksCntrl;
        
        [Inject]
        public void Construct(TasksCntrl tasksCntrl)
        {
            _tasksCntrl = tasksCntrl;
        }
        private void Start()
        {
            _basePrefab.SetActive(true);
            _basePrefab.GetComponent<MeshRenderer>().material.color = _substanceParams.Color;
            _tasksCntrl.Notify += CheckTasks;
        }

        protected override bool AddSubstance(Substance substance)
        {
            return false;
        }

        protected override bool RemoveSubstance(float maxVolume)
        {
            if (Substance == null && IsEnable())
            {
                return false;
            }
            if (maxVolume >= Substance.Weight)
            {
                Substance = null;
            }
            else
            {
                Substance.RemoveSubstanceWeight(maxVolume);
            }
            return true;
        }

        public void CheckTasks()
        {
            if (_tasksCntrl.CurrentTask().SubstancesParams is null) return;
            if (_substanceParams.SubName.Equals(_tasksCntrl.CurrentTask().SubstancesParams.SubName))
            {
                var substance = new Substance(_substanceParams, _tasksCntrl.CurrentTask().Weight);
                Substance = substance;
            }
        }
    }
}
