using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI;
using Zenject;
using Installers;

namespace Machines
{
    
    public class ScannerMachineCanvas : MonoBehaviour
    {
        [SerializeField] private UIButton _scannerBtn;
        [SerializeField] ScannerMachineCntrl _scannerMachineCntrl;

        private SignalBus _signalBus;

        private void Awake()
        {
            _scannerBtn.ClickBtnEvent += OnClickScannerBtn;
        }

        private void OnDestroy()
        {
            _scannerBtn.ClickBtnEvent -= OnClickScannerBtn;
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<LoadSignal>(OnLoadScene);
        }

        public void OnClickScannerBtn()
        {
            if (!_scannerBtn.State)
            {
                StopAnimation();
                _scannerMachineCntrl.OnFinishWork();
            }
            else
            {
                StartAnimation();
            }
        }

        private void StartAnimation()
        {
            gameObject.GetComponent<Animator>().Play("ButtonOn");
        }

        private void StopAnimation()
        {
            gameObject.GetComponent<Animator>().Play("ButtonOff");
            _scannerMachineCntrl.HideResultCanvas();
        }

        private void OnLoadScene()
        {
            if (!_scannerBtn.State)
            {
                StopAnimation();
            }
            else
            {
                StartAnimation();
            }
        }
    }
}

