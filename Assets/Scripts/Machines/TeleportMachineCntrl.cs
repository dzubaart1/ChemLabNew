using System.Collections.Generic;
using BNG;
using Interfaces;
using Machines;
using Tasks;
using UnityEngine;
using Zenject;

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
        if (_snapZone.HeldItem.gameObject.GetComponent<IAbleTeleport>() is null)
        {
            return;
        }

        foreach (var particleSystem in ParticleSystems)
        {
            particleSystem.Play();
        }
        _snapZone.HeldItem.gameObject.SetActive(false);
        _snapZone.HeldItem = null;
        _tasksCntrl.CheckStartMachineWork(MachinesTypes.TeleportMachine);
    } 
}
