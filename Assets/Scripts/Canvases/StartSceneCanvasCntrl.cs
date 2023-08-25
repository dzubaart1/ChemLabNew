using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


namespace Canvases
{
    public class StartSceneCanvasCntrl : MonoBehaviour
    {
        [SerializeField] private List<GameObject> panels;
        private int rulesCount;
        
        public void OnStartBtnClick()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void OnNextBtnClick()
        {
            panels[rulesCount].SetActive(false);
            rulesCount++;
            panels[rulesCount].SetActive(true);
        }

        public void OnPrevBtnClick()
        {
            panels[rulesCount].SetActive(false);
            rulesCount--;
            panels[rulesCount].SetActive(true);
        }
    }
}