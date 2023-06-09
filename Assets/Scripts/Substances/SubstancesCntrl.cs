using Containers;
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
        
        public void StirSubstance(SubstanceContainer substanceContainer)
        {
            Debug.Log("Here" + substanceContainer.CurrentCountSubstances);
            if (substanceContainer.CurrentCountSubstances != 1)
            {
                return;
            }
            
            var temp = substanceContainer.GetNextSubstance();
            substanceContainer.ClearSubstances();
            var newSubPar = _substancesParamsCollection.GetStirringSubstanceParams(temp.SubstanceProperty);
            substanceContainer.AddSubstanceToArray(new Substance(newSubPar, temp.GetWeight()));
            substanceContainer.UpdateDisplaySubstance();
        }
        
        public void DrySubstance(SubstanceContainer substanceContainer)
        {
            if (substanceContainer.CurrentCountSubstances != 1)
            {
                return;
            }

            var temp = substanceContainer.GetNextSubstance();
            substanceContainer.ClearSubstances();
            var newSubPar = _substancesParamsCollection.GetDrySubstanceParams(temp.SubstanceProperty);
            substanceContainer.AddSubstanceToArray(new Substance(newSubPar, temp.GetWeight()));
            substanceContainer.UpdateDisplaySubstance();
        }

        public void MixSubstances(SubstanceContainer substanceContainer, Substance secSub)
        {
            if (substanceContainer.CurrentCountSubstances != 1)
            {
                Debug.Log("CurrentCountSubstances " + substanceContainer.CurrentCountSubstances);
                return;
            }
            
            var temp = substanceContainer.GetNextSubstance();
            substanceContainer.ClearSubstances();
            var newSubPar = _substancesParamsCollection.GetMixSubstanceParams(temp.SubstanceProperty, secSub.SubstanceProperty);
            substanceContainer.AddSubstanceToArray(new Substance(newSubPar, temp.GetWeight() + secSub.GetWeight()));
            Debug.Log(newSubPar.SubName);
            substanceContainer.UpdateDisplaySubstance();
        }

        
        
        public void SplitSubstances(SubstanceContainer substanceContainer)
        {
            if (substanceContainer.CurrentCountSubstances != 1)
            {
                return;
            }

            var temp = substanceContainer.GetNextSubstance();
            substanceContainer.ClearSubstances();
            var newSubPar = _substancesParamsCollection.GetSplitSubstanceParams(temp.SubstanceProperty);
            if (newSubPar is not SubstancePropertySplit substancePropertySplit) return;
            
            var res = new Substance[MAX_LAYOURS_COUNT];
            if (substancePropertySplit.Sediment is not null)
            {
                res[0] = new Substance(substancePropertySplit.Sediment,
                    temp.GetWeight() * substancePropertySplit.GetPartOfSedimentWeight());
            }
            if (substancePropertySplit.Main is not null)
            {
                res[1] = new Substance(substancePropertySplit.Main,
                    temp.GetWeight() * substancePropertySplit.GetPartOfMainWeight());
            }
            if (substancePropertySplit.Membrane is not null)
            {
                res[2] = new Substance(substancePropertySplit.Membrane,
                    temp.GetWeight() * substancePropertySplit.GetPartOfMembraneWeight());
            }

            substanceContainer.UpdateSubstancesArray(res);
            substanceContainer.UpdateDisplaySubstance();
        }
        
        public void AddSubstance(SubstanceContainer substanceContainer, Substance addingSubstance)
        {
            if (substanceContainer.MaxVolume >= addingSubstance.GetWeight())
            {
                substanceContainer.AddSubstanceToArray(addingSubstance);
                return;
            }
            substanceContainer.AddSubstanceToArray(new Substance(addingSubstance.SubstanceProperty, substanceContainer.MaxVolume));
            substanceContainer.UpdateDisplaySubstance();
        }

        public void RemoveSubstance(SubstanceContainer substanceContainer, float removeVolume)
        {
            var temp = substanceContainer.GetNextSubstance();
            Debug.Log(temp?.SubstanceProperty.SubName+ " "+(temp?.GetWeight() ?? 0));
            if (removeVolume >= temp.GetWeight())
            {
                substanceContainer.RemoveSubstanceFromArray((int)temp.SubstanceProperty.SubstanceLayer);
                return;
            }
            
            temp.RemoveWeight(removeVolume);
            substanceContainer.UpdateDisplaySubstance();
        }
    }
}