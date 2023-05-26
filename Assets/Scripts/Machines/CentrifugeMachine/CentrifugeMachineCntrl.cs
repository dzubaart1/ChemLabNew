using System.Collections.Generic;
using BNG;
using Containers;
using Interfaces;
using Substances;
using Tasks;
using UnityEngine;
using Zenject;

namespace Machines.CentrifugeMachine
{
    public class CentrifugeMachineCntrl : MonoBehaviour, IMachine
    {
        [SerializeField]
        private List<SnapZone> _SnapZones;
        private TasksCntrl _tasksCntrl;
        private const int MINTOCOMPLITETASK = 2;

        private SubstancesCntrl _substancesCntrl;
        [Inject]
        public void Construct(TasksCntrl tasksCntrl, SubstancesCntrl substancesCntrl)
        {
            _tasksCntrl = tasksCntrl;
            _substancesCntrl = substancesCntrl;
        }

        public void OnEnterObject()
        {
            throw new System.NotImplementedException();
        }

        public void OnStartWork()
        {
            int countCurrentCentrifugeContainer = 0;
            foreach (var snapZone in _SnapZones)
            {
                if (snapZone.HeldItem is null)
                {
                    continue;
                }
                if (snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>() is null)
                {
                    continue;
                }
                
                //add split substance
                var substance = snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Pop();
                
                var temp = _substancesCntrl.SplitSubstances(substance).ToArray();
                for (int i = temp.Length-1; i >= 0; i--)
                {
                    snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentSubstancesList.Push(temp[i]);
                }
                snapZone.HeldItem.gameObject.GetComponent<DisplaySubstance>().UpdateDisplaySubstance();
                // end
                countCurrentCentrifugeContainer++;
            }

            if (countCurrentCentrifugeContainer >= MINTOCOMPLITETASK)
            {
                _tasksCntrl.CheckStartMachineWork(MachinesTypes.CentrifugeMachine);
            }
        }

        public void OnFinishWork()
        {
            foreach (var snapZone in _SnapZones)
            {
                if (snapZone.HeldItem is null)
                {
                    continue;
                }
                if (snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>() is null)
                {
                    continue;
                }
                _tasksCntrl.CheckFinishMachineWork(MachinesTypes.CentrifugeMachine, snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().CurrentSubstancesList.Peek().SubstanceProperty);
            }
        }
    }
}