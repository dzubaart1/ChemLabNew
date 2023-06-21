using Installers;
using Zenject;

namespace Canvases
{
    public class EndGameCanvasCntrl : CanvasBase
    {
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void LoadPrevSave()
        {
            _signalBus.Fire<LoadSignal>();
        }
    }
}