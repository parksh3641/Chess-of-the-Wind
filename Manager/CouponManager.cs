using Firebase.Analytics;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CouponManager : MonoBehaviour
{
    public GameObject couponView;

    public InputField inputFieldText;

    public MailBoxManager mailBoxManager;
    PlayerDataBase playerDataBase;

    List<string> itemList = new List<string>();

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        couponView.SetActive(false);
    }

    public void OpenCouponView()
    {
        if (!couponView.activeInHierarchy)
        {
            couponView.SetActive(true);

            inputFieldText.text = "";

        }
        else
        {
            couponView.SetActive(false);
        }
    }

    public void RedeemCouponCode()
    {
        if(inputFieldText.text.Length == 0)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.CouponNotion3);
            return;
        }

        switch (inputFieldText.text.ToUpper())
        {
            case "GRANDOPEN":
                if (System.DateTime.Now >= new System.DateTime(2024, 06, 25))
                {
                    if (playerDataBase.Coupon1 == 0)
                    {
                        playerDataBase.Coupon1 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon1", 1);

                        GrandOpenCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "WINDCHESS":
                if (System.DateTime.Now >= new System.DateTime(2024, 07, 1))
                {
                    if (playerDataBase.Coupon2 == 0)
                    {
                        playerDataBase.Coupon2 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon2", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "LILIA":
                if (System.DateTime.Now >= new System.DateTime(2024, 08, 1))
                {
                    if (playerDataBase.Coupon3 == 0)
                    {
                        playerDataBase.Coupon3 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon3", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "CORSIA":
                if (System.DateTime.Now >= new System.DateTime(2024, 09, 1))
                {
                    if (playerDataBase.Coupon4 == 0)
                    {
                        playerDataBase.Coupon4 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon4", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "THISMUCH":
                if (System.DateTime.Now >= new System.DateTime(2024, 10, 1))
                {
                    if (playerDataBase.Coupon5 == 0)
                    {
                        playerDataBase.Coupon5 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon5", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "BEGINNER":
                if (System.DateTime.Now >= new System.DateTime(2024, 11, 1))
                {
                    if (playerDataBase.Coupon6 == 0)
                    {
                        playerDataBase.Coupon6 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon6", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "BRAIN":
                if (System.DateTime.Now >= new System.DateTime(2024, 12, 1))
                {
                    if (playerDataBase.Coupon7 == 0)
                    {
                        playerDataBase.Coupon7 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon7", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "CONTROLLER":
                if (System.DateTime.Now >= new System.DateTime(2025, 01, 1))
                {
                    if (playerDataBase.Coupon8 == 0)
                    {
                        playerDataBase.Coupon8 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon8", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "FATE":
                if (System.DateTime.Now >= new System.DateTime(2025, 2, 1))
                {
                    if (playerDataBase.Coupon9 == 0)
                    {
                        playerDataBase.Coupon9 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon9", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "THEEND":
                if (System.DateTime.Now >= new System.DateTime(2025, 3, 1))
                {
                    if (playerDataBase.Coupon10 == 0)
                    {
                        playerDataBase.Coupon10 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon10", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "STRUGGLE":
                if (System.DateTime.Now >= new System.DateTime(2025, 4, 1))
                {
                    if (playerDataBase.Coupon11 == 0)
                    {
                        playerDataBase.Coupon11 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon11", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "BUTT":
                if (System.DateTime.Now >= new System.DateTime(2025, 5, 1))
                {
                    if (playerDataBase.Coupon12 == 0)
                    {
                        playerDataBase.Coupon12 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon12", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "GOALLOUT":
                if (System.DateTime.Now >= new System.DateTime(2025, 6, 1))
                {
                    if (playerDataBase.Coupon13 == 0)
                    {
                        playerDataBase.Coupon13 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon13", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "DNA":
                if (System.DateTime.Now >= new System.DateTime(2025, 7, 1))
                {
                    if (playerDataBase.Coupon14 == 0)
                    {
                        playerDataBase.Coupon14 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon14", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "ISTHISRIGHT":
                if (System.DateTime.Now >= new System.DateTime(2025, 8, 1))
                {
                    if (playerDataBase.Coupon15 == 0)
                    {
                        playerDataBase.Coupon15 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Coupon15", 1);

                        NormalCoupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            case "KGDCON2024":
                if (System.DateTime.Now >= new System.DateTime(2024, 8, 24))
                {
                    if (playerDataBase.Kgdcon2024 == 0)
                    {
                        playerDataBase.Kgdcon2024 = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Kgdcon2024", 1);

                        KgdCon2024Coupon();
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.CouponNotion2);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                }
                break;
            default:
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.CouponNotion3);
                break;
        }
    }

    void GrandOpenCoupon()
    {
        itemList.Clear();
        itemList.Add("Open_1");
        PlayfabManager.instance.GrantItemsToUser("Coupon", itemList);

        SoundManager.instance.PlaySFX(GameSfxType.Success);
        NotionManager.instance.UseNotion(NotionType.CouponNotion4);

        FirebaseAnalytics.LogEvent("Open_Coupon");
    }

    void KgdCon2024Coupon()
    {
        itemList.Clear();
        itemList.Add("Kgdcon2024_1");
        PlayfabManager.instance.GrantItemsToUser("Coupon", itemList);

        SoundManager.instance.PlaySFX(GameSfxType.Success);
        NotionManager.instance.UseNotion(NotionType.CouponNotion4);

        FirebaseAnalytics.LogEvent("Open_Kgdcon2024_Coupon");
    }

    void NormalCoupon()
    {
        itemList.Clear();
        itemList.Add("BoxNormal_10");
        PlayfabManager.instance.GrantItemsToUser("Coupon", itemList);

        SoundManager.instance.PlaySFX(GameSfxType.Success);
        NotionManager.instance.UseNotion(NotionType.CouponNotion4);

        FirebaseAnalytics.LogEvent("Open_Coupon");
    }


    void GetCoupon()
    {
        var primaryCatalogName = "Coupon"; // In your game, this should just be a constant matching your primary catalog
        var request = new RedeemCouponRequest
        {
            CatalogVersion = primaryCatalogName,
            CouponCode = inputFieldText.text // This comes from player input, in this case, one of the coupon codes generated above
        };
        PlayFabClientAPI.RedeemCoupon(request, OnCouponRedeemed, OnCouponRedeemError);
    }

    private void OnCouponRedeemed(RedeemCouponResult result)
    {
        if (result.Request != null)
        {
            Debug.Log("Coupon redeemed successfully!");

            mailBoxManager.GetUserInventoryCoupon();

            SoundManager.instance.PlaySFX(GameSfxType.Success);
            NotionManager.instance.UseNotion(NotionType.CouponNotion4);

            FirebaseAnalytics.LogEvent("Clear_Coupon");
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.CouponNotion3);
        }
    }

    private void OnCouponRedeemError(PlayFabError error)
    {
        Debug.LogError("Error redeeming coupon code: " + error.ErrorMessage);

        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
        NotionManager.instance.UseNotion(NotionType.CouponNotion3);
    }
}
