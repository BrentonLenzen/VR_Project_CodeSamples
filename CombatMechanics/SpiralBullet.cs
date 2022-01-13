using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpiralBullet : Bullet
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        StartCoroutine(Spiral());
    }

    IEnumerator Spiral()
    {
        for(; ; )
        {
            transform.RotateAround(accelDirection, 2f * Time.deltaTime);

            transform.position += transform.up * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
