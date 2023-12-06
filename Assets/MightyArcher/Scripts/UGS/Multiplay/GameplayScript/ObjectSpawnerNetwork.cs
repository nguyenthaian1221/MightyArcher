using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ObjectSpawnerNetwork : SingletonNetwork<ObjectSpawnerNetwork>
{

    public GameObject Spawner;
    public Vector3 position;
    public Quaternion rotation;


    // Ham nhan gia tri de ban 

    [ServerRpc]
    public void SpawnBulletForRightServerRpc()
    {

    }



    
}
