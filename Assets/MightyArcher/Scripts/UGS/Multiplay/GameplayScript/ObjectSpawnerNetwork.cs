using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class ObjectSpawnerNetwork : SingletonNetwork<ObjectSpawnerNetwork>
{

    private int baseShootPower = 30;

    public GameObject projectile;
    public Vector3 curPos;
    public float shootDirection;
    public Vector3 shootDirectionVector;
    public float shootPower;
    private GameObject cam; //main camera



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void shootArrow()
    {

        //set the unique flag for arrow in scene.
        GameNetworkController.isArrowInSceneLeft = true;

        //play shoot sound
        //playSfx(shootSfx[Random.Range(0, shootSfx.Length)]);

        //add to shoot counter
        //GameNetworkController.playerArrowShot++;

        //GameObject arr = Instantiate(arrow, playerShootPosition.transform.position, Quaternion.Euler(0, 180, shootDirection * -1)) as GameObject;
        //arr.name = "PlayerProjectile";
        //arr.GetComponent<MainLauncherNetworkController>().ownerID = 0;
        GameObject arr = NetworkObjectSpawner.SpawnNewNetworkObject(projectile, curPos, Quaternion.Euler(0, 180, shootDirection * -1));
        //arr.name = "PlayerProjectile";
        arr.GetComponent<MainLauncherNetworkController>().ownerID = 0;
        

        arr.GetComponent<MainLauncherNetworkController>().playerShootVector = shootDirectionVector * ((shootPower + baseShootPower) / 50);


        //cam.GetComponent<CameraNetworkController>().targetToFollow = arr;
        if (IsServer)
        {
            //FollowProjectilesClientRpc(arr);
            FollowProjectilesClientRpc(arr);
        }

        //reset body rotation
   }





    [ClientRpc]
    private void FollowProjectilesClientRpc(NetworkObjectReference target)
    {
        cam.GetComponent<CameraNetworkController>().targetToFollow = target;
    }



}
