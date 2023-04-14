#nullable enable
using System.Collections.Generic;
using BNG;
using Cups;
using Substances;
using UnityEngine;


namespace Containers
{
    public class BaseContainer : MonoBehaviour
    {
        [SerializeField] protected GameObject _baseFormPrefab;
        [SerializeField] protected List<BaseCup> _cupsList;
        [SerializeField] protected SnapZone _snapZone;
        
        public ContainersTypes ContainerType;
        public Substance? Substance;
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

        protected virtual bool AddSubstance(Substance substance)
        {
            UpdateSubstance(substance);
            return true;
        }

        protected virtual bool RemoveSubstance(float maxVolume)
        {
            if (Substance is null)
            {
                return false;
            }
            if (maxVolume >= Substance.Weight)
            {
                UpdateSubstance(null);
            }
            else
            {
                Substance.RemoveSubstanceWeight(maxVolume);
            }
            return true;
        }

        public virtual void UpdateSubstance(Substance? substance)
        {
            if (substance is null)
            {
                Substance = null;
                _baseFormPrefab.SetActive(false);
                return;
            }

            var newSubstance = substance;
            if (substance.Weight > MaxVolume)
            {
                newSubstance = new Substance(substance.SubParams, MaxVolume);
            }
            
            Substance = newSubstance;
            _baseFormPrefab.SetActive(true);
            _baseFormPrefab.GetComponent<MeshRenderer>().material.color = newSubstance.SubParams.Color;
        }
    }
}
