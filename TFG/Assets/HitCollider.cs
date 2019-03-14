using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public string hitName;
    public float damage;

    public Player owner;

    private void OnTriggerEnter(Collider other)
    {
        Player somebody = other.gameObject.GetComponent<Player>();

        Debug.Log("Que te pego");
    }
}
