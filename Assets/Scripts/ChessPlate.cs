using UnityEngine;

public class ChessPlate : MonoBehaviour
{
    [SerializeField] private int _height, _width;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Chess _chessPrefab;    

    [SerializeField] private Camera _camera;

    public void Initialize()
    {
        GeneratePlate();
    }

    private void GeneratePlate()
    {
        for (int x = 0; x < _height; x++)
        {
            for (int y = 0; y < _width; y++)
            {
                Cell spawnedCell = Instantiate(_cellPrefab, new Vector3(x, 0, y), Quaternion.identity, this.transform);

                spawnedCell.transform.name = "Cell" + x.ToString() + y.ToString();
                
                var isBlackCell = (x % 2 == 0 && y % 2 != 0) || (y % 2 == 0 && x % 2 != 0);

                spawnedCell.Initialize(isBlackCell);

                if (x < 2)
                    SpawnChess(spawnedCell, true);
                else if (x >= _height - 2)
                    SpawnChess(spawnedCell, false);

            }                             
        }

        _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)(_width + _height) / 2, (float)_height / 2 - 0.5f);
    }

    private void SpawnChess(Cell parentCell, bool isBlackChess)
    {
        Chess spawnedChess = Instantiate(_chessPrefab, parentCell.transform.position, Quaternion.identity, parentCell.transform);
        spawnedChess.Initialise(isBlackChess);
    }
}
