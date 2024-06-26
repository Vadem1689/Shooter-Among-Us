using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidBody;
    [SerializeField] private SpriteRenderer _sprite;

    [SerializeField] private float movementSpeed;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 movementAmmount)
    {
        playerRigidBody.AddForce(movementAmmount*movementSpeed);
        
        print("Тћтт"+movementAmmount);
    }
}
