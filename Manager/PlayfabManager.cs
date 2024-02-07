//using Facebook.Unity;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFab.ProfilesModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_IOS
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Extensions;
#endif

using EntityKey = PlayFab.ProfilesModels.EntityKey;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;

    public Text infoText;

    public UIManager uiManager;

    [ShowInInspector]
    string customId = "";

    public bool isActive = false;
    public bool isLogin = false;
    public bool isDelay = false;
    public bool isNone = false;

    private bool playerData = false;
    private bool statisticsData = false;
    private bool inventoryData = false;
    private bool grantItemData = false;

    private long coin = 0;
    private long coinA = 0;
    private long coinB = 0;

    private int consumeGold = 0;
    private int consumeGoldA = 0;
    private int consumeGoldB = 0;

#if UNITY_IOS
    private string AppleUserIdKey = "";
    private IAppleAuthManager _appleAuthManager;

#endif

    public CollectionManager collectionManager;
    public NickNameManager nickNameManager;
    public MoneyAnimation moneyAnimation;

    PlayerDataBase playerDataBase;
    ShopDataBase shopDataBase;


    [Header("Entity")]
    private string entityId;
    private string entityType;
    private readonly Dictionary<string, string> entityFileJson = new Dictionary<string, string>();

    private List<ItemInstance> inventoryList = new List<ItemInstance>();

    Dictionary<string, string> defaultCustomData = new Dictionary<string, string>() { { "Level", "0" }, { "SSRLevel", "0" } };


    private void Awake()
    {
        instance = this;

        isActive = false;
        isLogin = false;
        isDelay = false;

        if(infoText != null) infoText.text = "";

#if UNITY_ANDROID
        GoogleActivate();
#elif UNITY_IOS
        IOSActivate();
#endif

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
    }

    private void Start()
    {
        //if(GameStateManager.instance.IsLogin)
        //{
        //    StateManager.instance.ServerInitialize();

        //    return;
        //}

        if(isNone)
        {
            isActive = true;
            isLogin = true;
            return;
        }

        infoText.text = "";

        if (GameStateManager.instance.IsLogin)
        {
            infoText.text = LocalizationManager.instance.GetString("Login") + "...";

#if UNITY_EDITOR || UNITY_EDITOR_OSX
            OnClickGuestLogin();
#else
        GetTitleInternalData("CheckVersion", CheckVersion);
#endif
        }
        else
        {
            if (GameStateManager.instance.AutoLogin)
            {
                infoText.text = LocalizationManager.instance.GetString("Login") + "...";

                switch (GameStateManager.instance.Login)
                {
                    case LoginType.None:
                        break;
                    case LoginType.Guest:
                        OnClickGuestLogin();
                        break;
                    case LoginType.Google:
                        OnClickGoogleLogin();
                        break;
                    case LoginType.Facebook:
                        //OnClickFacebookLogin();
                        break;
                    case LoginType.Apple:
                        OnClickAppleLogin();
                        break;
                }
            }
        }
    }

    #region Initialize

#if UNITY_ANDROID
    private void GoogleActivate()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .AddOauthScope("profile")
        .RequestServerAuthCode(false)
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
#endif
#if UNITY_IOS
    private void IOSActivate()
    {
        // If the current platform is supported
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            _appleAuthManager = new AppleAuthManager(deserializer);
        }
        StartCoroutine(AppleAuthUpdate());
    }

    IEnumerator AppleAuthUpdate()
    {
        while (true)
        {
            _appleAuthManager?.Update();
            yield return null;
        }
    }
#endif
    #endregion

    public void LogOut()
    {
#if UNITY_EDITOR
        OnClickGuestLogout();
#elif UNITY_ANDROID
        OnClickGoogleLogout();
#endif

        //uiManager.OnLogout();

        GameStateManager.instance.Initialize();

        isActive = false;
        isLogin = false;
        isDelay = false;

        PlayerPrefs.SetString("AppleLogin", "");

        Debug.Log("Logout");

        SceneManager.LoadScene("LoginScene");
    }


    #region Message
    private void SetEditorOnlyMessage(string message, bool error = false)
    {
#if UNITY_EDITOR
        if (error) Debug.LogError("<color=red>" + message + "</color>");
        //else Debug.Log(message);
#endif
    }
    private void DisplayPlayfabError(PlayFabError error) => SetEditorOnlyMessage("error : " + error.GenerateErrorReport(), true);

    #endregion
    #region GuestLogin
    public void OnClickGuestLogin()
    {
        uiManager.loginUI.SetActive(false);

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            uiManager.LoginFail();

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        if (isLogin) return;

        isLogin = true;

        customId = GameStateManager.instance.CustomId;

        if (string.IsNullOrEmpty(customId))
            CreateGuestId();
        else
            LoginGuestId();
    }

    private void CreateGuestId()
    {
        Debug.Log("New PlayfabId");

        customId = GetRandomPassword(16);

        GameStateManager.instance.CustomId = customId;

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = customId,
            CreateAccount = true
        }, result =>
        {
            GameStateManager.instance.AutoLogin = true;
            GameStateManager.instance.Login = LoginType.Guest;
            OnLoginSuccess(result);
        }, error =>
        {
            uiManager.LoginFail();

            isLogin = false;

            Debug.LogError("Login Fail - Guest");
        });
    }

    private string GetRandomPassword(int _totLen)
    {
        string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var chars = Enumerable.Range(0, _totLen)
            .Select(x => input[UnityEngine.Random.Range(0, input.Length)]);
        return new string(chars.ToArray());
    }
    private void LoginGuestId()
    {
        Debug.Log("Guest Login");

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = customId,
            CreateAccount = false
        }, result =>
        {
            OnLoginSuccess(result);
        }, error =>
        {
            uiManager.LoginFail();

            isLogin = false;

            Debug.LogError("Login Fail - Guest");
        });
    }

    public void OnClickGuestLogout()
    {
        Debug.LogError("Guest Logout");

        PlayFabClientAPI.ForgetAllCredentials();
    }

    #endregion
    #region Google Login
    public void OnClickGoogleLogin()
    {
        uiManager.loginUI.SetActive(false);

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            uiManager.LoginFail();

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        if (isLogin) return;

        isLogin = true;

        Debug.Log("Google Login");

#if UNITY_ANDROID
        LoginGoogleAuthenticate();
#else
        SetEditorOnlyMessage("Only Android Platform");
#endif
    }

    private void LoginGoogleAuthenticate()
    {
#if UNITY_ANDROID

        //if (Social.localUser.authenticated)
        //{
        //    Debug.Log("???????? ???????????? ?????????????? ???????????????? ??????????????????????.");
        //    return;
        //}
        Social.localUser.Authenticate((bool success) =>
        {
            if (!success)
            {
                return;
            }

            var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                ServerAuthCode = serverAuthCode,
                CreateAccount = true,
            },
            result =>
            {
                GameStateManager.instance.AutoLogin = true;
                GameStateManager.instance.Login = LoginType.Google;

                Debug.Log("Google Login Success");

                OnLoginSuccess(result);
            },
            error =>
            {
                uiManager.LoginFail();

                isLogin = false;

                Debug.Log("Google Login Fail");

                DisplayPlayfabError(error);
            });
        });

