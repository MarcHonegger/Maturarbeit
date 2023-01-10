using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : NetworkBehaviour
{
    public float energy;
    public float energyRounded;
    public float energyGainPerSecond;
    public TextMeshProUGUI energyText;


    [SerializeField] private float cooldownDuration;
    public float currentCooldown;

    public static PlayerManager instance;
    public bool isLeftPlayer;

    [SerializeField] private bool hasResetted = false;
    [SerializeField] private bool clientConnected = false;
    [SerializeField] private bool hostConnected = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;

        FindObjectOfType<HandManager>().enabled = true;
        FindObjectOfType<CooldownTest>().enabled = true;
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

        // Debug.Log(NetworkServer.connections.Count);
        if (hasResetted==false)
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                if (isClientOnly && !clientConnected)
                {
                    CmdSetClientCon();
                }
            }

            if (clientConnected && !hostConnected)
            {
                if (isServer)
                {
                    RPCSetHostCon();
                }
            }
            
            if (hostConnected && clientConnected)
            {
                if (isServer)
                {
                    Debug.Log("should reset mana");
                    RPCResetMana();
                }
            }
        }

        energy = Mathf.Min(99, energy + energyGainPerSecond * Time.deltaTime);
        energyRounded = Mathf.Floor(energy * 2) / 2;
        energyText.text = energyRounded.ToString("F1");
    }


    [ClientRpc]
    private void RPCSetHostCon()
    {
        Debug.Log("RPCSetHostCon");
        hostConnected = true;
    }
    
    [Command(requiresAuthority = false)]
    private void CmdSetClientCon()
    {
        Debug.Log("CmdSetClientCon");
        RPCSetClientCon();
    }

    [ClientRpc]
    private void RPCSetClientCon()
    {
        Debug.Log("RPCSetClientCon");
        clientConnected = true;
    }
    
    [ClientRpc]
    private void RPCResetMana()
    {
        Debug.Log("Resetting Mana");
        energy = 3f;
        hasResetted = true;
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
