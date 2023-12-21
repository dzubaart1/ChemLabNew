#nullable enable
using System;
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
        public static DateTime StartTime;
        public static DateTime EndTime;
        private static List<TaskParams> _errorTasks;
        public static IReadOnlyList<TaskParams> ErrorTasks => _errorTasks;
        
        public List<TaskParams> _tasksParamsList;
        
        private int _taskCurrentId;
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
            _errorTasks = new List<TaskParams>();
            StartTime = DateTime.Now;
            
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
                EndTime = DateTime.Now;
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
        public TaskParams CurrentTask()
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
            foreach (var containersType in CurrentTask().ContainersType)
            {
                if (containersType != transferSubstanceSignal.To && containersType != transferSubstanceSignal.From)
                {
                    if(!_errorTasks.Contains(CurrentTask()))
                        _errorTasks.Add(CurrentTask());
                    _signalBus.Fire<EndGameSignal>();
                    return;
                }
            }
            
            if (CurrentTask().ResultSubstance && !CurrentTask().ResultSubstance.Equals(transferSubstanceSignal.TranserProperty))
            {
                if(!_errorTasks.Contains(CurrentTask()))
                    _errorTasks.Add(CurrentTask());
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            
            if (CurrentTask().MachinesType is not MachinesTypes.None)
            {
                return;
            }
            
            MoveToNext();
        }
        
        private void CheckMachineWork(MachineWorkSignal machineWorkSignal)
        {
            if (!CurrentTask().ContainersType.Contains(machineWorkSignal.ContainersType)
                && machineWorkSignal.ContainersType is not ContainersTypes.None)
            {
                if(!_errorTasks.Contains(CurrentTask()))
                    _errorTasks.Add(CurrentTask());
                _signalBus.Fire<EndGameSignal>();
                return;
            }

            Debug.Log("M1");

            if (!CurrentTask().MachinesType.Equals(machineWorkSignal.MachinesType)
                && machineWorkSignal.MachinesType is not MachinesTypes.None)
            {
                if(!_errorTasks.Contains(CurrentTask()))
                    _errorTasks.Add(CurrentTask());
                _signalBus.Fire<EndGameSignal>();
                return;
            }

            Debug.Log("M2");

            if (CurrentTask().ResultSubstance && !CurrentTask().ResultSubstance.Equals(machineWorkSignal.SubstancePropertyBase))
            {
                if(!_errorTasks.Contains(CurrentTask()))
                    _errorTasks.Add(CurrentTask());
                _signalBus.Fire<EndGameSignal>();
                return;
            }

            Debug.Log("M3");

            MoveToNext();
        }

        private void CheckDoorWork(DoorWorkSignal doorWorkSignal)
        {
            if (!CurrentTask().DoorTypes.Equals(doorWorkSignal.DoorType))
            {
                return;
            }

            if (!CurrentTask().IsOpenDoor.Equals(doorWorkSignal.IsOpen))
            {
                return;
            }

            MoveToNext();
        }
    }
}