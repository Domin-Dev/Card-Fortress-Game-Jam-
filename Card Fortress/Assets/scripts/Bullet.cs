using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    [SerializeField] float speed;
    private float destoryDistance = 0.05f;
    public int damage;
    public float push;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (target != null && target.tag == "Enemy")
        {
            Vector3 moveDir = (target.position - transform.position).normalized;

            transform.position = transform.position + moveDir * speed * Time.deltaTime;

            float angle = Mathf.Atan2(transform.position.y - target.position.y, transform.position.x - target.position.x);
            angle = (180 / Mathf.PI) * angle;

            transform.localEulerAngles = new Vector3(0, 0, angle);

            if (Vector3.Distance(transform.position, target.position) < destoryDistance)
            {
                MapGenerator.mapGenerator.SetText(transform.position, damage);
                target.gameObject.GetComponent<Enemy>().Hit(damage,push);
               // Instantiate(MapGenerator.mapGenerator.effect, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
