using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBoxManager : MonoBehaviour
{
    public GameObject boxView;

    public Text boxCountText;

    [Title("Content")]
    public BlockUIContent blockUIContent;

    public Transform blockUIContentTransform;

    List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();

    [Title("Box")]
    public GameObject boxPanel;

    public ButtonScaleAnimation boxAnim;

    public SoundManager soundManager;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        PlayerDataBase.eGetSnowBox += OpenSnowBoxView;
        PlayerDataBase.eGetUnderworldBox += OpenUnderworldBoxView;

        boxView.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        PlayerDataBase.eGetSnowBox -= OpenSnowBoxView;
        PlayerDataBase.eGetUnderworldBox -= OpenUnderworldBoxView;
    }

    public void Initialize()
    {
        for (int i = 0; i < 10; i++)
        {
            BlockUIContent monster = Instantiate(blockUIContent);
            monster.transform.parent = blockUIContentTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.gameObject.SetActive(false);

            blockUIContentList.Add(monster);
        }
    }

    public void OpenSnowBoxView()
    {
        if (!boxView.activeInHierarchy)
        {
            boxView.SetActive(true);

            InitializeSnowBox();
        }
    }

    public void OpenUnderworldBoxView()
    {
        if (!boxView.activeInHierarchy)
        {
            boxView.SetActive(true);

            InitializeUnderworldBox();
        }
    }

    public void CloseBoxView()
    {
        boxView.SetActive(false);
    }

    void InitializeSnowBox()
    {

    }

    void InitializeUnderworldBox()
    {

    }
}
