using AnchorCntrls;
using BNG;
using Substances;
using UnityEngine;
using UnityEngine.UI;

namespace Containers
{
    public class MixContainer : TransferSubstanceContainer
    {
        private AnchorCntrl _anchor;

        [SerializeField]
        private GameObject _hintCanvas;
        [SerializeField]
        private ChemicGlassCanvasCntrl _cgCanvasCntrl;
        private bool _hintCanvasIsOn = false;

        /*private void Start()
        {
            if (!IsNull(_hintCanvas))
                _hintCanvas.SetActive(false);
            if (gameObject.GetComponentsInChildren<ChemicGlassCanvasCntrl>().Length == 0
                || IsNull(gameObject.GetComponentsInChildren<ChemicGlassCanvasCntrl>()[0]))
            {
                return;
            }
            _cgCanvasCntrl = gameObject.GetComponentsInChildren<ChemicGlassCanvasCntrl>()[0].GetComponent<ChemicGlassCanvasCntrl>();
            _cgCanvasCntrl.target = _XRrig.GetComponentsInChildren<BNGPlayerController>()[0].transform;
        }*/
        /*private void Update()
        {
            if (IsNull(_hintCanvas))
                return;
            /*if (_rightGrabber.HeldGrabbable is null || _rightGrabber.HeldGrabbable.gameObject != gameObject)
            {
                _hintCanvasIsOn = false;
                _hintCanvas.SetActive(_hintCanvasIsOn);
                return;
            }*/
            if (InputBridge.Instance.AButtonDown)
            {
                _hintCanvasIsOn = !_hintCanvasIsOn;
            }
            _hintCanvas.SetActive(_hintCanvasIsOn);
        }*/
        public override bool AddSubstance(Substance substance)
        {
            if (CurrentCountSubstances == 0)
            {
                _substancesCntrl.AddSubstance(this,substance);
            }
            else
            {
                _substancesCntrl.AddSubstance(this, _substancesCntrl.MixSubstances(substance, GetNextSubstance()));
            }
            UpdateDisplaySubstance();
            
            /*if (!IsNull(_hintCanvas))
            {
                _hintCanvas.GetComponentsInChildren<Text>()[0].text = GetStringStack();
            }*/
            return true;
        }
        
        public AnchorCntrl Anchor => _anchor;

        public void AddAnchor(AnchorCntrl anchor)
        {
            _anchor = anchor;
        }
    }
}
