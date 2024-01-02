using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class SellManager : MonoBehaviour
{
    public GameObject sellView;

    public BlockUIContent blockUIContent;

    public Text titleText;
    public Text sellText;
    public Text sellPriceText;

    public Text sellButtonText;
    public Text cancleButtonText;

    private int price = 0;

    BlockClass blockClass;

    public CollectionManager collectionManager;
    public UpgradeManager upgradeManager;
    public EquipManager equipManager;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        sellView.SetActive(false);
    }

    public void OpenSellView(BlockClass block, int number)
    {
        if(!sellView.activeSelf)
        {
            sellView.SetActive(true);

            Initialize(block, number);

            FirebaseAnalytics.LogEvent("OpenSell");
        }
    }

    void Initialize(BlockClass block, int number)
    {
        blockClass = block;
        price = number;

        blockUIContent.Collection_Initialize(blockClass);
        sellPriceText.text = MoneyUnitString.ToCurrencyString(price);
    }

    public void SellButton()
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        upgradeManager.SellBlockOne(blockClass.instanceId);
        PlayfabManager.instance.UpdateAddGold(price);

        equipManager.CheckUnEquip(blockClass.instanceId);

        upgradeManager.CloseUpgradeView();
        CloseSellView();

        SoundManager.instance.PlaySFX(GameSfxType.SetBlock);

        NotionManager.instance.UseNotion(NotionType.SellBlock);

        FirebaseAnalytics.LogEvent("Sell");
    }
    public void CloseSellView()
    {
        sellView.SetActive(false);
    }
}