#endif
    }

    public void OnClickGoogleLogout()
    {
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).SignOut();
#endif
    }

    public void OnClickGoogleLink()
    {
#if UNITY_ANDROID
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                    LinkGoogleAccountRequest request = new LinkGoogleAccountRequest()
                    {
                        ForceLink = true,
                        ServerAuthCode = serverAuthCode
                    };

                    PlayFabClientAPI.LinkGoogleAccount(request, result =>
                    {
                        Debug.Log("Link Google Account Success");

                        GameStateManager.instance.AutoLogin = true;
                        GameStateManager.instance.Login = LoginType.Google;
                    }, error =>
                    {
                        Debug.Log(error.GenerateErrorReport());
                    });
                }
                else
                {
                    Debug.Log("Link Google Account Fail");
                }
            });
        }
        else
        {
            Debug.Log("Link Google Account Fail");
        }
#endif
    }
    #endregion
    #region Apple Login

    public void OnClickAppleLogin()
    {
        uiManager.loginUI.SetActive(false);

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            uiManager.LoginFail();

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        if (isLogin) return;

        isLogin = true;

#if UNITY_IOS
        Debug.Log("Try Apple Login");
        StartCoroutine(AppleLoginCor());
#endif
    }

#if UNITY_IOS
    void SignInWithApple()
    {
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        _appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    OnClickAppleLogin(appleIdCredential.IdentityToken);

                    PlayerPrefs.SetString("AppleLogin", appleIdCredential.IdentityToken.ToString());
                }
            }, error =>
            {
                uiManager.LoginFail();

                isLogin = false;

                var authorizationErrorCode = error.GetAuthorizationErrorCode();
            });
    }

    IEnumerator AppleLoginCor()
    {
        if (SaveByte.LoadByteArrayToPlayerPrefs() != null)
        {
            byte[] apple = SaveByte.LoadByteArrayToPlayerPrefs();

            Debug.Log("Apple Auto Login");

            OnClickAppleLogin(apple);
        }
        else
        {
            IOSActivate();

            var _newAppleUser = false;

            while (_appleAuthManager == null) yield return null;

            if (!_newAppleUser)
            {
                var quickLoginArgs = new AppleAuthQuickLoginArgs();

                _appleAuthManager.QuickLogin(
                    quickLoginArgs,
                    credential =>
                    {
                        var appleIdCredential = credential as IAppleIDCredential;
                        if (appleIdCredential != null)
                        {
                            OnClickAppleLogin(appleIdCredential.IdentityToken);
                        }
                    },
                    error =>
                    {
                        _newAppleUser = true;
                        SignInWithApple();
                        var authorizationErrorCode = error.GetAuthorizationErrorCode();
                    });
            }
            else
            {
                SignInWithApple();
            }
            yield return null;
        }
    }

    public void OnClickAppleLogin(byte[] identityToken)
    {
        PlayFabClientAPI.LoginWithApple(new LoginWithAppleRequest
        {
            CreateAccount = true,
            IdentityToken = Encoding.UTF8.GetString(identityToken),
            TitleId = PlayFabSettings.TitleId
        }
        , result =>
        {
            Debug.Log("Apple Login Success");

            SaveByte.SaveByteArrayToPlayerPref(identityToken);

            GameStateManager.instance.AutoLogin = true;
            GameStateManager.instance.Login = LoginType.Apple;

            OnLoginSuccess(result);
        }
        , DisplayPlayfabError);
    }

    public void OnClickAppleLink(bool forceLink = false)
    {
        var quickLoginArgs = new AppleAuthQuickLoginArgs();

        _appleAuthManager.QuickLogin(quickLoginArgs, credential =>
        {
            var appleIdCredential = credential as IAppleIDCredential;
            if (appleIdCredential != null)
            {
                TryLinkAppleAccount(appleIdCredential.IdentityToken, forceLink);
            }
        }, error =>
        {
            var authorizationErrorCode = error.GetAuthorizationErrorCode();
        });
    }

    public void TryLinkAppleAccount(byte[] identityToken, bool forceLink)
    {
        PlayFabClientAPI.LinkApple(new LinkAppleRequest
        {
            ForceLink = forceLink,
            IdentityToken = Encoding.UTF8.GetString(identityToken)
        }
        , result =>
        {
            Debug.Log("Link Apple Success!!");

            GameStateManager.instance.AutoLogin = true;
            GameStateManager.instance.Login = LoginType.Apple;
            //optionContent.SuccessLink(LoginType.Apple);
        }
        , DisplayPlayfabError);
    }
#endif
    #endregion

    public void OnLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        //SetEditorOnlyMessage("Playfab Login Success");

        Debug.Log("Playfab Login Success");

        GameStateManager.instance.IsLogin = true;

        customId = result.PlayFabId;
        entityId = result.EntityToken.Entity.Id;
        entityType = result.EntityToken.Entity.Type;

        GameStateManager.instance.PlayfabId = result.PlayFabId;

#if UNITY_EDITOR || UNITY_EDITOR_OSX
        StartCoroutine(LoadDataCoroutine());
#elif UNITY_ANDROID
        GetTitleInternalData("CheckAOSVersion", CheckVersion);
#elif UNITY_IOS
        GetTitleInternalData("CheckIOSVersion", CheckVersion);
