using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockEquipContent : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    public int index = 0;

    public EquipManager equipManager;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            switch (index)
            {
                case 0:
                    equipManager.ChangeArmor();
                    break;
                case 1:
                    equipManager.ChangeWeapon();
                    break;
                case 2:
                    equipManager.ChangeShield();
                    break;
                case 3:
                    equipManager.ChangeNewbie();
                    break;
            }
        }
    }
}
