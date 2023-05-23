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
        private List<SnapZone> SnapZones;
        private TasksCntrl _tasksCntrl;
        private const int MINTOCOMPLITETASK = 2;
        [SerializeField] private GameObject animatedPart;

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
            animatedPart.GetComponent<Animator>().enabled = true;
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
                snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().CurrentSubstance = _substancesCntrl.SplitSubstances(snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().CurrentSubstance);
                countCurrentCentrifugeContainer++;
            }

            if (countCurrentCentrifugeContainer >= MINTOCOMPLITETASK)
            {
                _tasksCntrl.CheckStartMachineWork(MachinesTypes.CentrifugeMachine);
            }
        }

        public void OnFinishWork()
        {
            animatedPart.GetComponent<Animator>().enabled = false;
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
                _tasksCntrl.CheckFinishMachineWork(MachinesTypes.CentrifugeMachine, snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().CurrentSubstance);
            }
        }
    }
}