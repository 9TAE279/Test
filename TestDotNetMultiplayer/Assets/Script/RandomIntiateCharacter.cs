using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIntiateCharacter : MonoBehaviour
{
   [SerializeField] private GameObject prefab;
   public void SpawnModel()
    {
        var position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        Instantiate(prefab, position, Quaternion.identity);
         }

}   
