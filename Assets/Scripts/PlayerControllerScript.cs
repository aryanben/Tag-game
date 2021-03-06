using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerControllerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public int id;

    public float moveSpeed;
    public float jumpForce;
    public GameObject hatObj;

    public float curHatTime;

    public Rigidbody rb;
    public Player photonPlayer;

    public ValuesHolderSO so;

    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;

        GameManager.instance.players[id - 1] = this;

        if (id == 1)
        {
            GameManager.instance.GiveHat(id, true);
        }

        if (!photonView.IsMine)
        {
            // rb.isKinematic = true;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (curHatTime >= GameManager.instance.timeToWin
                && !GameManager.instance.gameEnded)
            {
                GameManager.instance.gameEnded = true;
                GameManager.instance.photonView.RPC("WinGame", RpcTarget.All, id);

            }
        }
        if (photonView.IsMine)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (hatObj.activeInHierarchy)
            {
                curHatTime += Time.deltaTime;
            }
            Debug.Log(PhotonNetwork.NickName+"="+curHatTime);
        }

    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;
        // transform.Translate(new Vector3(x, rb.velocity.y, z) * Time.deltaTime);
        rb.velocity = new Vector3(x, rb.velocity.y, z);

    }
    void Jump()
    {
        Ray r = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(r, .7f))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }
    public void SetHat(bool hatIs)
    {
        hatObj.SetActive(hatIs);
        curHatTime = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (collision.gameObject.tag == "Player")
        {
            if (GameManager.instance.GetPlayer(collision.gameObject).id == GameManager.instance.playerWithHat)
            {
                if (GameManager.instance.CanGetHat())
                {
                    GameManager.instance.photonView.RPC("GiveHat", RpcTarget.All, id, false);
                }
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(curHatTime);
        }
        else if (stream.IsReading)
        {
            curHatTime=(float)stream.ReceiveNext();
        }
    }
}
