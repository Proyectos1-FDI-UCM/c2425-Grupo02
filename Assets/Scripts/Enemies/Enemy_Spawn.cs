//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;


/// <summary>
/// Clase que se encarga de definir un a zona en la que spawnean enemigos aleatoriamente.
/// Desde el editor se define una zona mediante un sprite, y se indican un tipo de enemigo, el número de enemigos
/// que se spawnean por iteración y el número de iteraciones.
/// Cada vez que se derrotan a todos los enemigos de una iteración salen a la vez todos los de la siguiente iteración
/// </summary>
public class Enemy_Spawn : MonoBehaviour {

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Enemigo que se va a instanciar
    /// </summary>
    [SerializeField] GameObject Enemy;
    /// <summary>
    /// Número de enemigos que se van a spawnear
    /// </summary>
    [SerializeField] int EnemyNumber;
    /// <summary>
    /// Número de iteraciones del spawn
    /// </summary>
    [SerializeField] int Iterations;
    /// <summary>
    /// Lista con las celdas en las que no pueden spawnear enemigos
    /// </summary>
    [SerializeField] List<Vector2Int> BannedCells = new();

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Sprite de la zona del spawn
    /// </summary>
    SpriteRenderer _sprite;
    /// <summary>
    /// Cuadrícula que delimita la zona de spawn
    /// </summary>
    CustomGrid _grid;
    /// <summary>
    /// Diccionario que asigna a un índice a cada celda en la que puede spawnear un enemigo
    /// </summary>
    Dictionary<int, Vector2> _cellDict;
    /// <summary>
    /// Lista de celdas en las que no puede spawnear un enemigo
    /// </summary>
    HashSet<Vector2Int> _bannedCells = new();
    /// <summary>
    /// Universo de celdas disponibles, para calcular la pool de celdas disponibles en las que pueden instanciarse enemigos.
    /// Esto es para que no salgan dos enemigos en la misma celda
    /// </summary>
    List<int> _universe;
    /// <summary>
    /// Ancho de la cuadrícula
    /// </summary>
    int _gridWidth; 
    /// <summary>
    /// Alto de la cuadrícula
    /// </summary>
    int _gridLength;   
    /// <summary>
    /// Iteración actual del spawn (empieza siempre en 0)
    /// </summary>
    int _currentIteration = 0; 
    /// <summary>
    /// Enemigos vivos spawneados por el spawn
    /// </summary>
    int _currentEnemies;                       
    
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se obtienen los componentes del objeto necesarios y se limita el número de enemigos a spawnear para que no se rompa.
    /// También se calcula el diccionario de celdas disponibles
    /// </summary>
    void Start() {
        _sprite = GetComponent<SpriteRenderer>();
        _gridWidth = Mathf.FloorToInt(_sprite.size.x);
        _gridLength = Mathf.FloorToInt(_sprite.size.y);
        _sprite.forceRenderingOff = true;

        _grid = new CustomGrid(_gridWidth, _gridLength, 1, gameObject);
        if (EnemyNumber >= _gridWidth * _gridLength) EnemyNumber = _gridWidth * _gridLength;
        if (EnemyNumber == _gridWidth * _gridLength) EnemyNumber -= BannedCells.Count;
        if (EnemyNumber < 0) EnemyNumber = 0;
        _bannedCells = BannedCells.ToHashSet();
        SetDict();

        //Debug
        SpawnEnemies();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Resta un enemigo al número de enemigos actuales. Si se ha completado la última iteración, se desactiva el spawn
    /// </summary>
    public void SubstractEnemy() {
        _currentEnemies--;
        if (_currentEnemies <= 0 && _currentIteration < Iterations) SpawnEnemies();
        else if (_currentIteration >= Iterations) gameObject.SetActive(false);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Método que se encarga de instanciar enemigos y avanzar de iteración
    /// </summary>
    void SpawnEnemies() {
        List<int> spawnList = SpawnList();
        foreach (int n in spawnList)
        {
            Vector2 pos = _cellDict[n];
            GameObject enemy = Instantiate(Enemy, pos, Quaternion.identity);
            enemy.GetComponent<Enemy_Health>().SetSpawn(gameObject.GetComponent<Enemy_Spawn>());
        }
        _currentEnemies = EnemyNumber;
        _currentIteration++;
    }

    /*
    /// <summary>
    /// Método que se encarga de instanciar enemigos y avanzar de iteración
    /// </summary>
    void SpawnEnemies() {
        List<int> spawnList = SpawnList();
        foreach (int n in spawnList)
        {
            Vector2 pos = _cellDict[n];
            GameObject enemy = Instantiate(Enemy, pos, Quaternion.identity);
            enemy.GetComponent<Enemy_Health>().SetSpawn(gameObject.GetComponent<Enemy_Spawn>());
        }
        _currentEnemies = EnemyNumber;
        _currentIteration++;
    }
     */

    /// <summary>
    /// Método que dado un número de enemigos devuelve un arrray de enteros con los índices a los que deberá acceder la función
    /// SpawnEnemies en _tileDict para saber en qué tiles instanciar a los enemigos
    ///
    /// También se encarga de obtener una lista con todas las celdas que no se encuentren en _bannedCells (universo)
    /// </summary>
    /// <param name="enemy_n"> número de enemigos que se van a instanciar </param>
    /// <returns> array de enteros con tantos índices de _tileDict como enemigos se hayan indicado </returns>
    List<int> SpawnList() {
        List<int> res = new List<int>(_cellDict.Count);
        
        for (int i = 0; i < EnemyNumber; i++)
        {
            List<int> random_pool = GetComplementary(res, _universe);
            int index = Random.Range(0, random_pool.Count);
            res.Add(random_pool[index]);
        }
        return res;
    }

    /// <summary>
    /// Método que se encarga de obtener un diccionario que asigna un índice a cada celda que no se encuentre en _bannedCells
    /// </summary>
    void SetDict() {
        _cellDict = new Dictionary<int, Vector2>();
        _universe = new List<int>();

        int it = 0;
        for (int i = 0; i < _gridWidth; i++)
        {
            for(int j = 0;  j < _gridLength; j++)
            {
                if (!_bannedCells.Contains(new Vector2Int(i, j)))
                {
                    _cellDict[it] = _grid.GetCells()[i, j];
                    _universe.Add(it);
                    it++;
                }
            }
        }
    }

    /// <summary>
    /// Método que se encarga de obtener la lista complementaria de una lista respecto a un universo
    /// </summary>
    /// <param name="list"> Lista de la que se va a calcular la complementaria </param>
    /// <param name="U"> Lista universo </param>
    /// <returns></returns>
    List<int> GetComplementary(List<int> list, List<int> U) {
        List<int> res = U.Except(list).ToList();
        return res;
    }

    #endregion   

} // class SpawnScript 
// namespace
