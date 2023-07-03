using Installers;
using UnityEngine;
using Zenject;


namespace Canvases
{
    public class StartCanvasCntrl : CanvasBase
    {
        private SignalBus _signalBus;
        [SerializeField] private GameObject _tabletCanvas;
        
        [Inject] 
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void OnStartBtnClick()
        {
            _tabletCanvas.SetActive(true);
            _signalBus.Fire(new StartGameSignal());
            _signalBus.Fire(new ToggleCanvasSignal(){Id = CanvasId.StartGameCanvas});
            
        }
    }
}