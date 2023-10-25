using System.Collections.Generic;
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
        [SerializeField] private GameObject animatedPart;
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

        private void Start()
        {
            _isStart = false;
            _isPower = false;
            _startBtn.GetComponent<Button>().enabled = false;
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
            if (_isPower)
            {
                _powerBtn.GetComponent<Image>().sprite = _offPowerBtnSprite;
                _startBtn.GetComponent<Button>().enabled = true;
            }

            if (!_isPower)
            {
                _powerBtn.GetComponent<Image>().sprite = _onPowerBtnSprite;
                if (!_isStart)
                {
                    _startBtn.GetComponent<Button>().enabled = false;
                    _centrifugeMachineCntrl.OnFinishWork();
                }
            }
        }
        
        public void OnClickStartBtn()
        {
            _isStart = !_isStart;
            if (_isStart)
            {
                _startBtn.GetComponent<Image>().sprite = _offStartBtnSprite;
                animatedPart.GetComponent<Animator>().enabled = true;
                gameObject.GetComponent<AudioSource>().Play();
                _centrifugeMachineCntrl.OnStartWork();
            }
            else
            {
                _startBtn.GetComponent<Image>().sprite = _onStartBtnSprite;
                animatedPart.GetComponent<Animator>().enabled = false;
                gameObject.GetComponent<AudioSource>().Stop();
            }
        }
    }
}

