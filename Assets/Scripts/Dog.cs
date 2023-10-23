using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    private float direction = 1;
    private float end;

    public void Turn(bool isRunningLeft)
    {
        direction = isRunningLeft ? -1 : 1;
        end = isRunningLeft ? -10 : 10;
        transform.localScale = new Vector3(transform.localScale.x * direction, transform.localScale.y, transform.localScale.z);
    }

    public bool Activity(float deltaTime)
    {
        transform.position += new Vector3(moveSpeed * direction * deltaTime, 0, 0);
        if ((end == -10 && (end >= transform.position.x)) || (end == 10 && (end <= transform.position.x))) {
            return true;
        }
        return false;
    }
}
