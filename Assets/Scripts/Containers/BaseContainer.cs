using System.Collections.Generic;
using BNG;
using Cups;
using Substances;
using UnityEngine;


namespace Containers
{
    public class BaseContainer : MonoBehaviour
    {
        public ContainersTypes ContainerType;
        public Substance Substance;
        public float MaxVolume = 9000;
        [SerializeField]
        protected GameObject _baseFormPrefab;
        [SerializeField]
        protected List<BaseCup> _cupsList;
        [SerializeField]
        protected SnapZone _snapZone;

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

        public virtual bool IsEnable()
        {
            return true;
        }
        public virtual bool AddSubstance(Substance substance)
        {
            var res = substance.SubParams;
            var weight = substance.Weight;
            Substance = new Substance(res, weight);
            _baseFormPrefab.GetComponent<MeshRenderer>().material.color = res.Color;
            _baseFormPrefab.SetActive(true);
            return true;
        }

        public virtual bool RemoveSubstance(float maxVolume)
        {
            if (Substance is null)
            {
                return false;
            }
            if (maxVolume >= Substance.Weight)
            {
                Substance = null;
                _baseFormPrefab.SetActive(false);
            }
            else
            {
                Substance.RemoveSubstanceWeight(maxVolume);
            }
            return true;
        }
    }
}
