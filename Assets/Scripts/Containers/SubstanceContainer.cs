using JetBrains.Annotations;
using Substances;
using UnityEngine;
using Zenject;

namespace Containers
{
    public class SubstanceContainer : DisplaySubstance
    {
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
            UpdateDisplaySubstance();
            Debug.Log("Increze" + ContainerType);
        }

        public void RemoveSubstanceFromArray(int index)
        {
            CurrentSubstances[index] = null;
            CurrentCountSubstances--;
            UpdateDisplaySubstance();
            Debug.Log("Decreze" + ContainerType);
        }

        public void ClearSubstances()
        {
            for (int i = 0; i < CurrentSubstances.Length; i++)
            {
                CurrentSubstances[i] = null;
            }
            CurrentCountSubstances = 0;
            UpdateDisplaySubstance();
        }
        
        public virtual bool AddSubstance(Substance substance)
        {
            if (CurrentCountSubstances > 0 || substance is null)
            {
                return false;
            }

            _substancesCntrl.AddSubstance(this, substance);
            return true;
        }

        public virtual bool RemoveSubstance(float targetVolume)
        {
            if (CurrentCountSubstances == 0)
            {
                return false;
            }
            _substancesCntrl.RemoveSubstance(this, targetVolume);
            return true;
        }
        
        [CanBeNull]
        public Substance GetNextSubstance()
        {
            for (var i = MAX_LAYOURS_COUNT-1; i >= 0; i--)
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
            for (var i = substances.Length - 1; i >= 0; i--)
            {
                if (substances[i] != null)
                {
                    CurrentCountSubstances++;
                }
            }
        }
    }
}