using System;
using System.Collections.Generic;
using Installers;
using UnityEngine;
using Zenject;

namespace Canvases
{
    public class CanvasesCntrl : MonoBehaviour
    {
        [SerializeField] private List<CanvasBase> _canvasBases;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<ToggleCanvasSignal>(ToggleCanvas);
        }

        private void ToggleCanvas(ToggleCanvasSignal showCanvasSignal)
        {
            foreach (var canvas in _canvasBases)
            {
                if (canvas.CanvasId.Equals(showCanvasSignal.Id))
                {
                    canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
                }
            }
        }
    }
}