using Substances;
using UnityEngine;
using UnityEngine.Events;

namespace Containers
{
    public class AtomicMicContainer : TransferSubstanceContainer
    {
        [SerializeField] private UnityEvent _onAddSubstanceEvent;
        public override bool AddSubstance(Substance substance)
        {
            if (CurrentCountSubstances > 0)
            {
                return false;
            }
            Debug.Log($"TRY ADD TO ATOMIC--- {substance.SubstanceProperty.SubName}");
            _substancesCntrl.AddSubstance(this,substance);
            _onAddSubstanceEvent.Invoke();
            return true;
        }
    }
}