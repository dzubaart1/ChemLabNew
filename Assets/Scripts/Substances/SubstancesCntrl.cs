using System.Collections.Generic;

namespace Substances
{
    public class SubstancesCntrl
    {
        private SubstancesParamsCollection _substancesParamsCollection;
        
        public SubstancesCntrl(SubstancesParamsCollection substancesParamsCollection)
        {
            _substancesParamsCollection = substancesParamsCollection;
        }
        
        public Substance StirSubstance(Substance substance)
        {
            var newSubPar = _substancesParamsCollection.GetStirringSubstanceParams(substance.SubstanceProperty);
            return new Substance(newSubPar, substance.GetWeight());
        }
        
        public Substance DrySubstance(Substance substance)
        {
            var newSubPar = _substancesParamsCollection.GetDrySubstanceParams(substance.SubstanceProperty);
            return new Substance(newSubPar, substance.GetWeight());
        }

        public Substance MixSubstances(Substance firstSub, Substance secSub)
        {
            var newSubPar = _substancesParamsCollection.GetMixSubstanceParams(firstSub.SubstanceProperty, secSub.SubstanceProperty);
            return new Substance(newSubPar, firstSub.GetWeight() + secSub.GetWeight());
        }
        
        public Stack<Substance> SplitSubstances(Substance substance)
        {
            if (substance.SubstanceProperty is not SubstancePropertySplit substancePropertySplit) return null;
            
            var res = new Stack<Substance>();
            if (substancePropertySplit.Sediment is not null)
            {
                res.Push(new Substance(substancePropertySplit.Sediment, substance.GetWeight()*substancePropertySplit.GetPartOfSedimentWeight()));
            }
            if (substancePropertySplit.Main is not null)
            {
                res.Push(new Substance(substancePropertySplit.Main, substance.GetWeight()*substancePropertySplit.GetPartOfMainWeight()));
            }
            if (substancePropertySplit.Membrane is not null)
            {
                res.Push(new Substance(substancePropertySplit.Membrane, substance.GetWeight()*substancePropertySplit.GetPartOfMembraneWeight()));
            }

            return res;
        }
        
        public Substance AddSubstance(Substance addingSubstance, float maxVolume)
        {
            if (maxVolume >= addingSubstance.GetWeight())
            {
                return addingSubstance;
            }

            return new Substance(addingSubstance.SubstanceProperty, maxVolume);
        }

        public Substance RemoveSubstance(Substance removingSubstance, float maxVolume)
        {
            if (maxVolume >= removingSubstance.GetWeight())
            {
                return null;
            }
            removingSubstance.RemoveWeight(maxVolume);
            return new Substance(removingSubstance.SubstanceProperty,removingSubstance.GetWeight());
        }
    }
}