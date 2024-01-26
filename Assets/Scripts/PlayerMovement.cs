using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log("Move!");
    }
    
    public void Join(InputAction.CallbackContext context)
    {
        Debug.Log("Join!");
    }
}
