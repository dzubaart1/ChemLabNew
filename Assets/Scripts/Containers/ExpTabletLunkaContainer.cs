using Machines;
using Substances;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class ExpTabletLunkaContainer : TransferSubstanceContainer
    {
        [SerializeField]
        private ExpTabletMachineCntrl _tabletMachineCntrl;
        private SubstancesParamsCollection _substancesCollection;

        [Inject]
        public void Construct(SubstancesParamsCollection substancesCollection)
        {
            _substancesCollection = substancesCollection;
        }
        public override bool AddSubstance(Substance substance)
        {
            if (!_tabletMachineCntrl.IsEnable())
                return false;
            var res = substance.SubParams;
            var weight = substance.Weight;
            if (Substance is not null)
            {
                res = _substancesCollection.GetMixSubstance(Substance.SubParams, substance.SubParams);
                weight += Substance.Weight;
            }
            //ДОПИСАТЬ ДОБАВЛЕНИЕ Больше volume
            Substance = new Substance(res, weight);
            _baseFormPrefab.GetComponent<MeshRenderer>().material.color = res.Color;
            _baseFormPrefab.SetActive(true);
            _tabletMachineCntrl.CheckCompliteFill();
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
            _tabletMachineCntrl.CheckCompliteFill();
            return true;
        }
    }
}
