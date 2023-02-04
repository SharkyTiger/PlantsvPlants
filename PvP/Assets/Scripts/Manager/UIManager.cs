using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI WaterCountText;
    public TextMeshProUGUI FertilizerCountText;
    public Button ShopToggle;

    private Int32 waterCount;
    private Int32 fertilizerCount;

    // Start is called before the first frame update
    void Start()
    {
        waterCount = 250;
        fertilizerCount = 250;
        SetCountTexts();
        ShopToggle.onClick.AddListener(OnShopToggle);
    }

    void OnDestroy()
    {
        
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

    private void SetCountTexts()
    {
        WaterCountText.text = $"W - {waterCount}";
        FertilizerCountText.text = $"F - {fertilizerCount}";
    }

    private void OnShopToggle()
    {
        AddToWater(Random.Range(-5, 6));
        AddToFertilizer(Random.Range(-5, 6));
    }
}
