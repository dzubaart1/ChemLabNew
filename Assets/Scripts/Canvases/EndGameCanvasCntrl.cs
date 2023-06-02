using Data;
using Zenject;

namespace Canvases
{
    public class EndGameCanvasCntrl : CanvasBase
    {
        //private SceneSetter _sceneSetter;
        private SignalBus _signalBus;
        [Inject]
        //public void Construct(SceneSetter sceneSetter, SignalBus signalBus)
        public void Construct(SignalBus signalBus)
        {
            //_sceneSetter = sceneSetter;
            _signalBus = signalBus;
        }

        public void LoadPrevSave()
        {
            //_sceneSetter.GetSavedSceneState();
        }
    }
}