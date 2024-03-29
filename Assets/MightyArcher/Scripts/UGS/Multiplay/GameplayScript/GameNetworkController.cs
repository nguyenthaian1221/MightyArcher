using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameNetworkController : SingletonNetwork<GameNetworkController>
{
    /// <summary>
    /// Main game controller class.
    /// Game controller is responsible for assigning turns to player and opponent (thus making this game a turn-based one!), 
    /// setting ground types (to curved or flat types), managing UI elements including health bars and info panels, managing player inputs (on UI), 
    /// checking for gameover events and post gameover settings.
    /// </summary>
    #region old 
    ////[Header("Background Type")]
    //public enum groundTypes { flat, curved }
    //public groundTypes groundType = groundTypes.flat;   //we have two options here. default is flat ground.
    //public GameObject flatBg;                           //reference to flag ground object.
    //public GameObject curvedBg;                         //reference to curved ground holder object.


    //// Static variables //

    //public static bool isArrowInSceneLeft;              //have any body shot an arrow? (is there an arrow inside the game?)
    //public static bool isArrowInSceneRight;              //have any body shot an arrow? (is there an arrow inside the game?)

    //public static bool gameIsStarted;               //global flag for game start state
    //public static bool gameIsFinished;              //global flag for game finish state
    //public static bool noMoreShooting;              //We use this to stop the shoots when someone has been killed but the game is yet to finish
    //public static int round;                        //internal counter to assign turn to player and AI



    //public static bool playersLeftTurn;                 //flag to check if this is player's turn
    //public static bool playersRightTurn;





    //public static string whosTurn;                  //current turn holder in string. useful if you want to extend the game.


    //public static int playerArrowShot;              //how many arrows player shot in this game
    //                                                // Static variables //

    //// Private vars //
    //private bool canTap;
    //private GameObject AdManagerObject;

    //[Header("AudioClips")]
    //public AudioClip tapSfx;
    //public AudioClip endSfx;


    //[Header("Public GameObjects")]
    ////Reference to scene game objects		
    //public GameObject uiPlayerLeftHealthBar;            //Names are self-explanatory
    //public GameObject uiLeftPlayerInfoPanel;
    //public GameObject uiLeftPlayerDistance;



    //public GameObject uiRightPlayerHealthBar;
    //public GameObject uiRightPlayerInfoPanel;
    //public GameObject uiRightPlayerDistance;




    ////public GameObject uiEnemyHealthBar;
    ////public GameObject uiEnemyInfoPanel;
    ////public GameObject uiEnemyDistance;



    //private GameObject playerLeft;
    //private GameObject playerRight;

    ////private GameObject enemy;
    //private GameObject cam;
    //private GameObject uiCam;
    //public GameObject gameoverManager;
    //public GameObject uiGameStateLabel;
    //public GameObject uiYouWon;


    //private Vector3 playerLeftHBSC;                     //player health bar starting scale                                                    
    //private float playerLeftHealthScale;                //player health bar real-time scale

    //private Vector3 playerRightHBSC;                     //player health bar starting scale                                                    
    //private float playerRightHealthScale;                //player health bar real-time scale



    ///// <summary>
    ///// INIT
    ///// </summary>





    ////void Start()
    ////{

    ////    StartCoroutine(activateTap());

    ////    //if (SceneManager.GetActiveScene().name.Equals("BirdHunt"))
    ////    //{
    ////    //    playersLeftTurn = true;
    ////    //    playersRightTurn = false;
    ////    //    enemysTurn = false;
    ////    //}

    ////    //if (SceneManager.GetActiveScene().name.Equals("Game"))
    ////    //{
    ////    //    StartCoroutine(roundTurnManagerWithCom());
    ////    //}

    ////    //if (SceneManager.GetActiveScene().name.Equals("GameWithPlayer"))
    ////    //{

    ////        StartCoroutine(roundTurnManagerWithPlayer());
    ////    //}

    ////}


    //private void InitAwake()
    //{

    //    //set ground type with high priority
    //    switch (groundType)
    //    {
    //        case groundTypes.flat:
    //            flatBg.SetActive(true);
    //            curvedBg.SetActive(false);
    //            break;
    //        case groundTypes.curved:
    //            flatBg.SetActive(false);
    //            curvedBg.SetActive(true);
    //            break;
    //    }

    //    //cache main objects
    //    playerLeft = GameObject.FindGameObjectWithTag("Player");
    //    playerLeftHBSC = uiPlayerLeftHealthBar.transform.localScale;


    //    cam = GameObject.FindGameObjectWithTag("MainCamera");
    //    uiCam = GameObject.FindGameObjectWithTag("uiCamera");

    //    //if (SceneManager.GetActiveScene().name.Equals("Game"))
    //    //{
    //    //    enemy = GameObject.FindGameObjectWithTag("enemy");        //  Only in computer mode
    //    //    enemyHBSC = uiEnemyHealthBar.transform.localScale;
    //    //}

    //    //if (SceneManager.GetActiveScene().name.Equals("GameWithPlayer"))
    //    //{

    //    playerRight = GameObject.FindGameObjectWithTag("player2");  // Only in PvP mode
    //    playerRightHBSC = uiRightPlayerHealthBar.transform.localScale;
    //    //}

    //    gameoverManager.SetActive(false);

    //    //if (uiBirdhuntStatPanel)
    //    //    uiBirdhuntStatPanel.SetActive(false);

    //    isArrowInSceneLeft = false;
    //    isArrowInSceneRight = false;



    //    canTap = false;
    //    gameIsStarted = false;
    //    gameIsFinished = false;
    //    noMoreShooting = false;
    //    round = 0;
    //    playerArrowShot = 0;


    //    //gameTimer = availableTime;
    //    //seconds = 0;
    //    //minutes = 0;
    //    //birdsHit = 0;

    //    //AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");


    //}


    //[ClientRpc]
    //public void RemoteInitClientRpc()
    //{
    //    InitAwake();


    //    StartCoroutine(activateTap());

    //    //if (SceneManager.GetActiveScene().name.Equals("BirdHunt"))
    //    //{
    //    //    playersLeftTurn = true;
    //    //    playersRightTurn = false;
    //    //    enemysTurn = false;
    //    //}

    //    //if (SceneManager.GetActiveScene().name.Equals("Game"))
    //    //{
    //    //    StartCoroutine(roundTurnManagerWithCom());
    //    //}

    //    //if (SceneManager.GetActiveScene().name.Equals("GameWithPlayer"))
    //    //{

    //    //StartCoroutine(roundTurnManagerWithPlayer());
    //    RoundTurn();
    //    //}

    //}


    ///// <summary>
    ///// FSM
    ///// </summary>
    //void Update()
    //{

    //    //receive inputs at all times
    //    StartCoroutine(inputManager());


    //    //manage health bar status
    //    if (playerLeft != null && playerRight != null)
    //    {
    //        updateUiHealthBars();
    //        updateUiEnemyInfoPanel();
    //    }


    //    //we no longer need to loop into gameController if the game is already finished.
    //    if (gameIsFinished)
    //        return;

    //}


    ////*****************************************************************************
    //// This function monitors player touches on menu buttons.
    //// detects both touch and clicks and can be used with editor, handheld device and 
    //// every other platforms at once.
    ////*****************************************************************************
    //private RaycastHit hitInfo;
    //private Ray ray;
    //IEnumerator inputManager()
    //{

    //    //Prevent double click
    //    if (!canTap)
    //        yield break;

    //    //Mouse of touch?
    //    if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)
    //        ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
    //    else if (Input.GetMouseButtonUp(0))
    //        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    else
    //        yield break;

    //    if (Physics.Raycast(ray, out hitInfo))
    //    {
    //        GameObject objectHit = hitInfo.transform.gameObject;
    //        //print ("objectHit: " + objectHit.name);
    //        switch (objectHit.name)
    //        {

    //            case "Button-Play":
    //                playSfx(tapSfx);                            //play touch sound
    //                canTap = false;                             //prevent double touch
    //                StartCoroutine(animateButton(objectHit));   //touch animation effect
    //                yield return new WaitForSeconds(0.25f);     //Wait for the animation to end
    //                SceneManager.LoadScene("Menu");
    //                StartCoroutine(activateTap());
    //                break;
    //            case "Button-Menu":
    //                playSfx(tapSfx);                            //play touch sound
    //                canTap = false;                             //prevent double touch
    //                StartCoroutine(animateButton(objectHit));   //touch animation effect
    //                yield return new WaitForSeconds(0.25f);     //Wait for the animation to end
    //                SceneManager.LoadScene("Menu");
    //                StartCoroutine(activateTap());
    //                break;
    //            case "Button-Retry":
    //                playSfx(tapSfx);                            //play touch sound
    //                canTap = false;                             //prevent double touch
    //                StartCoroutine(animateButton(objectHit));   //touch animation effect
    //                yield return new WaitForSeconds(0.25f);     //Wait for the animation to end
    //                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //                StartCoroutine(activateTap());
    //                break;
    //        }
    //    }
    //}


    ///// <summary>
    ///// shows the distance of player and enemy on the UI
    ///// </summary>
    //void updateUiEnemyInfoPanel()
    //{

    //    //Need Show UIInfo for Right-side Player


    //    //if (SceneManager.GetActiveScene().name.Equals("Game"))
    //    //{
    //    //    if (playersLeftTurn)
    //    //    {

    //    //        uiEnemyInfoPanel.SetActive(true);
    //    //        float enemyDistance = Vector3.Distance(playerLeft.transform.position, enemy.transform.position);
    //    //        uiEnemyDistance.GetComponent<TextMesh>().text = ((int)enemyDistance).ToString() + "m";

    //    //        uiEnemyInfoPanel.transform.position = new Vector3(uiEnemyInfoPanel.transform.position.x,
    //    //                                                            enemy.transform.position.y,
    //    //                                                            uiEnemyInfoPanel.transform.position.z);

    //    //    }
    //    //    else
    //    //    {

    //    //        uiEnemyInfoPanel.SetActive(false);

    //    //    }

    //    //}


    //    //if (SceneManager.GetActiveScene().name.Equals("GameWithPlayer"))
    //    //{

    //    if (playersLeftTurn)
    //    {
    //        uiLeftPlayerInfoPanel.SetActive(false);
    //        uiRightPlayerInfoPanel.SetActive(true);
    //        float enemyRightDistance = Vector3.Distance(playerLeft.transform.position, playerRight.transform.position);
    //        uiRightPlayerDistance.GetComponent<TextMesh>().text = ((int)enemyRightDistance).ToString() + "m";

    //        uiRightPlayerInfoPanel.transform.position = new Vector3(uiRightPlayerInfoPanel.transform.position.x,
    //                                                            playerRight.transform.position.y,
    //                                                            uiRightPlayerInfoPanel.transform.position.z);

    //    }
    //    else
    //    {

    //        uiRightPlayerInfoPanel.SetActive(false);
    //        uiLeftPlayerInfoPanel.SetActive(true);

    //        float enemyLeftDistance = Vector3.Distance(playerRight.transform.position, playerLeft.transform.position);
    //        uiLeftPlayerDistance.GetComponent<TextMesh>().text = ((int)enemyLeftDistance).ToString() + "m";

    //        uiLeftPlayerInfoPanel.transform.position = new Vector3(uiLeftPlayerInfoPanel.transform.position.x,
    //                                                            playerLeft.transform.position.y,
    //                                                            uiLeftPlayerInfoPanel.transform.position.z);

    //    }
    //}


    ////}


    ///// <summary>
    ///// Updates the user interface health bars based on available health for each side.
    ///// </summary>
    //void updateUiHealthBars()
    //{

    //    playerLeftHealthScale = (playerLeft.GetComponent<PlayerLeftNetworkController>().playerCurrentHealth * playerLeftHBSC.x) / playerLeft.GetComponent<PlayerLeftNetworkController>().playerHealth;
    //    uiPlayerLeftHealthBar.transform.localScale = new Vector3(playerLeftHealthScale, playerLeftHBSC.y, playerLeftHBSC.z);


    //    //if (SceneManager.GetActiveScene().name.Equals("Game"))
    //    //{
    //    //    enemyHealthScale = (enemy.GetComponent<EnemyController>().enemyCurrentHealth * enemyHBSC.x) / enemy.GetComponent<EnemyController>().enemyHealth;
    //    //    uiEnemyHealthBar.transform.localScale = new Vector3(enemyHealthScale, enemyHBSC.y, enemyHBSC.z);

    //    //}

    //    //if (SceneManager.GetActiveScene().name.Equals("GameWithPlayer"))
    //    //{
    //    playerRightHealthScale = (playerRight.GetComponent<PlayerRightNetworkController>().playerCurrentHealth * playerRightHBSC.x) / playerRight.GetComponent<PlayerRightNetworkController>().playerHealth;

    //    uiRightPlayerHealthBar.transform.localScale = new Vector3(playerRightHealthScale, playerRightHBSC.y, playerRightHBSC.z);

    //    //}

    //}


    ///// <summary>
    ///// Assign turns to player and player.
    ///// </summary>
    //public IEnumerator roundTurnManagerWithPlayer()
    //{

    //    //1. first check if the game is already finished
    //    if (gameIsFinished || playerLeft == null || playerRight == null)    //
    //    {
    //        yield break;
    //    }

    //    //2. then check if the situation meets a game over
    //    //check for game finish state
    //    if (playerLeft.GetComponent<PlayerLeftNetworkController>().playerCurrentHealth <= 0)
    //    {

    //        //player is winner
    //        //StartCoroutine(finishTheGame(1));

    //        print("Player Left is dead.Player Right wins");

    //        yield break;

    //    }
    //    else if (playerRight.GetComponent<PlayerRightNetworkController>().playerCurrentHealth <= 0)
    //    {

    //        //we have lost
    //        //StartCoroutine(finishTheGame(0));

    //        print("Player Right is dead. Player Left win");
    //        yield break;

    //    }

    //    //3. if none of the above is true, continue with the turn-change...

    //    round++;    //game starts with round 1
    //    print("Round: " + round);

    //    //if round carry is odd, its players turn, otherwise its opponent's turn
    //    int carry;
    //    carry = round % 2;

    //    if (carry == 1)
    //    {

    //        playersLeftTurn = true;
    //        playersRightTurn = false;
    //        whosTurn = "Player Left";

    //        yield return new WaitForSeconds(0.9f);

    //        //just incase we need to show the camera's starting animation, we do not need to switch to player, so we just leave the function
    //        if (!cam.GetComponent<CameraNetworkController>().startMoveIsDoneFlag)
    //            yield break;

    //        //reset camera's old target
    //        RemoveTarget();
    //        //tell the camera to go to player position
    //        StartCoroutine(cam.GetComponent<CameraNetworkController>().goToPosition(cam.GetComponent<CameraNetworkController>().cameraCurrentPos, playerLeft.transform.position, 1));

    //    }
    //    else
    //    {

    //        playersLeftTurn = false;
    //        playersRightTurn = true;
    //        whosTurn = "Player Right";

    //        yield return new WaitForSeconds(0.9f);

    //        //reset camera's old target
    //        RemoveTarget();
    //        //tell the camera to go to enemy position
    //        StartCoroutine(cam.GetComponent<CameraNetworkController>().goToPosition(cam.GetComponent<CameraNetworkController>().cameraCurrentPos, playerRight.transform.position, 1));

    //    }

    //    print("whosTurn: " + whosTurn);
    //}

    //public void RoundTurn()
    //{
    //    roundTurnManagerWithPlayerClientRpc();
    //}

    //[ClientRpc]
    //private void roundTurnManagerWithPlayerClientRpc()
    //{
    //    StartCoroutine(roundTurnManagerWithPlayer());
    //}


    //private void RemoveTarget()
    //{
    //    RemoveTargetServerRpc();
    //}


    //[ServerRpc(RequireOwnership =false)]
    //private void RemoveTargetServerRpc()
    //{
    //    RemoveTargetClientRpc();
    //}
    //[ClientRpc]
    //private void RemoveTargetClientRpc()
    //{
    //    cam.GetComponent<CameraNetworkController>().targetToFollow = null;
    //}


    ///// <summary>
    ///// Gameover sequence.
    ///// </summary>
    //IEnumerator finishTheGame(int res)
    //{

    //    //finish the game
    //    gameIsFinished = true;
    //    print("Game Is Finished");

    //    //play sfx
    //    playSfx(endSfx);

    //    //wait a little
    //    yield return new WaitForSeconds(1.0f);

    //    //disable ui camera
    //    uiCam.SetActive(false);

    //    //activate game finish plane
    //    gameoverManager.SetActive(true);

    //    //set the label
    //    if (res == 0)
    //    {
    //        uiGameStateLabel.GetComponent<TextMesh>().text = "You have Lost :(";
    //    }
    //    else if (res == 1)
    //    {
    //        uiGameStateLabel.GetComponent<TextMesh>().text = "You have Won !";
    //    }
    //    else if (res == 2)
    //    {
    //        //uiGameStateLabel.GetComponent<TextMesh>().text = "Did you have a good hunt?";
    //        ////set stat info
    //        //uiBirdhuntStatPanel.SetActive(true);
    //        //uiStatBirdHits.GetComponent<TextMesh>().text = birdsHit.ToString();
    //        //int BirdHuntBestScore = PlayerPrefs.GetInt("BirdHuntBestScore");
    //        //uiStatBestScore.GetComponent<TextMesh>().text = BirdHuntBestScore.ToString();
    //        ////save new best score
    //        //if (birdsHit > BirdHuntBestScore)
    //        //{
    //        //    PlayerPrefs.SetInt("BirdHuntBestScore", birdsHit);
    //        //    uiStatBestScore.GetComponent<TextMesh>().text = birdsHit.ToString();
    //        //}

    //    }

    //    //bring the panel inside game view
    //    float t = 0;
    //    while (t < 1)
    //    {
    //        t += Time.deltaTime;
    //        gameoverManager.transform.position = new Vector3(cam.transform.position.x,
    //                                                            Mathf.SmoothStep(-15, 0, t),
    //                                                            gameoverManager.transform.position.z);
    //        yield return 0;
    //    }

    //}


    ////*****************************************************************************
    //// This function animates a button by modifying it's scales on x-y plane.
    //// can be used on any element to simulate the tap effect.
    ////*****************************************************************************
    //IEnumerator animateButton(GameObject _btn)
    //{

    //    float buttonAnimationSpeed = 9.0f;
    //    canTap = false;
    //    Vector3 startingScale = _btn.transform.localScale;  //initial scale	
    //    Vector3 destinationScale = startingScale * 1.1f;    //target scale

    //    //Scale up
    //    float t = 0.0f;
    //    while (t <= 1.0f)
    //    {
    //        t += Time.deltaTime * buttonAnimationSpeed;
    //        _btn.transform.localScale = new Vector3(Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
    //            Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
    //            _btn.transform.localScale.z);
    //        yield return 0;
    //    }

    //    //Scale down
    //    float r = 0.0f;
    //    if (_btn.transform.localScale.x >= destinationScale.x)
    //    {
    //        while (r <= 1.0f)
    //        {
    //            r += Time.deltaTime * buttonAnimationSpeed;
    //            _btn.transform.localScale = new Vector3(Mathf.SmoothStep(destinationScale.x, startingScale.x, r),
    //                Mathf.SmoothStep(destinationScale.y, startingScale.y, r),
    //                _btn.transform.localScale.z);
    //            yield return 0;
    //        }
    //    }

    //    if (r >= 1)
    //        canTap = true;
    //}


    ///// <summary>
    ///// enable touch commands again
    ///// </summary>
    //IEnumerator activateTap()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    canTap = true;
    //}


    ///// <summary>
    ///// Plays the sfx.
    ///// </summary>
    //void playSfx(AudioClip _clip)
    //{
    //    GetComponent<AudioSource>().clip = _clip;
    //    if (!GetComponent<AudioSource>().isPlaying)
    //    {
    //        GetComponent<AudioSource>().Play();
    //    }
    //}


    ///// <summary>
    ///// Game timer manager
    ///// </summary>
    ////void manageGameTimer()
    ////{

    ////    if (gameIsFinished)
    ////        return;

    ////    seconds = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) % 60;
    ////    minutes = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) / 60;

    ////    if (seconds == 0 && minutes == 0)
    ////    {
    ////        StartCoroutine(finishTheGame(2));
    ////    }

    ////    remainingTime = string.Format("{0:00} : {1:00}", minutes, seconds);
    ////    uiTimeText.GetComponent<TextMesh>().text = remainingTime.ToString();

    ////    //Also show hitted birds counter on UI
    ////    uiBirdsHitText.GetComponent<TextMesh>().text = birdsHit.ToString();
    ////}


    ///// <summary>
    ///// Adds the bonus time.
    ///// </summary>
    ////public static void addBonusTime()
    ////{
    ////    gameTimer += bonusTime;
    ////}

    #endregion

    /// <summary>
    /// Main game controller class.
    /// Game controller is responsible for assigning turns to player and opponent (thus making this game a turn-based one!), 
    /// setting ground types (to curved or flat types), managing UI elements including health bars and info panels, managing player inputs (on UI), 
    /// checking for gameover events and post gameover settings.
    /// </summary>


    //[Header("Background Type")]
    public enum groundTypes { flat, curved }
    public groundTypes groundType = groundTypes.flat;   //we have two options here. default is flat ground.
    public GameObject flatBg;                           //reference to flag ground object.
    public GameObject curvedBg;                         //reference to curved ground holder object.


    // Static variables //

    public static bool isArrowInSceneLeft;              //have any body shot an arrow? (is there an arrow inside the game?)
    public static bool isArrowInSceneRight;              //have any body shot an arrow? (is there an arrow inside the game?)

    public static bool gameIsStarted;               //global flag for game start state
    public static bool gameIsFinished;              //global flag for game finish state
    public static bool noMoreShooting;              //We use this to stop the shoots when someone has been killed but the game is yet to finish
    public static int round;                        //internal counter to assign turn to player and AI



    public static bool playersLeftTurn;                 //flag to check if this is player's turn
    public static bool playersRightTurn;





    public static string whosTurn;                  //current turn holder in string. useful if you want to extend the game.


    public static int playerArrowShot;              //how many arrows player shot in this game
                                                    // Static variables //

    // Private vars //
    private bool canTap;
    private GameObject AdManagerObject;

    [Header("AudioClips")]
    public AudioClip tapSfx;
    public AudioClip endSfx;


    [Header("Public GameObjects")]
    //Reference to scene game objects		
    public GameObject uiPlayerLeftHealthBar;            //Names are self-explanatory
    public GameObject uiLeftPlayerInfoPanel;
    public GameObject uiLeftPlayerDistance;



    public GameObject uiRightPlayerHealthBar;
    public GameObject uiRightPlayerInfoPanel;
    public GameObject uiRightPlayerDistance;


    private GameObject playerLeft;
    private GameObject playerRight;

    private GameObject cam;
    private GameObject uiCam;
    public GameObject gameoverManager;
    public GameObject uiGameStateLabel;
    public GameObject uiYouWon;


    private Vector3 playerLeftHBSC;                     //player health bar starting scale                                                    
    private float playerLeftHealthScale;                //player health bar real-time scale

    private Vector3 playerRightHBSC;                     //player health bar starting scale                                                    
    private float playerRightHealthScale;                //player health bar real-time scale


    /// <summary>
    /// INIT
    /// </summary>

    [SerializeField]
    private CharacterDataSO[] m_characterDatas;
    [SerializeField]
    private CharacterDatabase charsDB;


    [Header("Icons")]
    public GameObject leftHudIcon;
    public GameObject rightHudIcon;
    public GameObject leftInfoIcon;
    public GameObject rightInfoIcon;




    //void Start()
    //{

    //    StartCoroutine(activateTap());

    //    //if (SceneManager.GetActiveScene().name.Equals("BirdHunt"))
    //    //{
    //    //    playersLeftTurn = true;
    //    //    playersRightTurn = false;
    //    //    enemysTurn = false;
    //    //}

    //    //if (SceneManager.GetActiveScene().name.Equals("Game"))
    //    //{
    //    //    StartCoroutine(roundTurnManagerWithCom());
    //    //}

    //    //if (SceneManager.GetActiveScene().name.Equals("GameWithPlayer"))
    //    //{

    //        StartCoroutine(roundTurnManagerWithPlayer());
    //    //}

    //}


    private void InitAwake()
    {

        //set ground type with high priority
        switch (groundType)
        {
            case groundTypes.flat:
                flatBg.SetActive(true);
                curvedBg.SetActive(false);
                break;
            case groundTypes.curved:
                flatBg.SetActive(false);
                curvedBg.SetActive(true);
                break;
        }

        //cache main objects
        playerLeft = GameObject.FindGameObjectWithTag("Player");
        playerLeftHBSC = uiPlayerLeftHealthBar.transform.localScale;


        cam = GameObject.FindGameObjectWithTag("MainCamera");
        uiCam = GameObject.FindGameObjectWithTag("uiCamera");

        playerRight = GameObject.FindGameObjectWithTag("player2");  // Only in PvP mode
        playerRightHBSC = uiRightPlayerHealthBar.transform.localScale;

        gameoverManager.SetActive(false);

        isArrowInSceneLeft = false;
        isArrowInSceneRight = false;


        canTap = false;
        gameIsStarted = false;
        gameIsFinished = false;
        noMoreShooting = false;
        round = 0;
        playerArrowShot = 0;

    }


    [ClientRpc]
    public void RemoteInitClientRpc()
    {
        InitAwake();
        StartCoroutine(activateTap());
        RoundTurn();


        // hard code set icon. Because I haven't found a better way yet
        leftHudIcon.GetComponent<MeshRenderer>().material = charsDB.GetCharacter(m_characterDatas[0].charId).iconleft;
        rightHudIcon.GetComponent<MeshRenderer>().material = charsDB.GetCharacter(m_characterDatas[1].charId).iconright;
        leftInfoIcon.GetComponent<MeshRenderer>().material = charsDB.GetCharacter(m_characterDatas[0].charId).iconleft;
        rightInfoIcon.GetComponent<MeshRenderer>().material = charsDB.GetCharacter(m_characterDatas[1].charId).iconright;

    }


    /// <summary>
    /// FSM
    /// </summary>
    void Update()
    {

        //receive inputs at all times
        StartCoroutine(inputManager());


        //manage health bar status
        if (playerLeft != null && playerRight != null)
        {
            updateUiHealthBars();
            updateUiEnemyInfoPanel();
        }


        //we no longer need to loop into gameController if the game is already finished.
        if (gameIsFinished)
            return;

    }


    //*****************************************************************************
    // This function monitors player touches on menu buttons.
    // detects both touch and clicks and can be used with editor, handheld device and 
    // every other platforms at once.
    //*****************************************************************************
    private RaycastHit hitInfo;
    private Ray ray;
    IEnumerator inputManager()
    {

        //Prevent double click
        if (!canTap)
            yield break;

        //Mouse of touch?
        if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)
            ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
        else if (Input.GetMouseButtonUp(0))
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        else
            yield break;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject objectHit = hitInfo.transform.gameObject;
            //print ("objectHit: " + objectHit.name);
            switch (objectHit.name)
            {

                case "Button-Play":
                    playSfx(tapSfx);                            //play touch sound
                    canTap = false;                             //prevent double touch
                    StartCoroutine(animateButton(objectHit));   //touch animation effect
                    yield return new WaitForSeconds(0.25f);     //Wait for the animation to end
                    SceneManager.LoadScene("Menu");
                    StartCoroutine(activateTap());
                    break;
                case "Button-Menu":
                    playSfx(tapSfx);                            //play touch sound
                    canTap = false;                             //prevent double touch
                    StartCoroutine(animateButton(objectHit));   //touch animation effect
                    yield return new WaitForSeconds(0.25f);     //Wait for the animation to end
                    SceneManager.LoadScene("Menu");
                    StartCoroutine(activateTap());
                    break;
                case "Button-Retry":
                    playSfx(tapSfx);                            //play touch sound
                    canTap = false;                             //prevent double touch
                    StartCoroutine(animateButton(objectHit));   //touch animation effect
                    yield return new WaitForSeconds(0.25f);     //Wait for the animation to end
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    StartCoroutine(activateTap());
                    break;
            }
        }
    }


    /// <summary>
    /// shows the distance of player and enemy on the UI
    /// </summary>
    void updateUiEnemyInfoPanel()
    {

        //Need Show UIInfo for Right-side Player


        //if (SceneManager.GetActiveScene().name.Equals("Game"))
        //{
        //    if (playersLeftTurn)
        //    {

        //        uiEnemyInfoPanel.SetActive(true);
        //        float enemyDistance = Vector3.Distance(playerLeft.transform.position, enemy.transform.position);
        //        uiEnemyDistance.GetComponent<TextMesh>().text = ((int)enemyDistance).ToString() + "m";

        //        uiEnemyInfoPanel.transform.position = new Vector3(uiEnemyInfoPanel.transform.position.x,
        //                                                            enemy.transform.position.y,
        //                                                            uiEnemyInfoPanel.transform.position.z);

        //    }
        //    else
        //    {

        //        uiEnemyInfoPanel.SetActive(false);

        //    }

        //}


        //if (SceneManager.GetActiveScene().name.Equals("GameWithPlayer"))
        //{

        if (playersLeftTurn)
        {
            uiLeftPlayerInfoPanel.SetActive(false);
            uiRightPlayerInfoPanel.SetActive(true);
            float enemyRightDistance = Vector3.Distance(playerLeft.transform.position, playerRight.transform.position);
            uiRightPlayerDistance.GetComponent<TextMesh>().text = ((int)enemyRightDistance).ToString() + "m";

            uiRightPlayerInfoPanel.transform.position = new Vector3(uiRightPlayerInfoPanel.transform.position.x,
                                                                playerRight.transform.position.y,
                                                                uiRightPlayerInfoPanel.transform.position.z);

        }
        else
        {

            uiRightPlayerInfoPanel.SetActive(false);
            uiLeftPlayerInfoPanel.SetActive(true);

            float enemyLeftDistance = Vector3.Distance(playerRight.transform.position, playerLeft.transform.position);
            uiLeftPlayerDistance.GetComponent<TextMesh>().text = ((int)enemyLeftDistance).ToString() + "m";

            uiLeftPlayerInfoPanel.transform.position = new Vector3(uiLeftPlayerInfoPanel.transform.position.x,
                                                                playerLeft.transform.position.y,
                                                                uiLeftPlayerInfoPanel.transform.position.z);

        }
    }


    //}


    /// <summary>
    /// Updates the user interface health bars based on available health for each side.
    /// </summary>
    void updateUiHealthBars()
    {

        playerLeftHealthScale = (playerLeft.GetComponent<PlayerNetworkController>().playerCurrentHealth * playerLeftHBSC.x) / playerLeft.GetComponent<PlayerNetworkController>().playerHealth;
        uiPlayerLeftHealthBar.transform.localScale = new Vector3(playerLeftHealthScale, playerLeftHBSC.y, playerLeftHBSC.z);


        //if (SceneManager.GetActiveScene().name.Equals("Game"))
        //{
        //    enemyHealthScale = (enemy.GetComponent<EnemyController>().enemyCurrentHealth * enemyHBSC.x) / enemy.GetComponent<EnemyController>().enemyHealth;
        //    uiEnemyHealthBar.transform.localScale = new Vector3(enemyHealthScale, enemyHBSC.y, enemyHBSC.z);

        //}

        //if (SceneManager.GetActiveScene().name.Equals("GameWithPlayer"))
        //{
        playerRightHealthScale = (playerRight.GetComponent<PlayerNetworkController>().playerCurrentHealth * playerRightHBSC.x) / playerRight.GetComponent<PlayerNetworkController>().playerHealth;

        uiRightPlayerHealthBar.transform.localScale = new Vector3(playerRightHealthScale, playerRightHBSC.y, playerRightHBSC.z);

        //}

    }


    /// <summary>
    /// Assign turns to player and player.
    /// </summary>
    public IEnumerator roundTurnManagerWithPlayer()
    {

        //1. first check if the game is already finished
        if (gameIsFinished || playerLeft == null || playerRight == null)    //
        {
            yield break;
        }

        //2. then check if the situation meets a game over
        //check for game finish state
        if (playerLeft.GetComponent<PlayerNetworkController>().playerCurrentHealth <= 0)
        {

            SendRewardClientRpc(0);
            //print("Player Left is dead.Player Right wins");
            yield break;

        }
        else if (playerRight.GetComponent<PlayerNetworkController>().playerCurrentHealth <= 0)
        {
            SendRewardClientRpc(1);
            //print("Player Right is dead. Player Left win");
            yield break;
        }

        //3. if none of the above is true, continue with the turn-change...

        round++;    //game starts with round 1
        print("Round: " + round);

        //if round carry is odd, its players turn, otherwise its opponent's turn
        int carry;
        carry = round % 2;

        if (carry == 1)
        {

            playersLeftTurn = true;
            playersRightTurn = false;
            whosTurn = "Player Left";

            yield return new WaitForSeconds(0.9f);

            //just incase we need to show the camera's starting animation, we do not need to switch to player, so we just leave the function
            if (!cam.GetComponent<CameraNetworkController>().startMoveIsDoneFlag)
                yield break;

            //reset camera's old target
            RemoveTarget();
            //tell the camera to go to player position
            StartCoroutine(cam.GetComponent<CameraNetworkController>().goToPosition(cam.GetComponent<CameraNetworkController>().cameraCurrentPos, playerLeft.transform.position, 1));

        }
        else
        {

            playersLeftTurn = false;
            playersRightTurn = true;
            whosTurn = "Player Right";

            yield return new WaitForSeconds(0.9f);

            //reset camera's old target
            RemoveTarget();
            //tell the camera to go to enemy position
            StartCoroutine(cam.GetComponent<CameraNetworkController>().goToPosition(cam.GetComponent<CameraNetworkController>().cameraCurrentPos, playerRight.transform.position, 1));

        }

        print("whosTurn: " + whosTurn);
    }

    public void RoundTurn()
    {
        roundTurnManagerWithPlayerClientRpc();
    }

    [ClientRpc]
    private void roundTurnManagerWithPlayerClientRpc()
    {
        StartCoroutine(roundTurnManagerWithPlayer());
    }


    private void RemoveTarget()
    {
        RemoveTargetServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void RemoveTargetServerRpc()
    {
        RemoveTargetClientRpc();
    }
    [ClientRpc]
    private void RemoveTargetClientRpc()
    {
        cam.GetComponent<CameraNetworkController>().targetToFollow = null;
    }


    /// <summary>
    /// Gameover sequence.
    /// </summary>
    IEnumerator finishTheGame(int res)
    {

        //finish the game
        gameIsFinished = true;
        print("Game Is Finished");

        //play sfx
        playSfx(endSfx);

        //wait a little
        yield return new WaitForSeconds(1.0f);

        //disable ui camera
        uiCam.SetActive(false);

        //activate game finish plane
        gameoverManager.SetActive(true);

        //set the label
        if (res == 0)
        {
            if (IsHost)
            {
                uiGameStateLabel.GetComponent<TextMesh>().text = "You have Lost :(";
                uiYouWon.GetComponent<TextMesh>().text = "500"; // Not available in this mode
                int savedCoins = PlayerPrefs.GetInt("PlayerCoins");
                PlayerPrefs.SetInt("PlayerCoins", 500 + savedCoins);
            }
            else
            {
                uiGameStateLabel.GetComponent<TextMesh>().text = "You have Won !";
                uiYouWon.GetComponent<TextMesh>().text = "2000"; // Not available in this mode
                int savedCoins = PlayerPrefs.GetInt("PlayerCoins");
                PlayerPrefs.SetInt("PlayerCoins", 2000 + savedCoins);
            }

        }
        else if (res == 1)
        {

            if (IsHost)
            {
                uiGameStateLabel.GetComponent<TextMesh>().text = "You have Won !";
                uiYouWon.GetComponent<TextMesh>().text = "2000"; // Not available in this mode
                int savedCoins = PlayerPrefs.GetInt("PlayerCoins");
                PlayerPrefs.SetInt("PlayerCoins", 2000 + savedCoins);
            }
            else
            {
                uiGameStateLabel.GetComponent<TextMesh>().text = "You have Lost :(";
                uiYouWon.GetComponent<TextMesh>().text = "500"; // Not available in this mode
                int savedCoins = PlayerPrefs.GetInt("PlayerCoins");
                PlayerPrefs.SetInt("PlayerCoins", 500 + savedCoins);
            }

        }

        //bring the panel inside game view
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            gameoverManager.transform.position = new Vector3(cam.transform.position.x,
                                                                Mathf.SmoothStep(-15, 0, t),
                                                                gameoverManager.transform.position.z);
            yield return 0;
        }

    }

    [ClientRpc]
    public void SendRewardClientRpc(int res)
    {
        Debug.LogError("Da vao day");
        StartCoroutine(finishTheGame(res));
    }

    //*****************************************************************************
    // This function animates a button by modifying it's scales on x-y plane.
    // can be used on any element to simulate the tap effect.
    //*****************************************************************************
    IEnumerator animateButton(GameObject _btn)
    {

        float buttonAnimationSpeed = 9.0f;
        canTap = false;
        Vector3 startingScale = _btn.transform.localScale;  //initial scale	
        Vector3 destinationScale = startingScale * 1.1f;    //target scale

        //Scale up
        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime * buttonAnimationSpeed;
            _btn.transform.localScale = new Vector3(Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
                Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
                _btn.transform.localScale.z);
            yield return 0;
        }

        //Scale down
        float r = 0.0f;
        if (_btn.transform.localScale.x >= destinationScale.x)
        {
            while (r <= 1.0f)
            {
                r += Time.deltaTime * buttonAnimationSpeed;
                _btn.transform.localScale = new Vector3(Mathf.SmoothStep(destinationScale.x, startingScale.x, r),
                    Mathf.SmoothStep(destinationScale.y, startingScale.y, r),
                    _btn.transform.localScale.z);
                yield return 0;
            }
        }

        if (r >= 1)
            canTap = true;
    }


    /// <summary>
    /// enable touch commands again
    /// </summary>
    IEnumerator activateTap()
    {
        yield return new WaitForSeconds(1.0f);
        canTap = true;
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
    /// Game timer manager
    /// </summary>
    //void manageGameTimer()
    //{

    //    if (gameIsFinished)
    //        return;

    //    seconds = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) % 60;
    //    minutes = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) / 60;

    //    if (seconds == 0 && minutes == 0)
    //    {
    //        StartCoroutine(finishTheGame(2));
    //    }

    //    remainingTime = string.Format("{0:00} : {1:00}", minutes, seconds);
    //    uiTimeText.GetComponent<TextMesh>().text = remainingTime.ToString();

    //    //Also show hitted birds counter on UI
    //    uiBirdsHitText.GetComponent<TextMesh>().text = birdsHit.ToString();
    //}


    /// <summary>
    /// Adds the bonus time.
    /// </summary>
    //public static void addBonusTime()
    //{
    //    gameTimer += bonusTime;
    //}


}
