//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que define una cuadrícula conformada por celdas.
/// Las celdas se guardan en un array bidimensional en el que los índices coinciden con la posición relativa
/// de cada celda en la cuadrícula.
/// Cada celda contiene un vector con las coordenadas (globales) de su centro
/// </summary>
public class CustomGrid {

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    Vector2 _origin;             //Vector con la posición de origen local
    int _width;                     //Anchura de la cuadrícula
    int _length;                    //Altura de la cuadrícula
    int _cellSize;                  //Tamaño de cada celda del grid
    Vector2[,] _cells;           //Array bidimiensional con la posición central de cada celda en coordenadas locales o globales, dependiendo del constructor

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
    public CustomGrid(int width, int length, int cellSize) {
        _width = width;
        _length = length;
        _cellSize = cellSize;
        _cells = new Vector2[width, length];

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
    public CustomGrid(int width, int length, int cellSize, GameObject gameObject) {
        Vector2 org = gameObject.transform.position;
        _origin = new(org.x, org.y);
        _width = width;
        _length = length;
        _cellSize = cellSize;
        _cells = new Vector2[width, length];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                _cells[i, j] = GetLocalPos(i, j);
            }
        }
    }

    //Getters

    public Vector2 GetOrigin() {
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

    public Vector2[,] GetCells() {
        return _cells;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private Vector2 GetWorldPos(float x, float y) {
        x += _cellSize * 0.5f;
        y += _cellSize * 0.5f;
        return new Vector2(x, y) * _cellSize;
    }

    private Vector2 GetLocalPos(float x, float y) {
        x += _origin.x + _cellSize * 0.5f;
        y += _origin.y + _cellSize * 0.5f;
        return new Vector2(x, y) * _cellSize;
    }

    #endregion   

} // class Grid 
// namespace
