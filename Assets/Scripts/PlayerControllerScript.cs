using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerControllerScript : MonoBehaviourPunCallbacks
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

        if (!photonView.IsMine)
        {
            rb.isKinematic = true;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;
        transform.Translate(new Vector3(x, rb.velocity.y, z) * Time.deltaTime);
        // rb.velocity = new Vector3(x, rb.velocity.y, z);
    }
    void Jump()
    {
        Ray r = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(r, .7f))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
       
    }


}
