using Cups;
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
            for (int i = 0; i < MAX_LAYOURS_COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        TogglePrefab(_sedimentPrefab, CurrentSubstances[i]?.SubstanceProperty);
                        break;
                    case 1:
                        TogglePrefab(_mainSubPrefab, CurrentSubstances[i]?.SubstanceProperty);
                        break;
                    case 2:
                        TogglePrefab(_membranePrefab, CurrentSubstances[i]?.SubstanceProperty);
                        break;
                }
            }
            //UpdateParticleSystem(CurrentSubstancesList.Peek().SubstanceProperty);
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

        /*public void UpdateParticleSystem([CanBeNull] SubstancePropertyBase substanceParams)
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
        }*/
    }
}