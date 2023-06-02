using System.Collections.Generic;
using BNG;
using Installers;
using UnityEngine;
using Zenject;

namespace Machines
{
    public class TeleportMachineCntrl : MonoBehaviour
    {
        [SerializeField]
        private SnapZone _snapZone;
        [SerializeField]
        private List<ParticleSystem> ParticleSystems;

        private SignalBus _signalBus;
    
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
    
        public void OnEnter()
        {
            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }
            _snapZone.HeldItem.gameObject.SetActive(false);
            _snapZone.HeldItem = null;
            var startMachineWorkSignal = new StartMachineWorkSignal()
            {
                MachinesType = MachinesTypes.TeleportMachine
            };
            _signalBus.Fire(startMachineWorkSignal);
        } 
    }
}
