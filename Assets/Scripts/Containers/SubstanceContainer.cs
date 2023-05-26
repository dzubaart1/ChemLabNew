using Containers;
using UnityEngine;
using Zenject;

namespace Substances
{
    public class SubstanceContainer : DisplaySubstance
    {
        protected SubstancesCntrl _substancesCntrl;

        [Inject]
        public void Construct(SubstancesCntrl substancesCntrl)
        {
            _substancesCntrl = substancesCntrl;
        }
        
        public virtual bool AddSubstance(SubstanceContainer substanceContainer)
        {
            if (CurrentSubstancesList.Count > 0 || substanceContainer.CurrentSubstancesList.Count == 0)
            {
                return false;
            }
            
            var temp = substanceContainer.CurrentSubstancesList.Peek();
            var addingRes = _substancesCntrl.AddSubstance(temp, MaxVolume);
            CurrentSubstancesList.Push(addingRes);
            UpdateDisplaySubstance();
            return true;
        }

        public virtual bool RemoveSubstance(float targetVolume)
        {
            if (CurrentSubstancesList.Count == 0)
            {
                return false;
            }
            var temp = CurrentSubstancesList.Peek();
            CurrentSubstancesList.Pop();
            var removingRes = _substancesCntrl.RemoveSubstance(temp, targetVolume);
            if (removingRes is not null)
            {
                CurrentSubstancesList.Push(removingRes);
            }

            UpdateDisplaySubstance();
            return true;
        }
        
    }
}