using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoGrid : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject nodePrefab;

    [Header("Parameters")]
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float cellSize;
    [SerializeField] float gap;

    [SerializeField] Vector3 firstAxis;
    [SerializeField] Vector3 secondAxis;

    Node[,] nodes; // [width, height]
    GameObject nodesContainer;

    private void Awake()
    {
        // Assert proper values
        if (width <= 0 || height <= 0)
        {
            Debug.LogError("Error. Width and Height must be positive integers.");
        }

        if (gap < 0)
        {
            Debug.LogError("Error. Gap should be positive");
        }

        if (firstAxis == secondAxis || firstAxis.magnitude != 1 || secondAxis.magnitude != 1)
        {
            Debug.LogError("Error. There must be 2 axis set to 1");
        }

        // Cache some values for efficient generation
        Vector3 firstOffset = gap * firstAxis;
        Vector3 secondOffset = gap * secondAxis;

        Vector3 firstScaledCell = cellSize * firstAxis;
        Vector3 secondScaledCell = cellSize * secondAxis;

        // Position nodeContainer so that nodes are center relative to the IsoGrid transform. This simplifies a lot of future math.
        nodesContainer = new GameObject("NodeContainer");
        nodesContainer.transform.parent = transform;
        nodesContainer.transform.localPosition = ((width - 1) * firstScaledCell + (width - 1) * firstOffset + (height - 1) * secondScaledCell + (height - 1) * secondOffset) / -2;

        // Create nodes and populate nodes array
        nodes = new Node[width, height];
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                nodes[w, h] = Instantiate(nodePrefab, nodesContainer.transform). GetComponent<Node>();
                nodes[w, h].transform.localPosition = firstScaledCell * w + secondScaledCell * h + firstOffset * w + secondOffset * h;
            }
        }
    }


}
