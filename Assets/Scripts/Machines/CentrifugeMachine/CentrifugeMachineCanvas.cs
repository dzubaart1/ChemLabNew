using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Machines.CentrifugeMachine
{
    public class CentrifugeMachineCanvas : MonoBehaviour
    {
        [SerializeField] private CentrifugeMachineCntrl _centrifugeMachineCntrl;
        [SerializeField] private GameObject _startBtn;
        [SerializeField] private GameObject _powerBtn;
        [SerializeField] private GameObject animatedPart;
        [SerializeField] private List<Sprite> powerButtonSprites;
        [SerializeField] private List<Sprite> startButtonSprites;

        private bool _isWorked, _isPowerOn;

        private void Start()
        {
            _isWorked = false;
            _isPowerOn = false;
            _startBtn.GetComponent<Button>().enabled = false;
        }

        public void OnClickPowerBtn()
        {
            
            _isPowerOn = !_isPowerOn;
            if (_isPowerOn)
            {
                _powerBtn.GetComponent<Image>().sprite = powerButtonSprites[1];
                _startBtn.GetComponent<Button>().enabled = true;
            }

            if (!_isPowerOn)
            {
                _powerBtn.GetComponent<Image>().sprite = powerButtonSprites[0];
                if (!_isWorked)
                {
                    _startBtn.GetComponent<Button>().enabled = false;
                    _centrifugeMachineCntrl.OnFinishWork();
                }
                else
                {
                    // АТАТАТ Так нельзя
                }
            }
            Debug.Log("power clicked, power is "+ _isPowerOn + " start is " + _isWorked + " start enable is " + _startBtn.GetComponent<Button>().enabled);
            
        }
        
        public void OnClickStartBtn()
        {
            _isWorked = !_isWorked;
            Debug.Log("start clicked, power is "+ _isPowerOn + " start is " + _isWorked + " start enable is " + _startBtn.GetComponent<Button>().enabled);
            if (_isWorked)
            {
                _startBtn.GetComponent<Image>().sprite = startButtonSprites[1];
                animatedPart.GetComponent<Animator>().enabled = true;
                _centrifugeMachineCntrl.OnStartWork();
            }
            else
            {
                _startBtn.GetComponent<Image>().sprite = startButtonSprites[0];
                animatedPart.GetComponent<Animator>().enabled = false;
            }
        }
    }
}