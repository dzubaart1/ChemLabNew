using System;
using BNG;
using Containers;
using Substances;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class SubstanceCanvasCntrl : MonoBehaviour
    {
        [SerializeField] private Text _substanceText;

        public void UpdateSubstanceText(string substances)
        {
            _substanceText.text = substances;
        }

        /*private void Start()
        {
            obj1 = gameObject.transform;
        }

        private void Update()
        {
            newDir = Vector3.RotateTowards(obj1.forward, target.position - obj1.position, Time.deltaTime * 1f, 5);
            obj1.rotation = Quaternion.LookRotation(newDir);
            obj1.eulerAngles = new Vector3(0, obj1.eulerAngles.y, 0);

        }*/
    }
}
