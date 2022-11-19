using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircleAttack : MonoBehaviour
{
    Rigidbody2D rigid;
    public GameManger gameManger;
    float EnemyDamage;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");
            OnAttack(collision.transform);
        }
        if (collision.gameObject.tag == "EnemyFly")
        {
            Debug.Log("hit");
            OnAttackFly(collision.transform);
        }
        if (collision.gameObject.tag == "EnemyBoss")
        {
            Debug.Log("hit");
            OnAttackBoss(collision.transform);
        }
        if (collision.gameObject.tag == "EnemyBossTest")
        {
            Debug.Log("hit");
            OnAttackBossTest(collision.transform);
        }
    }

    public void OnAttack(Transform Enemy)

    {
        EnemyMove enemymove = Enemy.GetComponent<EnemyMove>();
        enemymove.EnemyDamaged(PlayerMoving.TotalPlayerDamage);

    }
    //���߸� ����
    public void OnAttackFly(Transform Enemy)

    {
        EnemyFlyMove enemyFlyMove = Enemy.GetComponent<EnemyFlyMove>();
        enemyFlyMove.EnemyDamaged(PlayerMoving.TotalPlayerDamage);
    }
    //������ ����
    public void OnAttackBoss(Transform Enemy)

    {
        EnemyBossMove enemyBossMove = Enemy.GetComponent<EnemyBossMove>();
        enemyBossMove.EnemyDamaged(PlayerMoving.TotalPlayerDamage);
    }
    public void OnAttackBossTest(Transform Enemy)

    {
        EnemyBossMoveTest enemyBossMove = Enemy.GetComponent<EnemyBossMoveTest>();
        enemyBossMove.EnemyDamaged(PlayerMoving.TotalPlayerDamage);
    }
}
