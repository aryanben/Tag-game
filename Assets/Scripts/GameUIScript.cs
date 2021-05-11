using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameUIScript : MonoBehaviour
{
    public PlayerUIContainer[] pContainers;
    public TextMeshProUGUI winText;

    public static GameUIScript instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        InitializePlayerUI();
    }

    void InitializePlayerUI()
    {
        for (int i = 0; i < pContainers.Length; ++i)
        {
            PlayerUIContainer container = pContainers[i];

            if (i < PhotonNetwork.PlayerList.Length)
            {
                container.obj.SetActive(true);
                container.nameText.text = PhotonNetwork.PlayerList[i].NickName;
                container.hatTimeSlider.maxValue = GameManager.instance.timeToWin;
            }
            else
            {
                container.obj.SetActive(false);
            }
        }
    }
    void UpdatePlayerUI()
    {
        for (int i = 0; i < GameManager.instance.players.Length; ++i)
        {
            if (GameManager.instance.players[i] != null)
            {
                pContainers[i].hatTimeSlider.value = GameManager.instance.players[i].curHatTime;
            }
        }

    }
    public void SetWinText(string winnerName)
    {
        winText.gameObject.SetActive(true);
        winText.text = winnerName + " wins";
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePlayerUI();


    }
}
[System.Serializable]
public class PlayerUIContainer
{
    public GameObject obj;
    public TextMeshProUGUI nameText;
    public Slider hatTimeSlider;
}
