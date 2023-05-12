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
        [SerializeField] protected GameObject _basePrefab;
        [SerializeField] protected GameObject? _sedimentPrefab;
        [SerializeField] protected GameObject? _membranePrefab;
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
                _basePrefab.SetActive(false);
                return;
            }

            var newSubstance = substance;
            if (substance.Weight > MaxVolume)
            {
                newSubstance = new Substance(substance.SubParams, MaxVolume);
            }
            
            Substance = newSubstance;
            _basePrefab.SetActive(true);
            _basePrefab.GetComponent<MeshRenderer>().material.color = newSubstance.SubParams.Color;
            
            /*if (_sedimentPrefab is not null && newSubstance.SubParams.Sediment is not null)
            {
                _sedimentPrefab.GetComponent<MeshRenderer>().material.color = newSubstance.SubParams.Sediment.Color;
            }
            
            if (_membranePrefab is not null && newSubstance.SubParams.Membrane is not null)
            {
                _membranePrefab.GetComponent<MeshRenderer>().material.color = newSubstance.SubParams.Membrane.Color;
            }*/
        }

        public void ShowSediment()
        {
            if (Substance is not null && _sedimentPrefab is not null && Substance.SubParams.Sediment is not null)
            {
                _sedimentPrefab.SetActive(true);
            }
        }
        
        public void ShowMembrane()
        {
            if (Substance is not null && _membranePrefab is not null && Substance.SubParams.Membrane is not null)
            {
                _membranePrefab.SetActive(true);
            }
        }
    }
}
