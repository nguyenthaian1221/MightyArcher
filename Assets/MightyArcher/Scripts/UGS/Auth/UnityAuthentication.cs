using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using System;

public class UnityAuthentication : MonoBehaviour
{
    // Declare all input filled
    [SerializeField] GameObject lg_usernamefield;
    [SerializeField] GameObject lg_passwordfield;
    [SerializeField] GameObject rg_usernamefield;
    [SerializeField] GameObject rg_passwordfield;
    [SerializeField] GameObject rg_repeatpasswordfield;
    [SerializeField] CharacterDatabase charDB;

    private async void Start()
    {
        // UnityServices.InitializeAsync() will initialize all services that are subscribed to Core.
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
        Debug.Log(AuthenticationService.Instance.IsSignedIn);
        SetupEvents();
    }

    // Setup authentication event handlers if desired
    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Show how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

            // Load Cloud Save data if player is not anonymous
            LoadData();



            // Change Scene
        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }


    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        //finally
        //{
        //    if (AuthenticationService.Instance.IsSignedIn)
        //    {
        //        SceneManager.LoadScene("Menu");
        //    }
        //}
    }


    // Set up for custom username password
    async Task SignUpWithUsernamePassword(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync($"{username}", password);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        //finally
        //{
        //    if (AuthenticationService.Instance.IsSignedIn)
        //    {
        //        SceneManager.LoadScene("Menu");
        //    }
        //}
    }

    async Task AddUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.AddUsernamePasswordAsync(username, password);
            Debug.Log("Username and password added.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    async Task UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, newPassword);
            Debug.Log("Password updated.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }


    async Task SignInCachedUserAsync()
    {
        // Check if a cached player already exists by checking if the session token exists
        if (!AuthenticationService.Instance.SessionTokenExists)
        {
            // if not, then do nothing
            return;
        }

        // Sign in Anonymously
        // This call will sign in the cached player.
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }



    #region Manage Login By AN the NoobDev

    // Sign In as guest
    public async void OnSignInAsGuest()
    {
        //await SignInAnonymouslyAsync();
        StartCoroutine(CheckInternetConnection(async isConnected =>
        {
            if (isConnected)
            {
                await SignInAnonymouslyAsync();
            }
            else
            {
                SceneManager.LoadScene("Menu");
            }
        }));
    }

    // Register account
    public async void OnSignupBtnClicked()
    {
        var username = rg_usernamefield.GetComponent<TMP_InputField>().text;
        var password = rg_passwordfield.GetComponent<TMP_InputField>().text;
        var repeatpassword = rg_repeatpasswordfield.GetComponent<TMP_InputField>().text;

        //if (username == null || password == null || repeatpassword == null)
        //{
        //    Debug.LogError("Username and password can't be empty!!!");
        //    return;
        //}

        if (password != repeatpassword)
        {
            Debug.LogError("Password and Repeat Password are not identical!!!");
            return;
        }
        else
        {
            await SignUpWithUsernamePassword(username, password);
        }


    }

    // Login account

    public async void OnLoginBtnClicked()
    {
        var username = lg_usernamefield.GetComponent<TMP_InputField>().text;
        var password = lg_passwordfield.GetComponent<TMP_InputField>().text;

        await SignInWithUsernamePasswordAsync(username, password);

    }

    //Log out
    [ContextMenu("Log out")]
    public void OnLogOutBtnClicked()
    {

        AuthenticationService.Instance.SignOut();
        AuthenticationService.Instance.ClearSessionToken();

    }

    [ContextMenu("Check Is Login")]
    public void CheckIsLogIn()
    {

        Debug.Log(AuthenticationService.Instance.IsSignedIn);

    }

    #endregion


    public async void SaveData()
    {

        var data = new Dictionary<string, object> { { "keyName", "value" } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);

    }


    public async void LoadData()
    {

        #region manage all key 
        HashSet<string> listofkeys = new HashSet<string>();
        listofkeys.Add("PlayerCoins");

        //add all data name into set
        for (int i = 0; i < charDB.characterCount; i++)
        {
            var character = charDB.GetCharacter(i);

            listofkeys.Add(character.characterName);

        }


        #endregion

        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(listofkeys);

        if (playerData.TryGetValue("PlayerCoins", out var keyName))
        {
            Debug.LogError($"PlayerCoins: {keyName.Value.GetAs<int>()}");
            PlayerPrefs.SetInt("PlayerCoins", keyName.Value.GetAs<int>());
        }
        else
        {
            PlayerPrefs.SetInt("PlayerCoins", 0);
        }

        // Get PlayerPrefs
        for (int i = 0; i < charDB.characterCount; i++)
        {
            var character = charDB.GetCharacter(i);


            if (playerData.TryGetValue(character.characterName, out var namechar))
            {

                PlayerPrefs.SetInt(character.characterName, namechar.Value.GetAs<int>());
            }
            else
            {
                PlayerPrefs.SetInt(character.characterName, 0);
            }

        }

        SceneManager.LoadScene("Menu");
    }


    IEnumerator CheckInternetConnection(Action<bool> action)
    {

        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.Log("Error");
            action(false);
        }
        else
        {
            Debug.Log("Success");
            action(true);
        }
    }

}
