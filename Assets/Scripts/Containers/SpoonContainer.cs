using Substances;

namespace Containers
{
    public class SpoonContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(Substance substance)
        {
            if (CurrentCountSubstances > 0)
            {
                return false;
            }
            _substancesCntrl.AddSubstance(this, substance);
            UpdateDisplaySubstance();
            return true;
        }
    }
}
