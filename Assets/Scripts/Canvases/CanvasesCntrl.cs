using System.Collections.Generic;
using Installers;
using UnityEngine;
using Zenject;

namespace Canvases
{
    public class CanvasesCntrl : MonoBehaviour
    {
        public List<CanvasBase> _canvasBases;
        private SignalBus _signalBus;

        [SerializeField]
        private GameObject _tabletCanvas;
        
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
                if (!canvas.CanvasId.Equals(showCanvasSignal.Id) || (showCanvasSignal.Id == CanvasId.EndGameCanvas && canvas.gameObject.activeSelf))
                {
                    continue;
                }
                canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
                if (canvas.CanvasId == CanvasId.EndGameCanvas && canvas.gameObject.activeSelf == true)
                {
                    _tabletCanvas.SetActive(false);
                }
            }
        }
    }
}