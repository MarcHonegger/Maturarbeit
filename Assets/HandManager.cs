using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class HandManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private Vector3 _oldPos;
    public GameObject laneOptionPrefab;
    private readonly List<Image> _laneSprites = new();
    private readonly List<TextMeshProUGUI> _laneNumbers = new();
    private static Random _rng;  
    private int _screenHeight;
    private int _screenWidth;
    private int _thirdScreenHeight;
    private int _quarterScreenWidth;

    public List<GameObject> troopsForDeck;
    public GameObject cardPrefab;
    public List<GameObject> cardsInDeck;
    private readonly List<GameObject> _cardsInHand = new();

    public static HandManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
    
    void Start()
    {
        UpdateScreenSize();
        GenerateSprites();
        ShuffleDeck();
        CreateDeck();
        for (int i = 0; i < 5; i++)
        {
            DrawCard(cardsInDeck.First());
        }
        InvokeRepeating(nameof(Test), 10, 10);
        CreateDeck();
    }

    private void Test()
    {
        DrawCard(cardsInDeck.First());
    }

    private void CreateDeck()
    {
        cardsInDeck = new List<GameObject>();
        foreach (var troop in troopsForDeck)
        {
            CreateCard(troop);
        }
    }

    private void ShuffleDeck()
    {
        int n = cardsInDeck.Count;  
        while (n > 1) {  
            n--;  
            int k = _rng.NextInt(n + 1);  
            (cardsInDeck[k], cardsInDeck[n]) = (cardsInDeck[n], cardsInDeck[k]);
        } 
    }

    private void CreateCard(GameObject troop)
    {
        var card = Instantiate(cardPrefab, transform.parent);
        card.name = $"Card {troop.name}";
        card.GetComponent<CardHandler>().cardGameObject = troop;
        card.SetActive(false);
        cardsInDeck.Add(card);
    }

    private void DrawCard(GameObject card)
    {
        card.SetActive(true);
        _cardsInHand.Add(card);
        cardsInDeck.Remove(card);
        ResetCardPositions();
    }

    public void CardWasPlayed(GameObject card)
    {
        Destroy(card);
        _cardsInHand.Remove(card);
        ResetCardPositions();
    }

    private void UpdateScreenSize()
    {
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;
        _thirdScreenHeight = _screenHeight / 3;
        _quarterScreenWidth = _screenWidth / 4;
    }

    private void GenerateSprites()
    {
        Vector3 laneOptionScale = new Vector3(_quarterScreenWidth / 100f, _thirdScreenHeight * 2 / 100f, 1);

        var laneOptionParent = Instantiate(new GameObject("laneOverlay"), canvas.transform).transform;
        for (int i = 0; i < 4; i++)
        {
            var laneOption = Instantiate(laneOptionPrefab, new Vector3(_quarterScreenWidth / 2f + _quarterScreenWidth * i, _thirdScreenHeight * 2f, 0f), quaternion.identity, laneOptionParent);
            laneOption.transform.localScale = laneOptionScale;
            var laneNumber = laneOption.GetComponentInChildren<TextMeshProUGUI>();
            laneNumber.text = (i + 1).ToString();
            var laneOptionImage = laneOption.GetComponent<Image>();
            
            laneOptionImage.enabled = false;
            laneNumber.enabled = false; 
            _laneSprites.Add(laneOptionImage);
            _laneNumbers.Add(laneNumber);
        }
    }

    public void StartRendering()
    {
        InvokeRepeating(nameof(UpdateRendering), 0f, 0.5f);
    }
    
    public void StopRendering()
    {
        for (int i = 0; i < 4; i++)
        {
            _laneSprites[i].enabled = false;
            _laneNumbers[i].enabled = false;
        }
        CancelInvoke(nameof(UpdateRendering));
    }

    private void UpdateRendering()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerManager.instance.IsValidSpawn(i))
            {
                _laneSprites[i].enabled = true;
                _laneNumbers[i].enabled = true;
            }
            else
            {
                _laneSprites[i].enabled = false;
                _laneNumbers[i].enabled = false;
            }
        }
    }

    public void CheckPosition(Vector3 mousePosition, GameObject troopPrefab, GameObject card)
    {
        if (mousePosition.y > _thirdScreenHeight)
        {
            if (mousePosition.x < _quarterScreenWidth)
            {
                PlayerManager.instance.IsValidSpawn(0);
                PlayerManager.instance.PlayCard(troopPrefab, 0, card);
            } 
            else if (mousePosition.x > _quarterScreenWidth && mousePosition.x < 2 * _quarterScreenWidth)
            {
                PlayerManager.instance.IsValidSpawn(1);
                PlayerManager.instance.PlayCard(troopPrefab, 1, card);
            }
            else if (mousePosition.x > 2 * _quarterScreenWidth && mousePosition.x < 3 * _quarterScreenWidth)
            {
                PlayerManager.instance.IsValidSpawn(2);
                PlayerManager.instance.PlayCard(troopPrefab, 2, card);
            }
            else if (mousePosition.x > 3 * _quarterScreenWidth && mousePosition.x < 4 * _quarterScreenWidth)
            {
                PlayerManager.instance.IsValidSpawn(3);
                PlayerManager.instance.PlayCard(troopPrefab, 3, card);
            }
            else
            {
                ResetCardPositions();
            }
        }
        else
        {
            ResetCardPositions();
        }
    }
    
    private void ResetCardPositions()
    {
        var position = new Vector3(100 * -_cardsInHand.Count / 2f, 0, 0);
        foreach (var card in _cardsInHand)
        {
            card.transform.localPosition = position;
            position += new Vector3(100, 0, 0);
        }
    }
}
