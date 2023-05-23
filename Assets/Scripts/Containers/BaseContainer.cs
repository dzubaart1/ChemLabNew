using System.Collections.Generic;
using BNG;
using Cups;
using JetBrains.Annotations;
using Substances;
using UnityEngine;


namespace Containers
{
    public class BaseContainer : MonoBehaviour
    {
        [CanBeNull] public SubstanceSplit CurrentSubstance;
        [SerializeField] protected List<BaseCup> _cupsList;
        [SerializeField] protected SnapZone _snapZone;
        
        public ContainersTypes ContainerType;
        
        public float MaxVolume = 9000;
        public bool IsAbleToWeight;

        public void Awake()
        {
            if (_cupsList.Count == 0)
            {
                return;
            }
            _snapZone.OnlyAllowNames.Clear();

            foreach (BaseCup cup in _cupsList)
            {
                _snapZone.OnlyAllowNames.Add(cup.name);
            }
        }
        
        protected virtual bool IsEnable()
        {
            return true;
        }
    }
}
