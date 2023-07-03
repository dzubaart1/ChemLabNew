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
            if (CurrentCountSubstances > 0 || substance is null || !IsEnable())
            {
                return false;
            }

            _substancesCntrl.AddSubstance(this, substance);
            if (ContainerType == ContainersTypes.KspectrometrContainer)
                return true;
            var localScale = GetNextSubstance().GetWeight() > 1
                ? new Vector3(1, GetNextSubstance().GetWeight() / 4, 1)
                : GetNextSubstance().GetWeight() > 0.1
                    ? new Vector3(1  , GetNextSubstance().GetWeight(), 1 )
                    : new Vector3(0.5f , GetNextSubstance().GetWeight()  * 2, 0.5f);
            _mainSubPrefab.transform.localScale = localScale;
            return true;
        }

        public virtual bool RemoveSubstance(float targetVolume)
        {
            if (CurrentCountSubstances == 0 || !IsEnable())
            {
                return false;
            }
            
            _substancesCntrl.RemoveSubstance(this, targetVolume);
            return true;
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