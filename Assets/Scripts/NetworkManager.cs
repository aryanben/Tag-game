using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master gogo");
        
    }  
   
    public void CreateRoom(string name)
    {
        PhotonNetwork.CreateRoom(name);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("created room  "+PhotonNetwork.CurrentRoom.Name);
    }
    public void JoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }
    [PunRPC]
    public void ChangeScene(string name) 
    {
        PhotonNetwork.LoadLevel(name);
    }
}
