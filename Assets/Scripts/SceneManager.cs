using System.Collections.Generic;
using UnityEngine;


public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public Player Player;
    public List<Enemie> Enemies { get; private set; }

    [SerializeField] private GameObject Lose;
    [SerializeField] private GameObject Win;
    [SerializeField] private GameObject MiniGoblin;
    [SerializeField] private LevelConfig Config;
    private int currWave = 0;

    private void Awake()
    {
        Instance = this;
        Enemies = new List<Enemie>();
    }

    private void Start()
    {
        SpawnWave();
    }

    private void OnEnable()
    {
        MegaEnemy.MegaEnemyDied += CreateMiniGoblins;
    }

    private void OnDisable()
    {
        MegaEnemy.MegaEnemyDied -= CreateMiniGoblins;
    }

    public void AddEnemie(Enemie enemie)
    {
        Enemies.Add(enemie);
    }

    public void RemoveEnemie(Enemie enemie)
    {
        Enemies.Remove(enemie);
        Debug.Log(Enemies.Count);
        if (Enemies.Count == 0)
        {
            SpawnWave();
        }
    }

    public void GameOver()
    {
        Lose.SetActive(true);
        StopEnemies();
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void CreateMiniGoblins(Vector3 position)
    {
        int goblinsCount = 2;


        CreateMiniGoblin(new Vector3(position.x, position.y, position.z));
        CreateMiniGoblin(new Vector3(position.x + 1, position.y, position.z + 1));
    }

    private void CreateMiniGoblin(Vector3 position)
    {
        GameObject miniGoblinInstantiate = Instantiate(MiniGoblin, position, Quaternion.identity);

        miniGoblinInstantiate.transform.LookAt(Player.transform.position);
    }

    private void SpawnWave()
    {
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            return;
        }

        var wave = Config.Waves[currWave];
        foreach (var character in wave.Characters)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(character, pos, Quaternion.identity);
        }
        currWave++;
    }

    private void StopEnemies()
    {
        foreach(var enemy in Enemies) enemy.Stop();
    }
}
