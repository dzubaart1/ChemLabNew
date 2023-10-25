using Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Machines
{
    public class StirringMachineCanvas : MonoBehaviour
    {
        [SerializeField] private Image _heatingButtonImage, _stirringButtonImage;
        [SerializeField] private Sprite _onSprite, _offSprite;
        [SerializeField] private StirringMachineCntrl _heatMachineCntrl;

        public bool _isHeating;
        public bool _isStirring;

        private bool _isStart;
        
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<LoadSignal>(OnLoadSignal);
        }
        
        public void ClickHeatingBtn()
        {
            _isHeating = !_isHeating;
            if (_isHeating)
            {
                TryStart();
            }
            else
            {
                TryStop();
            }

            ChangeSpriteByUIBtnState(_heatingButtonImage, _isHeating);
        }
        public void ClickStirringBtn()
        {
            _isStirring = !_isStirring;
            if (_isStirring)
            {
                TryStart();
            }
            else
            {
                TryStop();
            }

            ChangeSpriteByUIBtnState(_stirringButtonImage, _isStirring);
        }

        private void OnLoadSignal()
        {
            if (_heatingButtonImage.sprite == _onSprite)
            {
                _isHeating = true;
            }
            else
            {
                _isHeating = false;
            }

            if (_stirringButtonImage.sprite == _onSprite)
            {
                _isStirring = true;
            }
            else
            {
                _isStirring = false;
            }

            if (_isHeating && _isStirring)
            {
                _isStart = true;
            }
            else
            {
                _isStart = false;
            }
        }

        private void ChangeSpriteByUIBtnState(Image image, bool state)
        {
            if (state)
            {
                image.sprite = _onSprite;
            }
            else
            {
                image.sprite = _offSprite;
            }
        }

        private void TryStart()
        {
            if (_isHeating && _isStirring)
            {
                Debug.Log("Start Canvas");
                _heatMachineCntrl.OnStartWork();
            }
        }
        
        private void TryStop()
        {
            if (!_isHeating && !_isStirring)
            {
                Debug.Log("Stop Canvas");
                _heatMachineCntrl.OnFinishWork();
            }
        }
    }
}
