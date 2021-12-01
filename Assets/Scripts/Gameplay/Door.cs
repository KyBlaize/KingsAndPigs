using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public delegate void DoorTouched();
    public event DoorTouched Touched;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            Touched?.Invoke();
        }
    }
}
