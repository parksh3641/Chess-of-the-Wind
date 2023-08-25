using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WindCharacterClass
{
    public WindCharacterType windCharacterType = WindCharacterType.Winter;
    public PresentType presentType = PresentType.A; //좋아하는 선물

    public int friendship_Level = 0; //호감도 레벨
    public int friendship_Exp = 0; //호감도 경험치

    public int storyProgress = 0; //스토리 진행도

    public int windlevel = 0; //레벨
    public int windPower = 0; //바람 세기

    public bool hidden = false;
}

[System.Serializable]
public class BlockClass
{
    public BlockType blockType = BlockType.Default;
    public RankType rankType = RankType.N;
    public string instanceId = "";

    public int level = 0;
    public int equipInfo = 0;
}

[System.Serializable]
public class WindCharacterUpgrade
{
    public int max = 99;

    public int gold = 0;
    public int addGold = 0;

    public int exp = 0; //레벨 업에 필요한 경험치
    public int addExp = 0;

    public int friendshipUnlock = 0; //다이아로 스토리 해금
    public int addFriendshipUnlock = 0;

    public float saleCardUpgradecost = 0; //카드 강화 비용 감소
}

[System.Serializable]
public class PresentClass
{
    public PresentType presentType = PresentType.A;

    public int holdNumber = 0;
}

[System.Serializable]
public class UpgradeTicketClass
{
    public RankType rankType = RankType.N;
    public int holdNumber = 0;
}


[CreateAssetMenu(fileName = "PlayerDataBase", menuName = "ScriptableObjects/PlayerDataBase")]
public class PlayerDataBase : ScriptableObject
{
    [Title("Money")]
    [SerializeField]
    private int gold = 0;
    [SerializeField]
    private int crystal = 0;

    [Space]
    [Title("User")]
    [SerializeField]
    private int formation = 0;
    [SerializeField]
    private int star = 0;
    [SerializeField]
    private int nowRank = 0;
    [SerializeField]
    private int highRank = 0;
    [SerializeField]
    private int accessDate = 0;
    [SerializeField]
    private int challengeCount = 0;

    [Space]
    [Title("Reset")]
    public int season = 0;
    public string attendanceDay = "";
    public int attendanceCount = 0;
    public bool attendanceCheck = false;
    [Space]
    public int welcomeCount = 0;
    public bool welcomeCheck = false;
    [Space]
    public string nextMonday = "";

    [Space]
    [Title("Info")]
    [SerializeField]
    private int newbieWin = 0;
    [SerializeField]
    private int newbieLose = 0;
    [SerializeField]
    private int gosuWin = 0;
    [SerializeField]
    private int gosuLose = 0;

    [Space]
    [Title("Equip")]
    [SerializeField]
    private string armor = "";
    [SerializeField]
    private string weapon = "";
    [SerializeField]
    private string shield = "";
    [SerializeField]
    private string newbie = "";

    [Space]
    [Title("Box_Snow")]
    [SerializeField]
    private int snowBox = 0;
    [SerializeField]
    private int snowBox_N = 0;
    [SerializeField]
    private int snowBox_R = 0;
    [SerializeField]
    private int snowBox_SR = 0;
    [SerializeField]
    private int snowBox_SSR = 0;
    [SerializeField]
    private int snowBox_UR = 0;
    [SerializeField]
    private int snowBox_NR = 0;
    [SerializeField]
    private int snowBox_RSR = 0;
    [SerializeField]
    private int snowBox_SRSSR = 0;

    [Space]
    [Title("Box_Under")]
    [SerializeField]
    private int underworldBox = 0;
    [SerializeField]
    private int underworldBox_N = 0;
    [SerializeField]
    private int underworldBox_R = 0;
    [SerializeField]
    private int underworldBox_SR = 0;
    [SerializeField]
    private int underworldBox_SSR = 0;
    [SerializeField]
    private int underworldBox_UR = 0;
    [SerializeField]
    private int underworldBox_NR = 0;
    [SerializeField]
    private int underworldBox_RSR = 0;
    [SerializeField]
    private int underworldBox_SRSSR = 0;

