using UnityEngine;

namespace Edu.Golf.Core
{
    public sealed class MainMenuSceneCoordinator : MonoBehaviour
    {
        [SerializeField]
        private MainMenuSceneUIController _uiController = default;

        private void Awake()
        {
            GameManager.Instance.ScoreController.GetHighScore(0);
            _uiController.OnAwake();
        }
    }
}
