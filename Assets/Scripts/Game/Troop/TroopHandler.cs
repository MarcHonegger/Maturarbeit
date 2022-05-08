using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;

public class TroopHandler : MonoBehaviour
{
    public int energyCost;
    public float movementSpeed;
    public float currentMovementSpeed;
    public bool ghostEffect;
    public float health;
    private HealthBar _healthBar;
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
    private Vector3 _offset;

    public bool isDead => health <= 0.001;

    private void Start()
    {
        _cam = FindObjectOfType<Camera>();
        _canvas = FindObjectOfType<Canvas>();
        _canvasRect = _canvas.GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _offset = new Vector3(0, _spriteRenderer.bounds.size.y + 1f, 0);

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
        Vector2 viewportPosition = _cam.WorldToViewportPoint(gameObject.transform.position + _offset);
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
        Death?.Invoke();
        Debug.Log($"Died {gameObject.name}");
        _animator.SetTrigger(DiedAnimation);
        
        Destroy(_healthBar.gameObject);
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = false;
        }
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Destroy(gameObject, 2);
        foreach (MonoBehaviour script in GetComponents<MonoBehaviour>())
        {
            script.CancelInvoke();
            script.enabled = false;
        }
    }

    private void GenerateHealthBar()
    {
        GameObject healthBarGameObject = Instantiate(healthBarPrefab, GameObject.Find("HealthBarFolder").transform);

        _healthBar = healthBarGameObject.GetComponent<HealthBar>();
        _healthBar.SetMaximumHealth(health);

        _rectTransform = _healthBar.GetComponent<RectTransform>();
        UpdateHealthBarPosition();
    }

    private void UpdateHealthBarValue(float healthChange)
    {
        _healthBar.ChangeHealth(healthChange);
    }

    private void UpdateHealthBarPosition() => _rectTransform.anchoredPosition = GetScreenPoint();

    public event Action Death;

}
