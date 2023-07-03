using UnityEngine;
using UnityEngine.UI;

namespace Machines
{
    
    public class ScannerMachineCanvas : MonoBehaviour
    {
        private bool _buttonIsOn = false;
        [SerializeField] private Image _img;
        [SerializeField] ScannerMachineCntrl _scannerMachineCntrl;
        
        public void clickButton()
        {
            _buttonIsOn = !_buttonIsOn;
            if (_buttonIsOn)
            {
                gameObject.GetComponent<Animator>().Play("ButtonOn");
                _img.color = new Color(76, 255, 0, 0.8f);
                TryStart();
            }
            else
            {
                gameObject.GetComponent<Animator>().Play("ButtonOff");
                _img.color = new Color(255, 103, 132, 0.6f);
                TryStop();
            }

        }
        public void TryStart()
        {
            _scannerMachineCntrl.OnStartWork();
        }
        public void TryStop()
        {
            _scannerMachineCntrl.OnFinishWork();
        }
    }
}

