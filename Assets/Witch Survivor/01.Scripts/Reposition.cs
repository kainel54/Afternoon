using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area"))
        {
            return;
        }

    }
}
