using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    [SerializeField] private float ratSpeed = 1.0f;
    public float direction = 1;

    private void ChangeDirection()
    {
        direction *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void Activity(float deltaTime)
    {
        if (transform.position.x < -8.0f || transform.position.x > 8.0f) {
            ChangeDirection();
        }
        transform.position = transform.position + new Vector3(direction * deltaTime * ratSpeed, 0, 0);
    }
}
