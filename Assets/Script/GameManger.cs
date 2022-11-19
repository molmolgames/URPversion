using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManger : MonoBehaviour
{


    public int Health_Point;
    public PlayerMoving player;
    public GameObject[] Stages;
    public GameObject Restart_Button;
    public Text playerLevelText;
    public Text playerHpText;
    //public Image[] Life;
  
    void Start()
    {
        
    }
    private void Awake()
    {
        playerLevelText.text = "Lv . " + player.PlayerLevel.ToString();
        playerHpText.text = player.CurrentHp.ToString() + " / " + player.PlayerHp.ToString();
    }
    private void FixedUpdate()
    {
        playerLevelText.text = "Lv . " + player.PlayerLevel.ToString();
        playerHpText.text = player.CurrentHp.ToString() + " / " + player.PlayerHp.ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //player reposition
            if (Health_Point > 1)
            {
                PlayerReposition();
            }
            HealthDown(10); //³«µ© 10

        }
    }
    //public void HealthDown()
    //{
    //    if(Health_Point>1)
    //    {
    //        Health_Point--;
    //        Life[Health_Point].color = new Color(1, 0, 0, 0.4f);
    //    }
    //    else
    //    {
    //        Health_Point--;
    //        Life[Health_Point].color = new Color(1, 0, 0, 0.4f);
    //        player.OnDie();
    //        Text buttonText = Restart_Button.GetComponentInChildren<Text>();
    //        Restart_Button.SetActive(true);
    //    }
    //}
    public void HealthDown(float damage)
    {
        if (player.CurrentHp > 0)
        {
            player.CurrentHp -= damage;
            if(player.CurrentHp <= 0) 
            {
                player.OnDie();
                Text buttonText = Restart_Button.GetComponentInChildren<Text>();
                Restart_Button.SetActive(true);
            }
        }
        else
        {
            player.CurrentHp -= damage;
            player.OnDie();
            Text buttonText = Restart_Button.GetComponentInChildren<Text>();
            Restart_Button.SetActive(true);
        }
    }
    public void PlayerReposition()
    {
        player.transform.position=new Vector3(0, 0, 0);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
}
