using System.Collections.Generic;
using BNG;
using Installers;
using UnityEngine;
using Zenject;

namespace Machines
{
    public class TeleportMachineCntrl : MonoBehaviour
    {
        [SerializeField] private SnapZone _snapZone;
        [SerializeField] private List<ParticleSystem> ParticleSystems;
        [SerializeField] private MachinesTypes _teleportType;
        [SerializeField] private GameObject _docPrefab;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
    
        public void OnEnter()
        {
            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }

            var _teleportedGameObject = _snapZone.HeldItem.gameObject;
            
            _teleportedGameObject.SetActive(false);
            _snapZone.HeldItem = null;
            var startMachineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = _teleportType
            };
            _signalBus.Fire(startMachineWorkSignal);
        }

        public void OnReturnBtnClick()
        {
            Instantiate(_docPrefab, _snapZone.transform.position, Quaternion.identity);
            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }
            var startMachineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = _teleportType
            };
            _signalBus.Fire(startMachineWorkSignal);
        }
    }
}
