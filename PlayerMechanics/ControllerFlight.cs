using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControllerFlight : MonoBehaviour
{
    [SerializeField] Rigidbody mainBody;
    [SerializeField] OVRInput.Controller leftController;
    [SerializeField] OVRInput.Controller rightController;

    [SerializeField] BulletProducer bulletProducer;
    [SerializeField] GameObject leftTarget;
    [SerializeField] GameObject rightTarget;

    [SerializeField] float speed = 1.5f;

    [SerializeField] AbilitySO primaryShot;
    [SerializeField] AbilitySO secondaryShot;
    [SerializeField] AbilitySO counter;
    float primaryCoolDown;
    float secondaryCoolDown;
    float counterCoolDown;
    GameObject primaryIndicator;
    GameObject secondaryIndicator;
    GameObject counterIndicator;

    // Start is called before the first frame update
    void Start()
    {
        primaryCoolDown = primaryShot.coolDown;
        secondaryCoolDown = secondaryShot.coolDown;
        counterCoolDown = counter.coolDown;

        primaryIndicator = GameObject.FindGameObjectWithTag("PrimaryProgressIndicator");
        secondaryIndicator = GameObject.FindGameObjectWithTag("SecondaryProgressIndicator");
        counterIndicator = GameObject.FindGameObjectWithTag("CounterProgressIndicator");
    }

    // Update is called once per frame
    void Update()
    {

        // flight
        if (OVRInput.Get(OVRInput.Button.One))
        {
            Vector3 orientation = (OVRInput.GetLocalControllerRotation(rightController) * Vector3.forward);
            mainBody.AddForce(Time.deltaTime * speed * new Vector3(orientation.x, -orientation.y, orientation.z));
        }
        if (OVRInput.Get(OVRInput.Button.Three))
        {
            Vector3 orientation = (OVRInput.GetLocalControllerRotation(leftController) * Vector3.forward);
            mainBody.AddForce(Time.deltaTime * speed * new Vector3(orientation.x, -orientation.y, orientation.z));
        }

        // air brake
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5f || OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.5f)
        {
            mainBody.velocity = new Vector3(0, 0, 0);
        }

        // abilities and cool downs
        if (primaryCoolDown >= primaryShot.coolDown)
        {
            // fire primary shot (right controller)
            if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5f)
            {
                primaryCoolDown = 0;
                primaryIndicator.transform.localScale = new Vector3(1, -1 * primaryCoolDown / primaryShot.coolDown, 1);

                if (primaryShot.type == AbilitySO.AbilityType.PRIMARY)
                {
                    bulletProducer.PrimaryShot(primaryShot.shotType, rightTarget.transform.position, rightTarget.transform.forward, rightTarget.transform.right);
                }
            }
        }
        else
        {
            primaryCoolDown += Time.deltaTime;
            primaryIndicator.transform.localScale = new Vector3(1, -1 * primaryCoolDown / primaryShot.coolDown, 1);
        }
        if (secondaryCoolDown >= secondaryShot.coolDown)
        {
            // fire secondary shot (left controller)
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5f)
            {
                secondaryCoolDown = 0;
                secondaryIndicator.transform.localScale = new Vector3(1, -1 * secondaryCoolDown / secondaryShot.coolDown, 1);

                if (secondaryShot.type == AbilitySO.AbilityType.PRIMARY)
                {
                    bulletProducer.PrimaryShot(secondaryShot.shotType, leftTarget.transform.position, leftTarget.transform.forward, leftTarget.transform.right);
                }
            }
        }
        else
        {
            secondaryCoolDown += Time.deltaTime;
            secondaryIndicator.transform.localScale = new Vector3(1, -1 * secondaryCoolDown / secondaryShot.coolDown, 1);
        }
        if (counterCoolDown >= counter.coolDown)
        {
            // fire counter
            if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
            {
                counterCoolDown = 0;
                counterIndicator.transform.localScale = new Vector3(1, counterCoolDown / counter.coolDown, 1);

                bulletProducer.OutwardBurst(transform.position);
            }
        }
        else
        {
            counterCoolDown += Time.deltaTime;
            counterIndicator.transform.localScale = new Vector3(1, counterCoolDown / counter.coolDown, 1);
        }
        
    }
}
