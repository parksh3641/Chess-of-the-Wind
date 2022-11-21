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
    private float money = 0;
    private int bettingMoney = 0;
    private int saveBettingMoney = 0;
    private float getMoney = 0;

    private int targetNumber = 0;
    private int gridConstraintCount = 0;

    private int[] number = new int[2];

    [Title("Bool")]
    public bool blockDrag = false;


    public GameObject blockRootParent;
    public GameObject blockParent;

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

        money = setMoney;
        RenewalMoney();

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
            content.Initialize(this, blockRootParent.transform, BlockType.Default + i + 1);
            blockContentList.Add(content);
        }
    }

    private void Start()
    {
        timer = setTimer;
        timerFillAmount.fillAmount = 1;
        StartCoroutine(TimerCoroution());
    }

    void RenewalMoney()
    {
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

        timerFillAmount.fillAmount = timer / 15.0f;
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

        string str = "";

        targetText.color = new Color(0, 0, 0);
        str = "<color=#000000>" + targetNumber.ToString() + "</color>";

        if (targetNumber % 2 == 0)
        {
            targetText.color = new Color(1, 0, 0);
            str = "<color=#ff0000>" + targetNumber.ToString() + "</color>";
        }

        recordText.text += str + ", ";
        targetText.text = targetNumber.ToString();

        //CheckGame();

        timerText.text = "5초 뒤에 다시 시작합니다.";

        yield return new WaitForSeconds(5f);

        targetText.color = new Color(0, 0, 0);

        timerFillAmount.fillAmount = 1;
        timer = setTimer;
        StartCoroutine(TimerCoroution());
    }

    public void EnterBlock(RouletteContent content)
    {

    }

    public void ExitBlock(RouletteContent content)
    {

    }
}
