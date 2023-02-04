using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI WaterCountText;
    public TextMeshProUGUI FertilizerCountText;
    public GameObject ClosedShopParent;
    public GameObject OpenShopParent;

    private Int32 waterCount;
    private Int32 fertilizerCount;

    // Start is called before the first frame update
    void Start()
    {
        waterCount = 250;
        fertilizerCount = 250;
        SetCountTexts();
    }

    public void AddToWater(int add)
    {
        waterCount += add;
        WaterCountText.text = $"W - {waterCount}";
    }

    public void AddToFertilizer(int add)
    {
        fertilizerCount += add;
        FertilizerCountText.text = $"F - {fertilizerCount}";
    }

    public void ToggleShop()
    {
        ClosedShopParent.SetActive(!ClosedShopParent.activeSelf);
        OpenShopParent.SetActive(!OpenShopParent.activeSelf);
    }

    private void SetCountTexts()
    {
        WaterCountText.text = $"W - {waterCount}";
        FertilizerCountText.text = $"F - {fertilizerCount}";
    }
}
