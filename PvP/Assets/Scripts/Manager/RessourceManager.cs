using System;
using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    public Int32 StartWater;
    public Int32 StartFertilizer;

    public Int32 WaterPerMine;
    public Int32 FertilizerPerMine;

    private Int32 waterCount;
    private Int32 fertilizerCount;

    private Int32 fixedTicks;
    private Int32 numberWaterMine;
    private Int32 numberFertilizerMine;

    private UIManager UIManagerObject;

    // Start is called before the first frame update
    void Start()
    {
        waterCount = StartWater;
        fertilizerCount = StartFertilizer;
        UIManagerObject = GameObject.FindGameObjectWithTag("UIManager")?.GetComponent<UIManager>();
        if (UIManagerObject == null) UIManagerObject = gameObject.GetComponent<UIManager>();
        fixedTicks = 0;

        UpdateRessourceUI();
    }

    private void FixedUpdate()
    {
        fixedTicks++;
        if(fixedTicks % 250 == 0)
        {
            fixedTicks = 0;
            AddRessourcesFromBuilding();
            UpdateRessourceUI();
        }
    }

    public void DeductRessourceCost(Int32 water, Int32 fertilizer)
    {
        waterCount -= water;
        fertilizerCount -= fertilizer;
        UpdateRessourceUI();
    }

    public Boolean HasEnough(Int32 waterCost, Int32 fertilizerCost)
    {
        return waterCost <= waterCount && fertilizerCost <= fertilizerCount;
    }

    public void AddWaterMine()
    {
        numberWaterMine++;
    }

    public void DestroyWaterMine()
    {
        numberWaterMine--;
    }

    public void AddFertilizerMine()
    {
        numberFertilizerMine++;
    }

    public void DestroyFertilizerMine()
    {
        numberFertilizerMine--;
    }

    private void AddRessourcesFromBuilding()
    {
        waterCount += numberWaterMine * WaterPerMine;
        fertilizerCount += numberFertilizerMine * FertilizerPerMine;
    }

    private void UpdateRessourceUI()
    {
        UIManagerObject.SetCountTexts(waterCount, fertilizerCount);
    }
}
