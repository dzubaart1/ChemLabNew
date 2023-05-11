using Substances;
using UnityEngine;

namespace Containers
{
    public class MeasuringContainer : TransferSubstanceContainer
    {
        protected override bool AddSubstance(Substance substance)
        {
            if (Substance is not null) return false;
            var newSubstance = substance;
            if (substance.Weight > MaxVolume)
            {
                newSubstance = new Substance(substance.SubParams, MaxVolume);
            }
            _basePrefab.transform.localScale = new Vector3(1, 1, substance.Weight / 10);
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
                _basePrefab.transform.localScale = new Vector3(1, 1, 10);
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
