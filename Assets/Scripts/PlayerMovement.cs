using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const float Speed = 10;

    private Vector2 _input;
    
    public int LastDirection { get; private set; }

    private void Update()
    {
        if (_input == Vector2.zero)
        {
            return;
        }

        LastDirection = Mathf.RoundToInt(Mathf.Sign(_input.x));
        transform.position += new Vector3(_input.x, 0, 0) * (Time.deltaTime * Speed);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _input.y = 0;
        _input.Normalize();
    }

    public void Join(InputAction.CallbackContext context)
    {
        Debug.Log("Join!");
    }

    public void PlayerDead()
    {
        throw new System.NotImplementedException();
    }

    public void PlayerRespawn()
    {
        throw new System.NotImplementedException();
    }
}
