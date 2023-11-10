using Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Machines.CentrifugeMachine
{
    public class CentrifugeMachineCanvas : MonoBehaviour
    {
        [SerializeField] private CentrifugeMachineCntrl _centrifugeMachineCntrl;
        [SerializeField] private GameObject _startBtn;
        [SerializeField] private GameObject _powerBtn;
        [SerializeField] private Sprite _onPowerBtnSprite;
        [SerializeField] private Sprite _offPowerBtnSprite;
        [SerializeField] private Sprite _onStartBtnSprite;
        [SerializeField] private Sprite _offStartBtnSprite;

        private bool _isStart, _isPower;

        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<LoadSignal>(OnLoadScene);
        }
        
        private void OnLoadScene()
        {
            if (_startBtn.GetComponent<Image>().sprite == _onStartBtnSprite)
            {
                _isStart = true;
            }
            else
            {
                _isStart = false;
            }

            if (_powerBtn.GetComponent<Image>().sprite == _onPowerBtnSprite)
            {
                _isPower = true;
            }
            else
            {
                _isPower = false;
            }
        }
        
        public void OnClickPowerBtn()
        {
            _isPower = !_isPower;
            
            _powerBtn.GetComponent<Image>().sprite = _isPower ? _onPowerBtnSprite : _offPowerBtnSprite;
            OnClickBtns();
        }
        
        public void OnClickStartBtn()
        {
            _isStart = !_isStart;
            _startBtn.GetComponent<Image>().sprite = _isStart ? _onStartBtnSprite : _offStartBtnSprite;
            OnClickBtns();
        }

        private void OnClickBtns()
        {
            if (_isStart && _isPower)
            {
                _centrifugeMachineCntrl.OnStartWork();
            }

            if (!_isStart && !_isPower)
            {
                _centrifugeMachineCntrl.OnFinishWork();
            }
        }
    }
}

