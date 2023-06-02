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
        private TasksCntrl _tasksCntrl;
        private const int MINTOCOMPLITETASK = 2;

        private SubstancesCntrl _substancesCntrl;
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus, SubstancesCntrl substancesCntrl)
        {
            _signalBus = signalBus;
            _substancesCntrl = substancesCntrl;
        }

        public void OnEnterObject()
        {
           //throw new System.NotImplementedException();
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
                StartMachineWorkSignal startMachineWorkSignal = new StartMachineWorkSignal()
                {
                    MachinesType = MachinesTypes.CentrifugeMachine
                };
                _signalBus.Fire(startMachineWorkSignal);
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

                FinishMashineWorkSignal finishMashineWorkSignal = new FinishMashineWorkSignal()
                {
                    MachinesType = MachinesTypes.CentrifugeMachine,
                    SubstancePropertyBase = snapZone.HeldItem.gameObject.GetComponent<CentrifugeContainer>()
                        .CurrentSubstancesList.Peek().SubstanceProperty
                };
                _signalBus.Fire(finishMashineWorkSignal);
            }
        }
    }
}