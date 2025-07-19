using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnCollisionEnter2D " + col.gameObject.name);
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Mario Collision! Speedup");
            this.gameObject.SetActive(false);
            // Gives player the speed-up effect by calling his power-up system
            col.gameObject.GetComponent<PlayerPowerUp>().CollectPowerUp(new SpeedUpPowerUp());
        }
    }
}
