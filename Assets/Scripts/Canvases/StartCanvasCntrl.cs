using Installers;
using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Canvases
{
    public class StartCanvasCntrl : CanvasBase
    {
        private SignalBus _signalBus;
        
        [SerializeField] private List<GameObject> panels;
        private int rulesCount = 0;
        
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

        public void OnNextBtnClick()
        {
            panels[rulesCount].SetActive(false);
            panels[rulesCount + 1].SetActive(true);
            rulesCount++;
        }
    }
}