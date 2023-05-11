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
            _dozeText.text = _dozatorMachineCntrl.GetDoze();
            //dozButton.Reset();
        }
    }
}
