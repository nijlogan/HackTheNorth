using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiders : MonoBehaviour
{
    public int people;

    public int getPeople()
    {
        return people;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Person")
        {
            people++;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Person")
        {
            people--;
        }
    }

}
