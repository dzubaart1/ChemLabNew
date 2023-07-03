using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


namespace Canvases
{
    public class StartSceneCanvasCntrl : CanvasBase
    {
        
        [SerializeField] private List<GameObject> panels;
        private int rulesCount = 0;
        
        public void OnStartBtnClick()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void OnNextBtnClick()
        {
            Destroy(panels[rulesCount]);
            rulesCount++;
            panels[rulesCount].SetActive(true);
        }
    }
}