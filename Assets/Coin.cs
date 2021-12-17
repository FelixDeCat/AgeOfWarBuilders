using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfWarBuilders.Entities;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerModel playerModel = other.GetComponent<PlayerModel>();
        if (playerModel != null)
        {
            Store.AddCoin(1);
            Destroy(this.gameObject);
        }
    }
}
