using System;
using UnityEngine;

namespace Edu.Golf.Interaction
{
    public sealed class InputController
    {
        private GameInputActions _gameInputActions = default;

        public void OnEnable(GameInputActions.IPlayerActions playerActions)
        {
            _gameInputActions = new();
            _gameInputActions.Enable();
            _gameInputActions.Player.SetCallbacks(playerActions);
        }

        public void OnDisable()
        {
            _gameInputActions.Disable();
            _gameInputActions = default;
        }
    }
}