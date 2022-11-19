using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class EnemyFlyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animation;
    SpriteRenderer Sprite_Renderer;
    CapsuleCollider2D collider;
    Vector3 enemyDirectionVector;
    public PlayerMoving player;
    public int Next_Move_X;
    public int Next_Move_Y;
    public float speed; // 이동 속도
    public float EnemyHP;
    public float EnemyCurrentHp;
    public float EnemyAtk;
    public float EnemyExp; // 적 경험치
    public GameObject hudDamageText; //데미지 텍스트
    public Transform hudPos; //데미지 텍스트 위치
    bool Enemy_Run;
    public Slider enemyHpSlider;
    public Transform box; // 플레이어 추적 박스
    public Vector2 boxSize; // // 플레이어 추적 반경
    public GameObject dropItem;
    int var = 0;  //몹이 죽어서도 움직이는 거 방지용




    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        Sprite_Renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
        EnemyCurrentHp = EnemyHP;
        enemyHpSlider.gameObject.SetActive(false);
        Think();
        Invoke("Think", 3);
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (rigid.velocity.x < 0)
        {
            Sprite_Renderer.flipX = false;
        }
        else if (rigid.velocity.x > 0)
        {
            Sprite_Renderer.flipX = true;
        }
        //raycast direction set
        if (rigid.velocity.y > 0)
        {
            enemyDirectionVector = Vector3.up;
        }
        else if (rigid.velocity.y < 0)
        {
            enemyDirectionVector = Vector3.down;
        }
        else if (rigid.velocity.x < 0)
        {
            enemyDirectionVector = Vector3.left;
        }
        else if (rigid.velocity.x > 0)
        {
            enemyDirectionVector = Vector3.right;
        }

        enemyHpSlider.maxValue = EnemyHP;
        enemyHpSlider.value = EnemyCurrentHp;
        enemyHpSlider.transform.position = hudPos.position;

        // 플레이어 추적
        //Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(box.position, boxSize, 0);
        //foreach (Collider2D collider in collider2Ds)
        //{
        //    if (collider.tag == "Player")
        //    {
        //        Next_Move_X = 0;
        //        Next_Move_Y = 0;
        //        transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, Time.deltaTime * speed);
        //    }

        //}
        if (var == 0)
        {
            StartCoroutine("Trace");
        }
        
        else if(var == 1)
        {
            StopCoroutine("Trace");
        }
    }


    void FixedUpdate()
    {
        //move
        rigid.velocity = new Vector2(Next_Move_X, Next_Move_Y);

        //Platform check
        //Vector2 Front_Vec = new Vector2(rigid.position.x + Next_Move * 0.5f, rigid.position.y);
        //Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

        //RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, enemyDirectionVector, 1f,
        //   LayerMask.GetMask("Wall"));
        //Debug.DrawRay(rigid.position, enemyDirectionVector, new Color(0, 1, 0));

        //if (rayHit.collider != null)
        //{
        //    EnemyTurn();
        //}

    }

    void Think()
    {
        Next_Move_X = Random.Range(-3, 4);
        Next_Move_Y = Random.Range(-2, 3);
        float Next_Think_Time = Random.Range(2f, 4f);
        Invoke("Think", Next_Think_Time);
    }
    //void EnemyTurn()
    //{
    //    Next_Move_X = Next_Move_X * -1;
    //    Sprite_Renderer.flipX = Next_Move_X == 1;
    //    Invoke("Think", 3);

    //}

    public void EnemyDamaged(float damage)
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = (int)damage;
        EnemyCurrentHp = EnemyCurrentHp - damage;
        ActiveHpSlider();
        if (EnemyCurrentHp <= 0)
        {
            Sprite_Renderer.color = new Color(1, 0, 1, 0.5f);
            Sprite_Renderer.flipY = true;
            collider.enabled = false;
            var = 1;
            Invoke("UnActiveHpSlider", 1);
            Invoke("DeActive", 1);
            Invoke("DropItem", 1.2f);
            StopMove();
            player.CurrentExp += EnemyExp;
            Invoke("Revive", 10);
        }
        else
        {
            Invoke("UnActiveHpSlider", 5);
            rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            Sprite_Renderer.color = new Color(1, 0.9f, 0.02f, 0.5f);
            Invoke("ReturnEnemyColor", 1);
        }
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }
    public void ReturnEnemyColor()
    {
        Sprite_Renderer.color = new Color(1, 1, 1, 1);
    }
    public void Revive()
    {
        EnemyCurrentHp = EnemyHP;
        gameObject.SetActive(true);
        Sprite_Renderer.flipY = false;
        Sprite_Renderer.color = new Color(1, 1, 1, 1);
        collider.enabled = true;
        var = 0;
    }
    public void StopMove()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezePosition;
        Invoke("ReMove", 10f);
    }
    public void ReMove()
    {
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void ActiveHpSlider()
    {
        enemyHpSlider.gameObject.SetActive(true);
    }
    public void UnActiveHpSlider()
    {
        enemyHpSlider.gameObject.SetActive(false);
    }
    public void DropItem()
    {
        int percentItem = Random.Range(0, 10);
        if (percentItem > 6)
        {
            GameObject item = Instantiate(dropItem);
            item.transform.position = rigid.position;
            item.SetActive(true);
        }
    }

    IEnumerator Trace()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(box.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                Next_Move_X = 0;
                Next_Move_Y = 0;
                transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, Time.deltaTime * speed);
            }

        }
        //yield return new WaitForSeconds(5f);
        yield return null;
    }
}
