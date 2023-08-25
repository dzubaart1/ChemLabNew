using Installers;
using UnityEngine;
using Zenject;

namespace Machines.Tablet
{
    public class EndGameTabletPanelCntrl : MonoBehaviour
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