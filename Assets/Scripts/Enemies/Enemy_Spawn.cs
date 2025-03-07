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
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Enemy_Spawn : MonoBehaviour {

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] GameObject Enemy;                      //Enemigo qque se va a instanciar por tanda
    [SerializeField] int EnemyNumber;                       //Número de enemigos que se van a instanciar por tanda
    [SerializeField] int Iterations;                        //Número de tandas de enemigos que se quieren instanciar
    [SerializeField] List<Vector2Int> BannedCells = new();  //Lista con las celdas en las que no pueden aparecer enemigos

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    SpriteRenderer _sprite;
    CustomGrid _grid;                              //Cuadrícula
    Dictionary<int, Vector2> _cellDict;      //Diccionario con la posición de cada celda asociada a un índice
    List<int> _universe;                        //Lista con números del 0 a EnemyNumber
    HashSet<Vector2Int> _bannedCells = new();   //Conversióna set de BannedCells
    int _gridWidth;                     //Ancho de la cuadrícula que define la zona de spawn
    int _gridLength;                    //Alto de la cuadrícula que define la zona de spawn
    int _currentIteration = 0;                  //Número de tanda de enemigos
    int _currentEnemies;                        //Número de enemigos actualmente vivos en escena instanciados por este spawn
    //bool _firstEnabled = false;               //Booleano que indica si se ha activado la zone de spawn por primera vez

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
        SetUniverse();

        //Debug
        SpawnEnemies();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void SubstractEnemy() {
        _currentEnemies--;
        if (_currentEnemies <= 0 && _currentIteration < Iterations) SpawnEnemies();
        else if (_currentIteration >= Iterations) gameObject.SetActive(false);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Método que se encarga de instanciar enemigos
    /// </summary>
    void SpawnEnemies() {
        List<int> spawnList = SpawnList();
        foreach (int n in spawnList)
        {
            Vector2 pos = _cellDict[n];
            Debug.Log("Insatnciado en: " + pos);
            GameObject enemy = Instantiate(Enemy, pos, Quaternion.identity);
            enemy.GetComponent<Enemy_Health>().SetSpawn(gameObject.GetComponent<Enemy_Spawn>());
        }
        _currentEnemies = EnemyNumber;
        _currentIteration++;
    }

    /// <summary>
    /// Método que dado un número de enemigos devuelve un arrray de enteros con los índices a los que deberá acceder la función
    /// SpawnEnemies en _tileDict para saber en qué tiles instanciar a los enemigos
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

    void SetDict() {
        _cellDict = new Dictionary<int, Vector2>();

        int it = 0;
        for (int i = 0; i < _gridWidth; i++)
        {
            for(int j = 0;  j < _gridLength; j++)
            {
                if (!_bannedCells.Contains(new Vector2Int(i, j)))
                {
                    _cellDict[it] = _grid.GetCells()[i, j];
                    it++;
                }
            }
        }
    }

    void SetUniverse() {
        _universe = new List<int>(_cellDict.Count);
        for (int i = 0; i < _cellDict.Count; i++)
        {
            _universe.Add(i);
        }
    }

    List<int> GetComplementary(List<int> list, List<int> U) {
        List<int> res = U.Except(list).ToList();
        return res;
    }

    #endregion   

} // class SpawnScript 
// namespace
