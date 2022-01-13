using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bullet : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float accelDelay = 0.5f;
    [SerializeField] float lifeTime = 7f;
    [SerializeField] Rigidbody rb;

    public Vector3 accelDirection = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    protected void Start()
    {
        StartCoroutine(LifeTime());
        StartCoroutine(Accelerate());
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    IEnumerator Accelerate()
    {
        yield return new WaitForSeconds(accelDelay);

        while (true)
        {
            yield return new WaitForEndOfFrame();
            rb.AddForce(acceleration * accelDirection);
        }
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(this.gameObject);
    }

}
