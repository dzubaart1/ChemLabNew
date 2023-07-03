using Installers;
using UnityEngine;
using Zenject;

namespace Canvases
{
    public class EndGameCanvasCntrl : CanvasBase
    {
        private SignalBus _signalBus;

        [SerializeField] private GameObject _tabletCanvas;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Start()
        {
            gameObject.SetActive(false);
        }
        public void LoadPrevSave()
        {
            _signalBus.Fire<LoadSignal>();
            gameObject.SetActive(false);
            _tabletCanvas.SetActive(true);
        }
    }
}