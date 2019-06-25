using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnergySphere : MonoBehaviour
{
    public Player caster;
    public  ParticleSystem m_particleSystem;
    public float movementSpeed = 200;
    public float damage;
    private float creationTime;
    public float lifeTime = 3;


    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
        body = GetComponent<Rigidbody2D>();
        float speed;
        if(caster.throwForward == true)
        {

            speed = movementSpeed;

        }
        else
        {

            speed = movementSpeed * -1;
            var sh = m_particleSystem.shape;
            sh.position = new Vector3(0, 0, -2);
            sh.rotation = new Vector3(0, -180, 0);
        }
        body.AddRelativeForce(new Vector3(speed, 0, 0));
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - creationTime > lifeTime)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && player != caster){
            player.DamageReceived(damage);
            Destroy(gameObject);
            caster.UpdateEnergy(15);

        }
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("EnergySphereP1") || collision.gameObject.CompareTag("EnergySphereP2"))
        {
            Destroy(gameObject);
        }
    }
}
