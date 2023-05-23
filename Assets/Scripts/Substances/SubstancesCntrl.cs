using UnityEngine;

namespace Substances
{
    public class SubstancesCntrl
    {
        private SubstancesParamsCollection _substancesParamsCollection;
        
        public SubstancesCntrl(SubstancesParamsCollection substancesParamsCollection)
        {
            _substancesParamsCollection = substancesParamsCollection;
        }
        
        public SubstanceSplit StirSubstance(SubstanceSplit substance)
        {
            var newSubPar = _substancesParamsCollection.GetStirringSubstanceParams(substance.SubstanceProperty);
            return new SubstanceSplit(newSubPar, substance.GetWeight());
        }

        public SubstanceSplit MixSubstances(SubstanceSplit firstSub, SubstanceSplit secSub)
        {
            var newSubPar = _substancesParamsCollection.GetMixSubstanceParams(firstSub.SubstanceProperty, secSub.SubstanceProperty);
            return new SubstanceSplit(newSubPar, firstSub.GetWeight() + secSub.GetWeight());
        }
        
        public SubstanceSplit SplitSubstances(SubstanceSplit substance)
        {
            var newSubPar = _substancesParamsCollection.GetSplitSubstanceParams(substance.SubstanceProperty);
            return new SubstanceSplit(newSubPar, substance.GetWeight());
        }
        
        public SubstanceSplit AddSubstance(SubstanceSplit addingSubstance, float maxVolume)
        {
            if (maxVolume > addingSubstance.GetWeight())
            {
                return new SubstanceSplit(addingSubstance.SubstanceProperty, maxVolume);
            }

            return addingSubstance;
        }

        public SubstanceSplit RemoveSubstance(SubstanceSplit removingSubstance, float maxVolume)
        {
            if (removingSubstance.SubstanceProperty is SubstancePropertySplit)
            {
                if (maxVolume < removingSubstance.GetWeight())
                {
                    removingSubstance.RemoveWeight(maxVolume);
                    return new SubstanceSplit(removingSubstance.GetCurrentSubstance().SubstanceProperty, maxVolume);
                }

                return removingSubstance.GetCurrentSubstance() is null ? null : removingSubstance.PopCurrentSubstance();
            }
            if (maxVolume >= removingSubstance.GetWeight())
            {
                return null;
            }
            removingSubstance.RemoveWeight(maxVolume);
            return new SubstanceSplit(removingSubstance.SubstanceProperty,maxVolume);
        }
    }
}