    [Space]
    [Title("Box_Piece")]
    [SerializeField]
    private int boxPiece_N = 0;
    [SerializeField]
    private int boxPiece_R = 0;
    [SerializeField]
    private int boxPiece_SR = 0;
    [SerializeField]
    private int boxPiece_SSR = 0;
    [SerializeField]
    private int boxPiece_UR = 0;

    [Space]
    [Title("Box Buy Count")]
    [SerializeField]
    private int buySnowBox = 0;
    [SerializeField]
    private int buyUnderworldBox = 0;
    [SerializeField]
    private int buySnowBoxSSRCount = 0;
    [SerializeField]
    private int buyUnderworldSSRCount = 0;

    [Space]
    [Title("Package")]
    [SerializeField]
    private int shopNewbie = 0;
    [SerializeField]
    private int shopSliver = 0;
    [SerializeField]
    private int shopGold = 0;
    [SerializeField]
    private int shopPlatinum = 0;
    [SerializeField]
    private int shopDiamond = 0;
    [SerializeField]
    private int shopLegend = 0;
    [SerializeField]
    private int shopSupply = 0;


    [Space]
    [Title("Wind Character")]
    [SerializeField]
    private List<WindCharacterClass> windCharacterList = new List<WindCharacterClass>();

    [Space]
    [Title("Block")]
    [SerializeField]
    private List<BlockClass> blockList = new List<BlockClass>();
    public List<BlockClass> successionLevel = new List<BlockClass>();
    public List<string> sellBlockList = new List<string>();

    [Space]
    [Title("Upgrade")]
    [SerializeField]
    private WindCharacterUpgrade windCharacterUpgrade;

    [Space]
    [Title("Present")]
    [SerializeField]
    private List<PresentClass> presentList = new List<PresentClass>();

    [Space]
    [Title("Upgrade")]
    [SerializeField]
    private List<UpgradeTicketClass> upgradeTicketList = new List<UpgradeTicketClass>();

    [Space]
    [Title("ETC")]
    [SerializeField]
    private int newsAlarm = 0;
    [SerializeField]
    private int defDestroyTicket = 0;

    Dictionary<string, string> levelCustomData = new Dictionary<string, string>();

    public delegate void BoxEvent();
    public static event BoxEvent eGetSnowBox, eGetSnowBox_N, eGetSnowBox_R, eGetSnowBox_SR, eGetSnowBox_SSR, eGetSnowBox_UR, eGetSnowBox_NR, eGetSnowBox_RSR, eGetSnowBox_SRSSR,
        eGetUnderworldBox, eGetUnderworldBox_N, eGetUnderworldBox_R, eGetUnderworldBox_SR, eGetUnderworldBox_SSR, eGetUnderworldBox_UR, eGetUnderworldBox_NR,
        eGetUnderworldBox_RSR, eGetUnderworldBox_SRSSR;

