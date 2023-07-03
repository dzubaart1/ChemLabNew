using BNG;
using Containers;
using UnityEngine;

namespace DefaultNamespace
{
    public class AnchorCntrl : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<MixContainer>() is null || GetComponent<Grabbable>().BeingHeld)
            {
                return;
            }
            var res = other.GetComponent<MixContainer>().AddAnchor(this);
            if (!res)
            {
                return;
            }
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            transform.parent = other.transform;
            transform.localPosition = new Vector3(0, 0.01f, 0);
            transform.rotation = Quaternion.identity;
        }

        public void StartAnimate()
        {
            _animator.enabled = true;
        }

        public void FinishAnimate()
        {
            _animator.enabled = false;
        }
    }
}