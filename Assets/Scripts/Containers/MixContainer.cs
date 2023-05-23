using AnchorCntrls;
using Substances;

namespace Containers
{
    public class MixContainer : TransferSubstanceContainer
    {
        private AnchorCntrl _anchor;
        
        protected override bool AddSubstance(SubstanceSplit newSub)
        {
            if (CurrentSubstance is null)
            {
                CurrentSubstance = _substancesCntrl.AddSubstance(newSub, MaxVolume);
            }
            else
            {
                CurrentSubstance = _substancesCntrl.MixSubstances(CurrentSubstance, newSub);
            }
            UpdateDisplaySubstance();
            return true;
        }
        
        public AnchorCntrl Anchor => _anchor;

        public void AddAnchor(AnchorCntrl anchor)
        {
            _anchor = anchor;
        }
    }
}
