using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using Mono.Cecil;

public class DamageText : MonoBehaviour
{
    public float textSpeed;
    public float alphaSpeed;
    public float destroyTime;
    public int damage;
    TextMeshPro damageText;
    Color alpha;

    void Start()
    {
        damageText = GetComponent<TextMeshPro>();
        damageText.text = damage.ToString();
        alpha = damageText.color;
        Invoke("DestoryObject", destroyTime);
    }


    void Update()
    {
        transform.Translate(new Vector3(0, textSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        damageText.color = alpha;
    }

    private void DestoryObject()
    {
        Destroy(gameObject);
    }
}
