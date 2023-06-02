using Installers;
using Zenject;

namespace Canvases
{
    public class StartCanvasCntrl : CanvasBase
    {
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void OnStartBtnClick()
        {
            _signalBus.Fire(new StartGameSignal());
            _signalBus.Fire(new ToggleCanvasSignal(){Id = CanvasId.StartGameCanvas});
        }
    }
}