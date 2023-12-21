using LiquidVolumeFX;
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
        }

        public void RemoveSubstanceFromArray(int index)
        {
            CurrentSubstances[index] = null;
            CurrentCountSubstances--;
            UpdateDisplaySubstance();
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
            {
                return true;
            }


            float weight = GetNextSubstance().GetWeight();


            if (ContainerType == ContainersTypes.WeightableContainer)
            {
                Vector3 scale = new Vector3(1 / MaxVolume, 1 / MaxVolume, 1 / MaxVolume);
                if (weight > 1)
                {
                    scale = new Vector3(weight / MaxVolume, weight / MaxVolume, weight / MaxVolume);
                }
                else if (weight > 0.1)
                {
                    scale = new Vector3(weight / MaxVolume * 4f, weight / MaxVolume * 4f, weight / MaxVolume * 4f);
                }

                _mainSubPrefab.transform.localScale = scale;

                return true;
            }

            if (_liquidVolume is not null)
            {
                var temp = weight / MaxVolume;
                if (temp < 0.1f)
                {
                    temp = 0.1f;
                }
                _liquidVolume.level = temp;
                _mainSubPrefab.transform.localScale = new Vector3(1, temp, 1);
            }
            else
            {
                _mainSubPrefab.transform.localScale = new Vector3(weight / MaxVolume, weight / MaxVolume, weight / MaxVolume);
            }
            
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