//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Enemy_Spawn : MonoBehaviour {

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] GameObject Enemy;          //Enemigo qque se va a instanciar por tanda
    [SerializeField] int EnemyNumber;           //Número de enemigos que se van a instanciar por tanda

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    IntGrid _grid;                              //Componente TileMap del Spawn
    Dictionary<int, Vector2Int> _tileDict;      //Diccionario con la posición de cada tile asociada a un índice
    List<int> _universe;                        //Lista con números del 0 a EnemyNumber

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
    void Start() {
        _grid = new IntGrid(5, 5, 1);
        SetDict();
        SetUniverse();

        //Debug
        SpawnEnemies(EnemyNumber);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que se encarga de instanciar enemigos
    /// </summary>
    public void SpawnEnemies(int enemy_n) {
        List<int> spawnList = SpawnList(enemy_n);
        foreach (int n in spawnList)
        {
            Vector2 pos = _tileDict[n];
            Instantiate(Enemy, pos, Quaternion.identity);
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
    List<int> SpawnList(int enemy_n) {
        int size = _tileDict.Keys.Count;
        if (enemy_n > size) enemy_n = size;

        List<int> res = new List<int>();
        
        for (int i = 0; i < enemy_n; i++)
        {
            List<int> random_pool = GetComplementary(res, _universe);
            int index = Random.Range(0, random_pool.Count);
            res.Add(random_pool[index]);
        }
        return res;
    }

    void SetDict() {
        _tileDict = new Dictionary<int, Vector2Int>();

        int it = 0;
        foreach (Vector2Int pos in _grid.GetCells())
        {
            _tileDict[it] = pos;
            it++;
        }
    }

    //Aqui tengo que definir qué celdas no son válidas
    void SetUniverse() {
        _universe = new List<int>();
        for (int i = 0; i < _tileDict.Count; i++)
        {
            _universe.Add(i);
        }
    }

    List<int> GetComplementary(List<int> list, List<int> U) {
        List<int> res = new List<int>();
        res =  U.Except(list).ToList();
        return res;
    }

    #endregion   

} // class SpawnScript 
// namespace
