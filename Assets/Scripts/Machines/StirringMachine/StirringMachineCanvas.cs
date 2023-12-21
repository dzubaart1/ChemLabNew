using Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Assets.Scripts.UI;

namespace Machines
{
    public class StirringMachineCanvas : MonoBehaviour
    {
        [SerializeField] private StirringMachineCntrl _heatMachineCntrl;
        [SerializeField] private UIButton _stirringBtn;
        [SerializeField] private UIButton _heatingBtn;
        
        private SignalBus _signalBus;
        private bool _isStart;

        private void Awake()
        {
            _heatingBtn.ClickBtnEvent += OnClickBtns;
            _stirringBtn.ClickBtnEvent += OnClickBtns;
        }

        private void OnDestroy()
        {
            _heatingBtn.ClickBtnEvent -= OnClickBtns;
            _stirringBtn.ClickBtnEvent -= OnClickBtns;
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<LoadSignal>(OnLoadSignal);
        }

        private void OnClickBtns()
        {
            if(_heatingBtn.State && _stirringBtn.State)
            {
                _heatMachineCntrl.StartWork();
                _isStart = true;
                return;
            }
            if (_isStart)
            {
                _heatMachineCntrl.FinishWork();
                _isStart = false;
            }
        }

        private void OnLoadSignal()
        {
            if (_heatingBtn.State && _stirringBtn.State)
            {
                _heatMachineCntrl.StartStirringAnimation();
                _isStart = true;
                return;
            }

            _heatMachineCntrl.StopStirringAnimation();
            _isStart = false;
        }
    }
}
