using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;



public class MenuScript : MonoBehaviourPunCallbacks
{
    public GameObject mainScreen;
    public GameObject lobbyScreen;

    public Button createButton;
    public Button joinButton;

    public Button startButton;
    public TextMeshProUGUI playerListText;
    void Start()
    {
        createButton.interactable = false;
        joinButton.interactable = false;
    }
    public override void OnConnectedToMaster()
    {
        createButton.interactable = true;
        joinButton.interactable = true;
    }

    void SetScreen(GameObject screen)
    {
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);

        screen.SetActive(true);
    }
    public void OnCreateButton(TMP_InputField roomName)
    {
        NetworkManager.instance.CreateRoom(roomName.text);
    }
    public void OnJoinButton(TMP_InputField roomName)
    {
        NetworkManager.instance.JoinRoom(roomName.text);
    }

    public void OnPlayerName(TMP_InputField nameInput)
    {
        PhotonNetwork.NickName = nameInput.text;
    }
    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);

        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();//this functiuon is already a default rpc
    }
    [PunRPC]
    public void UpdateLobbyUI()
    {
        playerListText.text = "";
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            playerListText.text += p.NickName + "\n";
        }
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }

    }
    public void OnLeaveButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }
    public void OnStartButton()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
    void Update()
    {

    }
}
