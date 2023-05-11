using UnityEngine;
using UnityEngine.UI;

namespace Machines.CentrifugeMachine
{
    public class CentrifugeMachineCanvas : MonoBehaviour
    {
        [SerializeField] private CentrifugeMachineCntrl _centrifugeMachineCntrl;
        [SerializeField] private Button _startBtn;

        private bool _isWorked, _isPowerOn;

        public void OnClickPowerBtn()
        {
            _isPowerOn = !_isPowerOn;
            if (_isPowerOn)
            {
                _startBtn.enabled = true;
            }

            if (!_isPowerOn)
            {
                if (!_isWorked)
                {
                    _startBtn.enabled = false;
                    _centrifugeMachineCntrl.OnFinishWork();
                }
                else
                {
                    // АТАТАТ Так нельзя
                }
            }
            
        }
        
        public void OnClickStartBtn()
        {
            _isWorked = !_isWorked;
            if (_isWorked)
            {
                _centrifugeMachineCntrl.OnStartWork();
            }
        }
    }
}