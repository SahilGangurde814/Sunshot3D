using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sunshot.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("UI Data")]
        [SerializeField] GameObject gameOverPanel;
        [SerializeField] TextMeshProUGUI currentScoreTxt;
        [SerializeField] TextMeshProUGUI highScoreTxt;

        private float score;
        private int highScore;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(instance);
        }

        private void Start()
        {
            score = 0;
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreTxt.text = "H S : " + highScore;

        }

        private void Update()
        {
            score += Time.deltaTime;
            int currentScoreInt = Mathf.FloorToInt(score);
            currentScoreTxt.text = "Score : " + currentScoreInt;

            if (score < highScore) return;

            highScore = currentScoreInt;
            highScoreTxt.text = "H S : " + highScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GameOver()
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
