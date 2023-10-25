using System.Globalization;
using BNG;
using Containers;
using Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace Machines.WeightingMachine
{
    public class WeightingMachineCntrl : MonoBehaviour
    {
        [SerializeField]
        private Text _weightText;

        [SerializeField]
        private Button _button;
        
        [SerializeField]
        private SnapZone _snapZone;

        private SignalBus _signalBus;
        
        private int _prevSubCount = -1;
        private float _currentDiscardWeight;
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
                _prevSubCount = -1;
                ResetValues();
                return;
            }
            
            if (_prevSubCount != _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentCountSubstances)
            {
                OnEnterObject();
            }
        }

        private void OnEnterObject()
        {
            ChangeValues();
            _signalBus.Fire(new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.WeightingMachine,
                SubstancePropertyBase = _snapZone.HeldItem.GetComponent<BaseContainer>().GetNextSubstance()?.SubstanceProperty,
                ContainersType = _snapZone.HeldItem.GetComponent<BaseContainer>().ContainerType
            });
            _prevSubCount = _snapZone.HeldItem.gameObject.GetComponent<BaseContainer>().CurrentCountSubstances;
        }

        public void OnClickContainerBtn()
        {
            _signalBus.Fire(new MachineWorkSignal()
            {
                MachinesType = MachinesTypes.WeightingMachine
            });
            _currentDiscardWeight = _snapZone.HeldItem.GetComponent<BaseContainer>().GetWeight();
            _weightText.text = "0.0000g";
        }
        

        private void ResetValues()
        {
            _weightText.text = "0.0000g";
            _currentDiscardWeight = 0;
        }

        private void ChangeValues()
        {
            var res = _snapZone.HeldItem.GetComponent<BaseContainer>().GetWeight()-_currentDiscardWeight;
            _weightText.text = res.ToString("0.0000", CultureInfo.InvariantCulture) + "g";
        }
    }
}
 