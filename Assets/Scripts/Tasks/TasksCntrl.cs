#nullable enable
using System.Collections.Generic;
using System.Linq;
using Containers;
using Machines;
using Substances;
using UnityEngine;

namespace Tasks
{
    public class TasksCntrl
    {
        private List<TaskParams> _tasksParamsList;
        private int _taskCurrentId;
        public TasksCntrl()
        {
            _tasksParamsList = new List<TaskParams>();
            _tasksParamsList = Resources.LoadAll<TaskParams>("Tasks/").ToList();
            _tasksParamsList.Sort(CompareToTaskParams);
            _taskCurrentId = 0;
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

        public void CheckTransferSubstance(BaseContainer firstContainer, BaseContainer secondContainer, SubstanceBase substance)
        {
            if (!CurrentTask().ContainersType.Contains(firstContainer.ContainerType) ||
                !CurrentTask().ContainersType.Contains(secondContainer.ContainerType)) 
                return;
            Debug.Log($"{CurrentTask().Id} is done");
            MoveToNext();
        }

        public void CheckEnteringIntoMachine(MachinesTypes machinesType, ContainersTypes enteringContainer)
        {
            if (!CurrentTask().MachinesType.Equals(machinesType) ||
                !CurrentTask().ContainersType.Contains(enteringContainer)) return;
            Debug.Log($"{CurrentTask().Id} is done");
            MoveToNext();
        }
        public void CheckStartMachineWork(MachinesTypes machinesType)
        {
            if (CurrentTask().MachinesType.Equals(machinesType))
            {
                Debug.Log($"{CurrentTask().Id} is done");
                MoveToNext();
            }
        }
        
        public void CheckFinishMachineWork(MachinesTypes machinesType, SubstanceBase substance)
        {
            if (CurrentTask().MachinesType.Equals(machinesType)
                && CurrentTask().ResultSubstance.Equals(substance.SubstanceProperty))
            {
                Debug.Log($"{CurrentTask().Id} is done");
                MoveToNext();
            }
        }
    }
}