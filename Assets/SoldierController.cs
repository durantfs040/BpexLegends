using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SoldierController : MonoBehaviour
{
    private Animator _animator;
    private float _moveSpeed = 5;
    public Transform[] waypoints;
    private Vector3 _cameraPosition;
    private int _waypointIndex = 0;
    public float bulletDamage = 0.03f;
    private PlayerController player;
    public GameObject dead;
    public GameObject gameover;
    public float enemyHealth = 1f;
    public GameObject RespawnPoint;
    private float RespawnTimer = 0;
    private float HealTimer = 0;
    AudioSource fireAudio;
    bool audioShouldBePlayed = true;
    int remainingSecondsToPlayAudio = 0;
    int currentDistance;
    public GameObject hitMarker;
    private float timer = 0;
    bool killAdded = false;

    bool IsPlayerVisible(Vector3 cameraPosition)
    {
        Vector3 enemyPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Vector3 directionToTarget = (cameraPosition - enemyPosition).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToTarget);
        RaycastHit hit;
        if (!(Physics.Raycast(enemyPosition, directionToTarget, out hit, 40f) &&
            Vector3.Distance(cameraPosition, hit.transform.position) < 3f))
        {
            return false;
        }

        // currentDistance is an approximated integer instead of the precise float value
        if (Vector3.Distance(cameraPosition, enemyPosition) < 5)
            currentDistance = 5;
        else
            currentDistance = (int)Vector3.Distance(cameraPosition, enemyPosition);

        if (Vector3.Distance(cameraPosition, enemyPosition) < 5)
        {
            return true;
        }
        if(dotProduct >= Mathf.Cos(75f * Mathf.Deg2Rad))
        {
            return true;
        }

        if (enemyHealth < 1)
        {
            return true;
        }
        return false;
    }
    
    IEnumerator FireAtPlayer() 
    {
        int currScore = ScoreManager.instance.getKills();
        float dmgBonus = Mathf.Pow(1.07f, currScore);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(playAudio());
        player = FindObjectOfType<PlayerController>();
        player.health -= (bulletDamage * dmgBonus * Time.deltaTime);
        player.healthTimer = 0;
        _animator.SetBool("isPlayerSeen", true);

        Vector3 shootDirection = (_cameraPosition - transform.position).normalized;
        Vector3 nonNormalized = _cameraPosition - transform.position;
        Vector2 twoDimension = new Vector2(nonNormalized[0], nonNormalized[2]).normalized;
        float asinDeg = Mathf.Asin(twoDimension.y) * Mathf.Rad2Deg;
        float acosDeg = Mathf.Acos(twoDimension.x) * Mathf.Rad2Deg;
        float soldierDirection = 0.0f;
        if (asinDeg <= 0.0f)
            soldierDirection = acosDeg;
        else if (acosDeg >= 90)
            soldierDirection = asinDeg + 180;
        else
            soldierDirection = -asinDeg;
        soldierDirection += 90;
        float playerDirection = InfimaGames.LowPolyShooterPack.CameraLook.instance.getDirection();
        float relativeDirection = soldierDirection + 180 - playerDirection;
        if (relativeDirection < 0)
            relativeDirection += 360;
        relativeDirection %= 360;
        Debug.Log(relativeDirection);
        if (relativeDirection < 22.5f || relativeDirection >= 337.5f)
            topWarningManager.instance.activate();
        else if (relativeDirection < 67.5f)
            topRightWarningManager.instance.activate();
        else if (relativeDirection < 112.5f)
            rightWarningManager.instance.activate();
        else if (relativeDirection < 157.5f)
            bottomRightWarningManager.instance.activate();
        else if (relativeDirection < 202.5f)
            bottomWarningManager.instance.activate();
        else if (relativeDirection < 247.5f)
            bottomLeftWarningManager.instance.activate();
        else if (relativeDirection < 292.5f)
            leftWarningManager.instance.activate();
        else
            topLeftWarningManager.instance.activate();
        shootDirection.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(shootDirection), 5 * Time.deltaTime);
        _moveSpeed = 0;
        
    }

    IEnumerator playAudio()
    {
        if (audioShouldBePlayed)
        {
            fireAudio.volume = 0.5f / currentDistance;
            fireAudio.Play();
            audioShouldBePlayed = false;
            remainingSecondsToPlayAudio = 10;
        }
        else
        {
            remainingSecondsToPlayAudio -= 1;
        }
        if (remainingSecondsToPlayAudio == 0)
        {
            audioShouldBePlayed = true;
        }
        yield return new WaitForSeconds(0.1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            Debug.Log("hit");
            hitMarker.SetActive(true);
            timer = 0;
        }
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        fireAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (hitMarker.activeInHierarchy)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 0.05f)
        {
            hitMarker.SetActive(false);
            timer = 0;
        }

        if(enemyHealth <= 0)
        {
            if (!killAdded)
            {
                ScoreManager.instance.AddKills();
                killAdded = true;
            }
            transform.position = RespawnPoint.transform.position;
            transform.rotation = RespawnPoint.transform.rotation;
            SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            collider.enabled = false;
            renderer.enabled = false;
            if (RespawnTimer < 3f)
            {
                RespawnTimer += Time.deltaTime;
            }
            else
            {
                killAdded = false;
                renderer.enabled = true;
                collider.enabled = true;
                enemyHealth = 1f;
                _waypointIndex = 1;
                RespawnTimer = 0f;
            }
            return;
        };

        if (enemyHealth < 1)
        {
            HealTimer += Time.deltaTime;
        }

        if (HealTimer >= 4f)
        {
            enemyHealth = 1f;
            HealTimer = 0;
        }
        
        _cameraPosition = Camera.main.transform.position;
        if (waypoints.Length == 0) return;
        
        if (_waypointIndex >= waypoints.Length)
        {
            _waypointIndex = 0;
        }

        Transform targetWaypoint = waypoints[_waypointIndex];
        Vector3 moveDirection = (targetWaypoint.position - transform.position).normalized;

        transform.position += moveDirection * _moveSpeed * Time.deltaTime;

        if(moveDirection != Vector3.zero) // This check prevents LookRotation from throwing an error with a zero vector
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), _moveSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            _waypointIndex++;
        }

        if (IsPlayerVisible(_cameraPosition) && !(dead.activeInHierarchy) && !(gameover.activeInHierarchy) && enemyHealth > 0)
        {
            StartCoroutine(FireAtPlayer());
        }
        else
        {
            _animator.SetBool("isPlayerSeen", false);
            _moveSpeed = 5;
        }
        
    }

    

}