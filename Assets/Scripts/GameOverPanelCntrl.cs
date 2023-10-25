using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelCntrl : MonoBehaviour
{
    [SerializeField] private string _nextScene;
    [SerializeField] private List<ParticleSystem> _teleportParticleSystems;
    
    public void OnClickNextLevelBtn()
    {
        StartCoroutine(LoadSceneAnimation());
    }
    
    private IEnumerator LoadSceneAnimation()
    {
        var operation = SceneManager.LoadSceneAsync(_nextScene);
            
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
