using AnchorCntrls;
using Substances;

namespace Containers
{
    public class MixContainer : TransferSubstanceContainer
    {
        private AnchorCntrl _anchor;
        public override bool AddSubstance(SubstanceContainer substanceContainer)
        {
            if (CurrentSubstancesList.Count > 1)
            {
                return false;
            }
            
            var temp = substanceContainer.CurrentSubstancesList.Peek();
            
            var addingRes = _substancesCntrl.AddSubstance(temp, MaxVolume);;
            if(CurrentSubstancesList.Count == 1)
            {
                addingRes = _substancesCntrl.MixSubstances(substanceContainer.CurrentSubstancesList.Peek(), CurrentSubstancesList.Pop());
            }
            
            CurrentSubstancesList.Push(addingRes);
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
