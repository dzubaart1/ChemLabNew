using UnityEngine;
using UnityEngine.UI;

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
