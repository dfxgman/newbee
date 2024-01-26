using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]Rigidbody rb;
    Vector2 moveTransform;
    Vector3 targetPos;

    public void Init()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new(moveTransform.x, 0, moveTransform.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveTransform = context.ReadValue<Vector2>() * Player.Instance.Movespeed.Value;
    }
    public void OnAim(InputAction.CallbackContext context) 
    {
        targetPos = context.ReadValue<Vector2>();
    }
    public Vector3 GetMousePoint()
    {
        Ray moucePos = Camera.main.ScreenPointToRay(targetPos);
        Physics.Raycast(moucePos, out RaycastHit mouseTransform);
        Vector3 mouse = mouseTransform.point;
        var dist = transform.position - mouse;
        Quaternion rotation = Quaternion.LookRotation(dist, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, rotation.y, 0, rotation.w);
        return mouse;
    }
}
