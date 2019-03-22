using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public string hitName;
    public float damage;

    public Player owner;
        private void Update()
    {
    

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
   
        Player somebody = other.gameObject.GetComponent<Player>();

        if (somebody != null && somebody != owner)
        {
            somebody.DamageReceived(damage);
            owner.UpdateEnergy(15);

        }
    }


}
