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
            if (substanceContainer.CurrentCountSubstances != 1)
            {
                return;
            }
            
            var temp = substanceContainer.GetNextSubstance();
            var newSubPar = _substancesParamsCollection.GetStirringSubstanceParams(temp.SubstanceProperty);
            var sub = new Substance(newSubPar, temp.GetWeight());
            
            substanceContainer.ClearSubstances();
            substanceContainer.AddSubstanceToArray(sub);
            substanceContainer.UpdateDisplaySubstance();
        }
        
        public bool DrySubstance(SubstanceContainer substanceContainer)
        {
            if (substanceContainer.CurrentCountSubstances != 1)
            {
                return false;
            }
            var temp = substanceContainer.GetNextSubstance();
            var newSubPar = _substancesParamsCollection.GetDrySubstanceParams(temp.SubstanceProperty);
            if (newSubPar.SubName.Equals("Bad Substance"))
            {
                return false;
            }

            var sub = new Substance(newSubPar, temp.GetWeight());
            substanceContainer.ClearSubstances();
            substanceContainer.AddSubstanceToArray(sub);
            substanceContainer.UpdateDisplaySubstance();
            return true;
        }

        public void MixSubstances(SubstanceContainer substanceContainer, Substance secSub)
        {
            if (substanceContainer.CurrentCountSubstances != 1)
            {
                Debug.Log("CurrentCountSubstances " + substanceContainer.CurrentCountSubstances);
                return;
            }
            
            var nextSub = substanceContainer.GetNextSubstance();
            if (nextSub is not null && nextSub.SubstanceProperty.SubName
                    .Equals(secSub.SubstanceProperty.SubName))
            {
                nextSub.AddWeight(secSub.GetWeight());
                return;
            }
            
            var temp = substanceContainer.GetNextSubstance();
            var newSubPar = _substancesParamsCollection.GetMixSubstanceParams(temp.SubstanceProperty, secSub.SubstanceProperty);
            var sub = new Substance(newSubPar, temp.GetWeight() + secSub.GetWeight());
            substanceContainer.ClearSubstances();
            substanceContainer.AddSubstanceToArray(sub);
            substanceContainer.UpdateDisplaySubstance();
        }
        
        public bool SplitSubstances(SubstanceContainer substanceContainer)
        {
            if (substanceContainer.CurrentCountSubstances != 1)
            {
                return false;
            }

            var temp = substanceContainer.GetNextSubstance();
            var newSubPar = _substancesParamsCollection.GetSplitSubstanceParams(temp.SubstanceProperty);
            
            if (newSubPar.SubName.Equals("Bad Substance"))
            {
                return false;
            }
            
            if (newSubPar is not SubstancePropertySplit substancePropertySplit)
            {
                return false;
            }
            
            var res = new Substance[MAX_LAYOURS_COUNT];
            if (substancePropertySplit.Sediment is not null)
            {
                res[0] = new Substance(substancePropertySplit.Sediment,
                    temp.GetWeight()); 
            }
            if (substancePropertySplit.Main is not null)
            {
                res[1] = new Substance(substancePropertySplit.Main,
                    temp.GetWeight());
            }
            if (substancePropertySplit.Membrane is not null)
            {
                res[2] = new Substance(substancePropertySplit.Membrane,
                    temp.GetWeight());
            }
            
            substanceContainer.ClearSubstances();
            substanceContainer.UpdateSubstancesArray(res);
            substanceContainer.UpdateDisplaySubstance();
            return true;
        }
        
        public void AddSubstance(SubstanceContainer substanceContainer, Substance addingSubstance)
        {
            substanceContainer.IsDirty = true;
            
            if (substanceContainer.MaxVolume - substanceContainer.GetWeight() >= addingSubstance.GetWeight())
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