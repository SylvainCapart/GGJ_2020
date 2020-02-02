using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurabilityManager : MonoBehaviour
{
    
    private int durability;
    public int durabilityMax;
    public int fallDamage;
    

    public StatusIndicator statusIndicator;


    void Start()
    {
        durability = durabilityMax;
    }

    void Update()
    {
        statusIndicator.SetHealth(durability, durabilityMax);
    }
    public float getDurabilityRatio()
    {
        float ratio = durability/durabilityMax;
        if(ratio >1)
        {
            durability = durabilityMax;
            ratio = 1;
        }
        else if (ratio <0)
        {
            durability = 0;
            ratio = 0;
        }
        return ratio;
    }

    public bool checkAlive()
    {
        return (durability >0);
    }

    public void resetDurability()
    {
        durability = durabilityMax;
    }

    public void addFallDamage()
    {
        durability -= fallDamage;
        if (durability <0)
        {
            durability = 0;
        }
    }
    public void decrementDurability()
    {
        if (durability >0)
        {
            durability--;
        }
    }
    public void augmentDurability(int value)
    {
        durability += value;
        if (durability >durabilityMax)
        {
            durability = durabilityMax;
        }
    }

    //Return values: false if value out of range, true if ok
    public bool setDurability(int value)
    {
        if (value<0 || value> durabilityMax)
        {
            return false;
        }
        durability = value;
        return true;
    }

    public int getDurability()
    {
        return durability;
    }

}
