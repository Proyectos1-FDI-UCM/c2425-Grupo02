//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class IntGrid {

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    Vector2Int _origin;             //Vector con la posición de origen local
    int _width;                     //Anchura de la cuadrícula
    int _length;                    //Altura de la cuadrícula
    int _cellSize;                  //Tamaño de cada celda del grid
    Vector2Int[,] _cells;           //Array bidimiensional con la posición central de cada celda en coordenadas locales o globales, dependiendo del constructor

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    //Constructor

    /// <summary>
    /// Constructor no asociado a un GameObject. Las coordenadas de las celdas son globales
    /// </summary>
    /// <param name="width"> Ancho de la cuadrícula </param>
    /// <param name="length"> Alto de la cuadrícula </param>
    /// <param name="cellSize"> Tamaño de cada celda </param>
    public IntGrid(int width, int length, int cellSize) {
        _width = width;
        _length = length;
        _cellSize = cellSize;
        _cells = new Vector2Int[width, length];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                _cells[i, j] = GetWorldPos(i, j);
            }
        }
    }

    /// <summary>
    /// Constructor asociado a un GameObject. Las posiciones de las celdas son locales del GameObject
    /// </summary>
    /// <param name="width"> Ancho de la cuadrícula </param>
    /// <param name="length"> Alto de la cuadrícula </param>
    /// <param name="cellSize"> Tamaño de cada celda </param>
    /// <param name="gameObject"> GameObject que contendrá la cuadrícula </param>
    public IntGrid(int width, int length, int cellSize, GameObject gameObject) {
        Vector3 org = gameObject.transform.position;
        _origin = new Vector2Int(Mathf.FloorToInt(org.x), Mathf.FloorToInt(org.y));
        _width = width;
        _length = length;
        _cellSize = cellSize;
        _cells = new Vector2Int[width, length];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                _cells[i, j] = GetLocalPos(i, j);
            }
        }
    }

    public void DebugGrid() {

    }

    //Getters

    public Vector2Int GetOrigin() {
        return _origin;
    }

    public int GetWidth() {
        return _width;
    }

    public int GetHeight() {
        return _length;
    }

    public int GetCellSize() {
        return _cellSize;
    }

    public Vector2Int[,] GetCells() {
        return _cells;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private Vector2Int GetWorldPos(int x, int y) {
        x += _cellSize / 2;
        y += _cellSize / 2;
        return new Vector2Int(x, y) * _cellSize;
    }

    private Vector2Int GetLocalPos(int x, int y) {
        x += _origin.x + _cellSize / 2; 
        y += _origin.y + _cellSize / 2;
        return new Vector2Int(x, y) * _cellSize;
    }

    #endregion   

} // class Grid 
// namespace
