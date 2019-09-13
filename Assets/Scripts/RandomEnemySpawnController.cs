using SBR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomEnemySpawnController : MonoBehaviour {
    [System.Serializable]
    public struct SpawnableEnemy {
        public GameObject enemy;
        public float cost;
        public bool isBoss;
    }

    [System.Serializable]
    public class SpawnableEnemyList : DraggableList<SpawnableEnemy> { }


    [Header("Enemy Spawning")]
    [DraggableListDisplay("enemy")]
    public SpawnableEnemyList spawnables;
    public AnimationCurve difficultyCurve;
    public int maxEnemiesOnScreen;
    public float budgetGainMultiplier;
    public float maxBudgetMultiplier;
    public int totalEnemyCount = 100;
    public float baseBudget = 1;
    public float baseBudgetGainRate = 0.5f;
    public Rect spawnArea;
    public float spawnCooldown = 0.5f;

    [Header("Round End")]
    public WeaponPickup pickupTemplate;
    public ShipWeapon[] possibleRewards;
    public GameObject alternativeReward;
    public float budgetGainIncrease = 0.5f;
    public float maxBudgetIncrease = 0.5f;

    private HashSet<int> alreadyGivenRewards;
    private CooldownTimer spawnTimer;
    private float currentBudget;
    private int enemiesDestroyed;
    private int enemiesSpawned;
    private List<GameObject> spawnedEnemies;
    private List<GameObject> spawnedBossEnemies;
    private int nextEnemyToSpawn;
    private GameObject endButtons;

    private bool spawnedReward;

    private float progress => (float)enemiesDestroyed / (float)totalEnemyCount;
    private float currentDifficulty => difficultyCurve.Evaluate(progress);
    private float budgetGainRate => baseBudgetGainRate + currentDifficulty * budgetGainMultiplier;
    private float maxBudget => baseBudget + currentDifficulty * maxBudgetMultiplier;

    private void Awake() {
        alreadyGivenRewards = new HashSet<int>();
        spawnTimer = new CooldownTimer(spawnCooldown);
        spawnedEnemies = new List<GameObject>();
        spawnedBossEnemies = new List<GameObject>();
        currentBudget = baseBudget;
        endButtons = Tags.FindGameObjectWithTag(Tag.Finish);

        if (this.TryGetComponentInChildren(out Renderer renderer)) {
            Bounds b = renderer.bounds;
            spawnArea = new Rect(b.min.x, b.min.z, b.size.x, b.size.z);
        }
    }

    public void Restart() {
        enemiesDestroyed = 0;
        enemiesSpawned = 0;
        spawnedEnemies.Clear();
        spawnedBossEnemies.Clear();
        baseBudgetGainRate += budgetGainIncrease;
        baseBudget += maxBudgetIncrease;

        currentBudget = baseBudget;

        Start();
    }

    private void Start() {
        endButtons.SetActive(false);
        StartCoroutine(UpdateCrt());
    }

    private int PickNextEnemy() {
        int numOptions = 1;

        for (int i = 1; i < spawnables.length; i++) {
            if (spawnables[i].cost <= maxBudget && (!spawnables[i].isBoss || spawnedBossEnemies.Count == 0)) {
                numOptions++;
            } else {
                break;
            }
        }

        return Random.Range(0, numOptions);
    }

    private void Update() {
        currentBudget = Mathf.Clamp(currentBudget + budgetGainRate * Time.deltaTime, 0, maxBudget);

        enemiesDestroyed += spawnedEnemies.Count(e => !e) + spawnedBossEnemies.Count(e => !e);
        spawnedEnemies.RemoveAll(e => !e);
        spawnedBossEnemies.RemoveAll(e => !e);
    }

    private IEnumerator UpdateCrt() {
        while (enemiesSpawned < totalEnemyCount) {
            if (enemiesSpawned < totalEnemyCount && spawnTimer.Use()) {
                nextEnemyToSpawn = PickNextEnemy();
                if (currentBudget >= spawnables[nextEnemyToSpawn].cost && spawnedEnemies.Count < maxEnemiesOnScreen) {
                    SpawnNextEnemy();
                    nextEnemyToSpawn = PickNextEnemy();
                }
            }
            yield return null;
        }

        while (enemiesDestroyed < totalEnemyCount) {
            yield return null;
        }

        GameObject reward = SpawnReward();
        while (reward) {
            yield return null;
        }

        yield return new WaitForSeconds(3);

        Pause.paused = true;
        endButtons.SetActive(true);
    }

    private GameObject SpawnReward() {
        Vector3 location = new Vector3(spawnArea.center.x, 0, spawnArea.center.y);
        if (alreadyGivenRewards.Count < possibleRewards.Length) {
            WeaponPickup pickup = Instantiate(pickupTemplate, location, Quaternion.identity);
            int sel = -1;
            do {
                sel = Random.Range(0, possibleRewards.Length);
            } while (!alreadyGivenRewards.Add(sel));
            pickup.weaponObject = possibleRewards[sel];
            return pickup.gameObject;
        } else {
            return Instantiate(alternativeReward, location, Quaternion.identity);
        }
    }

    private void SpawnNextEnemy() {
        Vector3 pos = new Vector3(Random.Range(spawnArea.xMin, spawnArea.xMax), 0, Random.Range(spawnArea.yMin, spawnArea.yMax));
        GameObject newEnemy = Instantiate(spawnables[nextEnemyToSpawn].enemy, pos, spawnables[nextEnemyToSpawn].enemy.transform.rotation);
        currentBudget -= spawnables[nextEnemyToSpawn].cost;
        enemiesSpawned++;

        if (spawnables[nextEnemyToSpawn].isBoss) {
            spawnedBossEnemies.Add(newEnemy);
        } else {
            spawnedEnemies.Add(newEnemy);
        }
    }
}
