using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombEnemyBehaviour : EnemyBehaviour
{
    private GameObject player;
    public GameObject bomb;
    public GameObject explosion;
    public int amountOfBombs;
    public int bombSpawnInterval;
    private List<GameObject> listOfBombs = new List<GameObject>();
    private List<GameObject> listOfExplosions = new List<GameObject>();

    protected Animator jackalAnimator;

    protected Vector2 enemyDirection;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;
        StartCoroutine(SpawnBombs());

        jackalAnimator = GetComponent<Animator>();

        StartCoroutine(UpdateAnimation());
    }

    private IEnumerator SpawnBombs()
    {
        while (this.gameObject != null)
        {
            yield return new WaitForSeconds(bombSpawnInterval);

            for (int i = 0; i < amountOfBombs; i++)
            {
                Vector3 positionAroundPlayer = player.transform.position;
                Vector3 randomPosition = Random.insideUnitCircle;
                positionAroundPlayer.x += randomPosition.x;
                positionAroundPlayer.y += randomPosition.y;
                listOfBombs.Add(Instantiate(bomb, positionAroundPlayer, Quaternion.identity));
            }
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(2);
        for(int i = 0; i < amountOfBombs; i++)
        {
            listOfExplosions.Add(Instantiate(explosion, listOfBombs[i].transform.position, Quaternion.identity));
            Destroy(listOfBombs[i]);
        }
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < amountOfBombs; i++)
        {
            Destroy(listOfExplosions[i]);
        }
        listOfBombs.Clear();
        listOfExplosions.Clear();
    }

    protected IEnumerator UpdateAnimation()
    {
        while (true)
        {
            enemyDirection.x = GetComponent<NavMeshAgent>().velocity.x;
            enemyDirection.y = GetComponent<NavMeshAgent>().velocity.y;

            //Debug.Log(enemyDirection);

            jackalAnimator.SetFloat("XMovement", enemyDirection.x);
            jackalAnimator.SetFloat("YMovement", enemyDirection.y);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
