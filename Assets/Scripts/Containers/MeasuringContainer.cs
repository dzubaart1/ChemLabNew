using Substances;
using UnityEngine;

namespace Containers
{
    public class MeasuringContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(SubstanceContainer substanceContainer)
        {
            if (CurrentSubstancesList.Count > 0 || substanceContainer.CurrentSubstancesList.Count == 0)
            {
                return false;
            }
            var temp = substanceContainer.CurrentSubstancesList.Peek();
            var addingRes = _substancesCntrl.AddSubstance(temp, MaxVolume);
            _mainSubPrefab.transform.localScale = new Vector3(1, 1, temp.GetWeight() / 10);
            CurrentSubstancesList.Push(addingRes);
            UpdateDisplaySubstance();
            return true;
        }
    }
}
