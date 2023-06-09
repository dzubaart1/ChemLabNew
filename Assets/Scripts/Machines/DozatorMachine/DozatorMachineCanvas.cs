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

        public void OnClickDozeBtn()
        {
            _dozeText.text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", _dozatorMachineCntrl.GetDoze());
        }
    }
}
