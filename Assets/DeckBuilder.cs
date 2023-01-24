using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SerializedDecks = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<(string name, int amount)>>;



public class DeckBuilder : MonoBehaviour
{
    public GameObject cardPreviewPrefab;
    public GameObject addCardButtonPrefab;
    public List<GameObject> allCards;
    private List<(GameObject troopGameObject, int amount)> _currentDeck = new List<(GameObject troopGameObject, int amount)>();
    private List<(GameObject troopGameObject, int amount)> _currentPreview = new List<(GameObject troopGameObject, int amount)>();
    
    public Vector2 buttonsStart;
    public Vector2 buttonsDistance;

    public int maxCardsPerDeck;
    public int maxCardsPerTroop;
    public int previewColumnCount;
    public int previewRowCount;
    public Vector2 cardStart;
    public Vector2 cardDistance;
    public int maxColumns;
    public int maxRows;

    private FileInfo _path;
    private string _currentDeckName;
    private SerializedDecks _decks;

    private Transform _cardPreviewFolder;
    private Transform _buttonFolder;

    void Start()
    {
        var transform1 = transform;
        var previewFolder = new GameObject("Card Previews");
        _cardPreviewFolder = previewFolder.transform;
        _cardPreviewFolder.parent = transform1;
        _cardPreviewFolder.localPosition = Vector3.zero;
        
        var buttonFolder = new GameObject("Buttons");
        _buttonFolder = buttonFolder.transform;
        _buttonFolder.parent = transform1;
        _buttonFolder.localPosition = Vector3.zero;
        
        GenerateButtons();

        _path = new FileInfo(Path.Combine(Application.persistentDataPath, "Decks.json"));

        LoadDeck();
        SaveDeck();

        transform1.localScale = transform1.parent.localScale;
    }

    public void LoadDeck()
    {
        if (!_path.Exists)
        {
            // TODO Default Deck
            return;
        }
        using StreamReader reader = _path.OpenText();
        string json = reader.ReadToEnd();

        _decks = JsonConvert.DeserializeObject<SerializedDecks>(json) ?? new SerializedDecks();
        (string deckName, var deckCards) = _decks.FirstOrDefault();
        if (deckCards is null)
        {
            _currentDeckName = "Default";
            _currentDeck = new List<(GameObject troopGameObject, int amount)>(); // Default Deck => Should not happen?
        }
        else
        {
            _currentDeckName = deckName;
            _currentDeck = deckCards.Select(d => (Resources.Load<GameObject>(d.name), d.amount)).ToList();
        }
        UpdateDeckPreview();
        reader.Close();
    }

    public void SaveDeck()
    {
        Debug.Log("Saving deck at " + _path);
        _decks[_currentDeckName] = _currentDeck.Select(d => (d.troopGameObject.name, d.amount)).ToList();
        WriteDecksToJson();
    }

    public void DeleteDeck()
    {
        Debug.Log("Deleting deck " + _currentDeckName);
        _decks.Remove(_currentDeckName);
        WriteDecksToJson();
        LoadDeck();
    }

    private void WriteDecksToJson()
    {
        var json = JsonConvert.SerializeObject(_decks);
        using FileStream fileStream = _path.Exists ? _path.OpenWrite() : _path.Create();
        var writer = new StreamWriter(fileStream);
        // using StreamWriter writer = new StreamWriter(_path);
        writer.Write(json);
    }


