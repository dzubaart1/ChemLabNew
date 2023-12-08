using UnityEngine;
using UnityEngine.UI;

namespace Machines
{
    
    public class ScannerMachineCanvas : MonoBehaviour
    {
        public bool _buttonIsOn = false;
        [SerializeField] private Image _img;
        [SerializeField] ScannerMachineCntrl _scannerMachineCntrl;
        
        public void clickButton()
        {
            _buttonIsOn = !_buttonIsOn;
            if (_buttonIsOn)
            {
                gameObject.GetComponent<Animator>().Play("ButtonOn");
                _img.color = new Color(76, 255, 0, 0.8f);
                _scannerMachineCntrl._buttonIsOn = true;
            }
            else
            {
                gameObject.GetComponent<Animator>().Play("ButtonOff");
                _img.color = new Color(255, 103, 132, 0.6f);
                TryStop();
            }

        }
        public void TryStop()
        {
            _buttonIsOn = false;
            _scannerMachineCntrl.OnFinishWork();
        }
    }
}

