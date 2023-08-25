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
            _signalBus.Subscribe<CheckTasksSignal>(OnFinishWork);
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
            }
        }

        private void OnEnterObject()
        {
            var enterIntoMachineSignal = new EnterIntoMachineSignal()
            {
                MachinesType = MachinesTypes.WeightingMachine,
                ContainersType = _snapZone.HeldItem.GetComponent<BaseContainer>().ContainerType
            };
            _signalBus.Fire(enterIntoMachineSignal);
            _isEnter = true;
        }


        private void ResetValues()
        {
            _currentWeight = 0f;
            _weightText.text = "0.0000g";
        }

        private void OnFinishWork(CheckTasksSignal signal)
        {
            Debug.Log(signal.CurrentTask.ResultSubstance);
            if (!signal.CurrentTask.MachinesType.Equals(MachinesTypes.WeightingMachine) || signal.CurrentTask.ResultSubstance is null)
            {
                return;
            }
            
            Debug.Log("---HERERERER--");
            
            _currentWeight = _snapZone.HeldItem.GetComponent<BaseContainer>().GetWeight();
            _weightText.text = _currentWeight.ToString("0.0000", CultureInfo.InvariantCulture) + "g";
            
            _signalBus.Fire(new FinishMashineWorkSignal()
            {
                MachinesType = MachinesTypes.WeightingMachine,
                SubstancePropertyBase = _snapZone.HeldItem.GetComponent<BaseContainer>().GetNextSubstance()?.SubstanceProperty
            });
        }
    }
}
 