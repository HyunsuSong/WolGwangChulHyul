using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisionAttribute : MonoBehaviour, IAttribute
{
    public void Use()
    {
        Debug.Log("Poision");
    }
}
