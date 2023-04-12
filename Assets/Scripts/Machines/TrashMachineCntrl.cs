using System;
using System.Collections.Generic;
using Cups;
using Interfaces;
using Machines;
using Tasks;
using UnityEngine;
using Zenject;


public class TrashMachineCntrl : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnPoint;
    
    private Stack<GameObject> thrownObjects;
    private TasksCntrl _tasksCntrl;

    public void Start()
    {
        thrownObjects = new Stack<GameObject>();
    }

    [Inject]
    public void Construct(TasksCntrl tasksCntrl)
    {
        _tasksCntrl = tasksCntrl;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IAbleThrown>() is null)
        {
            return;
        }
        
        AddObject(collision.gameObject);
    }

    private void AddObject(GameObject gameObj)
    {
        thrownObjects.Push(gameObj); 
        gameObj.SetActive(false);
        if (gameObj.GetComponent<DozatorCup>() is not null && gameObj.GetComponent<DozatorCup>().IsDirty)
        {
            _tasksCntrl.CheckStartMachineWork(MachinesTypes.TrashMachine);
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
