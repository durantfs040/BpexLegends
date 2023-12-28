using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Image image;
    public Transform[] respawnPoints;
    public GameObject dead;
    public GameObject gameover;
    public float health = 1f;
    private PlayerInput playerInput;
    private float RespawnTimer = 0f;
    private Rigidbody rb;
    public GameObject healthLine;
    private RectTransform rt;
    public float healthTimer = 0f;
    public GameObject ammunitionTotal;
    public GameObject ammunitionCurrent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        rt = healthLine.GetComponent<RectTransform>();
    }

    void Respawn()
    {
        playerInput.enabled = true;
        dead.SetActive(false);
        health = 1f;
        int randomNumber = Random.Range(0, 7);
        rb.MovePosition(respawnPoints[randomNumber].position);
        rb.MoveRotation(respawnPoints[randomNumber].rotation);
        ammunitionTotal.GetComponent<TextMeshProUGUI>().text = "120";
        ammunitionCurrent.GetComponent<TextMeshProUGUI>().text = "30";
    }

    private void Update()
    {
        
        if (gameover.activeInHierarchy)
        {
            gameObject.GetComponent<PlayerInput>().enabled = false;
        } 
        rt.sizeDelta = new Vector2 (200 * health, 4);
        image.GetComponent<Image>().color = new Color(image.color.r, image.color.g, image.color.b, 50*(1-health)/255);
        
        if (health <= 0) // Check the isDead flag
        {
            dead.SetActive(true);
            playerInput.enabled = false;
            if (RespawnTimer < 3f)
            {
                RespawnTimer += Time.deltaTime;
                return;
            }
            Respawn();
            RespawnTimer = 0f;
        }
        else if (health <= 1)
        {
            healthTimer += Time.deltaTime;
        }

        if (healthTimer >= 4f)
        {
            health += Time.deltaTime;
            health = Mathf.Min(health, 1f);
            if(health == 1f) healthTimer = 0;
        }
        
    }
}