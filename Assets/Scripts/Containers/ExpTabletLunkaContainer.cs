using Substances;

namespace Containers
{
    public class ExpTabletLunkaContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(Substance substance)
        {
            if (CurrentCountSubstances == 0)
            {
                _substancesCntrl.AddSubstance(this,substance);
            }
            _substancesCntrl.MixSubstances(this,substance);
            return true;
        }
    }
}