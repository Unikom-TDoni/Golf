using System;
using UnityEngine;
using Edu.Golf.Core;
using Edu.Golf.Interaction;
using UnityEngine.InputSystem;
using Lncodes.Module.Unity.Helper;
using UnityEngine.SceneManagement;

namespace Edu.Golf.Player
{
    [Serializable]
    public sealed class PlayerController : MonoBehaviour, GameInputActions.IPlayerActions
    {
        public event Action OnHole = default;

        public event Action OnShooting = default;

        [SerializeField]
        private float _force = default;

        [SerializeField]
        [Range(0.2f, 1)]
        private float _minVelocity = default;

        [SerializeField]
        private float _chargeSpeed = default;

        [SerializeField]
        private float _rotateSpeed = default;

        [SerializeField]
        private Boundary<float> _forceBoundary = default;

        [SerializeField]
        private Boundary<float> _rotateClampBoundary = default;

        [SerializeField]
        private Transform _pointArrow = default;

        private Rigidbody _rigidbody = default;

        private bool _isMoving = default;

        private bool _isInTheHole = default;

        private bool _isCharge = default;

        private Vector3 _direction = default;

        private Vector3 _lastPosition = default;

        private float _defaultForce = default;

        private void Awake()
        {
            _defaultForce = _force;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_isInTheHole) return;
            if (_isCharge)
            {
                RotateByInput();
                if (_force < _forceBoundary.Max && _force >= _forceBoundary.Min)
                {
                    _force += Time.deltaTime * _chargeSpeed;
                    _pointArrow.localScale = new Vector3(_force, _force, _force);
                }
                return;
            }

            RotateByInput();

            if (_rigidbody.velocity.magnitude < _minVelocity && _isMoving)
            {
                _isMoving = default;
                _lastPosition = transform.position;
                _rigidbody.velocity = Vector3.zero;
            }

            if (transform.position.y < -3)
                transform.position = _lastPosition;
        }

        private void RotateByInput()
        {
            transform.rotation *= Quaternion.AngleAxis(_direction.x * _rotateSpeed * Time.deltaTime, Vector3.up);
            transform.rotation *= Quaternion.AngleAxis(_direction.y * _rotateSpeed * Time.deltaTime, Vector3.right);

            var newAngle = transform.localEulerAngles;
            newAngle.z = default;

            var xAngle = transform.localEulerAngles.x;
            if (xAngle > 180 && xAngle < _rotateClampBoundary.Max)
                newAngle.x = _rotateClampBoundary.Max;
            else if (xAngle < 180 && xAngle > _rotateClampBoundary.Min)
                newAngle.x = _rotateClampBoundary.Min;

            transform.localEulerAngles = newAngle;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!context.performed || _isInTheHole || _isMoving) return;
            _direction = context.ReadValue<Vector2>();
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (_isInTheHole || _isMoving) return;
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    _isCharge = true;
                    break;
                case InputActionPhase.Canceled:
                    _rigidbody.AddForce(_force * transform.forward, ForceMode.Impulse);
                    _isMoving = true;
                    _isCharge = default;
                    _force = _defaultForce;
                    _pointArrow.localScale = Vector3.zero;
                    OnShooting();
                    break;
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(GameManager.Instance.Tag.Hole))
            {
                _isInTheHole = true;
                collider.GetComponentInParent<BoxCollider>().enabled = false;
                OnHole();
            }
        }
    }
}