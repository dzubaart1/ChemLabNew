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

        public void UpdateDisplaySubstance()
        {
            if (CurrentSubstancesList.Count == 0)
            {
                TogglePrefab(_mainSubPrefab, null);
                TogglePrefab(_sedimentPrefab, null);
                TogglePrefab(_membranePrefab, null);
                return;
            }

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
                    case 0:
                        TogglePrefab(_sedimentPrefab, substance.SubstanceProperty);
                        break;
                    case 1:
                        TogglePrefab(_mainSubPrefab, substance.SubstanceProperty);
                        break;
                    case 2:
                        TogglePrefab(_membranePrefab, substance.SubstanceProperty);
                        break;
                }

                i++;
            }
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
    }
}