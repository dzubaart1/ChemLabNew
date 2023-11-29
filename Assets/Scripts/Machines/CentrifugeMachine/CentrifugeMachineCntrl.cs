using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Animator _animatedPart;
        [SerializeField] private AudioSource _audioSource;
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
            if (_snapZones.Any(snapZone => snapZone.HeldItem is null || snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>() is null || !snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().IsEnable()))
            {
                _signalBus.Fire(new MachineWorkSignal() { MachinesType = MachinesTypes.CentrifugeMachine });
                return;
            }
            
            _audioSource.Play();
            _animatedPart.enabled = true;
            
            int countCurrentCentrifugeContainer = 0;
            foreach (var snapZone in _snapZones)
            {
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
            _audioSource.Stop();
            _animatedPart.enabled = false;
            
            for (var index = 0; index < _snapZones.Count; index++)
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