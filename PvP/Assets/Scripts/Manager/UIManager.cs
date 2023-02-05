using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI WaterCountText;
    public TextMeshProUGUI FertilizerCountText;
    public GameObject ClosedShopParent;
    public GameObject OpenShopParent;

    private void Awake()
    {
        SetCountTexts(0, 0);
    }

    public void ToggleShop()
    {
        ClosedShopParent.SetActive(!ClosedShopParent.activeSelf);
        OpenShopParent.SetActive(!OpenShopParent.activeSelf);
    }

    public void SetCountTexts(int water, int fertilizer)
    {
        WaterCountText.text = $"W : {water}";
        FertilizerCountText.text = $"F : {fertilizer}";
    }
}
