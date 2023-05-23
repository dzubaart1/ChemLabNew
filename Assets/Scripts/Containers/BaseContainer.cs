using System.Collections.Generic;
using BNG;
using Cups;
using Substances;
using UnityEngine;
using UnityEngine.UI;


namespace Containers
{
    public class BaseContainer : MonoBehaviour
    {
        [SerializeField] protected List<BaseCup> _cupsList;
        [SerializeField] protected SnapZone _snapZone;
        
        public GameObject _sedimentPrefab;
        public GameObject _membranePrefab;
        public GameObject _basePrefab;
        public ContainersTypes ContainerType;
        
        public Substance? Substance;
        public float MaxVolume = 9000;
        public bool IsAbleToWeight;
        
        public GameObject HiddenCanvas;

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
            
            if (_sedimentPrefab is not null && newSubstance.SubParams.Sediment is not null)
            {
                _sedimentPrefab.GetComponent<MeshRenderer>().material.color = newSubstance.SubParams.Sediment.Color;
            }
            
            if (_membranePrefab is not null && newSubstance.SubParams.Membrane is not null)
            {
                _membranePrefab.GetComponent<MeshRenderer>().material.color = newSubstance.SubParams.Membrane.Color;
            }

            if (IsNull(HiddenCanvas))
            {
                return;
            }
            HiddenCanvas.GetComponentInChildren<Text>().text = newSubstance.SubParams.SubName ?? "не определено";
        }

        public bool IsNull<T>(T myObject, string message = "") where T : class
        {
            switch (myObject)
            {
                case UnityEngine.Object obj when !obj:
                    return true;
                case null:
                    return true;
                default:
                    return false;
            }
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
