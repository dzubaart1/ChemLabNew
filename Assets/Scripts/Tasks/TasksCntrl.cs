#nullable enable
using System.Collections.Generic;
using Canvases;
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
            _signalBus.Subscribe<StartGameSignal>(OnStartGame);
            _signalBus.Subscribe<MachineWorkSignal>(CheckMachineWork);
            _signalBus.Subscribe<RevertTaskSignal>(RevertTask);
        }

        private void OnStartGame()
        {
            _isStartGame = true;
            _signalBus.Fire(new CheckTasksSignal() { CurrentTask = CurrentTask() });
            _signalBus.Fire(new SaveSignal(){TaskId = _taskCurrentId});
        }
        private void MoveToNext()
        {
            if (!_isStartGame)
            {
                return;
            }
            
            if (_taskCurrentId + 1 >= _tasksParamsList.Count)
            {
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
            if (CurrentTask().MachinesType is not MachinesTypes.None)
            {
                return;
            }
            
            if (!CurrentTask().ContainersType.Contains(transferSubstanceSignal.From))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            
            if (!CurrentTask().ContainersType.Contains(transferSubstanceSignal.To))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }

            if (CurrentTask().ResultSubstance && !CurrentTask().ResultSubstance.Equals(transferSubstanceSignal.TranserProperty))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            
            MoveToNext();
        }
        private void CheckMachineWork(MachineWorkSignal machineWorkSignal)
        {
            if (CurrentTask().ContainersType.Count > 0 &&
                machineWorkSignal.ContainersType is not ContainersTypes.None)
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }

            if (!CurrentTask().MachinesType.Equals(machineWorkSignal.MachinesType))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }

            if (CurrentTask().SubstancesParams && !CurrentTask().SubstancesParams.Equals(machineWorkSignal.SubstancePropertyBase))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }

            MoveToNext();
        }
    }
}