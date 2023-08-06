using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FormationManager : MonoBehaviour
{
    public GameObject formationView;

    public GameObject warningView;

    public GameObject animationView;
    public Text animationText;
    public Image icon;

    public Sprite[] iconArray;

    public Text warningText;

    private int selectNumber = 0;
    bool isWait = false;

    List<string> itemList = new List<string>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        formationView.SetActive(false);
        warningView.SetActive(false);
        animationView.SetActive(false);
    }

    public void Initialize()
    {
        formationView.SetActive(true);
    }

    public void SelectFormationButton(int number) //진형 선택
    {
        OpenWarningView(number);
    }

    void OpenWarningView(int number)
    {
        selectNumber = number;

        warningView.SetActive(true);

        if (selectNumber == 0)
        {
            warningText.text = LocalizationManager.instance.GetString("Formation_Winter");
        }
        else
        {
            warningText.text = LocalizationManager.instance.GetString("Formation_Under");
        }
    }
    public void SelectedFormation()
    {
        if (isWait) return;

        isWait = true;

        StartCoroutine(SelectedFormationCoroution());
    }

    IEnumerator SelectedFormationCoroution()
    {
        if (selectNumber == 0)
        {
            playerDataBase.Formation = 1;

            GameStateManager.instance.WindCharacterType = WindCharacterType.Winter;

            PlayfabManager.instance.UpdatePlayerStatisticsInsert("Formation", 1);

            itemList.Clear();
            itemList.Add("LeftQueen_2_N");
            itemList.Add("LeftNight_N");
            itemList.Add("Rook_V2_N");
            itemList.Add("Pawn_Snow_N");

            PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", itemList);

            SoundManager.instance.PlaySFX(GameSfxType.ChoiceWinter);

            Debug.Log("눈의 여왕 진형 선택");
        }
        else
        {
            playerDataBase.Formation = 2;

            GameStateManager.instance.WindCharacterType = WindCharacterType.UnderWorld;

            PlayfabManager.instance.UpdatePlayerStatisticsInsert("Formation", 2);

            itemList.Clear();
            itemList.Add("RightQueen_2_N");
            itemList.Add("RightNight_N");
            itemList.Add("Rook_V4_N");
            itemList.Add("Pawn_Under_N");

            PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", itemList);

            SoundManager.instance.PlaySFX(GameSfxType.ChoiceUnder);

            Debug.Log("지하 세계 진형 선택");
        }

        warningView.SetActive(false);

        animationView.SetActive(true);

        if(GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
        {
            icon.sprite = iconArray[0];

            animationText.text = LocalizationManager.instance.GetString("Select_Winter");

            //SoundManager.instance.PlayBGM(GameBgmType.Main_Snow);
        }
        else
        {
            icon.sprite = iconArray[1];

            animationText.text = LocalizationManager.instance.GetString("Select_Under");

            //SoundManager.instance.PlayBGM(GameBgmType.Main_Under);
        }

        GameStateManager.instance.Tutorial = true;

        yield return new WaitForSeconds(4);

        PlayerPrefs.SetString("LoadScene", "MainScene");
        SceneManager.LoadScene("LoadScene");
    }

    public void CancleFormationButton()
    {
        warningView.SetActive(false);
    }
}
