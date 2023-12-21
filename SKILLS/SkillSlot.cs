using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour
{
    public SkillScriptableObject skill;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text skillAmountText;

    private void Awake()
    {
        iconGO = transform.GetChild(0).gameObject;
        skillAmountText = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetIcon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
    }

}
