#nullable enable
using System.Collections.Generic;
using System.Linq;
using Canvases;
using Installers;
using UnityEngine;
using Zenject;

namespace Tasks
{
    public class TasksCntrl : MonoBehaviour
    {
        private List<TaskParams> _tasksParamsList;
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
            });
            _signalBus.Subscribe<EnterIntoMachineSignal>(CheckEnteringIntoMachine);
            _signalBus.Subscribe<StartMachineWorkSignal>(CheckStartMachineWork);
            _signalBus.Subscribe<FinishMashineWorkSignal>(CheckFinishMachineWork);
        }
        private void Start()
        {
            _tasksParamsList = new List<TaskParams>();
            _tasksParamsList = Resources.LoadAll<TaskParams>("Tasks/").ToList();
            _tasksParamsList.Sort(CompareToTaskParams);
            _taskCurrentId = 0;
        }
        private int CompareToTaskParams(TaskParams t1, TaskParams t2)
        {
            if (t1.Id > t2.Id)
            {
                return 1;
            }

            if (t1.Id < t2.Id)
            {
                return -1;
            }

            return 0;
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
            
            Debug.Log($"{CurrentTask().Id} is done");
            if (_taskCurrentId + 1 >= _tasksParamsList.Count) return;
            _taskCurrentId++;
            _signalBus.Fire(new CheckTasksSignal(){CurrentTask = CurrentTask()});
        }
        public TaskParams CurrentTask()
        {
            return GetTaskParamsById(_taskCurrentId);
        }
        public void SetCurrentTaskId(int taskId)
        {
            _taskCurrentId = taskId;
        }
        private void CheckTransferSubstance(TransferSubstanceSignal transferSubstanceSignal)
        {
            Debug.Log(transferSubstanceSignal.From + " " + transferSubstanceSignal.To + " " + transferSubstanceSignal.TranserProperty.SubName);
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