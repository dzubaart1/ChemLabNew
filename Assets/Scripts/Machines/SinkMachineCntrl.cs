using System.Collections.Generic;
using Containers;
using UnityEngine;
using Zenject;
using Installers;

namespace Machines
{
    public class SinkMachineCntrl : MonoBehaviour
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
            var gameObj = collision.gameObject;
            Debug.Log(gameObj.GetComponent<BaseContainer>().IsDirty);
            if (gameObj.GetComponent<BaseContainer>() is null ||
                !gameObj.GetComponent<BaseContainer>().IsDirty)
            {
                return;
            }
            
            thrownObjects.Push(gameObj);
            gameObj.SetActive(false);

            var startMachineWorkSignal = new StartMachineWorkSignal()
            {
                MachinesType = MachinesTypes.SinkMachine
            };
            Debug.Log("Here");
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
