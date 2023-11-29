using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkUIManager : NetworkBehaviour
{

    [Header("UI Element")]
    [SerializeField] GameObject btnBackHost;
    [SerializeField] GameObject btnNextHost;
    [SerializeField] GameObject btnBackClient;
    [SerializeField] GameObject btnNextClient;


    private void Start()
    {
        //Debug.Log("ActiveScene: ||" + SceneManager.GetActiveScene().name);

        if (btnBackClient == null || btnBackHost == null || btnNextClient == null || btnNextHost == null)
        {
            return;
        }

        if (!IsServer && !IsOwner)
        {

            btnBackHost.SetActive(false);
            btnNextHost.SetActive(false);
            btnBackClient.SetActive(true);
            btnNextClient.SetActive(true);
        }
        else
        {
            btnBackClient.SetActive(false);
            btnNextClient.SetActive(false);
            btnBackHost.SetActive(true);
            btnNextHost.SetActive(true);
        }


        //Debug.Log("ActiveScene: ||" + SceneManager.GetActiveScene().name);
    }



}
