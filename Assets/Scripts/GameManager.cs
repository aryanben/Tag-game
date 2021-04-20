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
    public LayerMask lm;
    public static GameManager instance;

    public List<Transform> sps;
    void Awake()
    {
        instance = this;
        sps = new List<Transform>();
        sps.AddRange(spawnPoints);
    }
    void Start()
    {
        players = new PlayerControllerScript[PhotonNetwork.PlayerList.Length];
        photonView.RPC("IAmInGame", RpcTarget.AllBuffered);
        //for (int i = 0; i < spawnPoints.Length; i++)
        //{
            
        //}
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
        int r = Random.Range(0, spawnPoints.Length);
       
        while (Physics.BoxCast(spawnPoints[r].position+(Vector3.up * 3), Vector3.one, Vector3.down,Quaternion.identity,5,lm))
        {

             r = Random.Range(0, spawnPoints.Length);
            Debug.Log(r);
        }
      
       
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[r].position, Quaternion.identity);
     
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
    [PunRPC]
    public void GiveHat(int playerId, bool firstGive) 
    {
        if (!firstGive)
        {
            GetPlayer(playerWithHat).SetHat(false);
            
        }
        playerWithHat = playerId;
        GetPlayer(playerWithHat).SetHat(true);
        hatPickupTime = Time.time;
    }

    public bool CanGetHat() 
    {
        if (Time.time>hatPickupTime+cooldownTime)
        {
            return true;
           
        }
        else
        {
            return false;
        }
    }
}
