#nullable enable
using System;
using System.Collections.Generic;
using Canvases;
using Installers;
using UnityEngine;
using Zenject;

namespace Tasks
{
    public class TasksCntrl : MonoBehaviour
    {
        public List<TaskParams> _tasksParamsList;
        private int _taskCurrentId;
        private bool _isStartGame;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<TransferSubstanceSignal>(CheckTransferSubstance);
            _signalBus.Subscribe<StartGameSignal>(_ =>
            {
                _isStartGame = true;
                _signalBus.Fire(new CheckTasksSignal() { CurrentTask = CurrentTask() });
                _signalBus.Fire(new SaveSignal(){TaskId = _taskCurrentId});
            });
            _signalBus.Subscribe<EnterIntoMachineSignal>(CheckEnteringIntoMachine);
            _signalBus.Subscribe<StartMachineWorkSignal>(CheckStartMachineWork);
            _signalBus.Subscribe<FinishMashineWorkSignal>(CheckFinishMachineWork);
            _signalBus.Subscribe<RevertTaskSignal>(RevertTask);
        }
        public TaskParams GetTaskParamsById(int id)
        {
            return _tasksParamsList[id];
        }
        public void MoveToNext()
        {
            if (!_isStartGame)
            {
                return;
            }

            if (CurrentTask().IsSpawnPoint)
            {
                _signalBus.Fire(new SaveSignal(){TaskId = _taskCurrentId});
                Debug.Log($"{_taskCurrentId} is saved");
            }
            
            Debug.Log($"{_taskCurrentId} is done");
            if (_taskCurrentId + 1 >= _tasksParamsList.Count)
            {
                return;
            }
            _taskCurrentId++;
            _signalBus.Fire(new CheckTasksSignal(){CurrentTask = CurrentTask()});
        }
        public TaskParams CurrentTask()
        {
            return GetTaskParamsById(_taskCurrentId);
        }
        public void RevertTask(RevertTaskSignal revertTaskSignal)
        {
            _taskCurrentId = revertTaskSignal.TaskId;
            _signalBus.Fire(new CheckTasksSignal() { CurrentTask = CurrentTask() });
            Debug.Log($"Revert id {_taskCurrentId}");
        }
        private void CheckTransferSubstance(TransferSubstanceSignal transferSubstanceSignal)
        {
            Debug.Log(transferSubstanceSignal.From + " " + transferSubstanceSignal.To + " " + transferSubstanceSignal.TranserProperty.SubName);
            var s = "";
            CurrentTask().ContainersType.ForEach(param => s += param + " ");
            Debug.Log(s + " " + transferSubstanceSignal.TranserProperty.SubName);
            if (!CurrentTask().ContainersType.Contains(transferSubstanceSignal.From) ||
                !CurrentTask().ContainersType.Contains(transferSubstanceSignal.To) ||
                !CurrentTask().ResultSubstance.SubName.Equals(transferSubstanceSignal.TranserProperty.SubName))
            {
                _signalBus.Fire(new ToggleCanvasSignal(){Id = CanvasId.EndGameCanvas});
                return;
            }
            MoveToNext();
        }
        private void CheckEnteringIntoMachine(EnterIntoMachineSignal enterIntoMachineSignal)
        {
            if (!CurrentTask().MachinesType.Equals(enterIntoMachineSignal.MachinesType) ||
                !CurrentTask().ContainersType.Contains(enterIntoMachineSignal.ContainersType))
            {
                _signalBus.Fire(new ToggleCanvasSignal(){Id = CanvasId.EndGameCanvas});
                return;
            }
            MoveToNext();
        }
        private void CheckStartMachineWork(StartMachineWorkSignal startMachineWorkSignal)
        {
            Debug.Log("Here 2");
            if (!CurrentTask().MachinesType.Equals(startMachineWorkSignal.MachinesType))
            {
                _signalBus.Fire(new ToggleCanvasSignal(){Id = CanvasId.EndGameCanvas});
                return;
            }
            MoveToNext();
        }
        private void CheckFinishMachineWork(FinishMashineWorkSignal finishMashineWorkSignal)
        {
            Debug.Log(finishMashineWorkSignal.MachinesType + " " + finishMashineWorkSignal.SubstancePropertyBase);
            if (!CurrentTask().MachinesType.Equals(finishMashineWorkSignal.MachinesType) ||
                !CurrentTask().ResultSubstance.Equals(finishMashineWorkSignal.SubstancePropertyBase))
            {
                _signalBus.Fire(new ToggleCanvasSignal(){Id = CanvasId.EndGameCanvas});
                return;
            }
            MoveToNext();
        }
    }
}