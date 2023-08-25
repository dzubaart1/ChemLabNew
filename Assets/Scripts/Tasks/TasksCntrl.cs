#nullable enable
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
        private int _taskCurrentId = 0;
        private bool _isStartGame;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<TransferSubstanceSignal>(CheckTransferSubstance);
            _signalBus.Subscribe<StartGameSignal>(OnStartGame);
            _signalBus.Subscribe<EnterIntoMachineSignal>(CheckEnteringIntoMachine);
            _signalBus.Subscribe<StartMachineWorkSignal>(CheckStartMachineWork);
            _signalBus.Subscribe<FinishMashineWorkSignal>(CheckFinishMachineWork);
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
            if (!CurrentTask().ContainersType.Contains(transferSubstanceSignal.From) ||
                !CurrentTask().ContainersType.Contains(transferSubstanceSignal.To) ||
                !CurrentTask().ResultSubstance.SubName.Equals(transferSubstanceSignal.TranserProperty.SubName))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            MoveToNext();
        }
        private void CheckEnteringIntoMachine(EnterIntoMachineSignal enterIntoMachineSignal)
        {
            if (!CurrentTask().MachinesType.Equals(enterIntoMachineSignal.MachinesType) ||
                !CurrentTask().ContainersType.Contains(enterIntoMachineSignal.ContainersType))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            MoveToNext();
        }
        private void CheckStartMachineWork(StartMachineWorkSignal startMachineWorkSignal)
        {
            if (!CurrentTask().MachinesType.Equals(startMachineWorkSignal.MachinesType))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            MoveToNext();
        }
        private void CheckFinishMachineWork(FinishMashineWorkSignal finishMashineWorkSignal)
        {
            if (!CurrentTask().MachinesType.Equals(finishMashineWorkSignal.MachinesType) ||
                !CurrentTask().ResultSubstance.Equals(finishMashineWorkSignal.SubstancePropertyBase))
            {
                _signalBus.Fire<EndGameSignal>();
                return;
            }
            MoveToNext();
        }
    }
}