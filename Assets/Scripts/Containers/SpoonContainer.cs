using Substances;

namespace Containers
{
    public class SpoonContainer : TransferSubstanceContainer
    {
        protected override bool AddSubstance(SubstanceSplit substance)
        {
            if (CurrentSubstance is not null)
            {
                return false;
            }

            CurrentSubstance = _substancesCntrl.AddSubstance(substance, MaxVolume);
            UpdateDisplaySubstance();
            return true;
        }
    }
}