    #region Data

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
        }
    }

    public int Crystal
    {
        get
        {
            return crystal;
        }
        set
        {
            crystal = value;
        }
    }

    public int Formation
    {
        get
        {
            return formation;
        }
        set
        {
            formation = value;
        }
    }

    public int Star
    {
        get
        {
            return star;
        }
        set
        {
            star = value;
        }
    }

    public int NowRank
    {
        get
        {
            return nowRank;
        }
        set
        {
            nowRank = value;
        }
    }

    public int HighRank
    {
        get
        {
            return highRank;
        }
        set
        {
            highRank = value;
        }
    }

    public int AccessDate
    {
        get
        {
            return accessDate;
        }
        set
        {
            accessDate = value;
        }
    }

    public int ChallengeCount
    {
        get
        {
            return challengeCount;
        }
        set
        {
            challengeCount = value;
        }
    }

    public int Season
    {
        get
        {
            return season;
        }
        set
        {
            season = value;
        }
    }

    public string AttendanceDay
    {
        get
        {
            return attendanceDay;
        }
        set
        {
            attendanceDay = value;
        }
    }

    public int AttendanceCount
    {
        get
        {
            return attendanceCount;
        }
        set
        {
            attendanceCount = value;
        }
    }

    public bool AttendanceCheck
    {
        get
        {
            return attendanceCheck;
        }
        set
        {
            attendanceCheck = value;
        }
    }

    public string NextMonday
    {
        get
        {
            return nextMonday;
        }
        set
        {
            nextMonday = value;
        }
    }

    public int WelcomeCount
    {
        get
        {
            return welcomeCount;
        }
        set
        {
            welcomeCount = value;
        }
    }

    public bool WelcomeCheck
    {
        get
        {
            return welcomeCheck;
        }
        set
        {
            welcomeCheck = value;
        }
    }

    public int NewbieWin
    {
        get
        {
            return newbieWin;
        }
        set
        {
            newbieWin = value;
        }
    }

    public int NewbieLose
    {
        get
        {
            return newbieLose;
        }
        set
        {
            newbieLose = value;
        }
    }

    public int GosuWin
    {
        get
        {
            return gosuWin;
        }
        set
        {
            gosuWin = value;
        }
    }

    public int GosuLose
    {
        get
        {
            return gosuLose;
        }
        set
        {
            gosuLose = value;
        }
    }

    public string Armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
        }
    }

    public string Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
        }
    }

    public string Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
        }
    }

    public string Newbie
    {
        get
        {
            return newbie;
        }
        set
        {
            newbie = value;
        }
    }

    public int SnowBox
    {
        get
        {
            return snowBox;
        }
        set
        {
            snowBox = value;

            if (snowBox > 0)
            {
                eGetSnowBox();
            }
        }
    }

    public int SnowBox_N
    {
        get
        {
            return snowBox_N;
        }
        set
        {
            snowBox_N = value;

            if (snowBox_N > 0)
            {
                eGetSnowBox_N();
            }
        }
    }

    public int SnowBox_R
    {
        get
        {
            return snowBox_R;
        }
        set
        {
            snowBox_R = value;

            if (snowBox_R > 0)
            {
                eGetSnowBox_R();
            }
        }
    }

    public int SnowBox_SR
    {
        get
        {
            return snowBox_SR;
        }
        set
        {
            snowBox_SR = value;

            if (snowBox_SR > 0)
            {
                eGetSnowBox_SR();
            }
        }
    }

    public int SnowBox_SSR
    {
        get
        {
            return snowBox_SSR;
        }
        set
        {
            snowBox_SSR = value;

            if (snowBox_SSR > 0)
            {
                eGetSnowBox_SSR();
            }
        }
    }

    public int SnowBox_UR
    {
        get
        {
            return snowBox_UR;
        }
        set
        {
            snowBox_UR = value;

            if (snowBox_UR > 0)
            {
                eGetSnowBox_UR();
            }
        }
    }

    public int SnowBox_NR
    {
        get
        {
            return snowBox_NR;
        }
        set
        {
            snowBox_NR = value;

            if (snowBox_NR > 0)
            {
                eGetSnowBox_NR();
            }
        }
    }

    public int SnowBox_RSR
    {
        get
        {
            return snowBox_RSR;
        }
        set
        {
            snowBox_RSR = value;

            if (snowBox_RSR > 0)
            {
                eGetSnowBox_RSR();
            }
        }
    }

    public int SnowBox_SRSSR
    {
        get
        {
            return snowBox_SRSSR;
        }
        set
        {
            snowBox_SRSSR = value;

            if (snowBox_SRSSR > 0)
            {
                eGetSnowBox_SRSSR();
            }
        }
    }

    public int UnderworldBox
    {
        get
        {
            return underworldBox;
        }
        set
        {
            underworldBox = value;

            if (underworldBox > 0)
            {
                eGetUnderworldBox();
            }
        }
    }

    public int UnderworldBox_N
    {
        get
        {
            return underworldBox_N;
        }
        set
        {
            underworldBox_N = value;

            if (underworldBox_N > 0)
            {
                eGetUnderworldBox_N();
            }
        }
    }

    public int UnderworldBox_R
    {
        get
        {
            return underworldBox_R;
        }
        set
        {
            underworldBox_R = value;

            if (underworldBox_R > 0)
            {
                eGetUnderworldBox_R();
            }
        }
    }

    public int UnderworldBox_SR
    {
        get
        {
            return underworldBox_SR;
        }
        set
        {
            underworldBox_SR = value;

            if (underworldBox_SR > 0)
            {
                eGetUnderworldBox_SR();
            }
        }
    }

    public int UnderworldBox_SSR
    {
        get
        {
            return underworldBox_SSR;
        }
        set
        {
            underworldBox_SSR = value;

            if (underworldBox_SSR > 0)
            {
                eGetUnderworldBox_SSR();
            }
        }
    }

    public int UnderworldBox_UR
    {
        get
        {
            return underworldBox_UR;
        }
        set
        {
            underworldBox_UR = value;

            if (underworldBox_UR > 0)
            {
                eGetUnderworldBox_UR();
            }
        }
    }

    public int UnderworldBox_NR
    {
        get
        {
            return underworldBox_NR;
        }
        set
        {
            underworldBox_NR = value;

            if (underworldBox_NR > 0)
            {
                eGetUnderworldBox_NR();
            }
        }
    }

    public int UnderworldBox_RSR
    {
        get
        {
            return underworldBox_RSR;
        }
        set
        {
            underworldBox_RSR = value;

            if (underworldBox_RSR > 0)
            {
                eGetUnderworldBox_RSR();
            }
        }
    }

    public int UnderworldBox_SRSSR
    {
        get
        {
            return underworldBox_SRSSR;
        }
        set
        {
            underworldBox_SRSSR = value;

            if (underworldBox_SRSSR > 0)
            {
                eGetUnderworldBox_SRSSR();
            }
        }
    }


    public int BoxPiece_N
    {
        get
        {
            return boxPiece_N;
        }
        set
        {
            boxPiece_N = value;
        }
    }

    public int BoxPiece_R
    {
        get
        {
            return boxPiece_R;
        }
        set
        {
            boxPiece_R = value;
        }
    }

    public int BoxPiece_SR
    {
        get
        {
            return boxPiece_SR;
        }
        set
        {
            boxPiece_SR = value;
        }
    }

    public int BoxPiece_SSR
    {
        get
        {
            return boxPiece_SSR;
        }
        set
        {
            boxPiece_SSR = value;
        }
    }

    public int BoxPiece_UR
    {
        get
        {
            return boxPiece_UR;
        }
        set
        {
            boxPiece_UR = value;
        }
    }

    public int BuySnowBox
    {
        get
        {
            return buySnowBox;
        }
        set
        {
            buySnowBox = value;
        }
    }
    public int BuyUnderworldBox
    {
        get
        {
            return buyUnderworldBox;
        }
        set
        {
            buyUnderworldBox = value;
        }
    }

    public int BuySnowBoxSSRCount
    {
        get
        {
            return buySnowBoxSSRCount;
        }
        set
        {
            buySnowBoxSSRCount = value;
        }
    }

    public int BuyUnderworldBoxSSRCount
    {
        get
        {
            return buyUnderworldSSRCount;
        }
        set
        {
            buyUnderworldSSRCount = value;
        }
    }

    public int ShopNewbie
    {
        get
        {
            return shopNewbie;
        }
        set
        {
            shopNewbie = value;
        }
    }

    public int ShopSliver
    {
        get
        {
            return shopSliver;
        }
        set
        {
            shopNewbie = value;
        }
    }

    public int ShopGold
    {
        get
        {
            return shopGold;
        }
        set
        {
            shopGold = value;
        }
    }

    public int ShopPlatinum
    {
        get
        {
            return shopPlatinum;
        }
        set
        {
            shopPlatinum = value;
        }
    }

    public int ShopDiamond
    {
        get
        {
            return shopDiamond;
        }
        set
        {
            shopDiamond = value;
        }
    }

    public int ShopLegend
    {
        get
        {
            return shopLegend;
        }
        set
        {
            shopLegend = value;
        }
    }

    public int ShopSupply
    {
        get
        {
            return shopSupply;
        }
        set
        {
            shopSupply = value;
        }
    }

    public int NewsAlarm
    {
        get
        {
            return newsAlarm;
        }
        set
        {
            newsAlarm = value;
        }
    }

    public int DefDestroyTicket
    {
        get
        {
            return defDestroyTicket;
        }
        set
        {
            defDestroyTicket = value;
        }
    }

    #endregion

    public void Initialize()
    {
        gold = 0;
        crystal = 0;

        formation = 0;
        star = 0;
        nowRank = 0;
        highRank = 0;
        accessDate = 0;
        ChallengeCount = 0;

        season = 0;
        attendanceDay = "";
        attendanceCount = 0;
        attendanceCheck = false;
        nextMonday = "";

        welcomeCount = 0;
        welcomeCheck = false;

        newbieWin = 0;
        newbieLose = 0;
        gosuWin = 0;
        gosuLose = 0;

        armor = "";
        weapon = "";
        shield = "";
        newbie = "";

        snowBox = 0;
        snowBox_N = 0;
        snowBox_R = 0;
        snowBox_SR = 0;
        snowBox_SSR = 0;
        snowBox_UR = 0;
        snowBox_NR = 0;
        snowBox_RSR = 0;
        snowBox_SRSSR = 0;

        underworldBox = 0;
        underworldBox_N = 0;
        underworldBox_R = 0;
        underworldBox_SR = 0;
        underworldBox_SSR = 0;
        underworldBox_UR = 0;
        underworldBox_NR = 0;
        underworldBox_RSR = 0;
        underworldBox_SRSSR = 0;

        boxPiece_N = 0;
        boxPiece_R = 0;
        boxPiece_SR = 0;
        boxPiece_SSR = 0;
        boxPiece_UR = 0;

        BuySnowBox = 0;
        BuyUnderworldBox = 0;
        BuySnowBoxSSRCount = 0;
        BuyUnderworldBoxSSRCount = 0;

        shopNewbie = 0;
        shopSliver = 0;
        shopGold = 0;
        shopPlatinum = 0;
        shopDiamond = 0;
        shopLegend = 0;
        shopSupply = 0;

        newsAlarm = 0;
        defDestroyTicket = 0;

        windCharacterList.Clear();

        for(int i = 0; i < System.Enum.GetValues(typeof(WindCharacterType)).Length; i ++)
        {
            WindCharacterClass content = new WindCharacterClass();
            content.windCharacterType = WindCharacterType.Winter + i;
            windCharacterList.Add(content);
        }

        blockList.Clear();
        sellBlockList.Clear();
        successionLevel.Clear();

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length; i++)
        //{
        //    BlockClass content = new BlockClass();
        //    content.blockType = BlockType.Default + i + 1;
        //    blockList.Add(content);
        //}

        presentList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(PresentType)).Length; i++)
        {
            PresentClass content = new PresentClass();
            content.presentType = PresentType.A + i;
            presentList.Add(content);
        }

        upgradeTicketList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(RankType)).Length; i++)
        {
            UpgradeTicketClass content = new UpgradeTicketClass();
            content.rankType = RankType.N + i;
            upgradeTicketList.Add(content);
        }
    }

    public void Initialize_BlockList()
    {
        blockList = new List<BlockClass>();
    }

    public void SetBlock(ItemInstance item)
    {
        for(int i = 0; i < blockList.Count; i ++)
        {
            if(item.ItemInstanceId.Equals(blockList[i].instanceId))
            {
                return;
            }
        }

        //for(int i = 0; i < sellBlockList.Count; i ++)
        //{
        //    if(item.ItemInstanceId.Equals(sellBlockList[i]))
        //    {
        //        return;
        //    }
        //}

        BlockClass blockClass = new BlockClass();

        blockClass.blockType = (BlockType)Enum.Parse(typeof(BlockType), item.DisplayName.ToString());

        switch (item.ItemClass)
        {
            case "UR":
                blockClass.rankType = RankType.UR;
                break;
            case "SSR":
                blockClass.rankType = RankType.SSR;
                break;
            case "SR":
                blockClass.rankType = RankType.SR;
                break;
            case "R":
                blockClass.rankType = RankType.R;
                break;
            case "N":
                blockClass.rankType = RankType.N;
                break;
            default:
                blockClass.rankType = RankType.N;
                break;

        }

        blockClass.instanceId = item.ItemInstanceId;

        if(item.CustomData != null)
        {
            blockClass.level = int.Parse(item.CustomData["Level"]);
        }

        for (int i = 0; i < successionLevel.Count; i++) //레벨 계승
        {
            if(blockClass.level == 0 && 
                blockClass.blockType.Equals(successionLevel[i].blockType) && 
                blockClass.rankType.Equals(successionLevel[i].rankType))
            {
                levelCustomData.Clear();
                levelCustomData.Add("Level", successionLevel[i].level.ToString());

                PlayfabManager.instance.SetInventoryCustomData(blockClass.instanceId, levelCustomData);
                blockClass.level = successionLevel[i].level;

                if (successionLevel[i].equipInfo == 1)
                {
                    armor = blockClass.instanceId;

                    Debug.Log("아머로 장비가 계승되었습니다");
                }
                else if (successionLevel[i].equipInfo == 2)
                {
                    weapon = blockClass.instanceId;

                    Debug.Log("검으로 장비가 계승되었습니다");
                }
                else if (successionLevel[i].equipInfo == 3)
                {
                    shield = blockClass.instanceId;

                    Debug.Log("쉴드로 장비가 계승되었습니다");
                }
                else if (successionLevel[i].equipInfo == 4)
                {
                    newbie = blockClass.instanceId;

                    Debug.Log("뉴비로 장비가 계승되었습니다");
                }

                Debug.LogError(blockClass.blockType + "_" + blockClass.rankType + " 가 " + (successionLevel[i].level + 1) + " 레벨로 계승되었습니다");

                successionLevel.RemoveAt(i);
            }
        }

        //Debug.Log(blockClass.blockType + "_" + blockClass.rankType + " 블럭이 추가되었습니다");

        blockList.Add(blockClass);
    }

    public void SetBlockLevel(string id, int level)
    {
        for(int i = 0; i < blockList.Count; i ++)
        {
            if(blockList[i].instanceId.Equals(id))
            {
                blockList[i].level = level;
                break;
            }
        }
    }

    public void SellBlock(string id)
    {
        sellBlockList.Add(id);
    }

    public bool CheckSellBlock(string id)
    {
        bool check = false;

        for(int i = 0; i < sellBlockList.Count; i ++)
        {
            if(sellBlockList[i].Equals(id))
            {
                check = true;
                break;
            }
        }

        return check;
    }

    public List<BlockClass> GetBlockClass()
    {
        return blockList;
    }

    public BlockClass GetBlockClass(string id)
    {
        BlockClass blockClass = new BlockClass();

        for(int i = 0; i < blockList.Count; i ++)
        {
            if(blockList[i].instanceId.Equals(id))
            {
                blockClass = blockList[i];
                break;
            }
        }

        return blockClass;
    }

    public int CheckEquip(string id)
    {
        int number = 0;

        if(armor != null)
        {
            if (armor.Equals(id))
            {
                number = 1;

                Debug.Log("장착 중인 아머가 선택되었습니다");
            }
        }

        if (weapon != null)
        {
            if (weapon.Equals(id))
            {
                number = 2;

                Debug.Log("장착 중인 검이 선택되었습니다");
            }
        }

        if (shield != null)
        {
            if (shield.Equals(id))
            {
                number = 3;

                Debug.Log("장착 중인 쉴드가 선택되었습니다");
            }
        }

        if (newbie != null)
        {
            if (newbie.Equals(id))
            {
                number = 4;

                Debug.Log("장착 중인 뉴비가 선택되었습니다");
            }
        }

        return number;
    }

    public void CheckUnEquip(string id)
    {
        if(armor != null)
        {
            if (armor.Equals(id))
            {
                armor = "";

                Debug.Log("장착 중인 아머가 해제되었습니다");
            }
        }

        if (weapon != null)
        {
            if (weapon.Equals(id))
            {
                weapon = "";

                Debug.Log("장착 중인 검이 해제되었습니다");
            }
        }

        if (shield != null)
        {
            if (shield.Equals(id))
            {
                shield = "";

                Debug.Log("장착 중인 쉴드가 해제되었습니다");
            }
        }

        if (newbie != null)
        {
            if (newbie.Equals(id))
            {
                newbie = "";

                Debug.Log("장착 중인 뉴비가 해제되었습니다");
            }
        }
    }

    public int CheckOverlapBlock(BlockClass block)
    {
        int index = 0;

        if(armor != null)
        {
            if (GetBlockClass(armor).blockType.Equals(block.blockType))
            {
                index = 1;
            }
        }

        if (weapon != null)
        {
            if (GetBlockClass(weapon).blockType.Equals(block.blockType))
            {
                index = 2;
            }
        }

        if (shield != null)
        {
            if (GetBlockClass(shield).blockType.Equals(block.blockType))
            {
                index = 3;
            }
        }

        return index;
    }

    public void SetSuccessionLevel(BlockClass block)
    {
        successionLevel.Add(block);

        Debug.Log("계승 정보를 저장했습니다");
    }

    #region Ticket
    public void SetUpgradeTicket(RankType type, int number)
    {
        for(int i = 0; i < upgradeTicketList.Count; i ++)
        {
            if(upgradeTicketList[i].rankType.Equals(type))
            {
                upgradeTicketList[i].holdNumber += number;
                break;
            }
        }
    }

    public int GetUpgradeTicket(RankType type)
    {
        int ticket = 0;
        for (int i = 0; i < upgradeTicketList.Count; i++)
        {
            if (upgradeTicketList[i].rankType.Equals(type))
            {
                ticket = upgradeTicketList[i].holdNumber;
                break;
            }
        }
        return ticket;
    }

    public void UseUpgradeTicket(RankType type, int number)
    {
        for (int i = 0; i < upgradeTicketList.Count; i++)
        {
            if (upgradeTicketList[i].rankType.Equals(type))
            {
                upgradeTicketList[i].holdNumber -= number;
                break;
            }
        }
    }
    #endregion

    public int CheckEquipBlock_Gosu()
    {
        int index = 0;

        if (armor != null)
        {
            if(armor.Length > 0)
            {
                index++;
            }
        }

        if (weapon != null)
        {
            if (weapon.Length > 0)
            {
                index++;
            }
        }

        if (shield != null)
        {
            if (shield.Length > 0)
            {
                index++;
            }
        }

        return index;
    }

    public bool CheckEquipBlock_Newbie()
    {
        bool check = true;

        if(newbie == null)
        {
            check = false;
        }
        else
        {
            if (newbie.Length == 0)
            {
                check = false;
            }
        }


        return check;
    }

    public bool CheckBlockLevel(int level)
    {
        bool check = false;

        for(int i = 0; i < blockList.Count; i ++)
        {
            if(blockList[i].level > level)
            {
                check = true;
                break;
            }
        }

        return check;
    }

    public int CheckBlockLevelCount()
    {
        int number = 0;

        if (armor != null)
        {
            if (GetBlockClass(armor).level > 0)
            {
                number++;

                Debug.Log("1");
            }
        }

        if (weapon != null)
        {
            if (GetBlockClass(weapon).level > 0)
            {
                number++;

                Debug.Log("2");
            }
        }

        if (shield != null)
        {
            if (GetBlockClass(shield).level > 0)
            {
                number++;

                Debug.Log("3");
            }
        }

        return number;
    }
}