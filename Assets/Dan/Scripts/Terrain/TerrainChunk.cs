using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour
{

    public int xIndex = 0;
    public int yIndex = 0;

    int startX;
    int startY;

    int endX;
    int endY;

    List<Vector3> normals;
    List<Vector3> vertices;
    List<MarchingSquares> squares;

    public MeshFilter terrain;
    List<ControlNodule> nodules;

    List<int> tri = new List<int>();
    List<MarchingTriangle> triangles = new List<MarchingTriangle>();

    public void Start()
    {
        //GenerateMesh();
    }
    public void Construct(int _xIndex, int _yIndex)
    {
        if (_xIndex != -1)
            xIndex = _xIndex;
        if (_yIndex != -1)
            yIndex = _yIndex;
        if (terrain == null) terrain = GetComponent<MeshFilter>();

        if (TerrainManager.tm.map != null)
            GenerateMesh();
    }

    private MarchingSquares ActivateControlNodes(MarchingSquares _square, int x, int y)
    {
        bool wasZero = false;

        bool aSurrounded = false;
        bool bSurrounded = false;
        bool cSurrounded = false;
        bool dSurrounded = false;

        _square.a = false;
        _square.b = false;
        _square.c = false;
        _square.d = false;

        //if (TerrainManager.tm.map[x, y] == 0)
        //    return _square;     

        if (TerrainManager.tm.map[x, y] == 0)
            wasZero = true;

        if (x == 0)
        {
            _square.d = true;
            _square.a = true;
        }
        else if (x == TerrainManager.tm.map.GetLength(0) - 1)
        {
            _square.c = true;
            _square.b = true;
        }
        if (y == 0)
        {
            _square.d = true;
            _square.c = true;
        }
        else if (y == TerrainManager.tm.map.GetLength(1) - 1)
        {
            _square.b = true;
            _square.a = true;
        }

        bool diagonal = false;
        bool vertical = false;
        bool horizontal = false;


        if (!_square.a)
        {
            if (TerrainManager.tm.map[x - 1, y + 1] == 1)
                diagonal = true;
            else
                diagonal = false;

            if (TerrainManager.tm.map[x, y + 1] == 1)
                vertical = true;
            else
                vertical = false;

            if (TerrainManager.tm.map[x - 1, y] == 1)
                horizontal = true;
            else
                horizontal = false;

            _square.a = diagonal;
            if (!diagonal && vertical && horizontal)
                _square.a = true;
            if (diagonal && vertical && horizontal)
                aSurrounded = true;
        }

        if (!_square.b)
        {
            if (TerrainManager.tm.map[x + 1, y + 1] == 1)
                diagonal = true;
            else
                diagonal = false;

            if (TerrainManager.tm.map[x, y + 1] == 1)
                vertical = true;
            else
                vertical = false;

            if (TerrainManager.tm.map[x + 1, y] == 1)
                horizontal = true;
            else
                horizontal = false;

            _square.b = diagonal;
            if (!diagonal && vertical && horizontal)
                _square.b = true;
            if (diagonal && vertical && horizontal)
                bSurrounded = true;
        }

        if (!_square.c)
        {
            if (TerrainManager.tm.map[x + 1, y - 1] == 1)
                diagonal = true;
            else
                diagonal = false;

            if (TerrainManager.tm.map[x, y - 1] == 1)
                vertical = true;
            else
                vertical = false;

            if (TerrainManager.tm.map[x + 1, y] == 1)
                horizontal = true;
            else
                horizontal = false;

            _square.c = diagonal;
            if (!diagonal && vertical && horizontal)
                _square.c = true;
            if (diagonal && vertical && horizontal)
                cSurrounded = true;
        }

        if (!_square.d)
        {
            if (TerrainManager.tm.map[x - 1, y - 1] == 1)
                diagonal = true;
            else
                diagonal = false;

            if (TerrainManager.tm.map[x, y - 1] == 1)
                vertical = true;
            else
                vertical = false;

            if (TerrainManager.tm.map[x - 1, y] == 1)
                horizontal = true;
            else
                horizontal = false;

            _square.d = diagonal;
            if (!diagonal && vertical && horizontal)
                _square.d = true;
            if (diagonal && vertical && horizontal)
                dSurrounded = true;
        }



        if (wasZero)
        {
            if (((_square.a && !_square.b && !_square.c && !_square.d) && aSurrounded) || ((!_square.a && _square.b && !_square.c && !_square.d) && bSurrounded) || ((!_square.a && !_square.b && _square.c && !_square.d) && cSurrounded) || ((!_square.a && !_square.b && !_square.c && _square.d) && dSurrounded))
            {
                TerrainManager.tm.map[x, y] = 2;
            }
            else
            {
                _square.a = false;
                _square.b = false;
                _square.c = false;
                _square.d = false;
            }
        }

        return _square;
    }

    public void GenerateMesh()
    {
        startX = TerrainManager.tm.chunkSizeX * xIndex;
        startY = TerrainManager.tm.chunkSizeY * yIndex;

        endX = startX + TerrainManager.tm.chunkSizeX;
        endY = startY + TerrainManager.tm.chunkSizeY;

        tri = new List<int>();

        Mesh mesh = new Mesh();
        terrain.mesh = mesh;

        nodules = new List<ControlNodule>();

        squares = new List<MarchingSquares>();
        triangles = new List<MarchingTriangle>();

        int tempCount = 0;
        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {

                MarchingSquares tempSquare = new MarchingSquares();
                tempSquare = ActivateControlNodes(tempSquare, x, y);

                if (TerrainManager.tm.map[x, y] != 0)
                {
                    int chunkX = (x - (TerrainManager.tm.chunkSizeX * xIndex));// +  Mathf.FloorToInt(TerrainManager.tm.startPosition.x));
                    int chunkY = (y - (TerrainManager.tm.chunkSizeY * yIndex));// + Mathf.FloorToInt(TerrainManager.tm.startPosition.z)); ;
                    int o = -1;



                    //Top Left
                    Vector3 tempVec = new Vector3(chunkX, 0, chunkY);
                    tempSquare.D = new ControlNodule(tempVec, tempCount);
                    tempCount++;
                    //Top Right
                    tempVec = new Vector3(chunkX + 1, 0, chunkY);
                    tempSquare.C = new ControlNodule(tempVec, tempCount);
                    tempCount++;
                    //Bottom Right
                    tempVec = new Vector3(chunkX, 0, chunkY + 1);
                    tempSquare.A = new ControlNodule(tempVec, tempCount);
                    tempCount++;
                    //Bottom Left
                    tempVec = new Vector3(chunkX + 1, 0, chunkY + 1);
                    tempSquare.B = new ControlNodule(tempVec, tempCount);
                    tempCount++;
                    //DC
                    tempVec = new Vector3(chunkX + 0.5f, 0, chunkY);
                    tempSquare.DC = new ControlNodule(tempVec, tempCount);
                    tempCount++;
                    //CB
                    tempVec = new Vector3(chunkX + 1, 0, chunkY + 0.5f);
                    tempSquare.CB = new ControlNodule(tempVec, tempCount);
                    tempCount++;
                    //BA
                    tempVec = new Vector3(chunkX + 0.5f, 0, chunkY + 1);
                    tempSquare.BA = new ControlNodule(tempVec, tempCount);
                    tempCount++;
                    //AD
                    tempVec = new Vector3(chunkX, 0, chunkY + 0.5f);
                    tempSquare.AD = new ControlNodule(tempVec, tempCount);
                    tempCount++;

                    TriangulateSquare(tempSquare);

                    squares.Add(tempSquare);
                }


            }
        }

        vertices = new List<Vector3>();
        for (int i = 0; i < squares.Count; i++)
        {
            vertices.Add(squares[i].D.vertexPosition);
            vertices.Add(squares[i].C.vertexPosition);
            vertices.Add(squares[i].A.vertexPosition);
            vertices.Add(squares[i].B.vertexPosition);
            vertices.Add(squares[i].DC.vertexPosition);
            vertices.Add(squares[i].CB.vertexPosition);
            vertices.Add(squares[i].BA.vertexPosition);
            vertices.Add(squares[i].AD.vertexPosition);
        }



        //ExtrudeSquare();

        ExtrudeTriangle();












        mesh.vertices = vertices.ToArray();
        mesh.triangles = tri.ToArray();


        List<Vector3> normals = new List<Vector3>();
        for (int x = 0; x < vertices.Count; x++)
        {
            normals.Add(-Vector3.forward);
        }

        mesh.normals = normals.ToArray();

        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / TerrainManager.tm.chunkSizeX, vertices[i].z / TerrainManager.tm.chunkSizeY);
        }

        mesh.uv = uvs;

        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;


    }

    private void ExtrudeTriangle()
    {
        int currentIndex = vertices.Count;
        foreach (MarchingTriangle triangle in triangles)
        {
            Vector3 a, b, c, d;
            a = triangle.A.vertexPosition - new Vector3(0, 1f, 0);
            b = triangle.B.vertexPosition - new Vector3(0, 1f, 0);
            c = triangle.C.vertexPosition - new Vector3(0, 1f, 0);

            /* 
           A 0
           B 1
           C 2
           a 3
           b 4
           c 5            
           */
            int temp0, temp1, temp2, temp3, temp4, temp5;
            temp0 = triangle.A.index;
            temp1 = triangle.B.index;
            temp2 = triangle.C.index;
            temp3 = currentIndex;
            temp4 = currentIndex + 1;
            temp5 = currentIndex + 2;

            tri.Add(temp2); //C
            tri.Add(temp5); //c
            tri.Add(temp0); //A
            tri.Add(temp5); //c
            tri.Add(temp3); //a
            tri.Add(temp0); //A

            tri.Add(temp0); //A
            tri.Add(temp3); //a
            tri.Add(temp1); //B
            tri.Add(temp3); //a
            tri.Add(temp4); //b
            tri.Add(temp1); //B

            tri.Add(temp1); //B
            tri.Add(temp4); //b
            tri.Add(temp2); //C
            tri.Add(temp4); //b
            tri.Add(temp5); //c
            tri.Add(temp2); //C


            currentIndex += 3;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
        }
    }

    private void ExtrudeSquare()
    {
        int currentIndex = vertices.Count;
        foreach (MarchingSquares square in squares)
        {
            Vector3 a, b, c, d;
            a = square.A.vertexPosition - new Vector3(0, 1f, 0);
            b = square.B.vertexPosition - new Vector3(0, 1f, 0);
            c = square.C.vertexPosition - new Vector3(0, 1f, 0);
            d = square.D.vertexPosition - new Vector3(0, 1f, 0);

            /* 
             A 0
             B 1
             C 2
             D 3
             a 4
             b 5
             c 6
             d 7             
             */
            int temp0, temp1, temp2, temp3, temp4, temp5, temp6, temp7;
            temp0 = square.A.index;
            temp1 = square.B.index;
            temp2 = square.C.index;
            temp3 = square.D.index;
            temp4 = currentIndex;
            temp5 = currentIndex + 1;
            temp6 = currentIndex + 2;
            temp7 = currentIndex + 3;

            tri.Add(temp2); //C
            tri.Add(temp6); //c
            tri.Add(temp3); //D
            tri.Add(temp6); //c
            tri.Add(temp7); //d
            tri.Add(temp3); //D

            tri.Add(temp1); //B
            tri.Add(temp5); //b
            tri.Add(temp2); //C
            tri.Add(temp5); //b
            tri.Add(temp6); //c
            tri.Add(temp2); //C

            tri.Add(temp0); //A
            tri.Add(temp4); //a
            tri.Add(temp1); //B
            tri.Add(temp4); //a
            tri.Add(temp5); //b
            tri.Add(temp1); //B

            tri.Add(temp3); //D 
            tri.Add(temp7); //d
            tri.Add(temp0); //A
            tri.Add(temp7); //d
            tri.Add(temp4); //a
            tri.Add(temp0); //A


            currentIndex += 4;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);
        }
    }


    private void TriangulateSquare(MarchingSquares _square)
    {
        int value = ReturnSquareDataValue(_square);

        switch (value)
        {
            case 0:

                break;
            case 1:
                CreateTriangles(_square.AD, _square.A, _square.BA);
                break;
            case 2:
                CreateTriangles(_square.BA, _square.B, _square.CB);
                break;
            case 3:
                CreateTriangles(_square.AD, _square.A, _square.CB, _square.A, _square.B, _square.CB);
                break;
            case 4:
                CreateTriangles(_square.DC, _square.CB, _square.C);
                break;
            case 5:
                CreateTriangles(_square.AD, _square.A, _square.DC, _square.A, _square.C, _square.DC, _square.A, _square.CB, _square.C, _square.A, _square.BA, _square.CB);
                break;
            case 6:
                CreateTriangles(_square.DC, _square.BA, _square.C, _square.BA, _square.B, _square.C);
                break;
            case 7:
                CreateTriangles(_square.AD, _square.A, _square.DC, _square.A, _square.B, _square.DC, _square.B, _square.C, _square.DC);
                break;
            case 8:
                CreateTriangles(_square.D, _square.AD, _square.DC);
                break;
            case 9:
                CreateTriangles(_square.D, _square.A, _square.DC, _square.A, _square.BA, _square.DC);
                break;
            case 10:
                CreateTriangles(_square.D, _square.AD, _square.BA, _square.D, _square.BA, _square.B, _square.D, _square.B, _square.CB, _square.D, _square.CB, _square.DC);
                break;
            case 11:
                CreateTriangles(_square.D, _square.CB, _square.DC, _square.D, _square.A, _square.CB, _square.A, _square.B, _square.CB);
                break;
            case 12:
                CreateTriangles(_square.D, _square.AD, _square.C, _square.AD, _square.CB, _square.C);
                break;
            case 13:
                CreateTriangles(_square.D, _square.A, _square.C, _square.A, _square.CB, _square.C, _square.A, _square.BA, _square.CB);
                break;
            case 14:
                CreateTriangles(_square.D, _square.AD, _square.C, _square.AD, _square.B, _square.C, _square.AD, _square.BA, _square.B);
                break;
            case 15:
                CreateTriangles(_square.D, _square.A, _square.C, _square.A, _square.B, _square.C);
                break;
        }


    }

    private void CreateTriangles(params ControlNodule[] nodules)
    {
        for (int i = 0; i < nodules.Length; i += 3)
        {
            tri.Add(nodules[i].index);
            tri.Add(nodules[i + 1].index);
            tri.Add(nodules[i + 2].index);
            triangles.Add(new MarchingTriangle(nodules[i], nodules[i + 1], nodules[i + 2]));
        }
    }


    private int ReturnSquareDataValue(MarchingSquares _square)
    {
        int value = 0;
        if (_square.a)
            value += 1;
        if (_square.b)
            value += 2;
        if (_square.c)
            value += 4;
        if (_square.d)
            value += 8;
        return value;
    }

    int doesExist(Vector3 nodule)
    {
        foreach (ControlNodule item in nodules)
        {
            if (item.vertexPosition == nodule)
                return item.index;
        }
        return -1;
    }

    public void ForceDestroy(Collision collision, int radius = 1)
    {
        if (!TerrainManager.tm.destructible)
            return;
        //if (collision.gameObject.tag != TerrainManager.tm.tagWhichCanDestroy)
        //    return;
        if (collision.contacts.Length == 0)
            return;
        Vector3 contact = collision.contacts[0].point;
        int x = Mathf.FloorToInt(contact.x);
        int y = Mathf.FloorToInt(contact.z);

        x -= Mathf.FloorToInt(TerrainManager.tm.startPosition.x);
        y -= Mathf.FloorToInt(TerrainManager.tm.startPosition.z);

        if (y == TerrainManager.tm.map.GetLength(1))
            y--;

        if (x >= TerrainManager.tm.map.GetLength(0) || y >= TerrainManager.tm.map.GetLength(1))
        {
            return;
        }

        if (x < 0 || y < 0)
        {
            return;
        }

        for (int i = 0; i < radius; i++)
        {
            //Top
            if ((x + i) < TerrainManager.tm.map.GetLength(0))
            {
                TerrainManager.tm.map[x + i, y] = 0;
            }
            //Bottom
            if ((x - i) > 0)
            {
                TerrainManager.tm.map[x - i, y] = 0;
            }
            //Right
            if ((y + i) < TerrainManager.tm.map.GetLength(1))
            {
                TerrainManager.tm.map[x, y + i] = 0;
            }
            //Left
            if ((y - i) > 0)
            {
                TerrainManager.tm.map[x, y - i] = 0;
            }
            for (int j = 0; j < radius; j++)
            {
                //Top Right
                if ((x + i) < TerrainManager.tm.map.GetLength(0) && (y + j) < TerrainManager.tm.map.GetLength(1))
                {
                    TerrainManager.tm.map[x + i, y + j] = 0;
                }
                //Bottom Right
                if ((x + j) < TerrainManager.tm.map.GetLength(0) && (y - i) > 0)
                {
                    TerrainManager.tm.map[x + j, y - i] = 0;
                }
                //Bottom Left
                if ((x - j) > 0 && (y - i) > 0)
                {
                    TerrainManager.tm.map[x - j, y - i] = 0;
                }
                //Top Left
                if ((x - i) > 0 && (y + j) < TerrainManager.tm.map.GetLength(1))
                {
                    TerrainManager.tm.map[x - i, y + j] = 0;
                }
            }

        }
        TerrainManager.tm.RefreshChunks();
    }


    private void DestroyBehaviour(Collision collision)
    {
        if (!TerrainManager.tm.destructible)
            return;
        if (collision.gameObject.tag != TerrainManager.tm.tagWhichCanDestroy)
            return;
        if (collision.contacts.Length == 0)
            return;
        DestroyOnImpact impact = collision.gameObject.GetComponent<DestroyOnImpact>();
        Vector3 contact = collision.contacts[0].point;
        int x = Mathf.FloorToInt(contact.x);
        int y = Mathf.FloorToInt(contact.z);

        x -= Mathf.FloorToInt(TerrainManager.tm.startPosition.x);
        y -= Mathf.FloorToInt(TerrainManager.tm.startPosition.z);

        if (y == TerrainManager.tm.map.GetLength(1))
            y--;

        if (x >= TerrainManager.tm.map.GetLength(0) || y >= TerrainManager.tm.map.GetLength(1))
        {
            return;
        }

        if (x < 0 || y < 0)
        {
            return;
        }

        if (impact == null)
        {
            if (TerrainManager.tm.map[x, y] == 1)
            {
                TerrainManager.tm.map[x, y] = 0;
            }
            GenerateMesh();
        }
        else
        {
            if (!impact.destroyTerrain)
                return;
            int radius = impact.radius;
            for (int i = 0; i < radius; i++)
            {
                //Top
                if ((x + i) < TerrainManager.tm.map.GetLength(0))
                {
                    TerrainManager.tm.map[x + i, y] = 0;
                }
                //Bottom
                if ((x - i) > 0)
                {
                    TerrainManager.tm.map[x - i, y] = 0;
                }
                //Right
                if ((y + i) < TerrainManager.tm.map.GetLength(1))
                {
                    TerrainManager.tm.map[x, y + i] = 0;
                }
                //Left
                if ((y - i) > 0)
                {
                    TerrainManager.tm.map[x, y - i] = 0;
                }
                for (int j = 0; j < radius; j++)
                {
                    //Top Right
                    if ((x + i) < TerrainManager.tm.map.GetLength(0) && (y + j) < TerrainManager.tm.map.GetLength(1))
                    {
                        TerrainManager.tm.map[x + i, y + j] = 0;
                    }
                    //Bottom Right
                    if ((x + j) < TerrainManager.tm.map.GetLength(0) && (y - i) > 0)
                    {
                        TerrainManager.tm.map[x + j, y - i] = 0;
                    }
                    //Bottom Left
                    if ((x - j) > 0 && (y - i) > 0)
                    {
                        TerrainManager.tm.map[x - j, y - i] = 0;
                    }
                    //Top Left
                    if ((x - i) > 0 && (y + j) < TerrainManager.tm.map.GetLength(1))
                    {
                        TerrainManager.tm.map[x - i, y + j] = 0;
                    }
                }

            }
        }
        TerrainManager.tm.RefreshChunks();
    }  

    private void OnCollisionEnter(Collision collision)
    {
        DestroyBehaviour(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        DestroyBehaviour(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        DestroyBehaviour(collision);
    }

    

}

public class ControlNodule
{
    public Vector3 vertexPosition;
    public int index;
    public ControlNodule(Vector3 vertex, int _index)
    {
        vertexPosition = vertex;
        index = _index;
    }
}
public class MarchingSquares
{
    public ControlNodule A, B, C, D, DC, CB, BA, AD;
    public bool a, b, c, d;
}

public class ExtrudedSquare
{
    public ControlNodule A, B, C, D;
}

public class MarchingTriangle
{
    public ControlNodule A, B, C;
    public MarchingTriangle(ControlNodule _A, ControlNodule _B, ControlNodule _C)
    {
        A = _A;
        B = _B;
        C = _C;
    }
}