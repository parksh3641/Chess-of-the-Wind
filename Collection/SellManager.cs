using System.Collections;
using System.Collections.Generic;
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
        }
    }

    void Initialize(BlockClass block, int number)
    {
        blockClass = block;
        price = number;

        blockUIContent.Collection_Initialize(blockClass);

        titleText.text = "�Ǹ� �˸�";
        sellText.text = "�Ǹ� ����";

        sellPriceText.text = price.ToString();

        sellButtonText.text = "�Ǹ��ϱ�";
        cancleButtonText.text = "�׸��α�";
    }

    public void SellButton()
    {
        upgradeManager.SellBlockOne(blockClass.instanceId);
        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, price);

        upgradeManager.CloseUpgradeView();
        CloseSellView();

        NotionManager.instance.UseNotion(NotionType.SellBlock);
    }
    public void CloseSellView()
    {
        sellView.SetActive(false);
    }
}