using UnityEngine;

public class ButtonShake : MonoBehaviour
{
    private int f = 0;
    private bool hover;
    private float size;

    private void Start()
    {
        size = transform.localScale.x;
    }

    public void setTrue()
    {
        hover = true;
    }

    public void setFalse()
    {
        hover = false;
    }

    private void Update()
    {
        if (hover)
        {
            Shake();
        }
    }

    public void Shake()
    {
        transform.localScale = new Vector3(size + Mathf.Cos((f * 3f) * Mathf.PI / 180.0f) * 0.02f, Mathf.Abs(size) + Mathf.Sin((f * 3f) * Mathf.PI / 180.0f) * 0.02f, 0);
        f++;
    }
}
