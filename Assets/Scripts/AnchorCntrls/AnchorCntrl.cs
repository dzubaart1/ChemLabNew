using BNG;
using Containers;
using UnityEngine;

namespace AnchorCntrls
{
    public class AnchorCntrl : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            var mixContainer = other.GetComponent<MixContainer>();
            if (mixContainer is null)
            {
                return;
            }
            if (GetComponent<Grabbable>().BeingHeld) return;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            gameObject.transform.parent = other.transform;
            gameObject.transform.localPosition = new Vector3(0, 0.008f, 0);
            mixContainer.AddAnchor(this);
        }
    }
}
    