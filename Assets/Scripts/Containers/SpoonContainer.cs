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

            if (ContainerType != ContainersTypes.TweezersContainer && ContainerType != ContainersTypes.MagicSpatulaContainer && ContainerType == ContainersTypes.SpatulaContainer)
            {
                _mainSubPrefab.transform.localScale = new Vector3(GetNextSubstance().GetWeight() / MaxVolume, GetNextSubstance().GetWeight() / MaxVolume, GetNextSubstance().GetWeight() / MaxVolume);
            }
            return true;
        }
    }
}
