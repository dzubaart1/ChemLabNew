using Substances;

namespace Containers
{
    public class SpoonContainer : TransferSubstanceContainer
    {
        public override bool AddSubstance(SubstanceContainer substanceContainer)
        {
            if (CurrentSubstancesList.Count > 0)
            {
                return false;
            }
            var temp = substanceContainer.CurrentSubstancesList.Peek();
            var addingRes = _substancesCntrl.AddSubstance(temp, MaxVolume);
            CurrentSubstancesList.Push(addingRes);
            UpdateDisplaySubstance();
            return true;
        }
    }
}
