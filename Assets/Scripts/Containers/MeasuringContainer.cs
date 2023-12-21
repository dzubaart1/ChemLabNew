using LiquidVolumeFX;
using Substances;
using UnityEngine;

namespace Containers
{
    public class MeasuringContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(Substance substance)
        {
            if (CurrentCountSubstances > 0 || substance is null)
            {
                return false;
            }
            _substancesCntrl.AddSubstance(this,substance);

            float weight = GetWeight();

            _liquidVolume.level = weight / MaxVolume;
            _mainSubPrefab.transform.localScale = new Vector3(1, weight / MaxVolume, 1);
            return true;
        }
    }
}
