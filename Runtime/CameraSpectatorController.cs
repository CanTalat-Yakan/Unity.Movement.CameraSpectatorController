using UnityEngine;

namespace UnityEssentials
{
    public class CameraSpectatorController : MonoBehaviour
    {
        [Space]
        [Tooltip("The script is currently active")]
        [SerializeField] private bool _active = true;

        [Space]
        [Tooltip("Camera rotation by mouse movement is active")]
        [SerializeField] private bool _enableRotation = true;
        [Tooltip("Sensitivity of mouse rotation")]
        [SerializeField] private float _mouseSense = 1.8f;

        [Space]
        [Tooltip("Camera zooming in/out by 'Mouse Scroll Wheel' is active")]
        [SerializeField] private bool _enableTranslation = true;
        [Tooltip("Velocity of camera zooming in/out")]
        [SerializeField] private float _translationSpeed = 55f;

        [Space]
        [Tooltip("Camera movement by 'W','A','S','D','Q','E' keys is active")]
        [SerializeField] private bool _enableMovement = true;
        [Tooltip("Camera movement speed")]
        [SerializeField] private float _movementSpeed = 10f;
        [Tooltip("Speed of the quick camera movement when holding the 'Left Shift' key")]
        [SerializeField] private float _boostedSpeed = 50f;
        [SerializeField] private KeyCode _boostSpeed = KeyCode.LeftShift;
        [SerializeField] private KeyCode _moveUp = KeyCode.E;
        [SerializeField] private KeyCode _moveDown = KeyCode.Q;

        [Space]
        [Tooltip("Acceleration at camera movement is active")]
        [SerializeField] private bool _enableSpeedAcceleration = true;
        [Tooltip("Rate which is applied during camera movement")]
        [SerializeField] private float _speedAccelerationFactor = 1.5f;

        [Space]
        [Tooltip("This keypress will move the camera to initialization position")]
        [SerializeField] private KeyCode _initPositonButton = KeyCode.R;

        private float _currentIncrease = 1;
        private float _currentIncreaseMem = 0;

        private Vector3 _initPosition;
        private Vector3 _initRotation;

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (_boostedSpeed < _movementSpeed)
                _boostedSpeed = _movementSpeed;
        }
#endif

        public void Start()
        {
            _initPosition = transform.position;
            _initRotation = transform.eulerAngles;
        }

        public void Update()
        {
            if (!_active)
                return;

            // Translation
            if (_enableTranslation)
                transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * Time.deltaTime * _translationSpeed);

            // Movement
            if (_enableMovement)
            {
                Vector3 deltaPosition = Vector3.zero;
                float currentSpeed = _movementSpeed;

                if (Input.GetKey(_boostSpeed))
                    currentSpeed = _boostedSpeed;

                if (Input.GetKey(KeyCode.W))
                    deltaPosition += transform.forward;

                if (Input.GetKey(KeyCode.S))
                    deltaPosition -= transform.forward;

                if (Input.GetKey(KeyCode.A))
                    deltaPosition -= transform.right;

                if (Input.GetKey(KeyCode.D))
                    deltaPosition += transform.right;

                if (Input.GetKey(_moveUp))
                    deltaPosition += transform.up;

                if (Input.GetKey(_moveDown))
                    deltaPosition -= transform.up;

                // Calculate acceleration
                CalculateCurrentIncrease(deltaPosition != Vector3.zero);

                transform.position += deltaPosition * currentSpeed * _currentIncrease;
            }

            // Rotation
            if (_enableRotation)
            {
                // Pitch
                transform.rotation *= Quaternion.AngleAxis(
                    -Input.GetAxis("Mouse Y") * _mouseSense,
                    Vector3.right);

                // Paw
                transform.rotation = Quaternion.Euler(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y + Input.GetAxis("Mouse X") * _mouseSense,
                    transform.eulerAngles.z);
            }

            // Return to initial position
            if (Input.GetKeyDown(_initPositonButton))
            {
                transform.position = _initPosition;
                transform.eulerAngles = _initRotation;
            }
        }

        private void SetCursorState(CursorLockMode mode)
        {
            // Apply cursor state
            Cursor.lockState = mode;
            // Hide cursor when locking
            Cursor.visible = (mode != CursorLockMode.Locked);
        }

        private void CalculateCurrentIncrease(bool moving)
        {
            _currentIncrease = Time.deltaTime;

            if (!_enableSpeedAcceleration || _enableSpeedAcceleration && !moving)
            {
                _currentIncreaseMem = 0;
                return;
            }

            _currentIncreaseMem += Time.deltaTime * (_speedAccelerationFactor - 1);
            _currentIncrease = Time.deltaTime + Mathf.Pow(_currentIncreaseMem, 3) * Time.deltaTime;
        }
    }
}