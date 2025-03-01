//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Spawn : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] GameObject enemy;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    Tilemap _tileMap;                       //Componente TileMap del Spawn
    Dictionary<int, Vector3> _tileDict;     //Diccionario con la posición de cada tile asociada a un índice
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _tileMap = GetComponent<Tilemap>();
        _tileDict = new Dictionary<int, Vector3>();

        int it = 0;
        foreach (Vector3Int pos in _tileMap.cellBounds.allPositionsWithin)
        {
            _tileDict[it] = _tileMap.CellToWorld(pos);
            it++;
        }

        //Debug
        SpawnEnemies();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que se encarga de instanciar enemigos
    /// </summary>
    public void SpawnEnemies() {
        HashSet<int> spawnList = SpawnList(3);

        foreach (int n in spawnList) {
            Vector3 pos = _tileDict[n];
            Instantiate(enemy, pos, Quaternion.identity);
        }
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

   /// <summary>
   /// Método que dado un número de enemigos devuelve un arrray de enteros con los índices a los que deberá acceder la función
   /// SpawnEnemies en _tileDict para saber en qué tiles instanciar a los enemigos
   /// </summary>
   /// <param name="enemy_n"> número de enemigos que se van a instanciar </param>
   /// <returns> array de enteros con tantos índices de _tileDict como enemigos se hayan indicado </returns>
    HashSet<int> SpawnList(int enemy_n) {
        int size = _tileDict.Keys.Count;
        if (enemy_n > size) throw new Exception("No se pueden instanciar más enemigos que espcaios disponibles");

        HashSet<int> res = new HashSet<int>();
        for (int i = 0; i < enemy_n; i++)
        {
            int n = UnityEngine.Random.Range(0, size);
            res.Add(n);
        }
        return res;
    }

    #endregion   

} // class Spawn 
// namespace
