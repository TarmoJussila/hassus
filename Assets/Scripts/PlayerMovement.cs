using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const float Speed = 10;

    private Vector2 _input;

    private void Update()
    {
        if (_input == Vector2.zero)
        {
            return;
        }

        transform.position += new Vector3(_input.x, 0, 0) * (Time.deltaTime * Speed);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log($"pressed={context.control.IsPressed()} | {context.ReadValue<Vector2>()}");
        Vector2 temp = context.ReadValue<Vector2>();
        _input = temp;
        _input.y = 0;
        _input.Normalize();
    }

    public void Join(InputAction.CallbackContext context)
    {
        Debug.Log("Join!");
    }
}
