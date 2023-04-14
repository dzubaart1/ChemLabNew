using Machines.StirringMachine;
using UnityEngine;

namespace Machines
{
    public class StirringMachineCanvas : MonoBehaviour
    {
        private bool _isHeating;
        private bool _isStirring;
        private bool _isStart = false;

        [SerializeField] private Material _heatingButtonMaterial, _stirringButtonMaterial;
        [SerializeField] private Texture _onTexture, _offTexture;
        [SerializeField] StirringMachineCntrl _heatMachineCntrl;
        private void Start()
        {
            _heatingButtonMaterial.mainTexture = _offTexture;
            _stirringButtonMaterial.mainTexture = _offTexture;
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
            _heatingButtonMaterial.mainTexture = _isHeating ? _onTexture : _offTexture;
            
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
            _stirringButtonMaterial.mainTexture = _isStirring ? _onTexture : _offTexture;
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
