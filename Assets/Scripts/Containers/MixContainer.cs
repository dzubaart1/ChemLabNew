using BNG;
using Substances;
using UnityEngine;

namespace Containers
{
    public class MixContainer : TransferSubstanceContainer
    {
        private GameObject _anchor;

        public GameObject AnchorCntrl => _anchor;
        public override bool AddSubstance(Substance substance)
        {
            Debug.Log(CurrentCountSubstances);
            if (CurrentCountSubstances == 0)
            {
                _substancesCntrl.AddSubstance(this,substance);
            }
            else
            {
                _substancesCntrl.MixSubstances(this, substance);
            }
            return true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Anchor") || other.GetComponent<Grabbable>().BeingHeld || _anchor is not null)
            {
                return;
            }
            
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.transform.parent = other.transform;
            other.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            _anchor = other.gameObject;
        }
    }
}
