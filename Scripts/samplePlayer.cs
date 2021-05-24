using UnityEngine;
using WolKwangChulHyeol.Core;

public class samplePlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(30, true);
        }

    }
}
