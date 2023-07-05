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
                return true;
            }
            return false;
        }
    }
}
