using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCat : MonoBehaviour
{
    private float height = 0.3f;
    private float currentHeight = 0.0f;
    [SerializeField] private float speed = 0.01f;

    public IEnumerator Up() {
        while (currentHeight < height) {
            transform.position = transform.position + new Vector3(0, speed, 0);
            currentHeight += speed;
            yield return new WaitForSeconds(speed);
        }
    }

    public IEnumerator Down() {
        while (currentHeight > 0) {
            transform.position = transform.position + new Vector3(0, -speed, 0);
            currentHeight -= speed;
            yield return new WaitForSeconds(speed);
        }
        gameObject.SetActive(false);
    }
}
