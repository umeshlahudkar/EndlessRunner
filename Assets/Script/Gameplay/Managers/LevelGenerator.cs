using RG.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Gameplay
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject roadPrefab;
        [SerializeField] private Coin coinPrefab;
        [SerializeField] private Obstacle[] obstaclePrefabs;

        [Header("Obstacle Layer Mask")]
        [SerializeField] private LayerMask obstcleLayerMask;

        [Header("Gameobjects Movement data")]
        private float currentSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float accelerationRate;

        private readonly int initialRoadSegment = 5;
        private readonly int roadLength = 56;
        private readonly int[] laneXPositions = new int[] { 0, -2, 2 };

        private float totalDistanceTraveled;
        public float TotalDistanceTraveled
        {
            get { return totalDistanceTraveled; }
        }

        [Header("Coin and Obstacle spawn data")]
        [SerializeField] private float distanceBetweenObstacleSpawns = 30f;
        private float nextObstacleSpawnDistance = 0f;
        [SerializeField] private float distanceBetweenCoinSpawns = 60f;
        private float nextCoinSpawnDistance = 0f;
        [SerializeField] private float spaceBetweenCoin;

        private List<GameObject> roadSegments = new List<GameObject>();
        private List<Coin> coins = new List<Coin>();
        private List<Obstacle> obstacles = new List<Obstacle>();

        private ObjectPool<Coin> coinPool;
        private ObjectPool<Obstacle> groundObstaclePool;
        private ObjectPool<Obstacle> floatingObstaclePool;


        public void GenerateLevel()
        {
            totalDistanceTraveled = 0;
            SpawnRoadSegments();

            if (coinPool == null)
            {
                coinPool = new ObjectPool<Coin>(coinPrefab, 100);
            }

            if (groundObstaclePool == null)
            {
                groundObstaclePool = new ObjectPool<Obstacle>(obstaclePrefabs[0], 10);
            }

            if (floatingObstaclePool == null)
            {
                floatingObstaclePool = new ObjectPool<Obstacle>(obstaclePrefabs[1], 10);
            }
        }

        private void Update()
        {
            if (GameManager.Instance.State != GameState.Playing) { return; }

            MoveRoadSegments();
            MoveCoinAndObstacles();

            if (currentSpeed < maxSpeed)
            {
                currentSpeed += accelerationRate * Time.deltaTime;
            }

            if (totalDistanceTraveled >= nextObstacleSpawnDistance)
            {
                SpawnObstacles();
                nextObstacleSpawnDistance += distanceBetweenObstacleSpawns;
            }

            if (totalDistanceTraveled >= nextCoinSpawnDistance)
            {
                SpawnCoins();
                nextCoinSpawnDistance += distanceBetweenCoinSpawns;
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.State != GameState.Playing) { return; }

            CheckForCoinInsideObstacle();
        }

        private void CheckForCoinInsideObstacle()
        {
            for (int i = 0; i < coins.Count; i++)
            {
                Coin coin = coins[i];
                if (!coin.HasCheckedForInsideObstacle)
                {
                    if (IsCoinInsideObstacle(coin))
                    {
                        coin.HasCheckedForInsideObstacle = true;
                        coin.gameObject.SetActive(false);
                    }
                }
            }
        }

        private bool IsCoinInsideObstacle(Coin coin)
        {
            if (Physics.CheckSphere(coin.ThisTransform.position, 2f, obstcleLayerMask))
            {
                return true;
            }

            return false;
        }


        private void SpawnRoadSegments()
        {
            if (roadSegments != null && roadSegments.Count > 0) { return; }

            for (int i = 0; i < initialRoadSegment; i++)
            {
                GameObject roadSegment = Instantiate(roadPrefab, new Vector3(0, 0.35f, i * roadLength), Quaternion.identity);
                roadSegments.Add(roadSegment);
                roadSegment.SetActive(true);
            }
        }

        private void MoveRoadSegments()
        {
            float distanceMoved = 0f;
            foreach (GameObject segment in roadSegments)
            {
                distanceMoved = Time.deltaTime * currentSpeed;
                segment.transform.position -= new Vector3(0, 0, distanceMoved);

                if (segment.transform.position.z < -roadLength)
                {
                    segment.transform.position = new Vector3(0, 0.35f, ((roadSegments.Count - 1) * roadLength));
                }
            }

            totalDistanceTraveled += distanceMoved;
        }

        private void MoveCoinAndObstacles()
        {
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                Coin coin = coins[i];
                coin.ThisTransform.position -= new Vector3(0, 0, Time.deltaTime * currentSpeed);

                if (coin.transform.position.z < -roadLength)
                {
                    coin.HasCheckedForInsideObstacle = false;
                    coinPool.ReturnObject(coin);
                    coins.RemoveAt(i);
                }
            }

            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                Obstacle obstacle = obstacles[i];
                obstacle.transform.position -= new Vector3(0, 0, Time.deltaTime * currentSpeed);

                if (obstacle.transform.position.z < -roadLength)
                {
                    if (obstacle.ObstacleType == ObstacleType.Ground)
                    {
                        groundObstaclePool.ReturnObject(obstacle);
                    }
                    else
                    {
                        floatingObstaclePool.ReturnObject(obstacle);
                    }
                    obstacles.RemoveAt(i);
                }
            }
        }

        private int GetRandomLane()
        {
            int index = Random.Range(0, laneXPositions.Length);
            return laneXPositions[index];
        }

        private void SpawnCoins()
        {
            int numLanesToSpawn = Random.Range(1, 4);

            for (int i = 0; i < numLanesToSpawn; i++)
            {
                int randomLaneIndex = GetRandomLane();

                int numCoinsToSpawn = Random.Range(7, 11);

                for (int j = 0; j < numCoinsToSpawn; j++)
                {
                    Coin coin = coinPool.GetObject();
                    coin.ThisTransform.position = new Vector3(randomLaneIndex, 1f, 200 + (j * spaceBetweenCoin));

                    coins.Add(coin);
                }

            }
        }

        private void SpawnObstacles()
        {
            bool flag = Random.value < 0.3f;

            if (flag)
            {
                Obstacle obstacle = floatingObstaclePool.GetObject();
                obstacle.ThisTransform.SetPositionAndRotation(new Vector3(-3.7f, 0, 200), Quaternion.Euler(0, 90, 0));

                obstacles.Add(obstacle);
            }
            else
            {
                int numLanesToSpawn = Random.Range(1, 3);

                for (int i = 0; i < numLanesToSpawn; i++)
                {
                    int randomLaneIndex = GetRandomLane();

                    Obstacle obstacle = groundObstaclePool.GetObject();
                    obstacle.ThisTransform.SetPositionAndRotation(new Vector3(randomLaneIndex, 0, 200), Quaternion.Euler(0, 90, 0));

                    obstacles.Add(obstacle);
                }
            }
        }

        public void ResetSpeed()
        {
            currentSpeed = 0;
        }

        public void ResetLevel()
        {
            totalDistanceTraveled = 0;
            currentSpeed = 0;
            nextObstacleSpawnDistance = 0;
            nextCoinSpawnDistance = 0;

            for (int i = 0; i < roadSegments.Count; i++)
            {
                roadSegments[i].transform.position = new Vector3(0, 0.35f, i * roadLength);
            }

            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].HasCheckedForInsideObstacle = false;
                coinPool.ReturnObject(coins[i]);
            }

            for (int i = 0; i < obstacles.Count; i++)
            {
                if (obstacles[i].ObstacleType == ObstacleType.Ground)
                {
                    groundObstaclePool.ReturnObject(obstacles[i]);
                }
                else
                {
                    floatingObstaclePool.ReturnObject(obstacles[i]);
                }
            }

            coins.Clear();
            obstacles.Clear();
        }
    }
}