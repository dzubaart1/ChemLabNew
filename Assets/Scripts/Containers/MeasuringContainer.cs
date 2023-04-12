using Substances;
using UnityEngine;

namespace Containers
{
    public class MeasuringContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(Substance substance)
        {
            if (Substance is not null) return false;
            if (substance.Weight > MaxVolume)
            {
                var newSubstance = new Substance(substance.SubParams, MaxVolume);
                Substance = newSubstance;
            }
            else
            {
                Substance = substance;
            }
            _baseFormPrefab.transform.localScale = new Vector3(1, 1, substance.Weight / 10);
            _baseFormPrefab.GetComponent<MeshRenderer>().material.color = substance.SubParams.Color;
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
                _baseFormPrefab.transform.localScale = new Vector3(1, 1, 10);
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
