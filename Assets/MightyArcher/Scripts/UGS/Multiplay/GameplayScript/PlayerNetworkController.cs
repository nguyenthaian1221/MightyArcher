using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerNetworkController : NetworkBehaviour
{
    /// <summary>
    /// Main player controller class.
    /// This class is responsible for player inputs, rotation, health-management, shooting arrows and helper-dots creation.
    /// </summary>

    [Header("Public GamePlay settings")]
    public bool useHelper = true;                   //use helper dots when player is aiming to shoot
    public int baseShootPower = 30;                 //base power. edit with care.
    public int playerHealth = 100;                  //starting (full) health. can be edited.
    private int minShootPower = 15;                 //powers lesser than this amount are ignored. (used to cancel shoots)
    internal int playerCurrentHealth;               //real-time health. not editable.
    public static bool isPlayerDead;                //flag for gameover event


    [Header("Linked GameObjects")]
    //Reference to game objects (childs and prefabs)
    public GameObject arrow;
    public GameObject trajectoryHelper;
    public GameObject playerTurnPivot;
    public GameObject playerShootPosition;
    public GameObject infoPanel;
    public GameObject UiDynamicPower;
    public GameObject UiDynamicDegree;
    //Hidden gameobjects
    private GameObject gc;  //game controller object
    private GameObject cam; //main camera

    [Header("Audio Clips")]
    public AudioClip[] shootSfx;
    public AudioClip[] hitSfx;

    //private settings
    private Vector2 icp;                            //initial Click Position
    private Ray inputRay;
    private RaycastHit hitInfo;
    private float inputPosX;
    private float inputPosY;
    private Vector2 inputDirection;
    private float distanceFromFirstClick;
    private float shootPower;
    private float shootDirection;
    private Vector3 shootDirectionVector;

    //helper trajectory variables
    private float helperCreationDelay = 0.12f;
    private bool canCreateHelper;
    private float helperShowDelay = 0.2f;
    private float helperShowTimer;
    private bool helperDelayIsDone;

    void Awake()
    {
        icp = new Vector2(0, 0);
        infoPanel.SetActive(false);
        shootDirectionVector = new Vector3(0, 0, 0);
        playerCurrentHealth = playerHealth;
        isPlayerDead = false;

        gc = GameObject.FindGameObjectWithTag("GameController");
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        canCreateHelper = true;
        helperShowTimer = 0;
        helperDelayIsDone = false;

    }

    /// <summary>
    /// FSM
    /// </summary>
    void Update()
    {

        //if the game has not started yet, or the game is finished, just return
        if (!GameNetworkController.gameIsStarted || GameNetworkController.gameIsFinished || !IsOwner)
            return;

        //Check if this object is dead or alive
        if (playerCurrentHealth <= 0)
        {
            print("Player is dead...");
            playerCurrentHealth = 0;
            isPlayerDead = true;
            return;
        }

        if (IsServer)
        {
            //if this is not our turn, just return
            if (!GameNetworkController.playersLeftTurn)
                return;

            //if we already have an arrow in scene, we can not shoot another one!
            if (GameNetworkController.isArrowInSceneLeft)
                return;

            //if (!PauseManager.enableInput)
            //    return;

            //Player pivot turn manager
            if (Input.GetMouseButton(0) && IsOwner)
            {

                turnPlayerBody();

                //only show shot info when we are fighting with an enemy
                //if (GameModeController.isEnemyRequired())
                infoPanel.SetActive(true);

                helperShowTimer += Time.deltaTime;
                if (helperShowTimer >= helperShowDelay)
                    helperDelayIsDone = true;
            }

            //register the initial Click Position
            if (Input.GetMouseButtonDown(0) && IsOwner)
            {
                icp = new Vector2(inputPosX, inputPosY);
                print("icp: " + icp);
                print("icp magnitude: " + icp.magnitude);
            }

            //clear the initial Click Position
            if (Input.GetMouseButtonUp(0) && IsOwner)
            {

                //only shoot if there is enough power applied to the shoot
                if (shootPower >= minShootPower)
                {
                    shootArrow();
                }
                else
                {
                    //reset body rotation
                    StartCoroutine(resetBodyRotation());
                }

                //reset variables
                icp = new Vector2(0, 0);
                infoPanel.SetActive(false);
                helperShowTimer = 0;
                helperDelayIsDone = false;
            }


        }
        else
        {
            //if this is not our turn, just return
            if (!GameNetworkController.playersRightTurn)
                return;

            //if we already have an arrow in scene, we can not shoot another one!
            if (GameNetworkController.isArrowInSceneRight)
                return;

            //if (!PauseManager.enableInput)
            //    return;

            //Player pivot turn manager
            if (Input.GetMouseButton(0) && IsOwner)
            {

                turnPlayerBodyRight();

                //only show shot info when we are fighting with an enemy
                //if (GameModeController.isEnemyRequired())
                infoPanel.SetActive(true);

                helperShowTimer += Time.deltaTime;
                if (helperShowTimer >= helperShowDelay)
                    helperDelayIsDone = true;
            }

            //register the initial Click Position
            if (Input.GetMouseButtonDown(0) && IsOwner)
            {
                icp = new Vector2(inputPosX, inputPosY);
                //print("icp: " + icp);
                //print("icp magnitude: " + icp.magnitude);
            }

            //clear the initial Click Position
            if (Input.GetMouseButtonUp(0) && IsOwner)
            {

                //only shoot if there is enough power applied to the shoot
                if (shootPower >= minShootPower)
                {
                    shootArrowRight();             //Right
                }
                else
                {
                    //reset body rotation
                    StartCoroutine(resetBodyRotationRight());
                }

                //reset variables
                icp = new Vector2(0, 0);
                infoPanel.SetActive(false);
                helperShowTimer = 0;
                helperDelayIsDone = false;
            }

        }


    }

    public void changeTurnsPlayer()
    {
        print("playerCurrentHealth: " + playerCurrentHealth);

        if (playerCurrentHealth > 0)
            //StartCoroutine(gc.GetComponent<GameNetworkController>().roundTurnManagerWithPlayer());
            gc.GetComponent<GameNetworkController>().RoundTurn();
        else
            GameNetworkController.noMoreShooting = true;
    }

    [ClientRpc]
    public void ChangeTurnClientRpc()
    {
        changeTurnsPlayer();
    }

    void turnPlayerBody()
    {

        inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out hitInfo, 50))
        {
            // determine the position on the screen
            inputPosX = this.hitInfo.point.x;
            inputPosY = this.hitInfo.point.y;


            // set the bow's angle to the arrow
            inputDirection = new Vector2(icp.x - inputPosX, icp.y - inputPosY);
            //print("Dir X-Y: " + inputDirection.x + " / " + inputDirection.y);

            shootDirection = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;

            //for an optimal experience, we need to limit the rotation to 0 ~ 90 euler angles.
            //so...
            if (shootDirection > 90)
                shootDirection = 90;
            if (shootDirection < 0)
                shootDirection = 0;

            //apply the rotation
            playerTurnPivot.transform.eulerAngles = new Vector3(0, 0, shootDirection);

            //calculate shoot power
            distanceFromFirstClick = inputDirection.magnitude / 4;
            shootPower = Mathf.Clamp(distanceFromFirstClick, 0, 1) * 100;
            //print ("distanceFromFirstClick: " + distanceFromFirstClick);
            //print("shootPower: " + shootPower);

            //modify camera cps - next update
            CameraNetworkController.cps = 5 + (shootPower / 100);

            //show informations on the UI text elements
            UiDynamicDegree.GetComponent<TextMesh>().text = ((int)shootDirection).ToString();
            UiDynamicPower.GetComponent<TextMesh>().text = ((int)shootPower).ToString() + "%";

            if (useHelper)
            {
                //create trajectory helper points, while preventing them to show when we start to click/touch
                if (shootPower > minShootPower && helperDelayIsDone)
                    StartCoroutine(shootTrajectoryHelper());
            }
        }
    }



    void shootArrow()
    {

        //set the unique flag for arrow in scene.
        GameNetworkController.isArrowInSceneLeft = true;

        //play shoot sound
        playSfx(shootSfx[Random.Range(0, shootSfx.Length)]);

        //add to shoot counter
        GameNetworkController.playerArrowShot++;

        //GameObject arr = Instantiate(arrow, playerShootPosition.transform.position, Quaternion.Euler(0, 180, shootDirection * -1)) as GameObject;
        //arr.name = "PlayerProjectile";
        //arr.GetComponent<MainLauncherNetworkController>().ownerID = 0;

        GameObject arr = NetworkObjectSpawner.SpawnNewNetworkObject(arrow, playerShootPosition.transform.position, Quaternion.Euler(0, 180, shootDirection * -1));
        //arr.name = "PlayerProjectile";
        arr.GetComponent<MainLauncherNetworkController>().ownerID = 0;
        shootDirectionVector = Vector3.Normalize(inputDirection);
        shootDirectionVector = new Vector3(Mathf.Clamp(shootDirectionVector.x, 0, 1), Mathf.Clamp(shootDirectionVector.y, 0, 1), shootDirectionVector.z);

        arr.GetComponent<MainLauncherNetworkController>().playerShootVector = shootDirectionVector * ((shootPower + baseShootPower) / 50);

        print("shootPower: " + shootPower + " --- " + "shootDirectionVector: " + shootDirectionVector);

        //cam.GetComponent<CameraNetworkController>().targetToFollow = arr;
        if (IsServer)
        {
            //FollowProjectilesClientRpc(arr);
            FollowProjectilesClientRpc(arr);
        }

        //reset body rotation
        StartCoroutine(resetBodyRotation());
    }

    void shootArrowRight()
    {

        //set the unique flag for arrow in scene.
        GameNetworkController.isArrowInSceneRight = true;

        //play shoot sound
        playSfx(shootSfx[Random.Range(0, shootSfx.Length)]);

        //add to shoot counter
        GameNetworkController.playerArrowShot++;

        //GameObject arr = Instantiate(arrow, playerShootPosition.transform.position, Quaternion.Euler(0, 180, shootDirection * -1)) as GameObject;
        //arr.name = "PlayerProjectile";




        shootDirectionVector = Vector3.Normalize(inputDirection);
        shootDirectionVector = new Vector3(Mathf.Clamp(shootDirectionVector.x, -1, 0), Mathf.Clamp(shootDirectionVector.y, 0, 1), shootDirectionVector.z);

     
        SpawnRightBulletServerRpc(shootDirectionVector,shootPower);



        //arr.GetComponent<MainLauncherNetworkController>().playerShootVector = shootDirectionVector * ((shootPower + baseShootPower) / 50);

        print("shootPower: " + shootPower + " --- " + "shootDirectionVector: " + shootDirectionVector);

        //cam.GetComponent<CameraNetworkController>().targetToFollow = arr;
        //FollowProjectiles(arr);
        //reset body rotation
        StartCoroutine(resetBodyRotationRight());
    }



    [ClientRpc]
    private void FollowProjectilesClientRpc(NetworkObjectReference target)
    {
        cam.GetComponent<CameraNetworkController>().targetToFollow = target;
    }

    /// <summary>
    /// tunr player body to default rotation
    /// </summary>
    IEnumerator resetBodyRotation()
    {

        //yield return new WaitForSeconds(1.5f);
        //playerTurnPivot.transform.eulerAngles = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.25f);
        float currentRotationAngle = playerTurnPivot.transform.eulerAngles.z;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 3;
            playerTurnPivot.transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(currentRotationAngle, 0, t));
            yield return 0;
        }

    }

    IEnumerator resetBodyRotationRight()
    {

        //yield return new WaitForSeconds(1.5f);
        //playerTurnPivot.transform.eulerAngles = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.25f);
        float currentRotationAngle = playerTurnPivot.transform.eulerAngles.z;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 3;

            playerTurnPivot.transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(currentRotationAngle - 360, 0, t));
            yield return 0;
        }


        //TestChangeTurnServerRpc();    // Bat cung TestChangeTurnServerRpc() de test ban khong co ten
    }

    IEnumerator shootTrajectoryHelper()
    {

        if (!canCreateHelper)
            yield break;

        canCreateHelper = false;

        GameObject t = Instantiate(trajectoryHelper, playerShootPosition.transform.position, Quaternion.Euler(0, 180, shootDirection * -1)) as GameObject;

        shootDirectionVector = Vector3.Normalize(inputDirection);

        shootDirectionVector = new Vector3(Mathf.Clamp(shootDirectionVector.x, 0, 1), Mathf.Clamp(shootDirectionVector.y, 0, 1), shootDirectionVector.z);
        //print("shootPower: " + shootPower + " --- " + "shootDirectionVector: " + shootDirectionVector);

        t.GetComponent<Rigidbody>().AddForce(shootDirectionVector * ((shootPower + baseShootPower) / 50), ForceMode.Impulse);

        yield return new WaitForSeconds(helperCreationDelay);
        canCreateHelper = true;
    }

    /// <summary>
    /// Plays the sfx.
    /// </summary>
    void playSfx(AudioClip _clip)
    {
        GetComponent<AudioSource>().clip = _clip;
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
    }


    /// <summary>
    /// Play a sfx when player is hit by an arrow
    /// </summary>
    public void playRandomHitSound()
    {

        int rndIndex = Random.Range(0, hitSfx.Length);
        playSfx(hitSfx[rndIndex]);
    }


    #region Right Player Only

    void turnPlayerBodyRight()
    {

        inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(inputRay, out hitInfo, 50))
        {
            // determine the position on the screen
            inputPosX = this.hitInfo.point.x;
            inputPosY = this.hitInfo.point.y;

            // set the bow's angle to the arrow
            inputDirection = new Vector2(icp.x - inputPosX, icp.y - inputPosY);

            shootDirection = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;

            // Debug.LogError("shootDirection:" + shootDirection);
            //for an optimal experience, we need to limit the rotation to 0 ~ 90 euler angles.
            //so...
            if (shootDirection > 180 || (shootDirection > -180 && shootDirection < 0))
                shootDirection = 180;

            if (shootDirection < 90)
                shootDirection = 90;

            //apply the rotation
            playerTurnPivot.transform.eulerAngles = new Vector3(0, 0, shootDirection - 180);  // Cus the direction is opposite

            //calculate shoot power
            distanceFromFirstClick = inputDirection.magnitude / 4;
            shootPower = Mathf.Clamp(distanceFromFirstClick, 0, 1) * 100;
            //print ("distanceFromFirstClick: " + distanceFromFirstClick);
            //print("shootPower: " + shootPower);

            //modify camera cps - next update
            CameraNetworkController.cps = 5 + (shootPower / 100);

            //show informations on the UI text elements
            UiDynamicDegree.GetComponent<TextMesh>().text = (180 - (int)shootDirection).ToString();     // Multiply with -1 to hide the math problem, easy for player to use
            UiDynamicPower.GetComponent<TextMesh>().text = ((int)shootPower).ToString() + "%";

            if (useHelper)
            {
                //create trajectory helper points, while preventing them to show when we start to click/touch
                if (shootPower > minShootPower && helperDelayIsDone)
                    StartCoroutine(shootTrajectoryHelperRight());
            }
        }



     
    }

    #endregion

    [ServerRpc]
    void SpawnRightBulletServerRpc(Vector3 dirrr,float power)
    {
        //GameObject newBullet = GetNewBullet();

        //PrepareNewlySpawnedBulltet(newBullet);
        GameObject arr = NetworkObjectSpawner.SpawnNewNetworkObject(arrow,
            playerShootPosition.transform.position, Quaternion.Euler(0, 180, shootDirection * -1));
        //arr.name = "PlayerProjectile";
        arr.GetComponent<MainLauncherNetworkController>().ownerID = 3;
        arr.GetComponent<MainLauncherNetworkController>().playerShootVector = dirrr * ((power + baseShootPower) / 50);

        
      //arr.GetComponent<Rigidbody>().velocity =new Vector3(-10,20,0);

        FollowProjectiles(arr);
    }

    void FollowProjectiles(GameObject target)
    {
        FollowProjectilesServerRpc(target);
    }
    [ServerRpc]
    void FollowProjectilesServerRpc(NetworkObjectReference target)
    {
        FollowProjectilesClientRpc(target);
       
    }

    //[ClientRpc]
    //void FollowProjectilesClientRpc(NetworkObjectReference target)
    //{
    //    cam.GetComponent<CameraNetworkController>().targetToFollow = target;

    //}




    /// <summary>
    /// Create helper dots that shows the possible fly path of the actual arrow
    /// </summary>
    IEnumerator shootTrajectoryHelperRight()
    {

        if (!canCreateHelper)
            yield break;

        canCreateHelper = false;

        GameObject t = Instantiate(trajectoryHelper, playerShootPosition.transform.position, Quaternion.Euler(0, 180, shootDirection * -1)) as GameObject;

        shootDirectionVector = Vector3.Normalize(inputDirection);

        shootDirectionVector = new Vector3(Mathf.Clamp(shootDirectionVector.x, -1, 0), Mathf.Clamp(shootDirectionVector.y, 0, 1), shootDirectionVector.z);
        //print("shootPower: " + shootPower + " --- " + "shootDirectionVector: " + shootDirectionVector);

        t.GetComponent<Rigidbody>().AddForce(shootDirectionVector * ((shootPower + baseShootPower) / 50), ForceMode.Impulse);

        yield return new WaitForSeconds(helperCreationDelay);
        canCreateHelper = true;
    }


}
