using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyAttribute : MonoBehaviour, IAttribute
{
    public void Use()
    {
        Debug.Log("Holy");
    }
}
