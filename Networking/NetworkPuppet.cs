using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class NetworkPuppet : MonoBehaviour
{
    // Puppet parts
    [SerializeField] GameObject puppetHead;
    [SerializeField] GameObject puppetLeftHand;
    [SerializeField] GameObject puppetRightHand;

    // Player limbs
    GameObject playerHead;
    GameObject playerLeftHand;
    GameObject playerRightHand;

    // Networking
    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if(photonView.IsMine)
        {
            playerHead = GameObject.FindGameObjectWithTag("MainCamera");
            playerLeftHand = GameObject.FindGameObjectWithTag("LeftHand");
            playerRightHand = GameObject.FindGameObjectWithTag("RightHand");

            puppetLeftHand.GetComponent<MeshRenderer>().enabled = false;
            puppetRightHand.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            puppetHead.transform.position = playerHead.transform.position;
            puppetHead.transform.rotation = playerHead.transform.rotation;

            puppetLeftHand.transform.position = playerLeftHand.transform.position;
            puppetLeftHand.transform.eulerAngles = new Vector3(playerLeftHand.transform.eulerAngles.x - 30, playerLeftHand.transform.eulerAngles.y, playerLeftHand.transform.eulerAngles.z + 90);

            puppetRightHand.transform.position = playerRightHand.transform.position;
            puppetRightHand.transform.eulerAngles = new Vector3(playerRightHand.transform.eulerAngles.x - 30, playerRightHand.transform.eulerAngles.y, playerRightHand.transform.eulerAngles.z - 90);
        }
    }
}
