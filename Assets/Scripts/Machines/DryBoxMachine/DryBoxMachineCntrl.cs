using BNG;
using Containers;
using Installers;
using Substances;
using UnityEngine;
using Assets.Scripts.UI;
using Zenject;

namespace Machines.DryBoxMachine
{
    public class DryBoxMachineCntrl : MonoBehaviour
    {
        [SerializeField] private SnapZone _snapZone;

        [SerializeField] private UIButton _startBtn;

        private bool _isEnter, _isStart;

        private SubstancesCntrl _substancesCntrl;
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus, SubstancesCntrl substancesCntrl)
        {
            _signalBus = signalBus;
            _substancesCntrl = substancesCntrl;
        }

        private void Awake()
        {
            _startBtn.ClickBtnEvent += OnClickStartBtn;
        }

        private void OnDestroy()
        {
            _startBtn.ClickBtnEvent -= OnClickStartBtn;
        }

        private void Update()
        {
            if (_snapZone.HeldItem is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>() is null ||
                _snapZone.HeldItem.gameObject.GetComponent<MixContainer>().ContainerType != ContainersTypes.PetriContainer)
            {
                _isEnter = false;
                return;
            }
            
            if (!_isEnter)
            {
                OnEnterObject();
            }
        }
        
        private void OnEnterObject()
        {
            _isEnter = true;
            _signalBus.Fire(new MachineWorkSignal() { MachinesType = MachinesTypes.DryBoxMachine, SubstancePropertyBase = _snapZone.HeldItem?.gameObject?.GetComponent<MixContainer>().GetNextSubstance()?.SubstanceProperty});
        }

        private void OnClickStartBtn()
        {
            if(_startBtn.State)
            {
                _signalBus.Fire(new MachineWorkSignal(){ MachinesType = MachinesTypes.DryBoxMachine, SubstancePropertyBase = _snapZone.HeldItem?.gameObject?.GetComponent<MixContainer>().GetNextSubstance()?.SubstanceProperty });
                return;
            }

            var temp = _snapZone.HeldItem.gameObject.GetComponent<SubstanceContainer>();
            var res = _substancesCntrl.DrySubstance(temp);
            if (!res)
            {
                _substancesCntrl.SplitSubstances(temp);
            }
            _signalBus.Fire(new MachineWorkSignal() { MachinesType = MachinesTypes.DryBoxMachine, SubstancePropertyBase = temp.GetNextSubstance()?.SubstanceProperty });
        }
    }
}


