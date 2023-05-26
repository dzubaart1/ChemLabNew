#nullable enable
using System.Collections.Generic;
using System.Linq;
using Canvases;
using Containers;
using Installers;
using Machines;
using Substances;
using UnityEngine;
using Zenject;

namespace Tasks
{
    public class TasksCntrl
    {
        private List<TaskParams> _tasksParamsList;
        private int _taskCurrentId;

        private SignalBus _signalBus;
        public TasksCntrl()
        {
            _tasksParamsList = new List<TaskParams>();
            _tasksParamsList = Resources.LoadAll<TaskParams>("Tasks/").ToList();
            _tasksParamsList.Sort(CompareToTaskParams);
            _taskCurrentId = 0;
        }
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        public delegate void TaskUpdateHandler();
        public event TaskUpdateHandler? Notify;
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
            if (_taskCurrentId + 1 >= _tasksParamsList.Count) return;
            _taskCurrentId++;
            Notify?.Invoke();
        }
        public TaskParams CurrentTask()
        {
            return GetTaskParamsById(_taskCurrentId);
        }
        public void SetCurrentTaskId(int taskId)
        {
            _taskCurrentId = taskId;
            Notify?.Invoke();
        }
        public void CheckTransferSubstance(BaseContainer firstContainer, BaseContainer secondContainer, SubstancePropertyBase substanceProperty)
        {
            if (!CurrentTask().ContainersType.Contains(firstContainer.ContainerType) ||
                !CurrentTask().ContainersType.Contains(secondContainer.ContainerType)
                /*!secondContainer.CurrentSubstancesList.Peek().SubstanceProperty.SubName
                    .Equals(substanceProperty.SubName)*/)
            {
                //_signalBus.Fire(new ShowCanvasSignal(){Id = CanvasId.EndGame});
                return;
            }
            Debug.Log($"{CurrentTask().Id} is done");
            MoveToNext();
        }
        public void CheckEnteringIntoMachine(MachinesTypes machinesType, ContainersTypes enteringContainer)
        {
            if (!CurrentTask().MachinesType.Equals(machinesType) ||
                !CurrentTask().ContainersType.Contains(enteringContainer))
            {
                //_signalBus.Fire(new ShowCanvasSignal(){Id = CanvasId.EndGame});
                return;
            }
            Debug.Log($"{CurrentTask().Id} is done");
            MoveToNext();
        }
        public void CheckStartMachineWork(MachinesTypes machinesType)
        {
            if (!CurrentTask().MachinesType.Equals(machinesType))
            {
                //_signalBus.Fire(new ShowCanvasSignal(){Id = CanvasId.EndGame});
                return;
            }
            Debug.Log($"{CurrentTask().Id} is done");
            MoveToNext();
        }
        public void CheckFinishMachineWork(MachinesTypes machinesType, SubstancePropertyBase substance)
        {
            if (!CurrentTask().MachinesType.Equals(machinesType)
                || !CurrentTask().ResultSubstance.Equals(substance))
            {
                //_signalBus.Fire(new ShowCanvasSignal(){Id = CanvasId.EndGame});
                return;
            }
            Debug.Log($"{CurrentTask().Id} is done");
            MoveToNext();
        }
    }
}