using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90.0f;
    [SerializeField] private float movementSpeed = 4.0f;

    private float xDirection;
    private float yDirection;

    public void SetDirection(float x, float y)
    {
        xDirection = x;
        yDirection = y;
    }

    private void Rotate(float deltaTime)
    {
        transform.Rotate(0, 0, deltaTime * rotationSpeed);
    }

    private void Move(float deltaTime)
    {
        transform.position += new Vector3(xDirection * deltaTime * movementSpeed, yDirection * deltaTime * movementSpeed, 0);
    }

    public bool Activity(float deltaTime)
    {
        Rotate(deltaTime);
        Move(deltaTime);

        if (Mathf.Abs(transform.position.x) >= 10.0f || Mathf.Abs(transform.position.y) >= 6.0f) {
            return true;
        }

        return false;
    }
}
