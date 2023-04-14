using Cups;
using Substances;
using UnityEngine;

namespace Containers
{
    public class DozatorMachineContainer : TransferSubstanceContainer
    {
        protected override bool AddSubstance(Substance substance)
        {
            if (Substance is not null || !IsEnable())
            {
                return false;
            }
            
            UpdateSubstance(substance);
            _snapZone.HeldItem.GetComponent<DozatorCup>().IsDirty = true;
            return true;
        }

        protected override bool RemoveSubstance(float maxVolume)
        {
            if (Substance is null || !IsEnable())
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

        protected override bool IsEnable()
        {
            return _snapZone.HeldItem is not null;
        }
    }
}
