using System.Collections.Generic;
using BNG;
using Tasks;
using UnityEngine;
using Zenject;

namespace Machines
{
    public class TeleportMachineCntrl : MonoBehaviour
    {
        [SerializeField]
        private SnapZone _snapZone;
        [SerializeField]
        private List<ParticleSystem> ParticleSystems;

        private TasksCntrl _tasksCntrl;
    
        [Inject]
        public void Construct(TasksCntrl tasksCntrl)
        {
            _tasksCntrl = tasksCntrl;
        }
    
        public void OnEnter()
        {
            foreach (var particleSystem in ParticleSystems)
            {
                particleSystem.Play();
            }
            _snapZone.HeldItem.gameObject.SetActive(false);
            _snapZone.HeldItem = null;
            _tasksCntrl.CheckStartMachineWork(MachinesTypes.TeleportMachine);
        } 
    }
}
