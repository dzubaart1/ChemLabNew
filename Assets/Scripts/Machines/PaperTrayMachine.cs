using BNG;
using Installers;
using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Machines
{
    public class PaperTrayMachine : MonoBehaviour
    {
        private SignalBus _signalBus;
        [SerializeField] private List<ParticleSystem> ParticleSystems;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void OnTriggerStay(Collider other)
        {
            if (!other.transform.tag.Equals("Document") || other.gameObject.GetComponent<Grabbable>().BeingHeld)
            {
                return;
            }
            other.gameObject.SetActive(false);
            
            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }
            gameObject.GetComponent<AudioSource>().Play();
            
            var startMachineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.PaperTray
            };
            _signalBus.Fire(startMachineWorkSignal);
        }
    }
}