    private void GenerateButtons()
    {
        var columns = Mathf.Min(Mathf.FloorToInt(Screen.width * 0.6f / 200f), maxColumns);
        var rows =  Mathf.Min(Mathf.FloorToInt(Screen.height * 0.6f / 200f), maxRows);
        
        Debug.Log("Generate Buttons for Deck building");
        for (int i = 0; i < Mathf.Min(allCards.Count, columns * rows); i++)
        {
            var troop = allCards[i];
            var addCardButton = Instantiate(addCardButtonPrefab, _buttonFolder);
            addCardButton.name = $"Add{troop.name}Button";
            var xCoordinate = buttonsStart.x + buttonsDistance.x * (i % columns);
            var yCoordinate = buttonsStart.y - buttonsDistance.y * Mathf.Floor((float) i / columns);
            addCardButton.transform.localPosition = new Vector3(xCoordinate, yCoordinate, 0);

            var troopImage = addCardButton.transform.Find("CardImage").GetComponent<Image>();
            troopImage.sprite = troop.GetComponent<SpriteRenderer>().sprite;
            troopImage.preserveAspect = true;
            
            addCardButton.GetComponent<Button>().onClick.AddListener(() => AddCard(troop));
        }
    }

    void UpdateDeckPreview()
    {
        _currentPreview = _currentDeck; // TODO
        
        Destroy(_cardPreviewFolder.gameObject);
        var previewFolder = new GameObject("Card Previews");
        _cardPreviewFolder = previewFolder.transform;
        _cardPreviewFolder.parent = transform;
        _cardPreviewFolder.localPosition = Vector3.zero;
        
        for (int i = 0; i < Mathf.Min(_currentPreview.Count, previewColumnCount * previewRowCount); i++)
        {
            var j = i % (previewColumnCount * previewRowCount);
            var troop = _currentPreview[i];
            // TODO var troopHandler = troop.GetComponent<TroopHandler>();
            var cardPreview = Instantiate(cardPreviewPrefab, _cardPreviewFolder);
            var troopImage = cardPreview.transform.Find("CardImage").GetComponent<Image>();
            troopImage.sprite = troop.troopGameObject.GetComponent<SpriteRenderer>().sprite;
            troopImage.preserveAspect = true;
            cardPreview.transform.Find("AmountText").GetComponent<TextMeshProUGUI>().text = troop.amount + "x";
            var xCoordinate = cardStart.x + cardDistance.x * (j % previewColumnCount);
            var yCoordinate = cardStart.y - cardDistance.y * Mathf.Floor(j / (float) previewColumnCount);
            cardPreview.transform.localPosition = new Vector3(xCoordinate, yCoordinate, 0);

            cardPreview.name = $"{troop.troopGameObject.name}preview ({troop.amount})";
            
            cardPreview.GetComponent<Button>().onClick.AddListener(() => RemoveCard(troop.troopGameObject));
        }
    }

    public void AddCard(GameObject troop)
    {
        Debug.Log("Add Card to Deck");
        if(_currentDeck.Sum(c => c.amount) > maxCardsPerDeck)
            return;
        if (_currentDeck.Any(c => c.troopGameObject.name == troop.name))
        {
            var foundCard = _currentDeck.Find(c => c.troopGameObject.name == troop.name);
            var index = _currentDeck.IndexOf(foundCard);
            _currentDeck.Remove(foundCard);
            _currentDeck.Insert(index, (foundCard.troopGameObject, Mathf.Min(foundCard.amount + 1, maxCardsPerTroop)));
        }
        else
        {
            _currentDeck.Insert(0, (troop, 1));
            _currentDeck = _currentDeck.OrderBy(c => c.troopGameObject.name).ToList();
        }
        UpdateDeckPreview();
        SaveDeck();
    }

    public void RemoveCard(GameObject troop)
    {
        Debug.Log("Remove Card from Deck");
        if (_currentDeck.Any(c => c.troopGameObject.name == troop.name))
        {
            var cardToRemove = _currentDeck.Find(c => c.troopGameObject.name == troop.name);
            var index = _currentDeck.IndexOf(cardToRemove);
            _currentDeck.Remove(cardToRemove);
            if(cardToRemove.amount > 1)
                _currentDeck.Insert(index, (cardToRemove.troopGameObject, cardToRemove.amount - 1));
            UpdateDeckPreview();
            SaveDeck();
        }
    }
}