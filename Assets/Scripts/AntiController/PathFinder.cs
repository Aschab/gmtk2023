using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NeighborRules
{
    public float radius;
    public float maxHeight;
    public float downAngle;
}

public class PathFinder : MonoBehaviour
{

    public GameObject[] _nodes;
    public GameObject _goal;
    public NeighborRules rules = new NeighborRules();


    // Start is called before the first frame update
    void Awake()
    {
        _goal = GameObject.FindWithTag("Goal");
        PathFind();
    }

    private void PathFind()
    {
        _nodes = GameObject.FindGameObjectsWithTag("Node");
        Map map = new Map(_nodes, transform.position, _goal.transform.position, rules);
    }

}


public class Node
{
    public Node parentNode;
    public List<Node> neighbors;

    public Vector2 worldPosition;

    public Node(Vector2 worldPosition)
    {
        this.worldPosition = worldPosition;
        neighbors = new List<Node>();
    }


}

public class Map
{
    public List<Node> nodes;
    public Node start;
    public Node end;

    public Map(GameObject[] _nodes, Vector2 s, Vector2 e, NeighborRules rules)
    {
        start = new Node(s);
        end = new Node(e);

        nodes = new List<Node>();
        nodes.Add(start);
        nodes.Add(end);

        foreach (GameObject n in _nodes)
        {
            nodes.Add(new Node(n.transform.position));
        }

        foreach (Node no in nodes) 
        {
            foreach (Node no2 in nodes) 
            {
                if (no != no2) continue;

                float x = no2.worldPosition.x - no.worldPosition.x;
                float y = no2.worldPosition.y - no.worldPosition.y;

                if (x*x + y*y < rules.radius)
                {
                    no.neighbors.Add(no2);
                    continue;
                }
            }
        }
    }
}