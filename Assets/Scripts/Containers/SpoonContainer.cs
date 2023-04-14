using Substances;
using UnityEngine;

namespace Containers
{
    public class SpoonContainer : TransferSubstanceContainer
    {
        protected override bool AddSubstance(Substance substance)
        {
            if (Substance is not null)
            {
                return false;
            }
            var newSubstance = Substance;
            if (substance.Weight > MaxVolume)
            {
                newSubstance = new Substance(substance.SubParams, MaxVolume);
            }
            UpdateSubstance(newSubstance);
            return true;

        }

        protected override bool RemoveSubstance(float maxVolume)
        {
            if (Substance is null)
            {
                return false;
            }
            if (maxVolume >= Substance.Weight)
            {
                UpdateSubstance(null);
            }
            else
            {
                Substance.RemoveSubstanceWeight(maxVolume);
            }
            return true;
        }
    }
}
