using UnityEngine;
using Edu.Golf.Score;
using Edu.Golf.Level;
using Edu.Golf.Player;
using Edu.Golf.Interaction;
using Cinemachine;

namespace Edu.Golf.Core
{
    public sealed class GameplaySceneCoordinator : MonoBehaviour
    {
        [SerializeField]
        private LevelSpawner _levelSpawner = default;

        [SerializeField]
        private PlayerController _playerController = default;

        [SerializeField]
        private GameplaySceneUIController _uiController = default;

        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera = default;

        private readonly InputController _inputController = new();

        private void OnEnable()
        {
            _inputController.OnEnable(_playerController);
        }

        private void Awake()
        {
            _levelSpawner.OnAwake();
            _uiController.OnAwake();
            _playerController.OnShooting += () =>
            {
                GameManager.Instance.ScoreController.IncreaseScore();
                _uiController.UpdateScore(GameManager.Instance.ScoreController.Score);
            };

            _playerController.OnHole += () =>
            {
                var highScore = GameManager.Instance.ScoreController.GetHighScore(GameManager.Instance.CurrentLevel);
                _uiController.ShowGameOver(GameManager.Instance.ScoreController.Score, highScore);
                GameManager.Instance.ScoreController.ResetScore();
                _virtualCamera.gameObject.SetActive(false);
            };
        }

        private void OnDisable()
        {
            _inputController.OnDisable();
        }
    }
}