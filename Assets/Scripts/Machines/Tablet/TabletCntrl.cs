using Installers;
using UnityEngine;
using Zenject;

namespace Machines.Tablet
{
    public class TabletCntrl: MonoBehaviour
    {
        [SerializeField] private GameObject _endGamePanel;
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _gameOverPanel;
        
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<EndGameSignal>(ShowEndGamePanel);
            _signalBus.Subscribe<StartGameSignal>(ShowMainPanel);
            _signalBus.Subscribe<LoadSignal>(ShowMainPanel);
            _signalBus.Subscribe<GameOverSignal>(ShowGameOverPanel);
        }

        private void Awake()
        {
            ShowStartPanel();
        }

        private void ShowStartPanel()
        {
            Debug.Log("START PANEL");
            _mainPanel.SetActive(false);
            _startPanel.SetActive(true);
            _endGamePanel.SetActive(false);
            _gameOverPanel.SetActive(false);
        }
        
        private void ShowEndGamePanel()
        {
            Debug.Log("END GAME PANEL");
            _mainPanel.SetActive(false);
            _startPanel.SetActive(false);
            _gameOverPanel.SetActive(false);
            _endGamePanel.SetActive(true);
        }
        
        private void ShowMainPanel()
        {
            Debug.Log("MAIN PANEL");
            _mainPanel.SetActive(true);
            _startPanel.SetActive(false);
            _endGamePanel.SetActive(false);
            _gameOverPanel.SetActive(false);
        }
        
        private void ShowGameOverPanel()
        {
            Debug.Log("GAME OVER PANEL");
            _mainPanel.SetActive(false);
            _startPanel.SetActive(false);
            _endGamePanel.SetActive(false);
            _gameOverPanel.SetActive(true);
        }
        
        
    }
}