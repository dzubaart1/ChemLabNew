using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Canvases
{
    public class SubstanceCanvasCntrl : MonoBehaviour
    {
        [SerializeField] private Text _substanceText;
        private Transform obj1;
        private GameObject _XRrig;
        private Transform target;

        [Inject]
        public void Construct(GameObject rigInst)
        {
            _XRrig = rigInst;
        }
        
        private void Start()
        {
            obj1 = gameObject.transform;
            target = _XRrig.GetComponentsInChildren<CharacterController>()[0].transform;
        }
        private void Update()
        {
            Vector3 newDir = Vector3.RotateTowards(obj1.forward, target.position - obj1.position, Time.deltaTime * 1f, 5);
            obj1.rotation = Quaternion.LookRotation(newDir);
            obj1.eulerAngles = new Vector3(0, obj1.eulerAngles.y, 0);
        }
        
        public void UpdateSubstanceText(string substances)
        {
            _substanceText.text = substances;
        }

    }
}
