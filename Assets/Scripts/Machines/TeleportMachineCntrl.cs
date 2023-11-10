using System.Collections.Generic;
using BNG;
using Containers;
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
        private bool _isTeleportTask;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CheckTasksSignal>(OnTaskChanged);
        }
    
        public void OnEnter()
        {
            if (!_isTeleportTask)
            {
                return;
            }
            
            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }
            gameObject.GetComponent<AudioSource>().Play();

            var _teleportedGameObject = _snapZone.HeldItem.gameObject;
            
            _teleportedGameObject.SetActive(false);
            _snapZone.HeldItem = null;
            var startMachineWorkSignal = new MachineWorkSignal()
            {
                MachinesType = _teleportType,
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

        private void OnTaskChanged(CheckTasksSignal signal)
        {
            _isTeleportTask = signal.CurrentTask.MachinesType == _teleportType;
        }
    }
}
