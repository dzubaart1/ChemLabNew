using Cups;
using Substances;
using UnityEngine;

namespace Containers
{
    public class DozatorMachineContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(Substance substance)
        {
            if (Substance is not null || !IsEnable())
            {
                return false;
            }

            if (substance.Weight > MaxVolume)
            {
                var newSubstance = new Substance(substance.SubParams, MaxVolume);
                Substance = newSubstance;
            }
            else
            {
                Substance = substance;
            }

            _baseFormPrefab.GetComponent<MeshRenderer>().material.color = Substance.SubParams.Color;
            _baseFormPrefab.SetActive(true);
            _snapZone.HeldItem.GetComponent<DozatorCup>().IsDirty = true;
            return true;
        }

        public override bool RemoveSubstance(float maxVolume)
        {
            if (Substance is null || !IsEnable())
            {
                return false;
            }

            if (maxVolume >= Substance.Weight)
            {
                Substance = null;
                _baseFormPrefab.SetActive(false);
            }
            else
            {
                Substance.RemoveSubstanceWeight(maxVolume);
            }
            return true;
        }

        public override bool IsEnable()
        {
            return _snapZone.HeldItem is not null;
        }
    }
}