#endif

    }

    public void CheckVersion(bool check)
    {
        Debug.Log("Checking Version...");

        if (check)
        {
#if UNITY_ANDROID
            GetTitleInternalData("AOSVersion", CheckUpdate);
#elif UNITY_IOS
            GetTitleInternalData("IOSVersion", CheckUpdate);
#endif
        }
        else
        {
            StartCoroutine(LoadDataCoroutine());
        }
    }

    public void CheckUpdate(bool check)
    {
        if (check)
        {
            StartCoroutine(LoadDataCoroutine());
        }
        else
        {
            uiManager.OnNeedUpdate();
        }
    }

    public void SetProfileLanguage(LanguageType type)
    {
        EntityKey entity = new EntityKey();
        entity.Id = entityId;
        entity.Type = entityType;

        var request = new SetProfileLanguageRequest
        {
            Language = type.ToString(),
            ExpectedVersion = 0,
            Entity = entity
        };
        PlayFabProfilesAPI.SetProfileLanguage(request, res =>
        {
            Debug.Log("The language on the entity's profile has been updated.");
        }, FailureCallback);
    }

    public void GetPlayerNickName()
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = customId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        (result) =>
        {
            GameStateManager.instance.NickName = result.PlayerProfile.DisplayName;

            if (GameStateManager.instance.NickName == null)
            {
                UpdateDisplayName(GameStateManager.instance.PlayfabId);
                //nickNameManager.OpenFreeNickName();
            }
            // GameStateManager.Instance.SavePlayerData();
        },
        DisplayPlayfabError);
    }

    void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    private void OnCloudUpdateStats(ExecuteCloudScriptResult result)
    {
        //SetEditorOnlyMessage(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        foreach (var json in jsonResult)
        {
            SetEditorOnlyMessage(json.Key + " / " + json.Value);
        }
        //object messageValue;
        //jsonResult.TryGetValue("OnCloudUpdateStats() messageValue", out messageValue);
        //SetEditorOnlyMessage((string)messageValue);

        //GetUserInventory();
    }


    IEnumerator LoadDataCoroutine()
    {
        if (infoText == null)
        {
            isActive = true;

            Debug.Log("튜토리얼 씬에서 로그인 완료");
            yield break;
        }

        playerData = false;
        statisticsData = false;
        inventoryData = false;

        infoText.text = LocalizationManager.instance.GetString("Loading");

        Debug.Log("Load Data...");

        playerDataBase.Initialize();
        shopDataBase.Initialize();

        GetPlayerNickName();

        //yield return new WaitForSeconds(0.5f);

        //yield return GetCatalog();

        //yield return new WaitForSeconds(0.5f);

        GetPlayerData();

        while(!playerData)
        {
            yield return null;
        }

        isActive = true;

        GetStatistics();

        while(!statisticsData)
        {
            yield return null;
        }

        GetUserInventory();

        while (!inventoryData)
        {
            yield return null;
        }


        uiManager.Renewal();

        Debug.Log("Load Data Complete");

        if(!GameStateManager.instance.Tutorial && playerDataBase.Formation == 0)
        {
            PlayerPrefs.SetString("LoadScene", "TutorialScene");
            SceneManager.LoadScene("LoadScene");
        }
        else
        {
            StateManager.instance.Initialize();
        }
    }

    public void GetUserInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            var Inventory = result.Inventory;
            int coinA = result.VirtualCurrency["GO"];
            int coinB = result.VirtualCurrency["GA"];
            int crystal = result.VirtualCurrency["ST"];
            int millage = result.VirtualCurrency["MG"];

            if (coinA < 0)
            {
                UpdateAddGold(Mathf.Abs(coinA));
            }

            if (coinA > 2000000000)
            {
                coinA = 2000000000;
            }

            if (coinB > 2000000000)
            {
                coinB = 2000000000;
            }

            if (crystal > 2000000000)
            {
                crystal = 2000000000;
            }

            if (millage > 999999999)
            {
                millage = 999999999;
            }

            playerDataBase.CoinA = coinA;
            playerDataBase.CoinB = coinB;
            playerDataBase.Crystal = crystal;
            playerDataBase.Millage = millage;

            playerDataBase.Initialize_BlockList();

            if (Inventory != null)
            {
                inventoryList.Clear();

                for (int i = 0; i < Inventory.Count; i++)
                {
                    inventoryList.Add(Inventory[i]);
                }

                foreach (ItemInstance list in inventoryList)
                {
                    for(int i = 0; i < Enum.GetValues(typeof(BlockType)).Length; i ++)
                    {
                        if(list.ItemId.Contains((BlockType.Default + i).ToString()))
                        {
                            if(list.CustomData == null)
                            {
                                SetInventoryCustomData(list.ItemInstanceId, defaultCustomData);

                                Debug.Log(list.ItemInstanceId + " 블럭이 초기화 되었습니다");
                            }

                            playerDataBase.SetBlock(list);
                        }
                    }

                    for (int i = 0; i < Enum.GetValues(typeof(TitleNormalType)).Length - 1; i++)
                    {
                        if (list.ItemId.Equals((TitleNormalType.Title1 + i).ToString()))
                        {
                            playerDataBase.SetNormalTitle(TitleNormalType.Title1 + i);
                        }
                    }

                    for (int i = 0; i < Enum.GetValues(typeof(TitleSpeicalType)).Length; i++)
                    {
                        if (list.ItemId.Equals((TitleSpeicalType.TitleSpeical1 + i).ToString()))
                        {
                            playerDataBase.SetSpeicalTitle(TitleSpeicalType.TitleSpeical1 + i);
                        }
                    }

                    //shopDataBase.SetItemInstanceId(list.ItemId, list.ItemInstanceId);
                }
            }

            inventoryData = true;

        }, DisplayPlayfabError);

    }

    public void ChangeUserInventory()
    {
        Debug.Log("인벤토리에서 유저 블럭을 다시 가져옵니다");

        collectionManager.change = true;

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            var Inventory = result.Inventory;

            inventoryList.Clear();

            if (Inventory != null)
            {
                for (int i = 0; i < Inventory.Count; i++)
                {
                    inventoryList.Add(Inventory[i]);
                }

                foreach (ItemInstance list in inventoryList)
                {
                    for (int i = 0; i < Enum.GetValues(typeof(BlockType)).Length; i++)
                    {
                        if (list.ItemId.Contains((BlockType.Default + i).ToString()))
                        {
                            //if (list.CustomData == null)
                            //{
                            //    SetInventoryCustomData(list.ItemInstanceId, defaultCustomData);

                            //    Debug.Log(list.ItemInstanceId + " 블럭이 초기화 되었습니다");
                            //}

                            playerDataBase.SetBlock(list);
                        }
                    }

                    //shopDataBase.SetItemInstanceId(list.ItemId, list.ItemInstanceId);
                }
            }

        }, DisplayPlayfabError);
    }

    public bool GetCatalog()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest() { CatalogVersion = "Shop" }, shop =>
        {
            for (int i = 0; i < shop.Catalog.Count; i++)
            {
                var catalog = shop.Catalog[i];

                ShopClass shopClass = new ShopClass();

                shopClass.catalogVersion = catalog.CatalogVersion;
                shopClass.itemClass = catalog.ItemClass;
                shopClass.itemId = catalog.ItemId;

                foreach (string item in catalog.VirtualCurrencyPrices.Keys)
                {
                    shopClass.virtualCurrency = item;
                }

                foreach (uint item in catalog.VirtualCurrencyPrices.Values)
                {
                    shopClass.price = item;
                }

                if (catalog.ItemId.Equals("RemoveAds"))
                {
                    shopDataBase.RemoveAds = shopClass;
                }

            }
        }, (error) =>
        {

        });

        return true;
    }

    public void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
           new GetPlayerStatisticsRequest(),
           (result) =>
           {
               foreach (var statistics in result.Statistics)
               {
                   switch (statistics.StatisticName)
                   {
                       //case "":
                       //    string text = statistics.Value.ToString();
                       //    break;
                       case "Formation":
                           playerDataBase.Formation = statistics.Value;

                           if(playerDataBase.Formation == 2)
                           {
                               GameStateManager.instance.WindCharacterType = WindCharacterType.UnderWorld;
                           }
                           else
                           {
                               GameStateManager.instance.WindCharacterType = WindCharacterType.Winter;
                           }

                           break;
                       case "ChallengeCount":
                           playerDataBase.ChallengeCount = statistics.Value;
                           break;
                       case "RankUpCount":
                           playerDataBase.RankUpCount = statistics.Value;
                           break;
                       case "TestAccount":
                           playerDataBase.TestAccount = statistics.Value;
                           break;
                       case "AttendanceDay":
                           playerDataBase.AttendanceDay = statistics.Value.ToString();
                           break;
                       case "NextMonday":
                           playerDataBase.NextMonday = statistics.Value.ToString();
                           break;
                       case "AttendanceCount":
                           playerDataBase.AttendanceCount = statistics.Value;
                           break;
                       case "AttendanceCheck":
                           if (statistics.Value == 0)
                           {
                               playerDataBase.AttendanceCheck = false;
                           }
                           else
                           {
                               playerDataBase.AttendanceCheck = true;
                           }
                           break;
                       case "WelcomeCount":
                           playerDataBase.WelcomeCount = statistics.Value;
                           break;
                       case "WelcomeCheck":
                           if (statistics.Value == 0)
                           {
                               playerDataBase.WelcomeCheck = false;
                           }
                           else
                           {
                               playerDataBase.WelcomeCheck = true;
                           }
                           break;
                       case "WelcomeBoxCount":
                           playerDataBase.WelcomeBoxCount = statistics.Value;
                           break;
                       case "WelcomeBoxCheck":
                           if (statistics.Value == 0)
                           {
                               playerDataBase.WelcomeBoxCheck = false;
                           }
                           else
                           {
                               playerDataBase.WelcomeBoxCheck = true;
                           }
                           break;
                       case "NewbieWin":
                           playerDataBase.NewbieWin = statistics.Value;
                           break;
                       case "NewbieLose":
                           playerDataBase.NewbieLose = statistics.Value;
                           break;
                       case "GosuWin":
                           playerDataBase.GosuWin = statistics.Value;
                           break;
                       case "GosuLose":
                           playerDataBase.GosuLose = statistics.Value;
                           break;
                       case "Emote1":
                           playerDataBase.Emote1 = statistics.Value;
                           break;
                       case "Emote2":
                           playerDataBase.Emote2 = statistics.Value;
                           break;
                       case "Emote3":
                           playerDataBase.Emote3 = statistics.Value;
                           break;
                       case "Emote4":
                           playerDataBase.Emote4 = statistics.Value;
                           break;
                       case "Emote5":
                           playerDataBase.Emote5 = statistics.Value;
                           break;
                       case "SnowBox":
                           playerDataBase.SnowBox = statistics.Value;
                           break;
                       case "SnowBox_N":
                           playerDataBase.SnowBox_N = statistics.Value;
                           break;
                       case "SnowBox_R":
                           playerDataBase.SnowBox_R = statistics.Value;
                           break;
                       case "SnowBox_SR":
                           playerDataBase.SnowBox_SR = statistics.Value;
                           break;
                       case "SnowBox_SSR":
                           playerDataBase.SnowBox_SSR = statistics.Value;
                           break;
                       case "SnowBox_UR":
                           playerDataBase.SnowBox_UR = statistics.Value;
                           break;
                       case "SnowBox_NR":
                           playerDataBase.SnowBox_NR = statistics.Value;
                           break;
                       case "SnowBox_RSR":
                           playerDataBase.SnowBox_RSR = statistics.Value;
                           break;
                       case "SnowBox_SRSSR":
                           playerDataBase.SnowBox_SRSSR = statistics.Value;
                           break;
                       case "UnderworldBox":
                           playerDataBase.UnderworldBox = statistics.Value;
                           break;
                       case "UnderworldBox_N":
                           playerDataBase.UnderworldBox_N = statistics.Value;
                           break;
                       case "UnderworldBox_R":
                           playerDataBase.UnderworldBox_R = statistics.Value;
                           break;
                       case "UnderworldBox_SR":
                           playerDataBase.UnderworldBox_SR = statistics.Value;
                           break;
                       case "UnderworldBox_SSR":
                           playerDataBase.UnderworldBox_SSR = statistics.Value;
                           break;
                       case "UnderworldBox_UR":
                           playerDataBase.UnderworldBox_UR = statistics.Value;
                           break;
                       case "UnderworldBox_NR":
                           playerDataBase.UnderworldBox_NR = statistics.Value;
                           break;
                       case "UnderworldBox_RSR":
                           playerDataBase.UnderworldBox_RSR = statistics.Value;
                           break;
                       case "UnderworldBox_SRSSR":
                           playerDataBase.UnderworldBox_SRSSR = statistics.Value;
                           break;
                       case "BoxPiece_N":
                           playerDataBase.BoxPiece_N = statistics.Value;
                           break;
                       case "BoxPiece_R":
                           playerDataBase.BoxPiece_R = statistics.Value;
                           break;
                       case "BoxPiece_SR":
                           playerDataBase.BoxPiece_SR = statistics.Value;
                           break;
                       case "BoxPiece_SSR":
                           playerDataBase.BoxPiece_SSR = statistics.Value;
                           break;
                       case "BoxPiece_UR":
                           playerDataBase.BoxPiece_UR = statistics.Value;
                           break;
                       case "BuySnowBox":
                           playerDataBase.BuySnowBox = statistics.Value;
                           break;
                       case "BuyUnderworldBox":
                           playerDataBase.BuyUnderworldBox = statistics.Value;
                           break;
                       case "BuySnowBoxSSRCount":
                           playerDataBase.BuySnowBoxSSRCount = statistics.Value;
                           break;
                       case "BuyUnderworldBoxSSRCount":
                           playerDataBase.BuyUnderworldBoxSSRCount = statistics.Value;
                           break;
                       case "UpgradeTicket":
                           playerDataBase.SetUpgradeTicket(RankType.N, statistics.Value);
                           break;
                       //case "UpgradeTicket_R":
                       //    playerDataBase.SetUpgradeTicket(RankType.R, statistics.Value);
                       //    break;
                       //case "UpgradeTicket_SR":
                       //    playerDataBase.SetUpgradeTicket(RankType.SR, statistics.Value);
                       //    break;
                       //case "UpgradeTicket_SSR":
                       //    playerDataBase.SetUpgradeTicket(RankType.SSR, statistics.Value);
                       //    break;
                       //case "UpgradeTicket_UR":
                       //    playerDataBase.SetUpgradeTicket(RankType.UR, statistics.Value);
                       //    break;
                       case "ShopNewbie":
                           playerDataBase.ShopNewbie = statistics.Value;
                           break;
                       case "ShopSliver":
                           playerDataBase.ShopSliver = statistics.Value;
                           break;
                       case "ShopGold":
                           playerDataBase.ShopGold = statistics.Value;
                           break;
                       case "ShopPlatinum":
                           playerDataBase.ShopPlatinum = statistics.Value;
                           break;
                       case "ShopDiamond":
                           playerDataBase.ShopDiamond = statistics.Value;
                           break;
                       case "ShopLegend":
                           playerDataBase.ShopLegend = statistics.Value;
                           break;
                       case "ShopSupply":
                           playerDataBase.ShopSupply = statistics.Value;
                           break;
                       case "DefDestroyTicket":
                           playerDataBase.DefDestroyTicket = statistics.Value;
                           break;
                       case "Star":
                           playerDataBase.Star = statistics.Value;
                           break;
                       case "PlayTime":
                           playerDataBase.PlayTime = statistics.Value;
                           break;
                       case "AdCount":
                           playerDataBase.AdCount = statistics.Value;
                           break;
                       case "NowRank":
                           playerDataBase.NowRank = statistics.Value;
                           GameStateManager.instance.GameRankType = GameRankType.Bronze_4 + playerDataBase.NowRank;
                           break;
                       case "HighRank":
                           playerDataBase.HighRank = statistics.Value;
                           break;
                       case "NewsAlarm":
                           playerDataBase.NewsAlarm = statistics.Value;
                           break;
                       case "DestroyBlockCount":
                           playerDataBase.DestroyBlockCount = statistics.Value;
                           break;
                       case "WinGetMoney":
                           playerDataBase.WinGetMoney = statistics.Value;
                           break;
                       case "TotalRaf":
                           playerDataBase.TotalRaf = statistics.Value;
                           break;
                       case "SynthesisGetBlock":
                           playerDataBase.SynthesisGetBlock = statistics.Value;
                           break;
                       case "RankDownCount":
                           playerDataBase.RankDownCount = statistics.Value;
                           break;
                       case "RankDownStreak":
                           playerDataBase.RankDownStreak = statistics.Value;
                           break;
                       case "WinNumber":
                           playerDataBase.WinNumber = statistics.Value;
                           break;
                       case "WinQueen":
                           playerDataBase.WinQueen = statistics.Value;
                           break;
                       case "GoalAchieveCount":
                           playerDataBase.GoalAchieveCount = statistics.Value;
                           break;
                       case "ChargingRM":
                           playerDataBase.ChargingRM = statistics.Value;
                           break;
                       case "BoxOpenCount":
                           playerDataBase.BoxOpenCount = statistics.Value;
                           break;
                       case "AccessDate":
                           playerDataBase.AccessDate = statistics.Value;
                           break;
                       case "UpgradeSuccessCount":
                           playerDataBase.UpgradeSuccessCount = statistics.Value;
                           break;
                       case "UpgradeFailCount":
                           playerDataBase.UpgradeFailCount = statistics.Value;
                           break;
                       case "UseUpgradeTicket":
                           playerDataBase.UseUpgradeTicket = statistics.Value;
                           break;
                       case "RepairBlockCount":
                           playerDataBase.RepairBlockCount = statistics.Value;
                           break;
                       case "ConsumeGold":
                           playerDataBase.ConsumeGold = statistics.Value;
                           break;
                       case "DailyWin":
                           playerDataBase.DailyWin = statistics.Value;
                           break;
                       case "DailyReward":
                           playerDataBase.DailyReward = statistics.Value;
                           break;
                       case "DailyBuy1":
                           playerDataBase.DailyBuy1 = statistics.Value;
                           break;
                       case "DailyBuy2":
                           playerDataBase.DailyBuy2 = statistics.Value;
                           break;
                       case "DailyBuyCount1":
                           playerDataBase.DailyBuyCount1 = statistics.Value;
                           break;
                       case "DailyBuyCount2":
                           playerDataBase.DailyBuyCount2 = statistics.Value;
                           break;
                       case "DailyNormalBox":
                           playerDataBase.DailyNormalBox = statistics.Value;
                           break;
                       case "DailyEpicBox":
                           playerDataBase.DailyEpicBox = statistics.Value;
                           break;
                       case "DailyNormalBox_1":
                           playerDataBase.DailyNormalBox_1 = statistics.Value;
                           break;
                       case "DailyNormalBox_10":
                           playerDataBase.DailyNormalBox_10 = statistics.Value;
                           break;
                       case "DailyEpicBox_1":
                           playerDataBase.DailyEpicBox_1 = statistics.Value;
                           break;
                       case "DailyEpicBox_10":
                           playerDataBase.DailyEpicBox_10 = statistics.Value;
                           break;
                       case "DailyAdsReward":
                           playerDataBase.DailyAdsReward = statistics.Value;
                           break;
                       case "DailyAdsReward2":
                           playerDataBase.DailyAdsReward2 = statistics.Value;
                           break;
                       case "DailyAdsReward3":
                           playerDataBase.DailyAdsReward3 = statistics.Value;
                           break;
                       case "DailyGoldReward":
                           playerDataBase.DailyGoldReward = statistics.Value;
                           break;
                       case "TitleNumber":
                           playerDataBase.TitleNumber = statistics.Value;
                           break;
                       case "ComicWorld2023":
                           playerDataBase.ComicWorld2023 = statistics.Value;
                           break;
                       case "IndieFestival2023":
                           playerDataBase.IndieFestival2023 = statistics.Value;
                           break;
                       case "NaverCafe202310":
                           playerDataBase.NaverCafe202310 = statistics.Value;
                           break;
                       case "NaverCafe202311":
                           playerDataBase.NaverCafe202311 = statistics.Value;
                           break;
                       case "NaverCafe202312":
                           playerDataBase.NaverCafe202312 = statistics.Value;
                           break;
                       case "NaverCafe202401":
                           playerDataBase.NaverCafe202401 = statistics.Value;
                           break;
                       case "NaverCafe202402":
                           playerDataBase.NaverCafe202402 = statistics.Value;
                           break;
                       case "Update":
                           playerDataBase.Update = statistics.Value;
                           break;
                   }
               }

               statisticsData = true;
           }
           , (error) =>
           {

           });
    }

    public void SetPlayerData(Dictionary<string, string> data)
    {
        try
        {
            if (NetworkConnect.instance.CheckConnectInternet())
            {
                if (!isActive) return;

                var request = new UpdateUserDataRequest() { Data = data, Permission = UserDataPermission.Public };
                try
                {
                    PlayFabClientAPI.UpdateUserData(request, (result) =>
                    {
                        Debug.Log("Update Player Data!");

                    }, DisplayPlayfabError);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.CheckInternet);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void GetPlayerData()
    {
        var request = new GetUserDataRequest() { PlayFabId = GameStateManager.instance.PlayfabId };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            foreach(var eachData in result.Data)
            {
                string key = eachData.Key;

                if (key.Contains("Armor"))
                {
                    playerDataBase.Armor = eachData.Value.Value;
                }

                if (key.Contains("Weapon"))
                {
                    playerDataBase.Weapon = eachData.Value.Value;
                }

                if (key.Contains("Shield"))
                {
                    playerDataBase.Shield = eachData.Value.Value;
                }

                if (key.Contains("NewBie"))
                {
                    playerDataBase.Newbie = eachData.Value.Value;
                }

                for(int i = 0; i < System.Enum.GetValues(typeof(AchievementType)).Length; i ++)
                {
                    if (key.Contains((AchievementType.AccessDate + i).ToString()))
                    {
                        playerDataBase.SetAchievementInfo((AchievementType)Enum.Parse(typeof(AchievementType), eachData.Key), int.Parse(eachData.Value.Value));
                    }
                }
            }

            playerData = true;

        }, DisplayPlayfabError);
    }

    public void GetPlayerProfile(string playFabId, Action<string> action)
    {
        string countryCode = "";

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowLocations = true
            }
        }, result =>
        {
            countryCode = result.PlayerProfile.Locations[0].CountryCode.Value.ToString();
            action?.Invoke(countryCode);

        }, error =>
        {
            action?.Invoke("");
        });
    }

    public void UpdatePlayerStatisticsInsert(string name, int value)
    {
        try
        {
            if (NetworkConnect.instance.CheckConnectInternet())
            {
                if (!isActive) return;

                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "UpdatePlayerStatistics",
                    FunctionParameter = new
                    {
                        Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName = name, Value = value}
                }
                    },
                    GeneratePlayStreamEvent = true,
                },
            result =>
            {
                OnCloudUpdateStats(result);
            }
            , DisplayPlayfabError);
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.CheckInternet);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

    }

    public void UpdateAddGold(int number)
    {
        moneyAnimation.PlusMoney(number);

        coin = playerDataBase.Coin;
        coinA = playerDataBase.CoinA;
        coinB = playerDataBase.CoinB;

        coin += number;

        coinB = coin / 100000000;
        coinA = coin - (coinB * 100000000);

        if (coinA > playerDataBase.CoinA)
        {
            UpdateAddCurrency(MoneyType.CoinA, (int)(coinA - playerDataBase.CoinA));
        }
        else if (coinA < playerDataBase.CoinA)
        {
            UpdateSubtractCurrency(MoneyType.CoinA, (int)(playerDataBase.CoinA - coinA));
        }

        if (coinB > playerDataBase.CoinB)
        {
            UpdateAddCurrency(MoneyType.CoinB, (int)(coinB - playerDataBase.CoinB));
        }
        else if (coinB < playerDataBase.CoinB)
        {
            UpdateSubtractCurrency(MoneyType.CoinB, (int)(playerDataBase.CoinB - coinB));
        }
    }

    public void UpdateSubtractGold(int number)
    {
        coin = playerDataBase.Coin;
        coinA = playerDataBase.CoinA;
        coinB = playerDataBase.CoinB;

        coin -= number;

        coinB = coin / 100000000;
        coinA = coin - (coinB * 100000000);

        if (coinA > playerDataBase.CoinA)
        {
            UpdateAddCurrency(MoneyType.CoinA, (int)(coinA - playerDataBase.CoinA));
        }
        else
        {
            UpdateSubtractCurrency(MoneyType.CoinA, (int)(playerDataBase.CoinA - coinA));
        }

        if (coinB > playerDataBase.CoinB)
        {
            UpdateAddCurrency(MoneyType.CoinB, (int)(coinB - playerDataBase.CoinB));
        }
        else
        {
            UpdateSubtractCurrency(MoneyType.CoinB, (int)(playerDataBase.CoinB - coinB));
        }


        consumeGoldA = GameStateManager.instance.ConsumeGold;

        consumeGoldA += number;

        if (consumeGoldA >= 1000000)
        {
            consumeGoldB = consumeGoldA / 1000000;

            consumeGoldA /= 1000000;

            playerDataBase.ConsumeGold += consumeGoldB;

            GameStateManager.instance.ConsumeGold = consumeGoldA;
            UpdatePlayerStatisticsInsert("ConsumeGold", playerDataBase.ConsumeGold);
        }
    }
    public void UpdateAddCurrency(MoneyType type, int number)
    {
        string currentType = "";

        switch (type)
        {
            case MoneyType.CoinA:
                currentType = "GO";
                break;
            case MoneyType.Crystal:
                currentType = "ST";
                break;
            case MoneyType.Millage:
                currentType = "MG";
                break;
            case MoneyType.CoinB:
                currentType = "GA";
                break;
        }

        if (NetworkConnect.instance.CheckConnectInternet())
        {
            try
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "AddMoney",
                    FunctionParameter = new { currencyType = currentType, currencyAmount = number },
                    GeneratePlayStreamEvent = true,
                }, OnCloudUpdateStats, DisplayPlayfabError);

                switch (type)
                {
                    case MoneyType.CoinA:
                        moneyAnimation.PlusMoney(number);

                        playerDataBase.CoinA += number;
                        break;
                    case MoneyType.Crystal:
                        playerDataBase.Crystal += number;
                        break;
                    case MoneyType.Millage:
                        playerDataBase.Millage += number;
                        break;
                    case MoneyType.CoinB:
                        playerDataBase.CoinB += number;
                        break;
                }

                uiManager.Renewal();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogError("Error : Internet Disconnected\nCheck Internet State");
        }

    }
    public void UpdateSubtractCurrency(MoneyType type, int number)
    {
        string currentType = "";

        switch (type)
        {
            case MoneyType.CoinA:
                currentType = "GO";
                break;
            case MoneyType.Crystal:
                currentType = "ST";
                break;
            case MoneyType.Millage:
                currentType = "MG";
                break;
            case MoneyType.CoinB:
                currentType = "GA";
                break;
        }

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.CheckInternet);

            return;
        }

        if (!isActive) return;

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SubtractMoney",
            FunctionParameter = new { currencyType = currentType, currencyAmount = number },
            GeneratePlayStreamEvent = true,
        }, OnCloudUpdateStats, DisplayPlayfabError);

        switch (type)
        {
            case MoneyType.CoinA:
                playerDataBase.CoinA -= number;
                playerDataBase.ConsumeGold += number;
                UpdatePlayerStatisticsInsert("ConsumeGold", playerDataBase.ConsumeGold);
                break;
            case MoneyType.Crystal:
                playerDataBase.Crystal -= number;
                break;
            case MoneyType.Millage:
                playerDataBase.Millage -= number;
                break;
            case MoneyType.CoinB:
                playerDataBase.CoinB -= number;
                break;
        }

        uiManager.Renewal();
    }

    public void UpdateDisplayName(string nickname, Action successAction, Action failAction)
    {
        try
        {
            if (NetworkConnect.instance.CheckConnectInternet())
            {
                if (!isActive) return;

                PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
                {
                    DisplayName = nickname
                },
        result =>
        {
            Debug.Log("Update NickName : " + result.DisplayName);

            GameStateManager.instance.NickName = result.DisplayName;
            successAction?.Invoke();
        }
        , error =>
        {
            string report = error.GenerateErrorReport();
            if (report.Contains("Name not available"))
            {
                failAction?.Invoke();
            }
            Debug.LogError(error.GenerateErrorReport());

            //NotionManager.instance.UseNotion(NotionType.NickNameNotion5);
        });
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.CheckInternet);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void UpdateDisplayName(string nickname)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nickname
        },
        result =>
        {
            Debug.Log("Update First NickName : " + result.DisplayName);

            GameStateManager.instance.NickName = result.DisplayName;
        }
        , error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }

    public void GetTitleInternalData(string name, Action<bool> action)
    {
        PlayFabServerAPI.GetTitleInternalData(new PlayFab.ServerModels.GetTitleDataRequest(),
            result =>
            {
                if (name.Equals("CheckAOSVersion"))
                {
                    if (result.Data[name].Equals("ON"))
                    {
                        action?.Invoke(true);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
                }
                if (name.Equals("CheckIOSVersion"))
                {
                    if (result.Data[name].Equals("ON"))
                    {
                        action?.Invoke(true);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
                }
                else if (name.Equals("AOSVersion") || name.Equals("IOSVersion"))
                {
                    if (result.Data[name].Equals(Application.version))
                    {
                        action?.Invoke(true);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
                }
                else if (name.Equals("Newbie") || name.Equals("Normal") || name.Equals("Rank"))
                {
                    if (result.Data[name].Equals("ON"))
                    {
                        action?.Invoke(true);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
                }
                else if (name.Equals("TestMode"))
                {
                    if (result.Data[name].Equals("ON"))
                    {
                        action?.Invoke(true);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
                }
                else if (name.Equals("Coupon"))
                {
                    if (result.Data[name].Equals("ON"))
                    {
                        action?.Invoke(true);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
                }
            },
            error =>
            {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());

                action?.Invoke(false);
            }
        );
    }

    public void GetTitleInternalData(string name, Action<string> action)
    {
        PlayFabServerAPI.GetTitleInternalData(new PlayFab.ServerModels.GetTitleDataRequest(),
            result =>
            {
                action(result.Data[name].ToString());
            },
            error =>
            {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }

    public void GetLeaderboarder(string name, Action<GetLeaderboardResult> successCalback)
    {
        var requestLeaderboard = new GetLeaderboardRequest
        {
            StartPosition = 0,
            StatisticName = name,
            MaxResultsCount = 100,

            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowLocations = true,
                ShowDisplayName = true,
                ShowStatistics = true
            }
        };

        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, successCalback, DisplayPlayfabError);
    }

    public void GetLeaderboardMyRank(string name, Action<GetLeaderboardAroundPlayerResult> successCalback)
    {
        var request = new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = name,
            MaxResultsCount = 1,
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, successCalback, DisplayPlayfabError);
    }


    public void SetProfileLanguage(string language)
    {
        EntityKey entity = new EntityKey();
        entity.Id = entityId;
        entity.Type = entityType;

        var request = new SetProfileLanguageRequest
        {
            Language = language,
            ExpectedVersion = 0,
            Entity = entity
        };
        PlayFabProfilesAPI.SetProfileLanguage(request, res =>
        {
            Debug.Log("The language on the entity's profile has been updated.");
        }, FailureCallback);
    }

    public void GetServerTime(Action<DateTime> action)
    {
        if (NetworkConnect.instance.CheckConnectInternet())
        {
            try
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "GetServerTime",
                    GeneratePlayStreamEvent = true,
                }, result =>
                {
                    string date = PlayFabSimpleJson.SerializeObject(result.FunctionResult);

                    string year = date.Substring(1, 4);
                    string month = date.Substring(6, 2);
                    string day = date.Substring(9, 2);
                    string hour = date.Substring(12, 2);
                    string minute = date.Substring(15, 2);
                    string second = date.Substring(18, 2);

                    DateTime serverTime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), 0, 0, 0);

                    serverTime = serverTime.AddDays(1);

                    DateTime time = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second));

                    TimeSpan span = serverTime - time;

                    action?.Invoke(DateTime.Parse(span.ToString()));
                }, DisplayPlayfabError);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogError("Error : Internet Disconnected\nCheck Internet State");
        }
    }

    //"2022-04-24T22:17:04.548Z"

    public void ReadTitleNews(Action<List<TitleNewsItem>> action)
    {
        List<TitleNewsItem> item = new List<TitleNewsItem>();

        PlayFabClientAPI.GetTitleNews(new GetTitleNewsRequest(), result =>
        {
            foreach (var list in result.News)
            {
                item.Add(list);
            }
            action.Invoke(item);

        }, error => Debug.LogError(error.GenerateErrorReport()));
    }


    #region PurchaseItem
    public void PurchaseCoin(int number)
    {
        UpdateAddCurrency(MoneyType.CoinA, number);

        //NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
    }

    public void PurchaseItemToRM(ShopClass shopClass)
    {
        //try
        //{
        //    PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        //    {
        //        FunctionName = "PurchaseItem",
        //        FunctionParameter = new {
        //            ItemId = shopClass.itemId,
        //            Price = (int)shopClass.price,
        //            VirtualCurrency = shopClass.virtualCurrency
        //        },
        //        GeneratePlayStreamEvent = true,
        //    }, OnCloudUpdateStats, DisplayPlayfabError);
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError(e.Message);
        //}

        //var request = new PurchaseItemRequest()
        //{
        //    CatalogVersion = shopClass.catalogVersion,
        //    ItemId = shopClass.itemId,
        //    VirtualCurrency = shopClass.virtualCurrency,
        //    Price = (int)shopClass.price
        //};
        //PlayFabClientAPI.PurchaseItem(request, (result) =>
        //{
        //    shopManager.CheckPurchaseItem();

        //    if (shopClass.itemId.Equals("PaidProgress"))
        //    {
        //        progressManager.BuyPaidProgress();
        //    }

        //    Debug.Log(shopClass.itemId + " Buy Success!");

        //    NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
        //}, error =>
        //{
        //    Debug.Log(shopClass.itemId + " Buy failed!");

        //    NotionManager.instance.UseNotion(NotionType.FailBuyItem);
        //});
    }

    public void PurchaseItem(ShopClass shopClass, Action<bool> action, int number)
    {
        bool failed = false;

        for (int i = 0; i < number; i++)
        {
            var request = new PurchaseItemRequest()
            {
                CatalogVersion = shopClass.catalogVersion,
                ItemId = shopClass.itemId,
                VirtualCurrency = shopClass.virtualCurrency,
                Price = (int)shopClass.price
            };

            PlayFabClientAPI.PurchaseItem(request, (result) =>
            {
                //switch (shopClass.itemId)
                //{
                //    case "Clock":
                //        playerDataBase.Clock += 1;
                //        break;
                //}

                switch (shopClass.virtualCurrency)
                {
                    case "GO":
                        playerDataBase.Coin -= (int)shopClass.price;
                        break;
                }
            }, error =>
            {
                failed = true;
            });

            if (failed)
            {
                action.Invoke(false);
                Debug.Log(shopClass.itemId + " Buy Failed!");
                break;
            }
        }

        //uiManager.RenewalVC();

        action.Invoke(true);
        Debug.Log(shopClass.itemId + " Buy Success!");
    }

    public void ConsumeItem(string itemInstanceID)
    {
        try
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "ConsumeItem",
                FunctionParameter = new { ConsumeCount = 1, ItemInstanceId = itemInstanceID },
                GeneratePlayStreamEvent = true,
            }, OnCloudUpdateStats, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void DeleteInventoryItem(string itemInstanceID)
    {
        try
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "DeleteInventoryItem",
                FunctionParameter = new { ItemInstanceId = itemInstanceID },
                GeneratePlayStreamEvent = true,
            }, OnCloudUpdateStats, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void DeleteInventoryItems(List<string> itemInstanceID)
    {
        try
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "DeleteInventoryItems",
                FunctionParameter = new { ItemsWantToDel = itemInstanceID },
                GeneratePlayStreamEvent = true,
            }, OnCloudUpdateStats, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    #endregion

    public void GrantItemToUser(string itemIds, string catalogVersion)
    {
        try
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "GrantItemToUser",
                FunctionParameter = new { ItemIds = itemIds, CatalogVersion = catalogVersion },
                GeneratePlayStreamEvent = true,
            }, OnCloudUpdateStats, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void GrantItemsToUser(string catalogversion, List<string> itemIds)
    {
        grantItemData = false;

        if (uiManager != null)
        {
            StartCoroutine(WaitGrantItemCoroution());
        }

        try
        {
            if (NetworkConnect.instance.CheckConnectInternet())
            {
                if (!isActive) return;

                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "GrantItemsToUser",
                    FunctionParameter = new { CatalogVersion = catalogversion, ItemIds = itemIds },
                    GeneratePlayStreamEvent = true,
                }
            , result =>
            {
                grantItemData = true;

                OnCloudUpdateStats(result);

            }, DisplayPlayfabError);
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.CheckInternet);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    IEnumerator WaitGrantItemCoroution()
    {
        while(!grantItemData)
        {
            yield return null;
        }

        ChangeUserInventory();
    }

    public void RestorePurchases()
    {
        if (isDelay) return;

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            var Inventory = result.Inventory;

            if (Inventory != null)
            {
                for (int i = 0; i < Inventory.Count; i++)
                {
                    inventoryList.Add(Inventory[i]);
                }

                foreach (ItemInstance list in inventoryList)
                {
                    //if (list.ItemId.Equals("RemoveAds"))
                    //{
                    //    playerDataBase.RemoveAd = true;

                    //    shopManager.CheckPurchaseItem();
                    //    profileManager.CheckPurchaseItem();
                    //}
                }
            }
            else
            {
                return;
            }

        }, DisplayPlayfabError);

        NotionManager.instance.UseNotion(NotionType.RestorePurchaseNotion);

        isDelay = true;
        Invoke("WaitDelay", 2f);
    }

    void WaitDelay()
    {
        isDelay = false;
    }

    public void SetInventoryCustomData(string itemInstanceID, Dictionary<string, string> datas)
    {
        try
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdateUserInventoryItemCustomData",
                FunctionParameter = new { Data = datas, ItemInstanceId = itemInstanceID },
                GeneratePlayStreamEvent = true,
            }, OnCloudUpdateStats, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
