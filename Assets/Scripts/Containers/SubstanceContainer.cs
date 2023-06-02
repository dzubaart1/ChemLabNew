using Containers;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Substances
{
    public class SubstanceContainer : DisplaySubstance
    {
        public int CurrentCountSubstances;
        protected SubstancesCntrl _substancesCntrl;

        [Inject]
        public void Construct(SubstancesCntrl substancesCntrl)
        {
            _substancesCntrl = substancesCntrl;
        }

        public void AddSubstanceToArray(Substance substance)
        {
            CurrentSubstances[(int)substance.SubstanceProperty.SubstanceLayer] = substance;
            CurrentCountSubstances++;
            Debug.Log("Increze" + ContainerType);
        }

        public void RemoveSubstanceFromArray(int index)
        {
            CurrentSubstances[index] = null;
            CurrentCountSubstances--;
            CurrentCountSubstances++;
            Debug.Log("Decreze" + ContainerType);
        }
        
        public virtual bool AddSubstance(Substance substance)
        {
            if (CurrentCountSubstances > 0 || substance is null)
            {
                return false;
            }
            
            _substancesCntrl.AddSubstance(this, substance);
            UpdateDisplaySubstance();
            return true;
        }

        public virtual bool RemoveSubstance(float targetVolume)
        {
            if (CurrentCountSubstances == 0)
            {
                return false;
            }
            _substancesCntrl.RemoveSubstance(this, targetVolume);
            UpdateDisplaySubstance();

            return true;
        }
        
        [CanBeNull]
        public Substance GetNextSubstance()
        {
            for (int i = MAX_LAYOURS_COUNT-1; i >= 0; i--)
            {
                if (CurrentSubstances[i] != null)
                {
                    return CurrentSubstances[i];
                }
            }
            return null;
        }

        public void UpdateSubstancesArray(Substance[] substances)
        {
            substances.CopyTo(CurrentSubstances, 0);
            CurrentCountSubstances = 0;
            for (int i = 0; i < substances.Length; i++)
            {
                if (substances[i] != null)
                {
                    CurrentCountSubstances++;
                }
            }
        }
    }
}