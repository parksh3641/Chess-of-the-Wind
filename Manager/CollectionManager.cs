using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public GameObject collectionView;

    PlayerDataBase playerDataBase;

    void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        collectionView.SetActive(false);
    }

    public void OpenCollectionView()
    {
        if (!collectionView.activeSelf)
        {
            collectionView.SetActive(true);

            CheckCollection();
        }
        else
        {
            collectionView.SetActive(false);
        }
    }

    void CheckCollection()
    {

    }
}
