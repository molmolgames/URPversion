using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using Mono.Cecil;

public class LevelUpText : MonoBehaviour
{
    public float textSpeed;
    public float alphaSpeed;
    public float destroyTime;
    TextMeshPro levelUpText;
    Color alpha;

    void Start()
    {
        levelUpText = GetComponent<TextMeshPro>();
        levelUpText.text = "LEVEL UP!";
        alpha = levelUpText.color;
        Invoke("DestoryObject", destroyTime);
    }


    void Update()
    {
        transform.Translate(new Vector3(0, textSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        levelUpText.color = alpha;
    }

    private void DestoryObject()
    {
        Destroy(gameObject);
    }
}