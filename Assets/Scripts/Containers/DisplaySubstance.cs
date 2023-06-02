using JetBrains.Annotations;
using Substances;
using UnityEngine;

namespace Containers
{
    public class DisplaySubstance : BaseContainer
    {
        [SerializeField] protected GameObject _mainSubPrefab;
        [SerializeField] private GameObject _sedimentPrefab;
        [SerializeField] private GameObject _membranePrefab;
        [SerializeField] private GameObject _particleSystem;

        public void UpdateDisplaySubstance()
        {
            
            if (CurrentSubstancesList.Count == 0)
            {
                TogglePrefab(_mainSubPrefab, null);
                TogglePrefab(_sedimentPrefab, null);
                TogglePrefab(_membranePrefab, null);
                return;
            }

            UpdateParticleSystem(CurrentSubstancesList.Peek().SubstanceProperty);
            
            if (CurrentSubstancesList.Count == 1)
            {
                TogglePrefab(_mainSubPrefab, CurrentSubstancesList.Peek().SubstanceProperty);
                TogglePrefab(_sedimentPrefab, null);
                TogglePrefab(_membranePrefab, null);
                return;
            }
            
            
            int i = 0;
            foreach (var substance in CurrentSubstancesList)
            {
                switch (i)
                {
                    case 1:
                        TogglePrefab(_sedimentPrefab, substance.SubstanceProperty);
                        break;
                    case 0:
                        TogglePrefab(_mainSubPrefab, substance.SubstanceProperty);
                        break;
                    /*case 0:
                        TogglePrefab(_membranePrefab, substance.SubstanceProperty);
                        break;*/
                }

                i++;
            }
            PrintStack();
        }
        
        public void TogglePrefab(GameObject prefab, [CanBeNull] SubstancePropertyBase substanceParams)
        {
            if (!prefab)
            {
                return;
            }
            if (substanceParams is null)
            {
                prefab.SetActive(false);
                return;
            }
            prefab.SetActive(true);
            prefab.GetComponent<MeshRenderer>().material.color = substanceParams.Color;
        }

        public void UpdateParticleSystem([CanBeNull] SubstancePropertyBase substanceParams)
        {
            if (IsNull(_particleSystem))
            {
                return;
            }
            if (substanceParams is null)
            {
                _particleSystem.SetActive(false);
                return;
            }
            
            _particleSystem.SetActive(true);
            Material _newMat = _particleSystem.GetComponent<Renderer>().material;
            _newMat.SetColor("_EmissionColor", substanceParams.Color);
        }
    }
}