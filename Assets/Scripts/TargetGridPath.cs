using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TargetGridPath : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private IList<Vector3Int> _path;

    public IList<Vector3Int> path
    {
        set
        {
            _path = value != null ? value : new List<Vector3Int>();
            UpdateLinePath();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (!lineRenderer)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        Debug.Assert(lineRenderer != null, "LineRenderer NOT FOUND!");

        UpdateLinePath();
    }

    public void UpdateLinePath()
    {
        if (!lineRenderer)
        {
            return;
        }

        var numPoints = _path.Count;
        var gameGrid = FindObjectOfType<GameGrid>();
        var points = new Vector3[numPoints];
        for (int i = 0; i < numPoints; i += 1)
        {
            points[i] = gameGrid.CellToWorld(_path[i]);
            points[i].z = 0.1f;
        }


        var gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );

        //var material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line");
        var material = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.positionCount = numPoints;
        lineRenderer.colorGradient = gradient;
        lineRenderer.material = material;
        lineRenderer.SetPositions(points);
    }
}
