using Substances;
using UnityEngine;

namespace Containers
{
    public class SpoonContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(Substance substance)
        {
            Debug.Log("Spoon" +CurrentCountSubstances);
            if (CurrentCountSubstances > 0)
            {
                return false;
            }
            _substancesCntrl.AddSubstance(this, substance);
            return true;
        }
    }
}
