using System.Globalization;
using Cups;
using Substances;
using UnityEngine;

namespace Containers
{
    public class DozatorMachineContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(SubstanceContainer substanceContainer)
        {
            if (substanceContainer.CurrentSubstancesList.Count == 0 || !IsEnable())
            {
                return false;
            }
            
            var temp = substanceContainer.CurrentSubstancesList.Peek();
            var addingRes = _substancesCntrl.AddSubstance(temp, MaxVolume);
            Debug.Log(MaxVolume + " " + temp.GetWeight().ToString(CultureInfo.InvariantCulture));
            CurrentSubstancesList.Push(addingRes);
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
