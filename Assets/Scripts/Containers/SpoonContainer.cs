using Substances;
using UnityEngine;

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

            float weight = GetWeight();

            if(ContainerType == ContainersTypes.SpoonContainer)
            {
                Vector3 scale = new Vector3(1 / MaxVolume, 1 / MaxVolume, 1 / MaxVolume);
                if (weight > 1)
                {
                    scale = new Vector3(weight / MaxVolume, weight / MaxVolume, weight / MaxVolume);
                }
                else if (weight > 0.1)
                {
                    scale = new Vector3(weight / MaxVolume * 4f, weight / MaxVolume * 4f, weight / MaxVolume * 4f);
                }

                _mainSubPrefab.transform.localScale = scale;
            }
            return true;
        }
    }
}
