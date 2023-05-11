using Substances;
using UnityEngine;

namespace Containers
{
    public class SpoonContainer : TransferSubstanceContainer
    {
        protected override bool AddSubstance(Substance substance)
        {
            Debug.Log("Here 1");
            if (Substance is not null)
            {
                return false;
            }
            Debug.Log("Here 2");
            var newSubstance = substance;
            if (substance.Weight > MaxVolume)
            {
                newSubstance = new Substance(substance.SubParams, MaxVolume);
            }
            Debug.Log(newSubstance.SubParams.SubName);
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
