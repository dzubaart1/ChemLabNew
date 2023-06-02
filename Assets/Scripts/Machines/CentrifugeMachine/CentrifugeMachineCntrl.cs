using System.Collections.Generic;
using BNG;
using Containers;
using Installers;
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
        private SignalBus _signalBus;
        private const int MINTOCOMPLITETASK = 2;

        private SubstancesCntrl _substancesCntrl;
        [Inject]
        public void Construct(SignalBus signalBus, SubstancesCntrl substancesCntrl)
        {
            _signalBus = signalBus;
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
                
                var subCont = snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>();
                var substance = subCont.GetNextSubstance();
                var temp = _substancesCntrl.SplitSubstances(substance);
                subCont.UpdateSubstancesArray(temp);
                subCont.UpdateDisplaySubstance();

                countCurrentCentrifugeContainer++;
            }

            if (countCurrentCentrifugeContainer >= MINTOCOMPLITETASK)
            {
                _signalBus.Fire(new StartMachineWorkSignal(){MachinesType = MachinesTypes.CentrifugeMachine});
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
                _signalBus.Fire(new FinishMashineWorkSignal(){MachinesType = MachinesTypes.CentrifugeMachine, SubstancePropertyBase = snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().GetNextSubstance().SubstanceProperty});
            }
        }
    }
}