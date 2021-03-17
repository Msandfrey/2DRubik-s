using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private GameObject UIHelper;

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIHelper.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIHelper.SetActive(false);
    }
}
