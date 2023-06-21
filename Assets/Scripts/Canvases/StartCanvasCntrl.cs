using Installers;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            //SceneManager.LoadScene(0);
            _signalBus.Fire(new StartGameSignal());
            _signalBus.Fire(new ToggleCanvasSignal(){Id = CanvasId.StartGameCanvas});
        }

        public void OnNextBtnClick()
        {
            Destroy(panels[rulesCount]);
            rulesCount++;
            panels[rulesCount].SetActive(true);
        }
    }
}