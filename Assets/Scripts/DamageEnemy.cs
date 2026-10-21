using UnityEngine;
using System;

public class DamageEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy in range");
            other.GetComponent<EnemyController>().SetState(new InRangeState());
        }
    }
}
