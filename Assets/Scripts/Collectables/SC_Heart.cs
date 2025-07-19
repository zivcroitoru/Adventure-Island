using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SC_Heart : MonoBehaviour
{
    public delegate void HeartCollisionHandler();
    public static event HeartCollisionHandler OnHeartCollision;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnCollisionEnter2D " + col.gameObject.name);
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Mario Collision!");
            if (OnHeartCollision != null)
                OnHeartCollision();

            this.gameObject.SetActive(false);
        }
    }
}
