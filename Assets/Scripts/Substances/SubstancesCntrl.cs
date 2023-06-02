using UnityEngine;

namespace Substances
{
    public class SubstancesCntrl
    {
        public const int MAX_LAYOURS_COUNT = 3;
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
            if (firstSub.SubstanceProperty.SubName.Equals(secSub.SubstanceProperty.SubName))
            {
                return new Substance(firstSub.SubstanceProperty, firstSub.GetWeight() + secSub.GetWeight());
            }

            var newSubPar = _substancesParamsCollection.GetMixSubstanceParams(firstSub.SubstanceProperty, secSub.SubstanceProperty);
            return new Substance(newSubPar, firstSub.GetWeight() + secSub.GetWeight());
        }
        
        public Substance[] SplitSubstances(Substance substance)
        {
            var newSubPar = _substancesParamsCollection.GetSplitSubstanceParams(substance.SubstanceProperty);
            if (newSubPar is not SubstancePropertySplit substancePropertySplit) return null;
            
            var res = new Substance[MAX_LAYOURS_COUNT];
            if (substancePropertySplit.Sediment is not null)
            {
                res[0] = new Substance(substancePropertySplit.Sediment,
                    substance.GetWeight() * substancePropertySplit.GetPartOfSedimentWeight());
            }
            if (substancePropertySplit.Main is not null)
            {
                res[1] = new Substance(substancePropertySplit.Main,
                    substance.GetWeight() * substancePropertySplit.GetPartOfMainWeight());
            }
            if (substancePropertySplit.Membrane is not null)
            {
                res[2] = new Substance(substancePropertySplit.Membrane,
                    substance.GetWeight() * substancePropertySplit.GetPartOfMembraneWeight());
            }

            return res;
        }
        
        public void AddSubstance(SubstanceContainer substanceContainer, Substance addingSubstance)
        {
            if (substanceContainer.MaxVolume >= addingSubstance.GetWeight())
            {
                substanceContainer.AddSubstanceToArray(addingSubstance);
            }
            substanceContainer.AddSubstanceToArray(new Substance(addingSubstance.SubstanceProperty, substanceContainer.MaxVolume));
            Debug.Log(addingSubstance?.SubstanceProperty.SubName+ " "+(addingSubstance?.GetWeight() ?? 0));
            Debug.Log("volume: "+ substanceContainer.MaxVolume);
            Debug.Log("adding weight: " + addingSubstance.GetWeight());
        }

        public void RemoveSubstance(SubstanceContainer fromSubstanceContainer, float removeVolume)
        {
            var temp = fromSubstanceContainer.GetNextSubstance();
            Debug.Log(temp?.SubstanceProperty.SubName+ " "+(temp?.GetWeight() ?? 0));
            if (removeVolume >= temp.GetWeight())
            {
                fromSubstanceContainer.RemoveSubstanceFromArray((int)temp.SubstanceProperty.SubstanceLayer);
                return;
            }
            
            temp.RemoveWeight(removeVolume);
        }
    }
}