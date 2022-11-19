using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float distance;
    public LayerMask isLayer;
    public float bulletDamage;
    float var;
    private void Awake()
    {
        
    }

    void Start()
    {
        Invoke("DestroyBullet", 5);      
        //bulletDamage = gameObject.GetComponent<EnemyRangedAttack>().BulletDamage;
    }

   
    void Update()
    {

        transform.Translate(transform.right * var * bulletSpeed * Time.deltaTime);
        Invoke("DestroyBullet", 5);
    }
    public void ShotLeftBullet()
    {
        var = -1;
    }
    public void ShotRightBullet()
    {
        var = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DestroyBullet();
        }
    }
        void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
