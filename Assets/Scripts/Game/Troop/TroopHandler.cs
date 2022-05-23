using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;

public class TroopHandler : NetworkBehaviour
{
    public int energyCost;
    public float movementSpeed;
    public float currentMovementSpeed;
    public bool ghostEffect;
    public float health;
    public HealthBar healthBar;
    public GameObject healthBarPrefab;

    private const int k_TroopLayer = 6;

    private Camera _cam;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private RectTransform _canvasRect;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private static readonly int DiedAnimation = Animator.StringToHash("Died");
    private static readonly int IdleAnimation = Animator.StringToHash("Idle");
    private static readonly int StopIdleAnimation = Animator.StringToHash("StopIdle");
    private bool _idle;
    private TroopHandler _troopInFront;
    private readonly Vector3 _offset = new Vector3(0, 1f, 0);
    public NewPlayerManager newPlayerManager;
    public bool isDead => health <= 0.001;

    private void Start()
    {
        _cam = FindObjectOfType<Camera>();
        _canvas = FindObjectOfType<Canvas>();
        _canvasRect = _canvas.GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        StartMoving();
        GenerateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBarPosition();
        if (isTagged == false)
        {
            NetworkIdentity netID = NetworkClient.connection.identity;
            newPlayerManager = netID.GetComponent<NewPlayerManager>();
            checkTagged();
        }
    }

    private bool isTagged = false;

    private void checkTagged()
    {
        Debug.Log("starting flipping");
        Debug.Log(gameObject);
        Debug.Log(gameObject.tag);
        if (gameObject.tag != "Untagged")
        {
            NetworkIdentity netID = NetworkClient.connection.identity;
            newPlayerManager = netID.GetComponent<NewPlayerManager>();
            newPlayerManager.CmdUpdateTag(gameObject);
            isTagged = true;
        }
    }

    /*
    private void CheckTroopsInFront()
    {
        if (_troopInFront.enabled)
        {
            if (_troopInFront.CompareTag(gameObject.tag))
            {
                _animator.SetTrigger(IdleAnimation);
            }
            StopMoving();
        }
        else
        {
            _animator.SetTrigger(StopIdleAnimation);
            StartMoving();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == k_TroopLayer)
        {
            _troopInFront = other.gameObject.GetComponent<TroopHandler>();
            InvokeRepeating(nameof(CheckTroopsInFront), 0f, 0.1f);
        }
    }*/

    private Vector2 GetScreenPoint()
    {
        Vector2 viewportPosition = _cam.WorldToViewportPoint(gameObject.transform.position - _offset);
        Vector2 sizeDelta = _canvasRect.sizeDelta;
        Vector2 worldObjectScreenPosition = new Vector2(
            ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
            ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));

        return worldObjectScreenPosition;
    }

    public void StartMoving() => currentMovementSpeed = movementSpeed;
    public void StopMoving() => currentMovementSpeed = 0;

    // TODO ChangeHealth()???
    public void TakeDamage(float amount)
    {
        ChangeTroopDesign();
        Invoke(nameof(ResetTroopDesign), 0.25f);

        UpdateHealthBarValue(-amount);

        health -= amount;
        if (isDead)
        {
            Die();
        }
    }

    private void ChangeTroopDesign()
    {
        var color = _spriteRenderer.color;
        color = new Color(color.r, color.g, color.b, 0.8f);
        _spriteRenderer.color = color;
    }

    private void ResetTroopDesign()
    {
        var color = _spriteRenderer.color;
        color = new Color(color.r, color.g, color.b, 1f);
        _spriteRenderer.color = color;
    }

    public void Die()
    {
        Debug.Log($"Died {gameObject.name}");
        NetworkIdentity netID = NetworkClient.connection.identity;
        newPlayerManager = netID.GetComponent<NewPlayerManager>();
        StopMoving(); 
        _animator.SetTrigger(DiedAnimation);  
        Death?.Invoke();
        NetworkServer.Destroy(gameObject.GetComponent<TroopHandler>().healthBar.gameObject);
        Debug.Log("hi");
        NetworkServer.Destroy(gameObject);
        //newPlayerManager.CmdDestroyTroop(healthBar.gameObject);
        //newPlayerManager.CmdDestroyTroop(gameObject);
        foreach (Collider c in GetComponents<Collider>())
        {
            Destroy(c);
        }
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (MonoBehaviour script in GetComponents<MonoBehaviour>())
        {
            if (script != GetComponent<NetworkIdentity>() && script != GetComponent<NetworkTransform>())
            {
                Destroy(script);
            }
        }
        
        Destroy(healthBar.gameObject);
        Destroy(gameObject, 2f);
    }

    private void GenerateHealthBar()
    {
        GameObject healthBarGameObject = Instantiate(healthBarPrefab, GameObject.Find("HealthBarFolder").transform);

        healthBar = healthBarGameObject.GetComponent<HealthBar>();
        healthBar.SetMaximumHealth(health);

        _rectTransform = healthBar.GetComponent<RectTransform>();
        UpdateHealthBarPosition();
        
        NetworkIdentity netID = NetworkClient.connection.identity;
        newPlayerManager = netID.GetComponent<NewPlayerManager>();
        //newPlayerManager.CmdSpawn2(healthBarGameObject);
        try
        {
            //NetworkServer.Spawn(healthBarGameObject, connectionToClient);
        }
        catch
        {
           Debug.Log("could not spawn in health bar"); 
        }
        
    }

    private void UpdateHealthBarValue(float healthChange)
    {
        healthBar.ChangeHealth(healthChange);
    }

    private void UpdateHealthBarPosition() => _rectTransform.anchoredPosition = GetScreenPoint();

    public event Action Death;

}

