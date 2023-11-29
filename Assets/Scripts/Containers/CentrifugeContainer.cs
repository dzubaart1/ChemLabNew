using UnityEngine;

namespace Containers
{
    public class CentrifugeContainer : TransferSubstanceContainer
    {
        [SerializeField] private GameObject _pipetkaCanvas;
        private bool _isOpen;
        private const string OPENCUPANIMNAME = "Armature_001|ArmatureAction_001";
        private const string CLOSECUPANIMNAME = "Armature_001|ArmatureAction_002";
        
        private void Start()
        {
            gameObject.GetComponent<Animator>().Play(CLOSECUPANIMNAME);
        }

        public void OnClickCupBtn()
        {
            _isOpen = !_isOpen;
            
            if (_isOpen)
            {
                gameObject.GetComponent<Animator>().Play(OPENCUPANIMNAME);
                _pipetkaCanvas.transform.Rotate(0, 0,75);
                _pipetkaCanvas.transform.localPosition = new Vector3(-0.2f, 1, 0);
            }
            else
            {
                gameObject.GetComponent<Animator>().Play(CLOSECUPANIMNAME);
                _pipetkaCanvas.transform.Rotate(0, 0,-75);
                _pipetkaCanvas.transform.localPosition = new Vector3(0, 0, 0);
            }
        }

        public override bool IsEnable()
        {
            return _isOpen;
        }
    }
}