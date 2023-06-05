using System.Collections.Generic;
using BNG;
using Cups;
using JetBrains.Annotations;
using Substances;
using UnityEngine;
using Zenject;


namespace Containers
{
    public class BaseContainer : MonoBehaviour
    {
        [ItemCanBeNull] public Substance[] CurrentSubstances;
        public float MaxVolume = 9000;
        public const int MAX_LAYOURS_COUNT = 3;

        public ContainersTypes ContainerType;
        public bool IsAbleToWeight;

        [SerializeField] protected List<BaseCup> _cupsList;
        [SerializeField] protected SnapZone _snapZone;
        
        public void Awake()
        {
            CurrentSubstances = new Substance[MAX_LAYOURS_COUNT];
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

        public float GetWeight()
        {
            float sumWeight = 0;
            foreach (var substance in CurrentSubstances)
            {
                sumWeight += substance?.GetWeight() ?? 0;
            }

            return sumWeight;
        }
        public static bool IsNull<T>(T myObject, string message = "") where T : class
        {
            switch (myObject)
            {
                case Object obj when !obj:
                    return true;
                case null:
                    return true;
                default:
                    return false;
            }
        }
    }
}
