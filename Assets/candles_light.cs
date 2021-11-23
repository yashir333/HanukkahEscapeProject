using System;
using UnityEngine;

public class candles_light : MonoBehaviour
{
    public int candleNum;
    public bool ifAfterSunset = false;
    

    // Start is called before the first frame update
    void Start()
    {

        DateTime dt1 = new DateTime(2021, 11, 28);
        DateTime dt2 = DateTime.Now;

        if (dt2.Hour > 16)
        {
            ifAfterSunset = true;
        }

        int ireturn = (int)dt2.Subtract(dt1).TotalDays;
        if (ireturn >= candleNum || ((ireturn+1)>=candleNum && ifAfterSunset))
        {
            gameObject.active = true;
        } else
        {
            gameObject.active = false;
        }

    }

}
