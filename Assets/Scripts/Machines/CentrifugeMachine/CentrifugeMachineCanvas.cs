using System;
using UnityEngine;
using UnityEngine.UI;

namespace Machines.CentrifugeMachine
{
    public class CentrifugeMachineCanvas : MonoBehaviour
    {
        [SerializeField] private CentrifugeMachineCntrl _centrifugeMachineCntrl;
        [SerializeField] private Button _startBtn;
        [SerializeField] private GameObject animatedPart;

        private bool _isWorked, _isPowerOn;

        private void Start()
        {
            _isWorked = false;
            _isPowerOn = false;
            _startBtn.enabled = false;
        }

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
            Debug.Log("power clicked, power is "+ _isPowerOn + " start is " + _isWorked + " start enable is " + _startBtn.enabled);
            
        }
        
        public void OnClickStartBtn()
        {
            _isWorked = !_isWorked;
            Debug.Log("start clicked, power is "+ _isPowerOn + " start is " + _isWorked + " start enable is " + _startBtn.enabled);
            if (_isWorked)
            {
                animatedPart.GetComponent<Animator>().enabled = true;
                _centrifugeMachineCntrl.OnStartWork();
            }
            else
            {
                animatedPart.GetComponent<Animator>().enabled = false;
            }
        }
    }
}