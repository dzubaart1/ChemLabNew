using Machines;
using Substances;
using UnityEngine;

namespace Containers
{
    public class ExpTabletLunkaContainer : TransferSubstanceContainer
    {
        [Header("ExpTabletLunka Container Params")]
        [SerializeField] private ExpTabletMachineCntrl _tabletMachineCntrl;

        public override bool AddSubstance(Substance substance)
        {
            if (CurrentCountSubstances == 0)
            {
                _substancesCntrl.AddSubstance(this,substance);
            }
            else
            {
                _substancesCntrl.MixSubstances(this, substance);
            }
            _tabletMachineCntrl.CheckCompliteFill();
            return true;
        }
    }
}
