using Mirror;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float energy;
    public float energyRounded;
    public float energyGainPerSecond;
    public TextMeshProUGUI energyText;


    [SerializeField] private float cooldownDuration;
    public float currentCooldown;

    public static PlayerManager instance;
    public bool isLeftPlayer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            currentCooldown = 0;
        }

        energy = Mathf.Min(99, energy + energyGainPerSecond * Time.deltaTime);
        energyRounded = Mathf.Floor(energy * 2) / 2;
        energyText.text = energyRounded.ToString("F1");
    }

    public void PlayCard(GameObject troopPrefab, int lane, GameObject playedCard)
    {
        HandManager.instance.CardWasPlayed(playedCard);
        GameManager.instance.spawnManager.Spawn(troopPrefab, lane, isLeftPlayer);

        var troop = troopPrefab.GetComponent<TroopHandler>();
        energy -= troop.energyCost;
        StartCooldown();
    }

    private void StartCooldown()
    {
        currentCooldown = cooldownDuration;
    }

    public int GetDirection(GameObject thing)
    {
        return thing.CompareTag("LeftPlayer") ? 1 : -1;
    }
    
    public bool IsPlayableCard(float energyCost) => energyCost < energy && currentCooldown <= 0;
    
    public bool IsValidSpawn(int lane) =>
        !GameManager.instance.spawnManager.GetSpawnPoint(lane, isLeftPlayer).isDisabled;

}
