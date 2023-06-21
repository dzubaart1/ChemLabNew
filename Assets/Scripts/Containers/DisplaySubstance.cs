using System;
using System.Collections.Generic;
using System.Linq;
using BNG;
using Canvases;
using JetBrains.Annotations;
using Substances;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
            for (int i = 0; i < MAX_LAYOURS_COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        TogglePrefab(_sedimentPrefab, CurrentSubstances[i]?.SubstanceProperty);
                        //UpdateParticleSystem(CurrentSubstances[i]?.SubstanceProperty);
                        break;
                    case 1:
                        TogglePrefab(_mainSubPrefab, CurrentSubstances[i]?.SubstanceProperty);
                        //UpdateParticleSystem(CurrentSubstances[i]?.SubstanceProperty);
                        break;
                    case 2:
                        TogglePrefab(_membranePrefab, CurrentSubstances[i]?.SubstanceProperty);
                        break;
                }
            }
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
            prefab.GetComponent<MeshRenderer>().material.color = substanceParams.Color;
        }
        
        private string GetStringStack()
        {
            return CurrentCountSubstances == 0 ? "Вещество не определено" :
                CurrentSubstances.Aggregate("", (current, sub) => current + (sub?.SubstanceProperty.SubName + " "));
        }

        /*private void UpdateParticleSystem([CanBeNull] SubstancePropertyBase substanceParams)
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
        }*/
    }
}