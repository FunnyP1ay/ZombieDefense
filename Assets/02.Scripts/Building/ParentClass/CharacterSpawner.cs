using Lean.Pool;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public Character spawnPrefab;
    public Transform spwanPoint;
    public virtual Character CharacterSpawn()
    {
        return LeanPool.Spawn(spawnPrefab, spwanPoint.position,Quaternion.identity,null);
    }
}
