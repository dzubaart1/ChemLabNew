using System.Collections.Generic;
using BNG;
using Cups;
using Substances;
using UnityEngine;
using Zenject;


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

        public GameObject _XRrig;

        [Inject]
        public void Construct(GameObject rigInst)
        {
            _XRrig = rigInst;
        }
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
            Debug.Log(GetStringStack());
        }

        public string GetStringStack()
        {
            string s = "";
            foreach (var VARIABLE in CurrentSubstancesList)
            {
                s += (VARIABLE.SubstanceProperty.SubName + ", ");
            }
            s = s.Remove(s.Length - 2);
            return s;
        }
        
        public static bool IsNull<T>(T myObject, string message = "") where T : class
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
    }
}
