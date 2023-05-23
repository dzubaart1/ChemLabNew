using Cups;
using Substances;

namespace Containers
{
    public class DozatorMachineContainer : TransferSubstanceContainer
    {
        protected override bool AddSubstance(SubstanceSplit substance)
        {
            if (CurrentSubstance is not null || !IsEnable())
            {
                return false;
            }

            CurrentSubstance = substance;
            UpdateDisplaySubstance();
            _snapZone.HeldItem.GetComponent<DozatorCup>().IsDirty = true;
            return true;
        }
        
        protected override bool IsEnable()
        {
            return _snapZone.HeldItem is not null;
        }
    }
}
