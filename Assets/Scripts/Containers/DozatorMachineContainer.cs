using Cups;
using Substances;

namespace Containers
{
    public class DozatorMachineContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(Substance substance)
        {
            if (substance is null || !IsEnable())
            {
                return false;
            }
            
            _substancesCntrl.AddSubstance(this, substance);
            _snapZone.HeldItem.GetComponent<DozatorCup>().IsDirty = true;
            UpdateDisplaySubstance();
            return true;
        }
        
        protected override bool IsEnable()
        {
            return _snapZone.HeldItem is not null;
        }
    }
}
