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
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<DozatorCup>() is null || !other.gameObject.GetComponent<DozatorCup>().IsDirty || other.gameObject.GetComponent<Grabbable>().BeingHeld)
            {
                return;
            }
            thrownObjects.Push(other.gameObject); 
            other.gameObject.SetActive(false);
            var startMachineWorkSignal = new StartMachineWorkSignal()
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
