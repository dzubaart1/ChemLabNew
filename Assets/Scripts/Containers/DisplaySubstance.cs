using BNG;
using Canvases;
using JetBrains.Annotations;
using LiquidVolumeFX;
using Substances;
using UnityEngine;

namespace Containers
{
    public class DisplaySubstance : BaseContainer
    {
        [SerializeField] protected GameObject _mainSubPrefab;

        [SerializeField] private SubstanceCanvasCntrl _substanceCanvasCntrl;
        [SerializeField] private GameObject _sedimentPrefab;
        [SerializeField] private GameObject _membranePrefab;
        [SerializeField] private GameObject _particleSystem;

        protected LiquidVolume _liquidVolume;
        private MeshRenderer _liquidVolumeMeshRenderer;

        private void Start()
        {
            if (_substanceCanvasCntrl is not null)
            {
                InputBridge.OnAButtonPressed += ToggleSubstanceCanvas;
            }

            if (_particleSystem)
            {
                _particleSystem.SetActive(false);
            }
            _liquidVolume = _mainSubPrefab.GetComponentInChildren<LiquidVolume>();
            _liquidVolumeMeshRenderer = _mainSubPrefab.GetComponentInChildren<MeshRenderer>();
        }

        private void ToggleSubstanceCanvas()
        {
            if (GetComponent<Grabbable>() is null || !GetComponent<Grabbable>().BeingHeld)
            {
                _substanceCanvasCntrl.gameObject.SetActive(false);
                return;
            }

            _substanceCanvasCntrl.gameObject.SetActive(!_substanceCanvasCntrl.gameObject.activeSelf);
        }
        
        public void UpdateDisplaySubstance()
        {
            if (_substanceCanvasCntrl)
            {
                _substanceCanvasCntrl.UpdateSubstanceText(this);
            }

            if (_mainSubPrefab && !_membranePrefab && !_sedimentPrefab)
            {
                TogglePrefab(_mainSubPrefab, GetNextSubstance()?.SubstanceProperty);
                UpdateParticleSystem(GetNextSubstance()?.SubstanceProperty);
                return;
            }
            TogglePrefab(_sedimentPrefab, CurrentSubstances[0]?.SubstanceProperty);
            TogglePrefab(_mainSubPrefab, CurrentSubstances[1]?.SubstanceProperty);
            TogglePrefab(_membranePrefab, CurrentSubstances[2]?.SubstanceProperty);

            UpdateParticleSystem(CurrentSubstances[1]?.SubstanceProperty);
        }


        private void TogglePrefab(GameObject prefab, SubstancePropertyBase substanceParams)
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


            if (_liquidVolume is null || _liquidVolumeMeshRenderer is null)
            {
                prefab.GetComponentInChildren<MeshRenderer>().material.color = substanceParams.Color;
                return;
            }

            _liquidVolume.liquidColor1 = substanceParams.Color;
            _liquidVolumeMeshRenderer.material.color = substanceParams.Color;
        }
        
        private void UpdateParticleSystem([CanBeNull] SubstancePropertyBase substanceParams)
        {
            if (!_particleSystem)
            {
                return;
            }

            if (substanceParams is null)
            {
                _particleSystem.SetActive(false);
                return;
            }
            
            _particleSystem.SetActive(true);
            _particleSystem.GetComponent<Renderer>().material.SetColor("_EmissionColor", substanceParams.Color);
        }
    }
}