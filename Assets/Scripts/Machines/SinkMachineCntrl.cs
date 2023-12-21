using System;
using System.Collections.Generic;
using BNG;
using Containers;
using UnityEngine;
using Zenject;
using Installers;

namespace Machines
{
    public class SinkMachineCntrl : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private List<ParticleSystem> ParticleSystems;
    
        private Stack<GameObject> thrownObjects;
        private SignalBus _signalBus;
        private AudioSource _sinkAudioSource;

        public void Awake()
        {
            thrownObjects = new Stack<GameObject>();

            _sinkAudioSource = GetComponent<AudioSource>();
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnTriggerStay(Collider other)
        {
            var thrownObject = other.gameObject;
            var grabbable = other.gameObject.GetComponent<Grabbable>(); ;
            var baseContainer = other.gameObject.GetComponent<BaseContainer>();

            if (baseContainer is null || grabbable is null || !baseContainer.IsDirty || grabbable.BeingHeld)
            {
                return;
            }

            thrownObject.SetActive(false);
            thrownObjects.Push(thrownObject);

            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }
            _sinkAudioSource.Play();

            _signalBus.Fire(new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.SinkMachine
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
