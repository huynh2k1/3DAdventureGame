using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public TypeItem typeItem;
    public int Value = 20;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<Character>().PickUpItem(this);
            Destroy(gameObject);
        }
    }
}
public enum TypeItem
{
    Coin,
    Heal
}

