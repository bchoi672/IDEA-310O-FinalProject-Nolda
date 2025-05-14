using Nolda.Collectibles;
using Nolda.GameManager;
using UnityEngine;

namespace Nolda.PlayerCharacterController
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float locomotionBlendSpeed = 0.02f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;

        public PlayerHealth _playerHealth;
        public FigureCollection _figureCollection;
        public PauseMenu _pauseMenu;

        private static int inputXHash = Animator.StringToHash("inputX");
        private static int inputYHash = Animator.StringToHash("inputY");
        private static int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");

        private static int isGroundedHash = Animator.StringToHash("isGrounded");
        private static int isFallingHash = Animator.StringToHash("isFalling");
        private static int isJumpingHash = Animator.StringToHash("isJumping");
            

        private Vector3 _currentBlendInput = Vector3.zero;



        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            bool isRunning = _playerState.CurrentPlayerMovementState == PlayerMovementState.Running;
            bool isJumping = _playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping;
            bool isFalling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Falling;
            bool isGrounded = _playerState.InGroundedState();

            Vector2 inputTarget = isRunning ? _playerLocomotionInput.MovementInput * 1.5f : _playerLocomotionInput.MovementInput;
            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed);

            if(_pauseMenu.isPaused)
            {
                FreezeGame();
            }
            else{
                _animator.enabled = true;
            }

            if(_playerHealth.health <= 0)
            {
                _animator.SetTrigger("Death");
                Invoke("FreezeGame", 1f);
            }
            if(_figureCollection.collectibleCount == _figureCollection.totalCollectibles)
            {
                _animator.SetTrigger("Win");
                Invoke("FreezeGame", 1.5f);
            }

            _animator.SetBool(isGroundedHash, isGrounded);
            _animator.SetBool(isFallingHash, isFalling);
            _animator.SetBool(isJumpingHash, isJumping);
            
            _animator.SetFloat(inputXHash, _currentBlendInput.x);
            _animator.SetFloat(inputYHash, _currentBlendInput.y);
            _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
        }

        void FreezeGame()
        {
            _animator.enabled = false;
            Time.timeScale = 0;
        }

    }
}
