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

                            itemList.Clear();
                            itemList.Add("BoxNR_5");
                            PlayfabManager.instance.GrantItemsToUser("Coupon", itemList);

                            SoundManager.instance.PlaySFX(GameSfxType.Success);
                            NotionManager.instance.UseNotion(NotionType.CouponNotion4);

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

                            itemList.Clear();
                            itemList.Add("BoxNR_5");
                            PlayfabManager.instance.GrantItemsToUser("Coupon", itemList);

                            SoundManager.instance.PlaySFX(GameSfxType.Success);
                            NotionManager.instance.UseNotion(NotionType.CouponNotion4);

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

                            itemList.Clear();
                            itemList.Add("BoxNR_5");
                            PlayfabManager.instance.GrantItemsToUser("Coupon", itemList);

                            SoundManager.instance.PlaySFX(GameSfxType.Success);
                            NotionManager.instance.UseNotion(NotionType.CouponNotion4);

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

                            itemList.Clear();
                            itemList.Add("BoxNR_5");
                            PlayfabManager.instance.GrantItemsToUser("Coupon", itemList);

                            SoundManager.instance.PlaySFX(GameSfxType.Success);
                            NotionManager.instance.UseNotion(NotionType.CouponNotion4);

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
