using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public BlockType blockType = BlockType.Default;
    Transform targetBlockContent;

    [Title("Developer")]
    public int autoTargetNumber = -1;
    public int setMoney = 50000;
    public int setTimer = 16;

    [Title("Setting")]
    public GridLayoutGroup gridLayoutGroup;

    [Title("Timer")]
    public Text timerText;
    public Image timerFillAmount;
    private int timer = 0;

    [Title("Text")]
    public Text moneyText;
    public Text targetText;
    public Text recordText;

    [Title("Value")]
    public float money = 0;
    private float bettingMoney = 0;
    private float getMoney = 0;

    private int targetNumber = 0;
    private int gridConstraintCount = 0;

    [Title("Drag")]
    private Transform dragPos;
    private bool checkDrag = false;

    [Title("Bool")]
    public bool blockDrag = false;
    public bool blockDrop = false;
    public bool blockOverlap = false;


    public GameObject blockRootParent;
    public GameObject blockParent;
    public GameObject blockGridParent;

    public GameObject dontTouchObj;
    public GameObject targetObj;

    [Title("Prefab")]
    public RouletteContent rouletteContent;
    public RectTransform rouletteContentTransform;

    public BlockContent blockContent;
    public RectTransform blockContentTransform;

    List<RouletteContent> rouletteContentList = new List<RouletteContent>();
    List<BlockContent> blockContentList = new List<BlockContent>();

    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;

        dontTouchObj.SetActive(false);
        targetObj.SetActive(false);

        ChangeMoney(setMoney);

        targetText.text = "?";
        recordText.text = "";

        gridConstraintCount = gridLayoutGroup.constraintCount;

        int index = 0;
        int count = 0;

        for (int i = 0; i < 25; i++)
        {
            int[] setIndex = new int[2];

            if (index >= gridConstraintCount)
            {
                index = 0;
                count++;
            }

            setIndex[0] = index;
            setIndex[1] = count;

            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = rouletteContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.StraightBet, setIndex, i);
            rouletteContentList.Add(content);

            index++;
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        {
            BlockContent content = Instantiate(blockContent);
            content.transform.parent = blockContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockRootParent.transform, blockGridParent.transform, BlockType.Default + i + 1);
            blockContentList.Add(content);
        }
    }

    private void Start()
    {
        timer = setTimer;
        timerFillAmount.fillAmount = 1;
        StartCoroutine(TimerCoroution());
    }

    void ChangeMoney(float plus)
    {
        money += plus;
        moneyText.text = "₩ " + money.ToString("N2");
    }

    IEnumerator TimerCoroution()
    {
        if (timer <= 0)
        {
            StartCoroutine(RandomTargetNumber());
            yield break;
        }

        targetText.text = "?";

        timer -= 1;

        timerFillAmount.fillAmount = timer / ((setTimer - 1) * 1.0f);
        timerText.text = timer.ToString();

        yield return new WaitForSeconds(1f);
        StartCoroutine(TimerCoroution());
    }

    IEnumerator RandomTargetNumber()
    {
        if (autoTargetNumber == -1)
        {
            targetNumber = Random.Range(1, rouletteContentList.Count + 1);
        }
        else
        {
            targetNumber = autoTargetNumber;
        }

        //string str = "";

        //targetText.color = new Color(0, 0, 0);
        //str = "<color=#000000>" + targetNumber.ToString() + "</color>";

        //if (targetNumber % 2 == 0)
        //{
        //    targetText.color = new Color(1, 0, 0);
        //    str = "<color=#ff0000>" + targetNumber.ToString() + "</color>";
        //}

        recordText.text += targetNumber + ", ";
        targetText.text = targetNumber.ToString();

        CheckTargetNumber();

        dontTouchObj.SetActive(true);

        timerText.text = "3초 뒤에 다시 시작합니다.";

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < blockContentList.Count; i++)
        {
            blockContentList[i].ResetPos();
        }

        for (int i = 0; i < rouletteContentList.Count; i++)
        {
            rouletteContentList[i].SetActiveFalse();
        }

        dontTouchObj.SetActive(false);
        targetObj.SetActive(false);

        //yield return new WaitForSeconds(3f);

        //targetText.color = new Color(0, 0, 0);

        timerFillAmount.fillAmount = 1;
        timer = setTimer;
        StartCoroutine(TimerCoroution());
    }

    private void CheckTargetNumber()
    {
        getMoney = 0;

        targetObj.SetActive(true);
        targetObj.transform.SetAsLastSibling();

        Transform trans = rouletteContentList[targetNumber - 1].transform;

        targetObj.transform.position = trans.position;

        for (int i = 0; i < rouletteContentList.Count; i ++)
        {
            if(rouletteContentList[i].number == targetNumber && rouletteContentList[i].isActive)
            {
                getMoney += 7000;
                ChangeMoney(getMoney);
                break;
            }
        }

        if(getMoney > 0)
        {
            NotionManager.instance.UseNotion(getMoney + " 만큼 돈을 땄어요 !", ColorType.Green);
        }
        else
        {
            if(bettingMoney == 0)
            {
                NotionManager.instance.UseNotion("시간 초과 !", ColorType.Red);
            }
            else
            {
                NotionManager.instance.UseNotion(bettingMoney + " 만큼 돈을 잃었어요 ㅠㅠ", ColorType.Red);
            }
        }

        bettingMoney = 0;
    }

    void Update()
    {
        if(blockDrag && targetBlockContent != null)
        {
            if(targetBlockContent.position.y > Screen.height * 0.5f + 450 || targetBlockContent.position.y < Screen.height * 0.5f - 650)
            {
                if (checkDrag)
                {
                    checkDrag = false;
                    ResetRouletteContent();
                }
            }
            else
            {
                if(!checkDrag) checkDrag = true;
            }

            if (targetBlockContent.position.x > Screen.width * 0.5f + 520 || targetBlockContent.position.x < Screen.width * 0.5f - 520)
            {
                if(checkDrag)
                {
                    checkDrag = false;
                    ResetRouletteContent();
                }
            }
            else
            {
                if (!checkDrag) checkDrag = true;
            }
        }
    }

    int[] index0 = new int[2];
    int[] index1 = new int[2];
    int[] index2 = new int[2];
    int[] index3 = new int[2];

    public void EnterBlock(RouletteContent rouletteContent, BlockContent blockContent)
    {
        targetBlockContent = blockContent.transform;

        for (int i = 0; i < rouletteContentList.Count; i ++)
        {
            if(rouletteContentList[i].blockType == blockContent.blockType)
            {
                rouletteContentList[i].SetActiveFalse();

                bettingMoney -= 1000;
                ChangeMoney(1000);
            }
        }

        index0 = new int[2];
        index1 = new int[2];
        index2 = new int[2];
        index3 = new int[2];

        switch (blockContent.blockType)
        {
            case BlockType.Default:
                break;
            case BlockType.I:
                index0[0] = rouletteContent.index[0];
                index0[1] = rouletteContent.index[1] - 2;

                index1[0] = rouletteContent.index[0];
                index1[1] = rouletteContent.index[1] - 1;

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0];
                index3[1] = rouletteContent.index[1] + 1;
                break;
            case BlockType.O:
                index0[0] = rouletteContent.index[0];
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] + 1;
                index1[1] = rouletteContent.index[1] - 1;

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];
                break;
            case BlockType.T:
                index0[0] = rouletteContent.index[0];
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] - 1;
                index1[1] = rouletteContent.index[1];

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];
                break;
            case BlockType.L:
                index0[0] = rouletteContent.index[0] + 1;
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] - 1;
                index1[1] = rouletteContent.index[1];

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];
                break;
            case BlockType.J:
                index0[0] = rouletteContent.index[0] - 1;
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] - 1;
                index1[1] = rouletteContent.index[1];

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];
                break;
            case BlockType.S:
                index0[0] = rouletteContent.index[0];
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] + 1;
                index1[1] = rouletteContent.index[1] - 1;

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] - 1;
                index3[1] = rouletteContent.index[1];
                break;
            case BlockType.Z:
                index0[0] = rouletteContent.index[0] - 1;
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0];
                index1[1] = rouletteContent.index[1] - 1;

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];
                break;
        }

        ResetRouletteContent();

        for (int i = 0; i < rouletteContentList.Count; i ++)
        {
            if(rouletteContentList[i].index.SequenceEqual(index0) || rouletteContentList[i].index.SequenceEqual(index1)
                || rouletteContentList[i].index.SequenceEqual(index2) || rouletteContentList[i].index.SequenceEqual(index3))
            {
                rouletteContentList[i].SetBackgroundColor(RouletteColorType.Yellow);
            }
        }

        if(index0[0] < 0 || index0[1] < 0 || index1[0] < 0 || index1[1] < 0 || index2[0] < 0 || index2[1] < 0 || index3[0] < 0 || index3[1] < 0
            || index0[0] >= gridConstraintCount || index0[1] >= gridConstraintCount || index1[0] >= gridConstraintCount || index1[1] >= gridConstraintCount
            || index2[0] >= gridConstraintCount || index2[1] >= gridConstraintCount || index3[0] >= gridConstraintCount || index3[1] >= gridConstraintCount)
        {
            blockDrop = false;
        }
        else
        {
            blockDrop = true;
        }

        //겹치는거 체크

        blockOverlap = false;

        for (int i = 0; i < rouletteContentList.Count; i++)
        {
            if (rouletteContentList[i].index.SequenceEqual(index0) || rouletteContentList[i].index.SequenceEqual(index1)
                || rouletteContentList[i].index.SequenceEqual(index2) || rouletteContentList[i].index.SequenceEqual(index3))
            {
                if (rouletteContentList[i].isActive) blockOverlap = true;
            }
        }

        numberCount = 0;
    }

    int[] numberArray = new int[4];
    int numberCount = 0;

    public void ExitBlock(BlockContent content)
    {
        ResetRouletteContent();

        for (int i = 0; i < rouletteContentList.Count; i++)
        {
            if (rouletteContentList[i].index.SequenceEqual(index0) || rouletteContentList[i].index.SequenceEqual(index1)
                || rouletteContentList[i].index.SequenceEqual(index2) || rouletteContentList[i].index.SequenceEqual(index3))
            {
                rouletteContentList[i].SetActiveTrue(content.blockType);

                numberArray[numberCount] = rouletteContentList[i].number;

                numberCount++;
            }
        }

        if(numberCount > 3)
        {
            content.SetNumber(numberArray);
        }

        bettingMoney += 4000;
        ChangeMoney(-4000);
    }


    public void ResetRouletteContent()
    {
        for (int i = 0; i < rouletteContentList.Count; i++)
        {
            rouletteContentList[i].ResetBackgroundColor();
        }
    }

    public void BetOptionCancleButton()
    {
        for (int i = 0; i < blockContentList.Count; i++)
        {
            blockContentList[i].ResetPos();
        }

        for (int i = 0; i < rouletteContentList.Count; i++)
        {
            rouletteContentList[i].SetActiveFalse();
        }

        ChangeMoney(bettingMoney);

        bettingMoney = 0;

        NotionManager.instance.UseNotion(NotionType.Cancle);
    }
}
