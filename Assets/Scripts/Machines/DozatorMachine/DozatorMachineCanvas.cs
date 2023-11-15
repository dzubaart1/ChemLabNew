using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


namespace Machines.DozatorMachine
{
    public class DozatorMachineCanvas : MonoBehaviour
    {
        [SerializeField] private Text _dozeText;
        [SerializeField] private DozatorMachineCntrl _dozatorMachineCntrl;

        private const float MAX_BREAK = 3f;
        
        private bool _isStartTimer;
        private float _timer;

        private void Update()
        {
            if (_isStartTimer)
            {
                _timer += Time.deltaTime;
            }

            if (_timer >= MAX_BREAK)
            {
                _isStartTimer = false;
                _timer = 0;
            }
        }

        public void OnClickDozeBtn()
        {
            if (!_isStartTimer)
            {
                _dozeText.text = String.Format(CultureInfo.InvariantCulture, "{0:#.00}", _dozatorMachineCntrl.GetDozeFromTask());
                _isStartTimer = true;
            }
        }

        public float GetDoze()
        {
            return _dozatorMachineCntrl.GetDoze();
        }

        public void SetDoze(float volume)
        {
            _dozatorMachineCntrl.SetDoze(volume);
            _dozeText.text = String.Format(CultureInfo.InvariantCulture, "{0:#.00}", volume);
        }
    }
}
