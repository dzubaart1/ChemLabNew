using System.Collections.Generic;
using Containers;
using DefaultNamespace;
using Machines;
using Substances;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    [CreateAssetMenu(fileName = "TaskParams", menuName = "TaskParams/Task Params", order = 1)]
    public class TaskParams : ScriptableObject
    {
        public int Number;
        public string Title;
        public SubstancePropertyBase SubstancesParams;
        public float Weight;
        public List<ContainersTypes> ContainersType;
        public float DozatorDoze;
        public MachinesTypes MachinesType;
        public DoorTypes DoorTypes;
        public bool IsOpenDoor;
        public SubstancePropertyBase ResultSubstance;
        public bool IsSpawnPoint;
        public string TaskDescription;
        public string TaskWarning;
        public Sprite TaskImage;
    }
}