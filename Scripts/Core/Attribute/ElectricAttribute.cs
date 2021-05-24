using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttribute : MonoBehaviour, IAttribute
{
    public void Use()
    {
        Debug.Log("Electric");
    }
}
