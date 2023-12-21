using BNG;
using Installers;
using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Machines
{
    public class PaperTrayMachine : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> ParticleSystems;
        
        private SignalBus _signalBus;
        
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
            
            _signalBus.Fire(new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.PaperTray
            });
        }
    }
}