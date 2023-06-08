using AnchorCntrls;
using BNG;
using Substances;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Containers
{
    public class MixContainer : TransferSubstanceContainer
    {
        private AnchorCntrl _anchor;

        

        
        
        public override bool AddSubstance(Substance substance)
        {
            Debug.Log(CurrentCountSubstances);
            if (CurrentCountSubstances == 0)
            {
                _substancesCntrl.AddSubstance(this,substance);
            }
            else
            {
                _substancesCntrl.MixSubstances(this, substance);
            }
            
            UpdateDisplaySubstance();
            
            if (!IsNull(_hintCanvas))
            {
                _hintCanvas.GetComponentsInChildren<Text>()[0].text = GetStringStack();
            }
            return true;
        }
        
        public AnchorCntrl Anchor => _anchor;

        public void AddAnchor(AnchorCntrl anchor)
        {
            _anchor = anchor;
        }
    }
}
