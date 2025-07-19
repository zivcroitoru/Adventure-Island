using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartModel : IHeartModel
{
    private int _amount = 1;

    public void AddHeart()
    {
        if (_amount < 3) _amount++;
    }

    public int GetHeartAmount()
    {
        return _amount;
    }
        public void RemoveHeart()
    {
        if (_amount > 0) _amount--;
    }

    public bool IsDead()
    {
        return _amount <= 0;
    }
}
