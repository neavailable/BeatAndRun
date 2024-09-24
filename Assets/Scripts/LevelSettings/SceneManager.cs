using System.Collections.Generic;
using UnityEngine;


public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public static System.Action<int> NewWaveStarted;

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
        int goblinAmount = 2;

        for (int i = 0; i < goblinAmount; i++)
        {
            CreateOneMiniGoblin(new Vector3(position.x + i, position.y, position.z + i));
        }
    }

    private void CreateOneMiniGoblin(Vector3 position)
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
            Vector3 pos = new Vector3(Random.Range(-15, 15), 0, Random.Range(-15, 15));
            Instantiate(character, pos, Quaternion.identity);
        }
        currWave++;
        NewWaveStarted?.Invoke(currWave);
    }

    private void StopEnemies()
    {
        foreach(var enemy in Enemies) enemy.Stop();
    }
}
