using Nolda.GameManager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nolda.PlayerCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocoMapActions
    {
        #region Class Variables
        [SerializeField] private bool holdToRun = true;
        public PauseMenu _pauseMenu;
        public PlayerHealth _playerHealth;

        public bool SprintToggledOn{ get; private set;}
        public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed{ get; private set;}

        #endregion
       
        #region Startup
        public void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerLocoMap.Enable();
            PlayerControls.PlayerLocoMap.SetCallbacks(this);
        }

        public void OnDisable()
        {
            PlayerControls.PlayerLocoMap.Disable();
            PlayerControls.PlayerLocoMap.RemoveCallbacks(this);
        }
        #endregion
       
        #region Late Update Logic
        private void LateUpdate()
        {
            JumpPressed = false;
        }
        #endregion
        
        #region Input Callbacks
        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }
            

        public void OnToggleRun(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                SprintToggledOn = holdToRun || !SprintToggledOn;
            }
            else if (context.canceled)
            {
                SprintToggledOn = !holdToRun && SprintToggledOn;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(!context.performed)
            {
                return;
            }
            JumpPressed = true;

        }

        #endregion
    }
}