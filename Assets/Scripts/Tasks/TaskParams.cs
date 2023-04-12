using System.Collections.Generic;
using Containers;
using Machines;
using Substances;
using UnityEngine;

namespace Tasks
{
    [CreateAssetMenu(fileName = "TaskParams", menuName = "TaskParams/Task Params", order = 1)]
    public class TaskParams : ScriptableObject
    {
        public string Title;
        public int StepId;
        public SubstanceParams SubstancesParams;
        public float Weight;
        public List<ContainersTypes> ContainersType;
        public float DozatorDoze;
        public MachinesTypes MachinesType;
        public SubstanceParams ResultSubstance;
    }
}