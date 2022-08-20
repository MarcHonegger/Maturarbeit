using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public enum AttackType
{
    Melee = 0, Ranged = 1, Suicide = 2, Effect = 3
}

public class TroopHandler : NetworkBehaviour
{
    public int energyCost;
    public float movementSpeed;
    public float currentMovementSpeed;
    public float health;
    public float maximumHealth;
    public GameObject deathPrefab;
    public String bonusInfo;
    public AttackType attackType;
    private Color _standardColor;
    
    public HealthBar healthBar;
    public GameObject healthBarPrefab;

    private Camera _cam;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private RectTransform _canvasRect;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private static readonly int IdleAnimation = Animator.StringToHash("Idle");
    private static readonly int StopIdleAnimation = Animator.StringToHash("StopIdle");
    private bool _idle;
    private TroopHandler _troopInFront;
    private readonly Vector3 _offset = new Vector3(0, 1f, 0);
    public bool isDead => health <= 0.001;

    private void Start()
    {
        health = maximumHealth;
        _cam = FindObjectOfType<Camera>();
        _canvas = FindObjectOfType<Canvas>();
        _canvasRect = _canvas.GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _standardColor = _spriteRenderer.color;

        StartMoving();
        GenerateHealthBar();

        if (gameObject.CompareTag("RightPlayer"))
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Update()
    {
        UpdateHealthBarPosition();
    }

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

    [Server]
    public void TakeDamage(float amount, TroopHandler attacker, AttackType type)
    {
        DamageTaken?.Invoke(attacker, type);
        TakeDamage(amount);
    }
    
    [ClientRpc]
    public void TakeDamage(float amount)
    {
        ChangeTroopDesign();
        Invoke(nameof(ResetTroopDesign), 0.25f);
        health -= amount;
        CheckDeath();
        UpdateHealthBarValue();
    }

    [Server]
    private void CheckDeath()
    {
        if (isDead)
        {
            Die();
        }
    }
    
    
    [Server]
    public void ChangeHealth(float amount, bool onlyCurrent)
    {
        health = Mathf.Max(health + amount, maximumHealth);
        if(!onlyCurrent)
            maximumHealth += amount;
        UpdateHealthBarValue();
    }

    private void ChangeTroopDesign()
    {
        var color = _spriteRenderer.color;
        _spriteRenderer.color = new Color(color.r, color.g * 0.3f, color.b * 0.3f);
    }

    private void ResetTroopDesign()
    {
        _spriteRenderer.color = _standardColor;
    }

    [ClientRpc]
    public void Die()
    {
        Debug.Log($"Died {gameObject.name}");
        Death?.Invoke();

        Vector3 position = transform.position;
        GameObject deathObject = Instantiate(deathPrefab, position, Quaternion.identity);
        deathObject.transform.RotateAround(deathObject.transform.GetChild(0).position, Vector3.right, 45);
        deathObject.GetComponent<SpriteRenderer>().flipX = !CompareTag("LeftPlayer");
        Destroy(deathObject, 4f);

        NetworkServer.Destroy(gameObject.GetComponent<TroopHandler>().healthBar.gameObject);
        NetworkServer.Destroy(gameObject);
    }

    private void GenerateHealthBar()
    {
        GameObject healthBarGameObject = Instantiate(healthBarPrefab, GameObject.Find("HealthBarFolder").transform);

        healthBar = healthBarGameObject.GetComponent<HealthBar>();
        healthBar.SetMaximumHealth(health);
        healthBar.tag = gameObject.tag;

        _rectTransform = healthBar.GetComponent<RectTransform>();
        UpdateHealthBarPosition();
        UpdateHealthBarValue();
    }

    private void UpdateHealthBarValue()
    {
        healthBar.HealthChanged(health, maximumHealth);
    }

    private void UpdateHealthBarPosition() => _rectTransform.anchoredPosition = GetScreenPoint();

    public event Action<TroopHandler, AttackType> DamageTaken;
    public event Action Death;

}

