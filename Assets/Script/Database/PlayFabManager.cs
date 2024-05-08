using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text messText;
    public TMP_InputField emailInput;
    public TMP_InputField passInput;
    public TMP_InputField nickNameInput;
    public GameObject SetNickName;
    public GameObject Login;
    public Button btnSubmitNickNam;
    public string nickName;
    private void Start()
    {
        if (passInput != null)
        {
            passInput.contentType = TMP_InputField.ContentType.Password;
        }
    }
    public void RegisterButton()
    {
        if (passInput.text.Length < 6)
        {
            messText.text = "Password to short";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passInput.text,
            RequireBothUsernameAndEmail = false,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request,OnRegisterSuccess,OnError);
    }
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messText.text = "Register and logged in";
        emailInput.text = "";
        passInput.text = "";
    }
    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }
    void OnLoginSuccess(LoginResult result)
    {
        nickName = null;
        messText.text = "Logged in";
        DataManager.Instance.SetIdPlayer(result.PlayFabId);
        if (result != null && result.InfoResultPayload != null && result.InfoResultPayload.PlayerProfile != null)
        {
            nickName = result.InfoResultPayload.PlayerProfile.DisplayName;
            if (nickName != null)
            {
                DataManager.Instance.SetNickName(nickName);
                SceneManager.LoadScene(1);
            }
        }
        if (nickName == null)
        {
            Login.SetActive(false);
            SetNickName.SetActive(true);
        }
        GetCharacters();
    }
    public void SubmitName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nickNameInput.text,
        };
        DataManager.Instance.SetNickName(nickNameInput.text);
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        GetSpecificKeyForPlayer();
        GetPlayerData("avatar");
        ClientGetUserPublisherData();
        SceneManager.LoadScene(1);
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display nam");
    }
    void GetCharacters()
    {

    }

    public void UpdatePlayerData(string key, string value)
    {
        var request = new UpdateUserDataRequest
        {
            // Mảng chứa dữ liệu cần cập nhật
            Data = new System.Collections.Generic.Dictionary<string, string>
            {
                { key, value }
            }
        };

        // Gọi API để cập nhật dữ liệu người chơi
        PlayFabClientAPI.UpdateUserData(request, OnUpdateUserDataSuccess, OnError);
    }

    // Xử lý khi cập nhật dữ liệu thành công
    private void OnUpdateUserDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Player data updated successfully");
    }

    // Xử lý khi có lỗi xảy ra
    private void OnError(PlayFabError error)
    {
        Debug.LogError("PlayFab Error: " + error.GenerateErrorReport());
    }
    public void ClientSetUserPublisherData(string key, string value)
    {
        PlayFabClientAPI.UpdateUserPublisherData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
             { "SomeKey", "SomeValue" }
         }
        },
        result => Debug.Log("Complete setting Regular User Publisher Data"),
        error =>
        {
            Debug.Log("Error setting Regular User Publisher Data");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    public void GetSpecificKeyForPlayer()
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = DataManager.Instance.idPlayer,
        };

        PlayFabClientAPI.GetUserData(request, result =>
        {

            if (result.Data.TryGetValue("avatar", out var specificValue))
            {
                Debug.Log(DataManager.Instance.idPlayer);

                if (specificValue.Value != null)
                {
                    Debug.Log(specificValue.Value);
                }
            }
        }, OnError);
    }
    public void GetPlayerData(string key)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = DataManager.Instance.idPlayer
        };

        PlayFabClientAPI.GetUserData(request, result =>
        {
            if (result.Data.TryGetValue(key, out var specificValue))
            {
                if (specificValue.Value != null)
                {
                    Debug.Log($"{key} value: {specificValue.Value}");
                    // Đoạn mã tiếp theo: xử lý giá trị tìm thấy
                }
                else
                {
                    Debug.Log($"{key} value is null.");
                }
            }
            else
            {
                Debug.Log($"Key '{key}' does not exist in user data.");
            }
        }, OnError);
    }
    public void ClientGetUserPublisherData()
    {
        PlayFabClientAPI.GetUserPublisherData(new GetUserDataRequest()
        {
            PlayFabId = DataManager.Instance.idPlayer,
        }, result =>
        {
            if (result.Data == null || !result.Data.ContainsKey("SomeKey")) Debug.Log("No SomeKey");
            else Debug.Log("SomeKey: " + result.Data["SomeKey"]);
        },
        error =>
        {
            Debug.Log("Got error getting Regular Publisher Data:");
            Debug.Log(error.GenerateErrorReport());
        }) ;
    }
}
