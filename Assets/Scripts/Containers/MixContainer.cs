using AnchorCntrls;
using Substances;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class MixContainer : TransferSubstanceContainer
    {
        private SubstancesParamsCollection _substancesCollection;
        private AnchorCntrl _anchor;
        
        [Inject]
        public void Construct(SubstancesParamsCollection substancesCollection)
        {
            _substancesCollection = substancesCollection;
        }

        protected override bool AddSubstance(Substance substance)
        {
            var res = substance.SubParams;
            var weight = substance.Weight;
            if (Substance is not null)
            {
                res = _substancesCollection.GetMixSubstance(Substance.SubParams, substance.SubParams);
                weight += Substance.Weight;
            }

            UpdateSubstance(new Substance(res, weight));
            return true;
        }

        protected override bool RemoveSubstance(float maxVolume)
        {
            if (Substance is null)
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

        public void StirringSubstance()
        {
            if (Substance is null)
            {
                return;
            }
            var res = _substancesCollection.GetStirringSubstance(Substance.SubParams);
            Substance = new Substance(res, Substance.Weight);
            _baseFormPrefab.GetComponent<MeshRenderer>().material.color = res.Color;
            _baseFormPrefab.SetActive(true);
        }
        public AnchorCntrl Anchor => _anchor;

        public void AddAnchor(AnchorCntrl anchor)
        {
            _anchor = anchor;
        }
    }
}
