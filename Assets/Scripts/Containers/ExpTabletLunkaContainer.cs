using Machines;
using Substances;
using UnityEngine;

namespace Containers
{
    public class ExpTabletLunkaContainer : TransferSubstanceContainer
    {
        [SerializeField] private ExpTabletMachineCntrl _tabletMachineCntrl;

        protected override bool AddSubstance(SubstanceSplit substance)
        {
            if (CurrentSubstance is null)
            {
                CurrentSubstance = _substancesCntrl.AddSubstance(substance, MaxVolume);
            }
            else
            {
                CurrentSubstance = _substancesCntrl.MixSubstances(CurrentSubstance, substance);
            }
            UpdateDisplaySubstance();
            _tabletMachineCntrl.CheckCompliteFill();
            return true;
        }
    }
}
