using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Line {
    // Fields for JSON file
    public Vector3 colour;
    public Vector3[] Positions;



    private Color Colour;
    private Vector3Storer[] SerializablePositions;
    private Vector2[] PositionsForDatabase;



    public Line(Color drawnColour, Vector3[] positions)
    {
        
        Colour = drawnColour;
        Positions = positions;
        PositionsForDatabase = MyVector3Extension.toVector2(positions);
        SerializePositions();
        colour.x = Colour.r;
        colour.y = Colour.g;
        colour.z = Colour.b;
    }

    public void SerializePositions()
    {
        SerializablePositions = new Vector3Storer[Positions.Length];
        for (int i = 0; i < Positions.Length; i++)
        {
            float x = Positions[i].x;
            float y = Positions[i].y;
            float z = Positions[i].z;
            SerializablePositions[i] = new Vector3Storer(x, y, z);
        }
    }

}

// Source: https://answers.unity.com/questions/1034471/c-convert-vector3-to-vector2.html
public static class MyVector3Extension
{
    public static Vector2[] toVector2(this Vector3[] v3)
    {
        return System.Array.ConvertAll<Vector3, Vector2>(v3, getV3fromV2);
    }

    public static Vector2 getV3fromV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
}

[System.Serializable]
public class Vector3Storer
{
    public float x;
    public float y;
    public float z;

    public Vector3Storer(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

}