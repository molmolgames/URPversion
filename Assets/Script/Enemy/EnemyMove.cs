using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D En; // 적
    SpriteRenderer SR; 
    Animator animation;
    public PlayerMoving player;
    CapsuleCollider2D CC;

    public int h; // 이동 방향
    public float speed; // 이동 속도
    public float EnemyHP; // 적 총 체력
    public float EnemyCurrentHp; // 적 현재체력
    public float EnemyAtk; // 적 공격력
    public float EnemyExp; // 적 경험치


 
    public GameObject hudDamageText; //데미지 텍스트
    public Transform hudPos; //데미지 텍스트 위치
    public Slider enemyHpSlider;
    public GameObject dropItem;
 


    //public Slider enemyHpSlider; // 적체력 표시

    void Awake()
    {
        En = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        animation = GetComponent<Animator>();
        CC = GetComponent<CapsuleCollider2D>();
        EnemyCurrentHp = EnemyHP;
        enemyHpSlider.gameObject.SetActive(false);
        Think();
        Invoke("Think", 5);
        SR.flipX = h==1; // 렌더링 방향 이동에 맞춰서 바꾸기
    }
    void Update()
    {
        if (Mathf.Abs(En.velocity.x) < 0.3)
        {
            animation.SetBool("isrunning", false);
        }
        else
        {
            animation.SetBool("isrunning", true);
        }
        //enemyHpSlider.maxValue = EnemyHP;
        //enemyHpSlider.value = EnemyCurrentHp;

        enemyHpSlider.maxValue = EnemyHP;
        enemyHpSlider.value = EnemyCurrentHp;
        enemyHpSlider.transform.position = hudPos.position;

    }

    void FixedUpdate() {
        
        // 이동
        En.velocity = new Vector2(h* speed,En.velocity.y);
        //Debug.Log(h);

        // 우측 이동 시 바닥 감지
        if(En.velocity.x > 0){
            Debug.DrawRay(En.position + Vector2.right * 0.3f, Vector2.down, new Color(0,1,0));
            RaycastHit2D rayHit = Physics2D.Raycast(En.position + Vector2.right * 0.3f, Vector2.down, 1, LayerMask.GetMask("normalfloor"));
            //Debug.Log(rayHit.collider);
            if(rayHit.collider == null){
                Turn();
            }
        }

        // 좌측 이동 시 바닥 감지
        else if(En.velocity.x < 0){
            Debug.DrawRay(En.position + Vector2.left * 0.3f, Vector2.down, new Color(0,1,0));
            RaycastHit2D rayHit = Physics2D.Raycast(En.position + Vector2.left * 0.3f, Vector2.down, 1, LayerMask.GetMask("normalfloor"));
            //Debug.Log(rayHit.collider);
            if(rayHit.collider == null){
                Turn();
            }
        }

    }
    
    void Think(){
        
        // 랜덤한 시간 마다 행동 패턴 변화 (우측 좌측 정지)
        h = Random.Range(-1,2);
        float thinktime = Random.Range(2f,4f);
    
        Invoke("Think", thinktime); // thinktime마다 think 재호출 회귀

        if (h != 0)
        {
            SR.flipX = h == 1;
        }
    }

    void Turn(){

        // 이동 방향 변화
        h *= (-1);
        SR.flipX = h==1;

        CancelInvoke(); // 이동 바꾸면 시간 초기화
        Invoke("Think", 5);            
    }


    //public void EnemyDamaged()
    //{
    //    SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
    //    SR.flipY = true;
    //    h = 0;
    //    CC.enabled = false;
    //    En.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    //    Invoke("DeActive", 3);
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
            SR.color = new Color(1, 0, 1, 0.5f);
            SR.flipY = true;
            CC.enabled = false;
            Invoke("DeActive", 1);
            Invoke("UnActiveHpSlider", 1);
            Invoke("DropItem", 1.2f);
            StopMove();
            player.CurrentExp += EnemyExp;
            Invoke("Revive", 10);
        }
        else
        {
            Invoke("UnActiveHpSlider", 5);
            En.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
            Invoke("ReturnEnemyColor", 1);
        }
    }
    //적 비활성화
    public void DeActive()
    {
        gameObject.SetActive(false);
    }
    public void ReturnEnemyColor()
    {
        SR.color = new Color(1, 1, 1, 1);
    }
    // 적 부활
    public void Revive()
    {
        EnemyCurrentHp = EnemyHP;
        gameObject.SetActive(true);
        SR.flipY = false;
        SR.color = new Color(1, 1, 1, 1);
        CC.enabled = true;
    }
    //적 죽었을시 위치 f초 멈춤
    public void StopMove()
    {
        En.constraints = RigidbodyConstraints2D.FreezePosition;
        Invoke("ReMove", 10f);
    }
    public void ReMove()
    {
        En.constraints = RigidbodyConstraints2D.None;
        En.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        if (percentItem > 7)
        {
            GameObject item = Instantiate(dropItem);
            item.transform.position = En.position;
            //Rigidbody2D itemRigid;
            //itemRigid = GetComponent<Rigidbody2D>();
            //itemRigid.AddForce(Vector2.up * 50, ForceMode2D.Impulse);
            item.SetActive(true);
        }
    }
}
