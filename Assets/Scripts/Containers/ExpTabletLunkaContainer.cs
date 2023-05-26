using Machines;
using Substances;
using UnityEngine;

namespace Containers
{
    public class ExpTabletLunkaContainer : TransferSubstanceContainer
    {
        [SerializeField] private ExpTabletMachineCntrl _tabletMachineCntrl;

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
            _tabletMachineCntrl.CheckCompliteFill();
            UpdateDisplaySubstance();
            return true;
        }
    }
}
