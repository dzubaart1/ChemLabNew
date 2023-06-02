using System.Collections.Generic;
using Cups;
using Installers;
using Tasks;
using UnityEngine;
using Zenject;

namespace Machines
{
    public class TrashMachineCntrl : MonoBehaviour
    {
        [SerializeField]
        private Transform _spawnPoint;
    
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
        private void OnCollisionEnter(Collision collision)
        {
            AddObject(collision.gameObject);
        }

        private void AddObject(GameObject gameObj)
        {
            thrownObjects.Push(gameObj); 
            gameObj.SetActive(false);
            if (gameObj.GetComponent<DozatorCup>() is not null && gameObj.GetComponent<DozatorCup>().IsDirty)
            {
                StartMachineWorkSignal startMachineWorkSignal = new StartMachineWorkSignal()
                {
                    MachinesType = MachinesTypes.TrashMachine
                };
                _signalBus.Fire(startMachineWorkSignal);
            }
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
