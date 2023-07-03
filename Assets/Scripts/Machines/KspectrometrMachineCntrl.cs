using Installers;
using UnityEngine;
using Zenject;

namespace Machines
{
    public class KspectrometrMachineCntrl : MonoBehaviour
    {
        private SignalBus _signalBus;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _docPrefab;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void OnReturnBtnClick()
        {
            Instantiate(_docPrefab, _spawnPoint.position, Quaternion.identity);
            
            var startMachineWorkSignal = new StartMachineWorkSignal()
            {
                MachinesType = MachinesTypes.KspectrometrMachine
            };
            _signalBus.Fire(startMachineWorkSignal);
        }
        
    }
}

