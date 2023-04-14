using Machines;
using Substances;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class ExpTabletLunkaContainer : TransferSubstanceContainer
    {
        [SerializeField] private ExpTabletMachineCntrl _tabletMachineCntrl;
        private SubstancesParamsCollection _substancesCollection;

        [Inject]
        public void Construct(SubstancesParamsCollection substancesCollection)
        {
            _substancesCollection = substancesCollection;
        }

        protected override bool AddSubstance(Substance substance)
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
            UpdateSubstance(new Substance(res, weight));
            _tabletMachineCntrl.CheckCompliteFill();
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
            _tabletMachineCntrl.CheckCompliteFill();
            return true;
        }
    }
}
