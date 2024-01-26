using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const float Speed = 10;

    private bool _moving;
    private Vector2 _input;

    private void Update()
    {
        if (!_moving)
        {
            return;
        }

        transform.position += new Vector3(_input.x, 0, 0) * (Time.deltaTime * Speed);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moving = context.control.IsPressed();
        _input = context.ReadValue<Vector2>();
    }

    public void Join(InputAction.CallbackContext context)
    {
        Debug.Log("Join!");
    }
}
