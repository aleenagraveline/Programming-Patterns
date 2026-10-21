using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    [SerializeField] float moveSpeed = 2f;
    private EnemyState currentState;
    private bool inRange;

    public void SetState(EnemyState state)
    {
        currentState = state;
    }
    public void Update()
    {
        currentState.Execute(this);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Spawn()
    {
        gameObject.SetActive(true);  // Spawned enemies should reactivate
        SetState(new TrackState());
    }

    public void Die()
    {
        if(IsInRange())
        {
            gameObject.SetActive(false);
            player.gameObject.GetComponent<Player>().GainPoints(100);
        }
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }

    public Transform GetPlayer()
    {
        return player;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetInRange(bool inRange)
    {
        this.inRange = inRange;
    }

    public bool IsInRange()
    {
        return inRange;
    }

} 
    public abstract class EnemyState
    {
        public abstract void Execute(EnemyController enemy);
    }

    public class TrackState : EnemyState
    {
        public override void Execute(EnemyController enemy)
        {
            enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            enemy.GetPlayer().position,
            enemy.GetMoveSpeed() * Time.deltaTime
            );

            enemy.SetInRange(false);
        }
    }
    public class InRangeState : EnemyState
    {
        public override void Execute(EnemyController enemy)
        {
            enemy.transform.position = Vector3.MoveTowards(
                enemy.transform.position,
                enemy.GetPlayer().position,
                enemy.GetMoveSpeed() * 1.5f * Time.deltaTime
                );
            enemy.SetInRange(true);
        }
    }
