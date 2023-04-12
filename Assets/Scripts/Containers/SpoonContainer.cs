using Substances;
using UnityEngine;

namespace Containers
{
    public class SpoonContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(Substance substance)
        {
            if (Substance is not null)
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
            return true;

        }

        public override bool RemoveSubstance(float maxVolume)
        {
            if (Substance is null)
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
    }
}
