using Substances;
using UnityEngine;

namespace Containers
{
    public class MeasuringContainer : TransferSubstanceContainer
    {
        protected override bool AddSubstance(SubstanceSplit substance)
        {
            if (CurrentSubstance is not null)
            {
                return false;
            }

            CurrentSubstance = _substancesCntrl.AddSubstance(substance, MaxVolume);
            _mainSubPrefab.transform.localScale = new Vector3(1, 1, substance.GetWeight() / 10);
            UpdateDisplaySubstance();
            return true;
        }
    }
}
