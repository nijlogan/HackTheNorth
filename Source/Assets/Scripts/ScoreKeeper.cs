using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private static ScoreKeeper _score;

    public static ScoreKeeper _instance
    {
        get
        {
            if (_score == null)
            {
                _score = new GameObject("ScoreKeeper").AddComponent<ScoreKeeper>();
            }
            return _score;
        }
    }

    private void Awake()
    {
        if (_score == null)
        {
            _score = this;
        } else if (_score != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void addScore(string num, int score)
    {
        PlayerPrefs.SetInt(num, score);
    }
}
