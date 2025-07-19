using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeartModel
{
    void AddHeart();
    int GetHeartAmount();
    void RemoveHeart();
    bool IsDead();
}