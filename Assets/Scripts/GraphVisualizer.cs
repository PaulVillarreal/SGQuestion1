using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVisualizer : MonoBehaviour
{
  [SerializeField]
  LineRenderer lineRenderer;

  /// <summary>
  /// Renders the line to match the provided data
  /// </summary>
  /// <param name="data">The dataset to be displayed</param>
  /// <param name="highestEntry">The amount of the highest point on graph to scale down the y of the points</param>
  public void InitLine(int[] data, float highestEntry)
  {
    lineRenderer.positionCount = data.Length;
    
    for (int i = 0; i < data.Length; ++i)
    {
      Vector3 newPosition = new Vector3(i * 0.5f, (data[i]/highestEntry) * 25, 0);
      lineRenderer.SetPosition(i, newPosition);
    }
  }
}
