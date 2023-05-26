using System.Collections.Generic;
using BNG;
using Cups;
using Substances;
using UnityEngine;


namespace Containers
{
    public class BaseContainer : MonoBehaviour
    {
        public Stack<Substance> CurrentSubstancesList;
        public float MaxVolume = 9000;
        [SerializeField] protected List<BaseCup> _cupsList;
        [SerializeField] protected SnapZone _snapZone;
        
        public ContainersTypes ContainerType;
        public bool IsAbleToWeight;

        public void Awake()
        {
            CurrentSubstancesList = new Stack<Substance>();
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
            foreach (var substance in CurrentSubstancesList)
            {
                sumWeight += substance.GetWeight();
            }

            return sumWeight;
        }
        
        public void PrintStack()
        {
            string s = "";
            foreach (var VARIABLE in CurrentSubstancesList)
            {
                s += (VARIABLE.SubstanceProperty.SubName + " ");
            }
            Debug.Log(s);
        }
    }
}
