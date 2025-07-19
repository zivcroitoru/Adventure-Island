using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformStick : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingTile"))
            transform.SetParent(collision.transform, true);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingTile"))
            transform.SetParent(null, true);
    }
}
