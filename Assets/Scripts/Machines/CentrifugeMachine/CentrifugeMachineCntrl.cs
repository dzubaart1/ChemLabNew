using System.Collections.Generic;
using BNG;
using Containers;
using Installers;
using Substances;
using UnityEngine;
using Zenject;

namespace Machines.CentrifugeMachine
{
    public class CentrifugeMachineCntrl : MonoBehaviour
    {
        [SerializeField] private List<SnapZone> _snapZones;
        private SignalBus _signalBus;
        private const int MINTOCOMPLITETASK = 2;

        private SubstancesCntrl _substancesCntrl;

        [Inject]
        public void Construct(SignalBus signalBus, SubstancesCntrl substancesCntrl)
        {
            _signalBus = signalBus;
            _substancesCntrl = substancesCntrl;
        }

        public void OnStartWork()
        {
            int countCurrentCentrifugeContainer = 0;
            foreach (var snapZone in _snapZones)
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
                _substancesCntrl.SplitSubstances(subCont);

                countCurrentCentrifugeContainer++;
            }

            if (countCurrentCentrifugeContainer >= MINTOCOMPLITETASK)
            {
                _signalBus.Fire(new MachineWorkSignal() { MachinesType = MachinesTypes.CentrifugeMachine });
            }
        }

        public void OnFinishWork()
        {
            var index = 0;
            for (; index < _snapZones.Count; index++)
            {
                var snapZone = _snapZones[index];
                if (snapZone.HeldItem is null)
                {
                    continue;
                }

                if (snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>() is null)
                {
                    continue;
                }

                var substanceProp = snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().GetNextSubstance()
                    .SubstanceProperty;
                _signalBus.Fire(new MachineWorkSignal()
                    { MachinesType = MachinesTypes.CentrifugeMachine, SubstancePropertyBase = substanceProp });
                return;
            }
        }
    }
}