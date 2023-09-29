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
            _mainSubPrefab.transform.localScale = new Vector3(1, GetNextSubstance().GetWeight() / MaxVolume, 1);
            return true;
        }
    }
}
