using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolKwangChulHyeol.Core;

public class SkyStoneReaction : MonoBehaviour
{
    private Health myHealth = null;
    private float myCurHealth = 0.0f;

    private Vector3 originPosition;
    private Vector3 shakePosition;
    private Quaternion originRotation;
    private Quaternion shakeRotation;
    private float shake_decay = 0.0015f;
    private float shake_intensity;
    private float coef_shake_intensity = 0.1f;

    void Start()
    {
        if (myHealth == null)
        {
            myHealth = GetComponent<Health>();
            myCurHealth = myHealth.MHealthCurrentPoints;
        }

        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        MyTakeDamage();
    }

    void MyTakeDamage()
    {
        if (myCurHealth > myHealth.MHealthCurrentPoints)
        {
            myCurHealth = myHealth.MHealthCurrentPoints;

            shakePosition = transform.position;
            shakeRotation = transform.rotation;
            shake_intensity = coef_shake_intensity;
        }

        if (shake_intensity > 0)
        {
            transform.position = shakePosition + Random.insideUnitSphere * shake_intensity;

            transform.transform.rotation = new Quaternion
            (
                shakeRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                shakeRotation.y + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                shakeRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.2f,
                shakeRotation.w + Random.Range(-shake_intensity, shake_intensity) * 0.2f
            );

            shake_intensity -= shake_decay;
        }
        else
        {
            transform.position = originPosition;
            transform.rotation = originRotation;
        }

        if (myHealth.IsDead())
        {
            Destroy(gameObject);
        }
    }

}
