using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


namespace Canvases
{
    public class StartSceneCanvasCntrl : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _panels;
        [SerializeField] private List<ParticleSystem> _teleportParticleSystems;
        
        [SerializeField] private String _sceneName;
        
        private int _rulesCount;
        
        
        public void OnStartBtnClick()
        {
            StartCoroutine(LoadSceneAnimation());
        }

        public void OnNextBtnClick()
        {
            Debug.Log("here");
            _panels[_rulesCount].SetActive(false);
            _rulesCount++;
            _panels[_rulesCount].SetActive(true);
        }

        public void OnPrevBtnClick()
        {
            _panels[_rulesCount].SetActive(false);
            _rulesCount--;
            _panels[_rulesCount].SetActive(true);
        }

        private IEnumerator LoadSceneAnimation()
        {
            var operation = SceneManager.LoadSceneAsync(_sceneName);
            
            foreach (var particleSystem in _teleportParticleSystems)
            {
                particleSystem.Play();
            }

            while (!operation.isDone)
            {
                yield return null;
            }
        }
    }
}