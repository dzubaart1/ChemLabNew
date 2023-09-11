using System.Linq;
using BNG;
using Canvases;
using JetBrains.Annotations;
using Substances;
using UnityEngine;

namespace Containers
{
    public class DisplaySubstance : BaseContainer
    {
        [Header("Display Container Params")]
        [SerializeField] private SubstanceCanvasCntrl _substanceCanvasCntrl;
        [SerializeField] protected GameObject _mainSubPrefab;
        [SerializeField] private GameObject _sedimentPrefab;
        [SerializeField] private GameObject _membranePrefab;
        [SerializeField] private GameObject _particleSystem;

        private void Start()
        {
            if (_substanceCanvasCntrl is not null)
            {
                InputBridge.OnAButtonPressed += ToggleSubstanceCanvas;
            }
            if (IsNull(_particleSystem))
            {
                return;
            }
            _particleSystem.SetActive(false);
        }

        private void ToggleSubstanceCanvas()
        {
            if (GetComponent<Grabbable>() is null || !GetComponent<Grabbable>().BeingHeld)
            {
                _substanceCanvasCntrl.gameObject.SetActive(false);
                return;
            }
            _substanceCanvasCntrl.UpdateSubstanceText(GetStringStack());
            _substanceCanvasCntrl.gameObject.SetActive(!_substanceCanvasCntrl.gameObject.activeSelf);
        }
        
        public void UpdateDisplaySubstance()
        {
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
        
        private void TogglePrefab(GameObject prefab, [CanBeNull] SubstancePropertyBase substanceParams)
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
            var _mr = prefab.GetComponentInChildren<MeshRenderer>();
            if (_mr == null)
            {
                return;
            }
            _mr.material.color = substanceParams.Color;
        }
        
        private string GetStringStack()
        {
            return CurrentCountSubstances == 0 ? "Вещество не определено" :
                CurrentSubstances.Aggregate("", (current, sub) => current + (sub?.SubstanceProperty.HintName + " "));
        }

        private void UpdateParticleSystem([CanBeNull] SubstancePropertyBase substanceParams)
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
            var newMat = _particleSystem.GetComponent<Renderer>().material;
            newMat.SetColor("_EmissionColor", substanceParams.Color);
        }
    }
}