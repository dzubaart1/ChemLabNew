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
        public Substance[] CurrentSubstances;
        public float Weight;
        public float MaxVolume = 9000;
        public ContainersTypes ContainerType;
        public bool IsAbleToWeight;
        public bool IsDirty;
        public int CurrentCountSubstances;

        [SerializeField] protected List<BaseCup> _cupsList;
        [SerializeField] protected SnapZone _snapZone;

        protected const int MAX_LAYOURS_COUNT = 3;
        
        public void Awake()
        {
            CurrentSubstances = new Substance[MAX_LAYOURS_COUNT];

            if (_cupsList.Count == 0)
            {
                return;
            }
            _snapZone.OnlyAllowNames.Clear();

            foreach (var cup in _cupsList)
            {
                _snapZone.OnlyAllowNames.Add(cup.name);
            }
        }

        public virtual bool IsEnable()
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

            return sumWeight + Weight;
        }
        
        [CanBeNull]
        public Substance GetNextSubstance()
        {
            for (var i = MAX_LAYOURS_COUNT-1; i >= 0; i--)
            {
                if (CurrentSubstances[i] != null)
                {
                    return CurrentSubstances[i];
                }
            }
            return null;
        }

        public void PrintAllSubstances()
        {
            for (var i = MAX_LAYOURS_COUNT - 1; i >= 0; i--)
            {
                if (CurrentSubstances[i] != null)
                {
                    Debug.Log($"На слое {i}: {CurrentSubstances[i].SubstanceProperty.SubName}");
                }
                else
                {
                    Debug.Log($"На слое {i}: ничего");
                }
            }
        }
    }
}
