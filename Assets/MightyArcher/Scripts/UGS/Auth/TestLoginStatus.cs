using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave.Models;
public class TestLoginStatus : MonoBehaviour
{

    void Start()
    {

        Debug.LogError(UnityServices.State);
        Debug.LogError(AuthenticationService.Instance.IsSignedIn);

        
    }

    // Update is called once per frame
    [ContextMenu("Test  Save")]
    [System.Obsolete]
    public async void SaveSomeData()
    {
        var data = new Dictionary<string, object> { { "key", "someValue" } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    [ContextMenu("Test Load")]
    [System.Obsolete]
    public async void  LoadSomeData()
    {
         var savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "key" });

        Debug.Log("Done: " + savedData["key"]);
    }

    [ContextMenu("Test Delete")]
    [System.Obsolete]
    public async void DeleteSomeData()
    {
        await CloudSaveService.Instance.Data.ForceDeleteAsync("key");
    }

    //Money



}
