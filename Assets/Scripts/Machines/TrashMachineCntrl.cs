using System;
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

        public void Start()
        {
            thrownObjects = new Stack<GameObject>();
        }
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<DozatorCup>() is null || !other.gameObject.GetComponent<DozatorCup>().IsDirty || other.gameObject.GetComponent<Grabbable>().BeingHeld)
            {
                return;
            }
            gameObject.GetComponent<AudioSource>().Play();
            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<DozatorCup>() is null || !other.gameObject.GetComponent<DozatorCup>().IsDirty || other.gameObject.GetComponent<Grabbable>().BeingHeld)
            {
                return;
            }
            thrownObjects.Push(other.gameObject); 
            other.gameObject.SetActive(false);
            var startMachineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.TrashMachine
            };
            _signalBus.Fire(startMachineWorkSignal);
        }

        public void ReturnObject()
        {
            if (thrownObjects.Count == 0 )
                return;
            var removeObj = thrownObjects.Pop();
            removeObj.transform.localPosition = new Vector3(_spawnPoint.position.x,_spawnPoint.position.y,_spawnPoint.position.z);
            removeObj.SetActive(true);
        }
    }
}
