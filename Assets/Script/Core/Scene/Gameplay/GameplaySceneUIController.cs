using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Edu.Golf.Core
{
    [Serializable]
    public sealed class GameplaySceneUIController
    {
        [SerializeField]
        private GameObject _gameOver = default;

        [SerializeField]
        private Button _btnRestart = default;

        [SerializeField]
        private Button _btnNext = default;

        [SerializeField]
        private Button _btnExit = default;

        [SerializeField]
        private TextMeshProUGUI _txtScore = default;

        [SerializeField]
        private TextMeshProUGUI _txtHighScore = default;

        [SerializeField]
        private TextMeshProUGUI _txtScoreFinal = default;

        public void OnAwake()
        {
            var sceneObjects = GameManager.Instance.SceneObjects;
            _btnExit.onClick.AddListener(() => SceneManager.LoadScene(sceneObjects.MainMenu));
            _btnRestart.onClick.AddListener(() => SceneManager.LoadScene(sceneObjects.Gameplay));
            _btnNext.onClick.AddListener(() => {
                GameManager.Instance.CurrentLevel++;
                SceneManager.LoadScene(sceneObjects.Gameplay);
            });
        }

        public void UpdateScore(uint score)
        {
            _txtScore.text = score.ToString();
        }

        public void ShowGameOver(uint score, uint highScore)
        {
            if(GameManager.Instance.CurrentLevel == 4) 
                _btnNext.gameObject.SetActive(false);
            highScore = score > highScore && highScore != 0 ? highScore : score;
            _gameOver.SetActive(true);
            _txtScoreFinal.text = $"Score : {score}";
            _txtHighScore.text = $"High Score : {highScore}";
            GameManager.Instance.ScoreController.SaveHighScore(GameManager.Instance.CurrentLevel);
        }

    }
}
