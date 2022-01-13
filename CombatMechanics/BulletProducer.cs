using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletProducer : MonoBehaviour
{
    [SerializeField] GameObject basicBullet;
    [SerializeField] GameObject spiralBullet;
    [SerializeField] GameObject falloffBullet;
    [SerializeField] GameObject mineBullet;
    [SerializeField] Rigidbody rb;

    GameObject bulletPrefab;

    PhotonView photonView;
    void Start()
    {
        bulletPrefab = basicBullet;
        photonView = GetComponent<PhotonView>();
    }

    // primary shots
    public void PrimaryShot(ShotTypeSO shot, Vector3 origin, Vector3 direction, Vector3 perpendicular)
    {
        // set bullet type to be spawned by shot
        switch (shot.bullet)
        {
            case ShotTypeSO.BulletType.STRAIGHT:
                bulletPrefab = basicBullet;
                break;

            case ShotTypeSO.BulletType.SPIRAL:
                bulletPrefab = spiralBullet;
                break;

            case ShotTypeSO.BulletType.FALLOFF:
                bulletPrefab = falloffBullet;
                break;

            case ShotTypeSO.BulletType.MINE:
                bulletPrefab = mineBullet;
                break;

            default:
                bulletPrefab = basicBullet;
                break;
        }

        switch (shot.spread)
        {
            case ShotTypeSO.SpreadType.POINT:
                photonView.RPC("PointShotProxy", RpcTarget.All, shot.numberOfBullets, origin, direction, perpendicular);
                break;

            case ShotTypeSO.SpreadType.FLOWER:
                photonView.RPC("FlowerShotProxy", RpcTarget.All, shot.numberOfBullets, origin, direction, perpendicular);
                break;

            case ShotTypeSO.SpreadType.RANDOM:
                photonView.RPC("RandomShotProxy", RpcTarget.All, shot.numberOfBullets, origin, direction, perpendicular);
                break;

            case ShotTypeSO.SpreadType.WAVE:
                break;

            default:
                break;
        }
    }

    [PunRPC]
    public void PointShotProxy(int numBullets, Vector3 origin, Vector3 direction, Vector3 perpendicular)
    {
        StartCoroutine(PointShotCoroutine(numBullets, origin, direction, perpendicular));
    }
    IEnumerator PointShotCoroutine(int numBullets, Vector3 origin, Vector3 direction, Vector3 perpendicular)
    {
        // spawn bullets in same direction with a short delay between them
        for (int i = 0; i < numBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, origin + direction, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(300 * direction);
            bullet.GetComponent<Rigidbody>().velocity += rb.velocity;
            bullet.GetComponent<Bullet>().accelDirection = direction;

            yield return new WaitForSeconds(0.1f);
        }
    }

    [PunRPC]
    public void FlowerShotProxy(int numBullets, Vector3 origin, Vector3 direction, Vector3 perpendicular)
    {
        GameObject bullet = Instantiate(bulletPrefab, origin + direction, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(100 * direction);
        bullet.GetComponent<Rigidbody>().velocity += rb.velocity;
        bullet.GetComponent<Bullet>().accelDirection = direction;

        // evenly space bullets in circle around origin point
        float angleDifference = 360f / numBullets;
        for (int i = 0; i < numBullets; i++)
        {
            GameObject outerbullet = Instantiate(bulletPrefab, origin + direction + 0.5f * perpendicular, Quaternion.identity);
            outerbullet.transform.RotateAround(origin, direction, i * angleDifference);
            outerbullet.GetComponent<Rigidbody>().AddForce((120 * direction) + (60 * (outerbullet.transform.position - (origin + direction))));
            outerbullet.GetComponent<Rigidbody>().velocity += rb.velocity;
            outerbullet.GetComponent<Bullet>().accelDirection = direction + (0.5f * (outerbullet.transform.position - (origin + direction)));
        }
    }

    [PunRPC]
    public void RandomShotProxy(int numBullets, Vector3 origin, Vector3 direction, Vector3 perpendicular)
    {
        // spawn bullets with random directions
        Vector3 up = Vector3.Cross(direction, perpendicular);
        for(int i = 0; i < numBullets; i++)
        {
            Vector3 randomDirection = direction + ((Random.value - 0.5f) * up) + ((Random.value - 0.5f) * perpendicular);
            GameObject bullet = Instantiate(bulletPrefab, origin + direction, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(300 * randomDirection);
            bullet.GetComponent<Rigidbody>().velocity += rb.velocity;
            bullet.GetComponent<Bullet>().accelDirection = randomDirection;
        }
    }

    // counter shots
    public void OutwardBurst(Vector3 origin)
    {
        photonView.RPC("OutwardBurstProxy", RpcTarget.All, origin);
    }
    [PunRPC]
    public void OutwardBurstProxy(Vector3 origin)
    {
        for (int i = 0; i < 5; i++)
        {
            float phi = i * (Mathf.PI / 5f);
            for (int j = 0; j < 20; j++)
            {
                float theta = j * ((2f * Mathf.PI) / 20f);
                GameObject bullet = Instantiate(basicBullet, origin + new Vector3(Mathf.Sin(phi) * Mathf.Cos(theta), Mathf.Cos(phi), Mathf.Sin(phi) * Mathf.Sin(theta)), new Quaternion());
                bullet.GetComponent<Rigidbody>().AddForce(10 * new Vector3(Mathf.Sin(phi) * Mathf.Cos(theta), Mathf.Cos(phi), Mathf.Sin(phi) * Mathf.Sin(theta)), ForceMode.Impulse);
                bullet.GetComponent<Rigidbody>().velocity += rb.velocity;
                bullet.GetComponent<Bullet>().accelDirection = new Vector3(Mathf.Sin(phi) * Mathf.Cos(theta), Mathf.Cos(phi), Mathf.Sin(phi) * Mathf.Sin(theta));
            }
        }
    }
}
