using System.Globalization;
using BNG;
using Containers;
using Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Machines.WeightingMachine
{
    public class WeightingMachineCntrl : MonoBehaviour
    {
        [SerializeField]
        private Text _weightText;
        [SerializeField]
        private SnapZone _snapZone;

        private SignalBus _signalBus;
        private bool _isEnter, _hasObject;
        private float _currentWeight;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Awake()
        {
            ResetValues();
        }

        private void Update()
        {
            if (_snapZone.HeldItem is null ||
                _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().IsAbleToWeight is false)
            {
                _isEnter = false;
                ResetValues();
                return;
            }
            
            if (!_isEnter)
            {
                OnEnterObject();
            }

            if (_snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>().CurrentCountSubstances == 0)
            {
                ResetValues();
                return;
            }
            
            if (!_snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().GetWeight()
                    .Equals(_currentWeight))
            {
                OnFinishWork();
            }
        }
        
        public void OnEnterObject()
        {
            EnterIntoMachineSignal enterIntoMachineSignal = new EnterIntoMachineSignal()
            {
                MachinesType = MachinesTypes.WeightingMachine,
                ContainersType = _snapZone.HeldItem.GetComponent<BaseContainer>().ContainerType
            };
            _signalBus.Fire(enterIntoMachineSignal);
            _isEnter = true;
        }
        

        public void ResetValues()
        {
            _currentWeight = 0f;
            _weightText.text = "0.0000g";
        }
        
        public void OnFinishWork()
        {
            _currentWeight = _snapZone.HeldItem.GetComponent<BaseContainer>().GetWeight();
            _weightText.text = _currentWeight.ToString("0.0000", CultureInfo.InvariantCulture) + "g";
            _signalBus.Fire(new FinishMashineWorkSignal()
            {
                MachinesType = MachinesTypes.WeightingMachine,
                SubstancePropertyBase = _snapZone.HeldItem.GetComponent<BaseContainer>().GetNextSubstance().SubstanceProperty
            });
        }
    }
}
 