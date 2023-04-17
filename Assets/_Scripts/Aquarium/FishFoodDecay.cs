using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.uapb.edu/sites/www/Uploads/AQFI/Ext/Classroom/09Calculate_Ammonia.pdf 
// For future Reference
public class FishFoodDecay : AquariumObject
{
    [Header("FishFoodDecay")]
    [SerializeField] private float _ammoniaProductionRate;
    [SerializeField] private float _foodDecayFactor;

    public override void DoProcess(AquariumParameterData parameters)
    {
        float avalibleFood = GetParameter(Parameter.FishFood, parameters);

        if(avalibleFood == 0f) { return; }

        float foodDecayAmount = avalibleFood * _foodDecayFactor + 0.02f;        
        float producedAmmoniaPPM = foodDecayAmount * _ammoniaProductionRate;

        parameters.AddToParameter(Parameter.Ammonia, producedAmmoniaPPM);
        parameters.SubtractFromParameter(Parameter.FishFood, foodDecayAmount);
    }
}

