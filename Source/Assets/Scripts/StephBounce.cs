using System.Collections;
using UnityEngine;

public class StephBounce : MonoBehaviour
{
    private float size;

    private void Start()
    {
        size = transform.localScale.x;
        StartCoroutine(Bounce());
    }

    IEnumerator Bounce()
    {
        int f = 0;
        while (true)
        {
            transform.localScale = new Vector3(size + Mathf.Cos((f * 3f) * Mathf.PI / 180.0f) * 0.02f, Mathf.Abs(size) + Mathf.Sin((f * 3f) * Mathf.PI / 180.0f) * 0.02f, 0);

            f++;

            yield return new WaitForFixedUpdate();
        }
    }
}
