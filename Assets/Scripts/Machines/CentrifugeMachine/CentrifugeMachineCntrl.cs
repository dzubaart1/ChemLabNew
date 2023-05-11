using System.Collections.Generic;
using System.Runtime.InteropServices;
using BNG;
using Containers;
using Interfaces;
using Tasks;
using UnityEngine;
using Zenject;

namespace Machines.CentrifugeMachine
{
    public class CentrifugeMachineCntrl : IMachine
    {
        [SerializeField]
        private List<SnapZone> SnapZones;
        private TasksCntrl _tasksCntrl;
        private const int MINTOCOMPLITETASK = 2;

        [Inject]
        public void Construct(TasksCntrl tasksCntrl)
        {
            _tasksCntrl = tasksCntrl;
        }

        public void OnEnterObject()
        {
            throw new System.NotImplementedException();
        }

        public void OnStartWork()
        {
            int countCurrentCentrifugeContainer = 0;
            // анимация
            foreach (var snapZone in SnapZones)
            {
                if (snapZone.HeldItem is null)
                {
                    continue;
                }
                if (snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>() is null)
                {
                    continue;
                }
                snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().ShowSediment();
                countCurrentCentrifugeContainer++;
            }

            if (countCurrentCentrifugeContainer >= MINTOCOMPLITETASK)
            {
                _tasksCntrl.CheckStartMachineWork(MachinesTypes.CentrifugeMachine);
            }
        }

        public void OnFinishWork()
        {
            foreach (var snapZone in SnapZones)
            {
                if (snapZone.HeldItem is null)
                {
                    continue;
                }
                if (snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>() is null)
                {
                    continue;
                }
                _tasksCntrl.CheckFinishMachineWork(MachinesTypes.CentrifugeMachine, snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().Substance);
            }
        }
    }
}