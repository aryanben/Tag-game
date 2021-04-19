using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    public bool gameEnded = false;
    public float timeToWin;
    public float cooldownTime;
    public float hatPickupTime;

    public string playerPrefabLocation;
    public Transform[] spawnPoints;
    public PlayerControllerScript[] players;
    public int playerWithHat;
    int playersInGame;

    public static GameManager instance;


    void Awake()
    {
        instance = this;

    }
    void Start()
    {
        players = new PlayerControllerScript[PhotonNetwork.PlayerList.Length];
        photonView.RPC("IAmInGame", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void IAmInGame()
    {
        playersInGame++;
        if (playersInGame ==  PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayer();
        }
    }
    void SpawnPlayer() 
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        PlayerControllerScript playerScript = playerObj.GetComponent<PlayerControllerScript>();

        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    public PlayerControllerScript GetPlayer(int playerId) 
    {
        return players.First(x => x.id == playerId);
    }
    public PlayerControllerScript GetPlayer(GameObject pObj) 
    {
        return players.First(x => x.gameObject == pObj);
    }
    void Update()
    {
        
    }
}
