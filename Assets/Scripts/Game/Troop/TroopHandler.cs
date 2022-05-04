using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TroopHandler : MonoBehaviour
{
    public int energyCost;
    public float movementSpeed;
    public float currentMovementSpeed;
    public bool ghostEffect;
    public float health;
    private Color _color;
    private HealthBar _healthBar;
    public GameObject healthBarPrefab;

    private const int k_TroopLayer = 6;

    private Camera _cam;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private RectTransform _canvasRect;

    private void Start()
    {
        _cam = FindObjectOfType<Camera>();
        _canvas = FindObjectOfType<Canvas>();
        _canvasRect = _canvas.GetComponent<RectTransform>();
        
        StartMoving();
        GenerateHealthBar();
    }

    private void Update()
    {
        Vector2 viewportPosition = _cam.WorldToViewportPoint(gameObject.transform.position);
        var sizeDelta = _canvasRect.sizeDelta;
        Vector2 worldObjectScreenPosition = new Vector2(
            ((viewportPosition.x*sizeDelta.x)-(sizeDelta.x*0.5f)),
            ((viewportPosition.y*sizeDelta.y)-(sizeDelta.y*0.5f)));
 
        _rectTransform.anchoredPosition=worldObjectScreenPosition;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == k_TroopLayer && (ghostEffect || !other.CompareTag(gameObject.tag)))
        {
            StopMoving();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == k_TroopLayer)
        {
            StartMoving();
        }
    }

    public void StopMoving() => currentMovementSpeed = 0;
    public void StartMoving() => currentMovementSpeed = movementSpeed;

    // TODO ChangeHealth()???
    public void TakeDamage(float amount)
    {
        ChangeTroopDesign();
        Invoke(nameof(ResetTroopDesign), 0.25f);
        
        UpdateHealthBar(-amount);
        
        health -= amount;
        if (health <= 0.001)
        {
            Die();
        }
    }

    private void ChangeTroopDesign()
    {
        _color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
    }

    private void ResetTroopDesign()
    {
        _color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    
    public void Die()
    {
        Destroy(_healthBar.gameObject);
        Destroy(gameObject);
    }

    private void GenerateHealthBar()
    {
        var healthBarGameObject = Instantiate(healthBarPrefab, GameObject.Find("HealthBarFolder").transform);
        _healthBar = healthBarGameObject.GetComponent<HealthBar>();
        _healthBar.SetMaximumHealth(health);
        _rectTransform = _healthBar.GetComponent<RectTransform>();
    }

    private void UpdateHealthBar(float healthChange)
    {
        _healthBar.ChangeHealth(healthChange);
    }

}
