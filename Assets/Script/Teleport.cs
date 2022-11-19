using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject toObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetObject = collision.gameObject;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TeleportRoutine());
        }
    }
    IEnumerator TeleportRoutine()
    {
        yield return null;
        targetObject.transform.position = toObject.transform.position;
    }
}
