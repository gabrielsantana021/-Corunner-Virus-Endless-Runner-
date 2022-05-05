using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed = 10f;
    public float minSpeed = 10f;
    public float maxSpeed = 60f;
    public float laneSpeed;
    public float jumpLenght;
    public float jumpHeight;
    public float slideLength;
    public int maxLife = 3;
    [HideInInspector]
    public int coins;
    [HideInInspector]
    public float score;
    public float invincibleTime;
    public GameObject model;

    private Animator anim;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Vector3 boxColliderSize;
    private Vector3 boxColliderCenter;
    private int currentLane = 2;
    private Vector3 verticalTargetPosition;
    private bool jumping = false;
    private float jumpStart;
    private bool sliding = false;
    private float slideStart;
    private int currentLife;
    private bool invincible = false;
    static int blinkingValue;
    private UIManager uiManager;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        anim.Play("runStart");
        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;
        boxColliderCenter = boxCollider.center;
        currentLife = maxLife;
        playerSpeed = minSpeed;
        blinkingValue = Shader.PropertyToID("_BlinkingValue");
        uiManager = FindObjectOfType<UIManager>();
        GameManager.gm.StartMissions();
    }
    private void Update()
    {
        score += Time.deltaTime * playerSpeed;
        uiManager.UpdateScore((int)score);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-2);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(2);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide();
        }

        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLenght;
            if(ratio >= 1f)
            {
                jumping = false;
                anim.SetBool("Jumping", false);
            }
            else
                {
                    verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
                }
            }
            else
            {
                verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
            }

        if (sliding)
        {
            float ratio = (transform.position.z - slideStart) / slideLength;
            if (ratio >= 1f)
            {
                sliding = false;
                anim.SetBool("Sliding", false);
               boxCollider.size = boxColliderSize;
               boxCollider.center = boxColliderCenter;
            }
        }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * playerSpeed;
    }
    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < -1 || targetLane > 4)
            return;
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 2), 0, 0);
    }

    void Jump()
    {
        if(!jumping)
        {
            jumpStart = transform.position.z;
            anim.SetFloat("JumpSpeed", playerSpeed / jumpLenght);
            anim.SetBool("Jumping", true);
            jumping = true;

        }
    }
    void Slide()
    {
        if (!jumping && !sliding)
        {
            slideStart = transform.position.z;
            this.transform.position += new Vector3(0, -1, 0);
            anim.SetFloat("JumpSpeed", playerSpeed / slideLength);
            anim.SetBool("Sliding", true);
            Vector3 newSize = boxCollider.size;
            Vector3 newCenter = boxCollider.center;
            newSize.y = newSize.y / 2;
            boxCollider.size = newSize;
            newCenter.y = newCenter.y / 2;
            boxCollider.center = newCenter;
            sliding = true;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coins++;
            uiManager.UpdateCoins(coins);
            other.transform.parent.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().PlaySound("Coin Pickup");
        }


        if (invincible)
            return;

        if (other.CompareTag("Obstacle"))
        {
            currentLife--;
            uiManager.UpdateLives(currentLife);
            anim.SetTrigger("Hit");
            playerSpeed = 0;
            if(currentLife <= 0)
            {
                playerSpeed = 0;
                anim.SetBool("Dead", true);
                uiManager.gameOverPanel.SetActive(true);
                Invoke("CallMenu", 2f);
            }
            else
            {
                StartCoroutine(Blinking(invincibleTime));
            }
        }
    }

    IEnumerator Blinking(float time)
    {
        invincible = true;
        float timer = 0;
        float currentBlink = 1f;
        float lastBlink = 0;
        float blinkPeriod = 0.1f;
        bool enabled = false;
        yield return new WaitForSeconds(1f);
        playerSpeed = minSpeed;
        while(timer < time && invincible)
        {
            //Shader.SetGlobalFloat(blinkingValue, currentBlink);
            model.SetActive(enabled);
            yield return null;
            timer += Time.deltaTime;
            lastBlink += Time.deltaTime;
            if(blinkPeriod < lastBlink)
            {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled;
            }
        }
        model.SetActive(true);
        //Shader.SetGlobalFloat(blinkingValue, 0);
        invincible = false;
    }

    public void IncreaseSpeed()
    {
        playerSpeed *= 1.15f;
        if (playerSpeed >= maxSpeed)
            playerSpeed = maxSpeed;
    }

    void CallMenu()
    {
        GameManager.gm.coins += coins;
        GameManager.gm.EndRun();
    }
}