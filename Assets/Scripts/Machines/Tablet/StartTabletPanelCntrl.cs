using Installers;
using UnityEngine;
using Zenject;

namespace Machines.Tablet
{
    public class StartTabletPanelCntrl : MonoBehaviour
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
        }
    }
}