using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using TMPro;

public class MenuManager : SingletonNetwork<GameplayManager>
{

    private string nextScene = "CharacterSelection";

    public static string joincodestatic;

    [SerializeField]
    private CharacterDataSO[] m_characterDatas;

    [SerializeField]
    GameObject joincodefield;
    private IEnumerator Start()
    {
        // -- To test with latency on development builds --
        // To set the latency, jitter and packet-loss percentage values for develop builds we need
        // the following code to execute before NetworkManager attempts to connect (changing the
        // values of the parameters as desired).
        //
        // If you'd like to test without the simulated latency, just set all parameters below to zero(0).
        //
        // More information here:
        // https://docs-multiplayer.unity3d.com/netcode/current/tutorials/testing/testing_with_artificial_conditions#debug-builds
#if DEVELOPMENT_BUILD && !UNITY_EDITOR
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().
            SetDebugSimulatorParameters(
                packetDelay: 50,
                packetJitter: 5,
                dropRate: 3);
#endif

        ClearAllCharacterData();

        // Wait for the network Scene Manager to start
        yield return new WaitUntil(() => NetworkManager.Singleton.SceneManager != null);

        // Set the events on the loading manager
        // Doing this because every time the network session ends the loading manager stops
        // detecting the events
        LoadingSceneManager.Instance.Init();
    }

    public void OnClickHost()
    {
        NetworkManager.Singleton.StartHost();
        //AudioManager.Instance.PlaySoundEffect(m_confirmClip);
        LoadingSceneManager.Instance.LoadScene(nextScene);
    }

    public void OnclickHost2()
    {
        StartHostWithRelay();
    }

    public void OnClickJoin()
    {
        //AudioManager.Instance.PlaySoundEffect(m_confirmClip);
        StartCoroutine(Join());
    }


    public void OnClickJoin2()
    {
        //get data from input
        var password = joincodefield.GetComponent<TMP_InputField>().text;

        //StartClientWithRelay(password);
        StartCoroutine(Join2(password));
    }


    public void OnClickQuit()
    {
        //AudioManager.Instance.PlaySoundEffect(m_confirmClip);
        Application.Quit();
    }

    private void ClearAllCharacterData()
    {
        // Clean the all the data of the characters so we can start with a clean slate
        //foreach (CharacterDataSO data in m_characterDatas)
        //{
        //    data.EmptyData();
        //}
    }

    //private void TriggerMainMenuTransitionAnimation()
    //{
    //    m_menuAnimator.SetTrigger(k_enterMenuTriggerAnim);
    //    AudioManager.Instance.PlaySoundEffect(m_confirmClip);
    //}

    private IEnumerator Join()
    {
        LoadingFadeEffect.Instance.FadeAll();

        yield return new WaitUntil(() => LoadingFadeEffect.s_canLoad);

        NetworkManager.Singleton.StartClient();
    }

    private IEnumerator Join2(string joincode)
    {
        LoadingFadeEffect.Instance.FadeAll();
        yield return new WaitUntil(() => LoadingFadeEffect.s_canLoad);
        StartClientWithRelay(joincode);

    }



    public async Task<string> StartHostWithRelay(int maxConnections = 2)
    {
        //await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.LogError("Ao that day " + joinCode);
        joincodestatic = joinCode;
        LoadingSceneManager.Instance.LoadScene(nextScene);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;

    }


    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        //await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }



}
