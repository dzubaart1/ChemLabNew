using System.Collections.Generic;
using Containers;
using Tasks;
using UnityEngine;
using Zenject;
using Cups;
using BNG;
using Installers;
using Substances;

namespace Machines
{
    public class ExpTabletMachineCntrl: MonoBehaviour
    {
        public List<ExpTabletLunkaContainer> _lunkaContainers;
        
        [SerializeField]
        protected BaseCup _cup;
        [SerializeField]
        protected SnapZone _snapZone;

        private SignalBus _signalBus;
        private TaskParams _currentTaskParams;

        public void Awake()
        {
            _snapZone.OnlyAllowNames.Clear();
            _snapZone.OnlyAllowNames.Add(_cup.name);
            
            _signalBus.Subscribe<CheckTasksSignal>(CheckTasks);
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void CheckCompliteFill()
        {
            foreach (var container in _lunkaContainers)
            {
                if (container.GetComponent<SubstanceContainer>().CurrentCountSubstances == 0)
                {
                    return;
                }
                if (!container.GetComponent<SubstanceContainer>().GetNextSubstance().SubstanceProperty.SubName
                        .Equals(_currentTaskParams.ResultSubstance.SubName))
                {
                    return;
                }
            }
            _signalBus.Fire(new StartMachineWorkSignal(){MachinesType =  MachinesTypes.ExpTabletMachine});
        }

        public void CheckTasks(CheckTasksSignal checkTasksSignal)
        {
            _currentTaskParams = checkTasksSignal.CurrentTask;
        }
        public virtual bool IsEnable()
        {
            return _snapZone.HeldItem is null;
        }
    }
}