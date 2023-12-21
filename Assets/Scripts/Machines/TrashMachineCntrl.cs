using System.Collections.Generic;
using BNG;
using Cups;
using Installers;
using UnityEngine;
using Zenject;

namespace Machines
{
    public class TrashMachineCntrl : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private List<ParticleSystem> ParticleSystems;
    
        private Stack<GameObject> thrownObjects;
        private SignalBus _signalBus;
        private AudioSource _trashAudioSource;

        public void Awake()
        {
            thrownObjects = new Stack<GameObject>();

            _trashAudioSource = GetComponent<AudioSource>();
        }
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnTriggerStay(Collider other)
        {
            var thrownObject = other.gameObject;
            var dozatorCup = thrownObject.GetComponent<DozatorCup>();
            var grabbable = thrownObject.GetComponent<Grabbable>();

            if (dozatorCup is null || !dozatorCup.IsDirty || grabbable.BeingHeld)
            {
                return;
            }

            thrownObjects.Push(thrownObject);
            thrownObject.SetActive(false);

            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }
            _trashAudioSource.Play();
            
            _signalBus.Fire(new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.TrashMachine,
            });
        }

        public void ReturnObject()
        {
            if (thrownObjects.Count == 0)
            {
                return;
            }

            var removeObj = thrownObjects.Pop();
            removeObj.transform.localPosition = new Vector3(_spawnPoint.position.x,_spawnPoint.position.y,_spawnPoint.position.z);
            removeObj.SetActive(true);
        }
    }
}
