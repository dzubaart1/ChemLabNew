using Data;
using Zenject;

namespace Canvases
{
    public class EndGameCanvasCntrl : CanvasBase
    {
        private SceneSetter _sceneSetter;
        [Inject]
        public void Construct(SceneSetter sceneSetter, SignalBus signalBus)
        {
            _sceneSetter = sceneSetter;
        }

        public void LoadPrevSave()
        {
            _sceneSetter.GetSavedSceneState();
        }
    }
}