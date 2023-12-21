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
        private SubstancesCntrl _substancesCntrl;

        private const int MINTOCOMPLITETASK = 2;

        [Inject]
        public void Construct(SignalBus signalBus, SubstancesCntrl substancesCntrl)
        {
            _signalBus = signalBus;
            _substancesCntrl = substancesCntrl;
        }

        public void OnStartWork()
        {
            if (_snapZones.Any(snapZone => snapZone.HeldItem is null || snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>() is null || snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().IsEnable()))
            {
                _signalBus.Fire(new EndGameSignal());
                return;
            }

            StartAnimation();

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
            StopAnimation();

            var snapZone = _snapZones[0];

            var substanceProp = snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>().GetNextSubstance().SubstanceProperty;

            _signalBus.Fire(new MachineWorkSignal(){
                MachinesType = MachinesTypes.CentrifugeMachine, SubstancePropertyBase = substanceProp });
        }

        public void StartAnimation()
        {
            _audioSource.Play();
            _animatedPart.enabled = true;
        }

        public void StopAnimation()
        {
            _audioSource.Stop();
            _animatedPart.enabled = false;
        }
    }
}