using Firebase.Analytics;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxManager : MonoBehaviour
{
    public GameObject mailView;

    public GameObject alarm;
    public GameObject mainAlarm;

    public GameObject noMailObj;

    public MailContent mailContent;
    public RectTransform mailTransform;


    private bool firstLoad = false;

    public int count = 0;

    List<MailContent> mailContentList = new List<MailContent>();

    List<string> bundleItemIdList = new List<string>();
    List<string> bundleInstanceIdList = new List<string>();

    Dictionary<string, string> refreshCustom = new Dictionary<string, string>() { { "Receive", "0" } };
    Dictionary<string, string> inputCustom = new Dictionary<string, string>() { { "Receive", "1" } };
    List<string> rewardID = new List<string>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        mailView.SetActive(false);

        alarm.SetActive(false);
        mainAlarm.SetActive(false);
        noMailObj.SetActive(false);
    }

    public void Initialize()
    {
        for (int i = 0; i < 20; i++)
        {
            MailContent content = Instantiate(mailContent);
            content.transform.SetParent(mailTransform);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            mailContentList.Add(content);
        }

        mailTransform.anchoredPosition = new Vector2(0, -999);

        GetUserInventoryCoupon();
    }

    public void OpenMail()
    {
        if (!mailView.activeSelf)
        {
            mailView.SetActive(true);

            if (!firstLoad)
            {
                firstLoad = true;

                CheckMailBox();
            }
            else
            {
                bundleItemIdList.Clear();
                bundleInstanceIdList.Clear();

                count = 0;

                for(int i = 0; i < mailContentList.Count; i ++)
                {
                    mailContentList[i].gameObject.SetActive(false);
                }

                GetUserInventoryCoupon();
            }

            mailTransform.anchoredPosition = new Vector2(0, -999);

            FirebaseAnalytics.LogEvent("MailBox");
        }
        else
        {
            mailView.SetActive(false);
        }
    }

    public void AddBundleContent(string itemid, string instanceid)
    {
        bundleItemIdList.Add(itemid);
        bundleInstanceIdList.Add(instanceid);

        alarm.SetActive(true);
        mainAlarm.SetActive(true);

        count++;
    }

    void CheckMailBox()
    {
        if (bundleItemIdList.Count <= 0)
        {
            noMailObj.SetActive(true);
            alarm.SetActive(false);
            mainAlarm.SetActive(false);
            return;
        }

        noMailObj.SetActive(false);

        for (int i = 0; i < bundleItemIdList.Count; i ++)
        {
            int temp = i;
            mailContentList[i].gameObject.SetActive(true);
            mailContentList[i].Initialize(bundleItemIdList[i]);
            mailContentList[i].unityEvent.RemoveAllListeners();
            mailContentList[i].unityEvent.AddListener(() => ReceiveButton(bundleItemIdList[temp], bundleInstanceIdList[temp]));
        }
    }

    void ReceiveButton(string itemid, string instanceid)
    {
        rewardID.Clear();

        SetInventoryCustomData(instanceid, inputCustom);

        count -= 1;

        if(count == 0)
        {
            noMailObj.SetActive(true);
            alarm.SetActive(false);
            mainAlarm.SetActive(false);
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.GetReward);
    }

    #region Message
    private void SetEditorOnlyMessage(string message, bool error = false)
    {
#if UNITY_EDITOR
        if (error) Debug.LogError("<color=red>" + message + "</color>");
        else Debug.Log(message);
#endif
    }
    private void DisplayPlayfabError(PlayFabError error) => SetEditorOnlyMessage("error : " + error.GenerateErrorReport(), true);

    #endregion

    public void GetUserInventoryCoupon()
    {
        bundleItemIdList.Clear();
        bundleInstanceIdList.Clear();

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            var coupon = result.Inventory;

            for (int i = 0; i < coupon.Count; i++)
            {
                var list = coupon[i];

                if (list.ItemClass.Equals("Coupon"))
                {
                    if (list.CustomData == null)
                    {
                        SetInventoryCustomData(list.ItemInstanceId, refreshCustom);

                        AddBundleContent(list.ItemId, list.ItemInstanceId);
                    }
                    else
                    {
                        if (int.Parse(list.CustomData["Receive"]) == 0)
                        {
                            AddBundleContent(list.ItemId, list.ItemInstanceId);
                        }
                    }
                }
            }

            if (firstLoad) CheckMailBox();

        }, DisplayPlayfabError);
    }

    public void GrantItemToUser(string catalogversion, List<string> itemIds)
    {
        try
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "GrantItemToUser",
                FunctionParameter = new { CatalogVersion = catalogversion, ItemIds = itemIds },
                GeneratePlayStreamEvent = true,
            }, OnCloudUpdateStats, DisplayPlayfabError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
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

    private void OnCloudUpdateStats(ExecuteCloudScriptResult result)
    {
        SetEditorOnlyMessage(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        foreach (var json in jsonResult)
        {
            SetEditorOnlyMessage(json.Key + " / " + json.Value);
        }
        object messageValue;
        jsonResult.TryGetValue("OnCloudUpdateStats() messageValue", out messageValue);
        SetEditorOnlyMessage((string)messageValue);

        //GetUserInventory();
    }
}