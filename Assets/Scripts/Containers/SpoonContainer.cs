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

            if (ContainerType != ContainersTypes.TweezersContainer && ContainerType != ContainersTypes.MagicSpatulaContainer && ContainerType != ContainersTypes.SpatulaContainer)
            {
                if (GetNextSubstance().GetWeight() > 1)
                    _mainSubPrefab.transform.localScale = new Vector3(GetNextSubstance().GetWeight() / MaxVolume, GetNextSubstance().GetWeight() / MaxVolume, GetNextSubstance().GetWeight() / MaxVolume);
                else if (GetNextSubstance().GetWeight() > 0.1)
                {
                    _mainSubPrefab.transform.localScale = new Vector3(GetNextSubstance().GetWeight() / MaxVolume * 4f, GetNextSubstance().GetWeight() / MaxVolume* 4f, GetNextSubstance().GetWeight() / MaxVolume* 4f);
                }
                else 
                {
                    _mainSubPrefab.transform.localScale = new Vector3(1 / MaxVolume, 1 / MaxVolume, 1 / MaxVolume);
                }
            }
            return true;
        }
    }
}
