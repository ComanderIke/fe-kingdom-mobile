using System;
using Game.WorldMapStuff.Controller;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class Road: IEquatable<Road>
{
    private WorldMapPosition location1;
    private WorldMapPosition location2;
    private LineRenderer lineRenderer;//TODO

    public Road(WorldMapPosition location1, WorldMapPosition location2)
    {
        this.location1 = location1;
        this.location2 = location2;
        location1.AddRoad(this);
        location2.AddRoad(this);
    }

    public void Spawn(GameObject roadPrefab, Transform parent)
    {
        //Debug.Log("Spawn Road: "+location1.gameObject.name + " - " + location2.gameObject.name);
        var go = GameObject.Instantiate(roadPrefab, parent, false);
        lineRenderer = go.GetComponent<LineRenderer>();
        go.name = location1.gameObject.name + " - " + location2.gameObject.name;
        
        Draw();
    }

    public void Draw()
    {
        if(lineRenderer!=null)
            UpdateLineRenderer(lineRenderer, location1.transform.position, location2.transform.position );
    }
    private void UpdateLineRenderer(LineRenderer lineRenderer,Vector3 startPos, Vector3 endPos)
    {
        lineRenderer.positionCount = 2;
        Vector3[] pos = new Vector3[2];
        pos[0] = startPos;
        pos[1] = endPos;
        lineRenderer.SetPositions(pos);
    }
    
    public bool Equals(Road other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(location1, other.location1) && Equals(location2, other.location2);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Road) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((location1 != null ? location1.GetHashCode() : 0) * 397) ^ (location2 != null ? location2.GetHashCode() : 0);
        }
    }

    public bool Contains(WorldMapPosition connection)
    {
        return location1==connection|| location2==connection;
    }
    

    public bool Exists()
    {
        return lineRenderer != null && lineRenderer.gameObject != null;
    }
}