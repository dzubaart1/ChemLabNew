using Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Assets.Scripts.UI;

namespace Machines.CentrifugeMachine
{
    public class CentrifugeMachineCanvas : MonoBehaviour
    {
        [SerializeField] private CentrifugeMachineCntrl _centrifugeMachineCntrl;
        [SerializeField] private UIButton _startBtn;
        [SerializeField] private UIButton _powerBtn;

        private SignalBus _signalBus;

        private void Awake()
        {
            _startBtn.ClickBtnEvent += OnClickBtns;
            _powerBtn.ClickBtnEvent += OnClickBtns;
        }

        private void OnDestroy()
        {
            _startBtn.ClickBtnEvent -= OnClickBtns;
            _powerBtn.ClickBtnEvent -= OnClickBtns;
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<LoadSignal>(OnLoadScene);
        }
        
        private void OnClickBtns()
        {
            if (_startBtn.State && _powerBtn.State)
            {
                _centrifugeMachineCntrl.OnStartWork();
                return;
            }
            
            _centrifugeMachineCntrl.OnFinishWork();
        }

        private void OnLoadScene()
        {
            if (_startBtn.State && _powerBtn.State)
            {
                _centrifugeMachineCntrl.StartAnimation();
                return;
            }

            _centrifugeMachineCntrl.StopAnimation();
        }
    }
}

