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
            
            Debug.Log(substance.GetWeight());   
            _substancesCntrl.AddSubstance(this, substance);
            if (ContainerType == ContainersTypes.TweezersContainer || ContainerType == ContainersTypes.SpatulaContainer || ContainerType == ContainersTypes.MagicSpatulaContainer)
                return true;
            _mainSubPrefab.transform.localScale = GetNextSubstance().GetWeight() > 1
                ? new Vector3(1, GetNextSubstance().GetWeight() / 4, 1)
                : GetNextSubstance().GetWeight() > 0.1
                    ? new Vector3(GetNextSubstance().GetWeight()+0.4f  , GetNextSubstance().GetWeight(), GetNextSubstance().GetWeight()+0.4f )
                    : new Vector3(0.5f , GetNextSubstance().GetWeight()  * 2, 0.5f);

            return true;
        }
    }
}
