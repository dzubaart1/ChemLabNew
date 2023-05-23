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
            if (CurrentSubstance is null)
            {
                TogglePrefab(_mainSubPrefab, null);
                TogglePrefab(_sedimentPrefab, null);
                TogglePrefab(_membranePrefab, null);
                return;
            }
            
            if (CurrentSubstance.SubstanceProperty is SubstancePropertySplit)
            {
                Debug.Log("Show 2");
                TogglePrefab(_mainSubPrefab, CurrentSubstance.MainSubstance?.SubstanceProperty);
                TogglePrefab(_sedimentPrefab, CurrentSubstance.SedimentSubstance?.SubstanceProperty);
                TogglePrefab(_membranePrefab, CurrentSubstance.MembraneSubstance?.SubstanceProperty);
            }
            else
            {
                Debug.Log($"Show 3 {CurrentSubstance.SubstanceProperty.SubName} {ContainerType}");
                TogglePrefab(_mainSubPrefab, CurrentSubstance.SubstanceProperty);
                TogglePrefab(_sedimentPrefab, null);
                TogglePrefab(_membranePrefab, null);
            }
        }
        
        public void TogglePrefab(GameObject prefab, [CanBeNull] SubstancePropertyBase substanceParams)
        {
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