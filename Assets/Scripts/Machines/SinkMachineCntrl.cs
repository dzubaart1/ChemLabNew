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
            var gameObj = other.gameObject;
            if (gameObj.GetComponent<BaseContainer>() is null || !gameObj.GetComponent<BaseContainer>().IsDirty || gameObj.GetComponent<Grabbable>().BeingHeld)
            {
                return;
            }
            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }
            gameObject.GetComponent<AudioSource>().Play();
        }

        private void OnTriggerStay(Collider other)
        {
            var gameObj = other.gameObject;
            if (gameObj.GetComponent<BaseContainer>() is null || !gameObj.GetComponent<BaseContainer>().IsDirty || gameObj.GetComponent<Grabbable>().BeingHeld)
            {
                return;
            }
            
            thrownObjects.Push(gameObj);
            gameObj.SetActive(false);

            var startMachineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.SinkMachine
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
