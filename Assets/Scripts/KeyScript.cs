using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MirPlayer"))
        {
            // ”бираем все тайлы стены
            if (wallTilemap != null)
            {
                wallTilemap.ClearAllTiles();
            }
            // ”ничтожаем ключ
            Destroy(gameObject);
        }
    }
}
