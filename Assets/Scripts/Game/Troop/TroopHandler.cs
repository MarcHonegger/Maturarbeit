using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public enum AttackType
{
    Melee = 0, Ranged = 1
}

public class TroopHandler : NetworkBehaviour
{
    public int energyCost;
    public float movementSpeed;
    public float currentMovementSpeed;
    public bool ghostEffect;
    public float thornDamage;
    public AttackType thornType;
    public float health;
    public GameObject deathPrefab;
    
    public HealthBar healthBar;
    public GameObject healthBarPrefab;
    private Sprite _redHealthBarFill;
    private Sprite _greenHealthBarFill;

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
    public NewPlayerManager newPlayerManager;
    public bool isDead => health <= 0.001;

    private void Start()
    {
        _cam = FindObjectOfType<Camera>();
        _canvas = FindObjectOfType<Canvas>();
        _canvasRect = _canvas.GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _redHealthBarFill = Resources.Load<Sprite>("Game/HealthBar/RedFill");
        _greenHealthBarFill = Resources.Load<Sprite>("Game/HealthBar/GreenFill");

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
        _animator.SetTrigger(movementSpeed > 0 ? StopIdleAnimation : IdleAnimation);
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

    // TODO ChangeHealth()???
    [ClientRpc]
    public void TakeDamage(float amount, TroopHandler attacker, AttackType type)
    {
        if(thornDamage > 0 && attacker && type == thornType)
            attacker.TakeDamage(thornDamage);
        TakeDamage(amount);
    }
    
    public void TakeDamage(float amount)
    {
        ChangeTroopDesign();
        Invoke(nameof(ResetTroopDesign), 0.25f);

        UpdateHealthBarValue(-amount);

        health -= amount;
        CheckDeath();
    }

    [Server]
    private void CheckDeath()
    {
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

    [ClientRpc]
    public void Die()
    {
        Debug.Log($"Died {gameObject.name}");
        NetworkIdentity netID = NetworkClient.connection.identity;
        newPlayerManager = netID.GetComponent<NewPlayerManager>();
        StopMoving(); 
        Death?.Invoke();
        NetworkServer.Destroy(gameObject.GetComponent<TroopHandler>().healthBar.gameObject);
        Debug.Log("hi");
        NetworkServer.Destroy(gameObject);

        var deathObject = Instantiate(deathPrefab, transform.position, quaternion.identity);
        deathObject.transform.RotateAround(deathObject.transform.GetChild(0).position, Vector3.right, 45);
        deathObject.GetComponent<SpriteRenderer>().flipX = !CompareTag("LeftPlayer");
        Destroy(deathObject, 8f);
        
        Destroy(healthBar.gameObject);
    }

    private void GenerateHealthBar()
    {
        GameObject healthBarGameObject = Instantiate(healthBarPrefab, GameObject.Find("HealthBarFolder").transform);

        healthBar = healthBarGameObject.GetComponent<HealthBar>();
        healthBar.SetMaximumHealth(health);
        healthBar.transform.GetChild(1).GetComponent<Image>().sprite = CompareTag("LeftPlayer") ? _redHealthBarFill : _greenHealthBarFill;

        _rectTransform = healthBar.GetComponent<RectTransform>();
        UpdateHealthBarPosition();
    }

    private void UpdateHealthBarValue(float healthChange)
    {
        healthBar.ChangeHealth(healthChange);
    }

    private void UpdateHealthBarPosition() => _rectTransform.anchoredPosition = GetScreenPoint();

    public event Action Death;

}

