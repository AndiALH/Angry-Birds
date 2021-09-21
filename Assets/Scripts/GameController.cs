using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController TrailController;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    private Bird _shotBird;
    public BoxCollider2D TapCollider;
    public int level;
    private static int maxLevel = 2;

    public Text winMessage;

    private bool _isGameEnded = false;
    private bool _isWinning = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];
    }

    // Update is called once per frame
    //void Update(){}

    public void ChangeBird()
    {
        TapCollider.enabled = false;

        if (_isGameEnded)
        {
            return;
        }

        Birds.RemoveAt(0);

        if (Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }
        else
        {
            _isGameEnded = true;
            EndGame();
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if (Enemies.Count == 0)
        {
            _isGameEnded = true;
            _isWinning = true;
            EndGame();
        }
    }

    public void AssignTrail(Bird bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }
    void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }

    void EndGame()
    {
        if (_isGameEnded)
        {
            if (_isWinning)
            {
                Debug.Log("You Win");
                winMessage.text = "You Win";
                StartCoroutine(LoadScene());
            }

            else
            {
                Debug.Log("You Lose");
                winMessage.text = "You Lose";
                StartCoroutine(LoadScene());
            }

            winMessage.enabled = true;
        }
    }
    
    //Dipanggil untuk restart/ke level selanjutnya
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3f);
        if (_isWinning && level < maxLevel)
        {
            Debug.Log("Going to next level");
            SceneManager.LoadScene(level);
        }
        else if (!_isWinning)
        {
            Debug.Log("restarting level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
