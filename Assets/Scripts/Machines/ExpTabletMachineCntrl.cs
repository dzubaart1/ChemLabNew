using System.Collections.Generic;
using Containers;
using Tasks;
using UnityEngine;
using Zenject;
using Cups;
using BNG;

namespace Machines
{
    public class ExpTabletMachineCntrl: MonoBehaviour
    {
        public List<ExpTabletLunkaContainer> _lunkaContainers;

        private TasksCntrl _tasksCntrl;
        [SerializeField]
        protected BaseCup _cup;
        [SerializeField]
        protected SnapZone _snapZone;

        public void Awake()
        {
            _snapZone.OnlyAllowNames.Clear();
            _snapZone.OnlyAllowNames.Add(_cup.name);
        }

        [Inject]
        public void Construct(TasksCntrl tasksCntrl)
        {
            _tasksCntrl = tasksCntrl;
        }

        public void CheckCompliteFill()
        {
            foreach (var container in _lunkaContainers)
            {
                
                if (container.GetComponent<BaseContainer>().Substance is null)
                {
                    return;
                }
                if (!container.GetComponent<BaseContainer>().Substance.SubParams.SubName
                        .Equals(_tasksCntrl.CurrentTask().ResultSubstance.SubName))
                {
                    return;
                }
            }
            _tasksCntrl.CheckStartMachineWork(MachinesTypes.ExpTabletMachine);
        }
        public virtual bool IsEnable()
        {
            return _snapZone.HeldItem is null;
        }
    }
}