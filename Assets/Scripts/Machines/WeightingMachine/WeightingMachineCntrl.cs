using System.Globalization;
using BNG;
using Containers;
using Installers;
using Interfaces;
using Substances;
using Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Machines
{
    public class WeightingMachineCntrl : MonoBehaviour, IMachine
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
                OnStartWork();
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
        
        public void OnStartWork()
        {
            _currentWeight = _snapZone.HeldItem.GetComponent<BaseContainer>().GetWeight();
            _weightText.text = _currentWeight.ToString("0.0000", CultureInfo.InvariantCulture) + "g";
            OnFinishWork();
        }

        public void OnFinishWork()
        {
        }
    }
}
 