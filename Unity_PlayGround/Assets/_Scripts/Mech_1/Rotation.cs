using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    void Update()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 2);
    }
}
