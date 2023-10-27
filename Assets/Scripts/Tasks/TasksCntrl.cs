#nullable enable
using System.Collections.Generic;
using Containers;
using Installers;
using Machines;
using UnityEngine;
using Zenject;

namespace Tasks
{
    public class TasksCntrl : MonoBehaviour
    {
        public List<TaskParams> _tasksParamsList;
        private int _taskCurrentId = 0;
        private bool _isStartGame;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<TransferSubstanceSignal>(CheckTransferSubstance);
            _signalBus.Subscribe<MachineWorkSignal>(CheckMachineWork);
            _signalBus.Subscribe<RevertTaskSignal>(RevertTask);
            _signalBus.Subscribe<DoorWorkSignal>(CheckDoorWork);
        }

        private void Start()
        {
            _signalBus.Fire(new StartGameSignal());
            _signalBus.Fire(new CheckTasksSignal() { CurrentTask = CurrentTask() });
            _signalBus.Fire(new SaveSignal(){TaskId = _taskCurrentId});
            _isStartGame = true;
        }

        private void MoveToNext()
        {
            if (!_isStartGame)
            {
                return;
            }
            
            if (_taskCurrentId + 1 >= _tasksParamsList.Count)
            {
                _signalBus.Fire(new GameOverSignal());
                return;
            }
            _taskCurrentId++;
            
            if (CurrentTask().IsSpawnPoint)
            {
                _signalBus.Fire(new SaveSignal(){TaskId = _taskCurrentId});
            }
            
            _signalBus.Fire(new CheckTasksSignal(){CurrentTask = CurrentTask()});
        }
        private TaskParams CurrentTask()
        {
            return _tasksParamsList[_taskCurrentId];
        }
        
        private void RevertTask(RevertTaskSignal revertTaskSignal)
        {
            _taskCurrentId = revertTaskSignal.TaskId;
            _signalBus.Fire(new CheckTasksSignal() { CurrentTask = CurrentTask() });
        }
        
        private void CheckTransferSubstance(TransferSubstanceSignal transferSubstanceSignal)
        {
            Debug.Log($"Transfer Enter {_taskCurrentId}");

            Debug.Log("Transfer 0");
            
            if (!CurrentTask().ContainersType.Contains(transferSubstanceSignal.To)
                && transferSubstanceSignal.To is not ContainersTypes.None)
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            
            Debug.Log("Transfer 1");
            
            if (!CurrentTask().ContainersType.Contains(transferSubstanceSignal.From) 
                && transferSubstanceSignal.From is not ContainersTypes.None)
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            
            Debug.Log("Transfer 2");

            if (CurrentTask().ResultSubstance && !CurrentTask().ResultSubstance.Equals(transferSubstanceSignal.TranserProperty))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            
            Debug.Log("Transfer 3");
            
            if (CurrentTask().MachinesType is not MachinesTypes.None)
            {
                return;
            }
            
            MoveToNext();
        }
        
        private void CheckMachineWork(MachineWorkSignal machineWorkSignal)
        {
            Debug.Log($"Machine Enter {_taskCurrentId}");
            Debug.Log($"{machineWorkSignal.MachinesType} {machineWorkSignal.ContainersType} {machineWorkSignal.SubstancePropertyBase}");
            
            if (!CurrentTask().ContainersType.Contains(machineWorkSignal.ContainersType)
                && machineWorkSignal.ContainersType is not ContainersTypes.None)
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            
            Debug.Log("Machine 1");

            if (!CurrentTask().MachinesType.Equals(machineWorkSignal.MachinesType)
                && machineWorkSignal.MachinesType is not MachinesTypes.None)
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            
            Debug.Log("Machine 2");

            if (CurrentTask().SubstancesParams && !CurrentTask().SubstancesParams.Equals(machineWorkSignal.SubstancePropertyBase))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            
            Debug.Log("Machine 3");

            MoveToNext();
        }

        private void CheckDoorWork(DoorWorkSignal doorWorkSignal)
        {
            Debug.Log("0");
            if (!CurrentTask().DoorTypes.Equals(doorWorkSignal.DoorType))
            {
                Debug.Log("1");
                //_signalBus.Fire<EndGameSignal>();
                return;
            }

            if (!CurrentTask().IsOpenDoor.Equals(doorWorkSignal.IsOpen))
            {
                Debug.Log("2");
                //_signalBus.Fire<EndGameSignal>();
                return;
            }

            MoveToNext();
        }
    }
}