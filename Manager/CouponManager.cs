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

        if(inputFieldText.text.Contains("-"))
        {
            inputFieldText.text.ToLower();

            GetCoupon();
        }
        else
        {
            switch(inputFieldText.text)
            {
                case "GRANDOPEN":
                    if (System.DateTime.Now >= new System.DateTime(2023, 10, 1))
                    {
                        if(playerDataBase.NaverCafe202310 == 0)
                        {
                            playerDataBase.NaverCafe202310 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202310", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2023, 11, 1))
                    {
                        if (playerDataBase.NaverCafe202311 == 0)
                        {
                            playerDataBase.NaverCafe202311 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202311", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2023, 12, 1))
                    {
                        if (playerDataBase.NaverCafe202312 == 0)
                        {
                            playerDataBase.NaverCafe202312 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202312", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 1, 1))
                    {
                        if (playerDataBase.NaverCafe202401 == 0)
                        {
                            playerDataBase.NaverCafe202401 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202401", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 2, 1))
                    {
                        if (playerDataBase.NaverCafe202402 == 0)
                        {
                            playerDataBase.NaverCafe202402 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202402", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 3, 1))
                    {
                        if (playerDataBase.NaverCafe202403 == 0)
                        {
                            playerDataBase.NaverCafe202403 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202403", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 4, 1))
                    {
                        if (playerDataBase.NaverCafe202404 == 0)
                        {
                            playerDataBase.NaverCafe202404 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202404", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 5, 1))
                    {
                        if (playerDataBase.NaverCafe202405 == 0)
                        {
                            playerDataBase.NaverCafe202405 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202405", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 6, 1))
                    {
                        if (playerDataBase.NaverCafe202406 == 0)
                        {
                            playerDataBase.NaverCafe202406 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202406", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 7, 1))
                    {
                        if (playerDataBase.NaverCafe202407 == 0)
                        {
                            playerDataBase.NaverCafe202407 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202407", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 8, 1))
                    {
                        if (playerDataBase.NaverCafe202408 == 0)
                        {
                            playerDataBase.NaverCafe202408 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202408", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 9, 1))
                    {
                        if (playerDataBase.NaverCafe202409 == 0)
                        {
                            playerDataBase.NaverCafe202409 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202409", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 10, 1))
                    {
                        if (playerDataBase.NaverCafe202410 == 0)
                        {
                            playerDataBase.NaverCafe202410 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202410", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 11, 1))
                    {
                        if (playerDataBase.NaverCafe202411 == 0)
                        {
                            playerDataBase.NaverCafe202411 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202411", 1);

                            NaverCafeCoupon();
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
                    if (System.DateTime.Now >= new System.DateTime(2024, 12, 1))
                    {
                        if (playerDataBase.NaverCafe202412 == 0)
                        {
                            playerDataBase.NaverCafe202412 = 1;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NaverCafe202412", 1);

                            NaverCafeCoupon();
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
    }

    void NaverCafeCoupon()
    {
        itemList.Clear();
        itemList.Add("Box_10");
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
