using UnityEngine;

namespace Nolda.PlayerCharacterController{
    public class PlayerState : MonoBehaviour
    {
        [field: SerializeField] public PlayerMovementState CurrentPlayerMovementState { get; private set; } = PlayerMovementState.Idling;    
        
        public void SetPlayerMovementState(PlayerMovementState playerMovementState)
        {
            CurrentPlayerMovementState = playerMovementState;
        }

        public bool InGroundedState()
        {
            return CurrentPlayerMovementState == PlayerMovementState.Idling ||
                   CurrentPlayerMovementState == PlayerMovementState.Walking ||
                   CurrentPlayerMovementState == PlayerMovementState.Running;
        }
    }
    public enum PlayerMovementState
    {
        Idling = 0,
        Walking = 1,
        Running = 2,
        Jumping = 3,
        Falling = 4,
        Landing = 5,
        Strafing = 6, 
    }
}