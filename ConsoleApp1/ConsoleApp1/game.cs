using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

public class SeasonWindow : GameWindow
{
    private int _vbo, _vao, _shaderProgram;
    private Matrix4 _view, _projection;
    private int _cubeVao, _cubeVbo, _cubeEbo;
    private int _roofVao, _roofVbo, _roofEbo;
    private int _windowVao, _windowVbo, _windowEbo;
    private int _interiorVao, _interiorVbo, _interiorEbo;
    private int _trunkVao, _trunkVbo, _trunkEbo;
    private int _crownVao, _crownVbo, _crownEbo;
    private int _mirrorLanternVao, _mirrorLanternVbo, _mirrorLanternNormalVbo, _mirrorLanternEbo;
    private float[] _trunkVertices;
    private uint[] _trunkIndices;
    private float[] _crownVertices;
    private uint[] _crownIndices;
    private Vector3[] _mirrorLanternVertices;
    private Vector3[] _mirrorLanternNormals;
    private uint[] _mirrorLanternIndices;


    private float[] _groundVertices = {
        // X     Y     Z
        -100f, -1f, -10f,
         100f, -1f, -10f,
         100f, -1f,  10f,
        -100f, -1f,  10f,
    };
    private float[] _cubeVertices = {

        -0.5f, -1.0f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f, -1.0f, -0.5f,  0.0f,  0.0f, -1.0f,
         0.5f,  0.0f, -0.5f,  0.0f,  0.0f, -1.0f,
        -0.5f,  0.0f, -0.5f,  0.0f,  0.0f, -1.0f,

        -0.5f, -1.0f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f, -1.0f,  0.5f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.0f,  0.5f,  0.0f,  0.0f,  1.0f,
        -0.5f,  0.0f,  0.5f,  0.0f,  0.0f,  1.0f,

        -0.5f, -1.0f, -0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f, -1.0f,  0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f,  0.0f,  0.5f, -1.0f,  0.0f,  0.0f,
        -0.5f,  0.0f, -0.5f, -1.0f,  0.0f,  0.0f,

         0.5f, -1.0f, -0.5f,  1.0f,  0.0f,  0.0f,
         0.5f, -1.0f,  0.5f,  1.0f,  0.0f,  0.0f,
         0.5f,  0.0f,  0.5f,  1.0f,  0.0f,  0.0f,
         0.5f,  0.0f, -0.5f,  1.0f,  0.0f,  0.0f,

        -0.5f, -1.0f, -0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -1.0f, -0.5f,  0.0f, -1.0f,  0.0f,
         0.5f, -1.0f,  0.5f,  0.0f, -1.0f,  0.0f,
        -0.5f, -1.0f,  0.5f,  0.0f, -1.0f,  0.0f,

        -0.5f,  0.0f, -0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.0f, -0.5f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.0f,  0.5f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.0f,  0.5f,  0.0f,  1.0f,  0.0f,
    };

    private uint[] _cubeIndices = {
        0, 1, 2, 2, 3, 0,
        4, 5, 6, 6, 7, 4,
        8, 9, 10, 10, 11, 8,
        12, 13, 14, 14, 15, 12,
        16, 17, 18, 18, 19, 16,
        20, 21, 22, 22, 23, 20
    };
    
    private float[] _roofVertices = {
        -0.55f, 0.0f, -0.5f, -0.29f,  0.58f,  0.1f,
         0.55f, 0.0f, -0.5f,  0.50f,  0.50f,  0.9f,
         0.55f, 0.0f,  0.5f,  0.29f,  0.58f, 0.41f,
        -0.55f, 0.0f,  0.5f, -0.50f,  0.50f,  0.00f,
         0.0f, 0.8f,  0.0f,  0.00f,  1.00f,  0.2f, 
    };

    private float[] _windowVertices = {

        0.05f, -0.8f, -0.501f,  0.0f, 0.0f, 1.0f,
        0.25f, -0.8f, -0.501f,  0.0f, 0.0f, 1.0f,
        0.25f, -0.5f, -0.501f,  0.0f, 0.0f, 1.0f,
        0.05f, -0.5f, -0.501f,  0.0f, 0.0f, 1.0f,
    };
    private float[] _interiorVertices = {
        0.06f, -0.78f, -0.49f,
        0.23f, -0.78f, -0.49f,
        0.23f, -0.52f, -0.49f,
        0.06f, -0.52f, -0.49f,
    };
    private uint[] _interiorIndices = {
        0, 1, 2,
        2, 3, 0
    };

    private uint[] _windowIndices = {
        0, 1, 2,
        2, 3, 0
    };

    private uint[] _roofIndices = {
        0, 1, 4,
        1, 2, 4,
        2, 3, 4,
        3, 0, 4
    };

    private uint[] _groundIndices = {
        0, 1, 2,
        2, 3, 0
    };

    private int _ebo;



    private enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }

    private Season _currentSeason = Season.Summer;

    private readonly Dictionary<Season, Vector4> _groundColors = new Dictionary<Season, Vector4>
    {
        { Season.Spring, new Vector4(0.3f, 0.7f, 0.3f, 1f) },
        { Season.Summer, new Vector4(0.2f, 0.6f, 0.2f, 1f) },
        { Season.Autumn, new Vector4(0.5f, 0.3f, 0.1f, 1f) },
        { Season.Winter, new Vector4(0.9f, 0.9f, 0.9f, 1f) }
    };

    private readonly Dictionary<Season, Vector4> _crownColors = new Dictionary<Season, Vector4>
    {
        { Season.Spring, new Vector4(0.4f, 0.8f, 0.4f, 1f) },
        { Season.Summer, new Vector4(0.1f, 0.5f, 0.1f, 1f) },
        { Season.Autumn, new Vector4(0.8f, 0.4f, 0.0f, 1f) },
        { Season.Winter, new Vector4(0.7f, 0.7f, 0.7f, 1f) }
    };
    private struct Snowflake
    {
        public Vector3 Position;
        public float FallSpeed;
        public float SwayPhase;
    }

    private Snowflake[] _snowflakes;
    private int _snowflakeVao, _snowflakeVbo;
    private float[] _snowflakeVertices;
    private const int SnowflakeCount = 100;


    private int _fenceVao, _fenceVbo, _fenceEbo;
    private float[] _fenceVertices;
    private uint[] _fenceIndices;

    private int _bushVao, _bushVbo, _bushEbo;
    private float[] _bushVertices;
    private uint[] _bushIndices;

    private readonly Dictionary<Season, Vector4> _bushColors = new Dictionary<Season, Vector4>
    {
        { Season.Spring, new Vector4(0.5f, 0.9f, 0.5f, 1f) }, 
        { Season.Summer, new Vector4(0.2f, 0.6f, 0.2f, 1f) },
        { Season.Autumn, new Vector4(0.9f, 0.5f, 0.0f, 1f) }, 
        { Season.Winter, new Vector4(0.0f, 0.0f, 0.0f, 0f) }
    };


    private int _lanternPostVao, _lanternPostVbo, _lanternPostEbo;
    private float[] _lanternPostVertices;
    private uint[] _lanternPostIndices;

    private int _lanternGlassVao, _lanternGlassVbo, _lanternGlassEbo;
    private float[] _lanternGlassVertices;
    private uint[] _lanternGlassIndices;


    private bool _lanternIsLit = false; // Состояние фонаря (вкл/выкл)

    private float _seasonTimer = 0.0f; // Таймер для смены сезона (в секундах)
    private const float SeasonChangeInterval = 10.0f; 

    private int _cubemapFbo; 
    private int _cubemapTexture; 
    private int _cubemapDepthRbo; 
    private readonly int _cubemapSize = 512; 


    public SeasonWindow()
        : base(GameWindowSettings.Default, new NativeWindowSettings
        {
            Size = new Vector2i(800, 600),
            Title = "Дом и времена года"
        })
    { }
    private void SetupTree()
    {
        // Ствол (сужающийся цилиндр) 
        const int segments = 12;
        float bottomRadius = 0.15f;  
        float topRadius = 0.06f; 
        float height = 1.5f;  
        float bottomY = -1.0f;
        float topY = -1.0f + height;
        float centerX = -1.8f;
        float centerZ = 0.0f;

        _trunkVertices = new float[segments * 2 * 6];
        for (int i = 0; i < segments; i++)
        {
            float theta = 2.0f * MathF.PI * i / segments;
            float cosTheta = MathF.Cos(theta);
            float sinTheta = MathF.Sin(theta);

            _trunkVertices[i * 6] = centerX + bottomRadius * cosTheta;
            _trunkVertices[i * 6 + 1] = bottomY;
            _trunkVertices[i * 6 + 2] = centerZ + bottomRadius * sinTheta;
            float nx = cosTheta;
            float ny = (bottomRadius - topRadius) / height;
            float nz = sinTheta;
            float len = MathF.Sqrt(nx * nx + ny * ny + nz * nz);
            _trunkVertices[i * 6 + 3] = nx / len;
            _trunkVertices[i * 6 + 4] = ny / len;
            _trunkVertices[i * 6 + 5] = nz / len;

            _trunkVertices[(i + segments) * 6] = centerX + topRadius * cosTheta;
            _trunkVertices[(i + segments) * 6 + 1] = topY;
            _trunkVertices[(i + segments) * 6 + 2] = centerZ + topRadius * sinTheta;
            _trunkVertices[(i + segments) * 6 + 3] = nx / len;
            _trunkVertices[(i + segments) * 6 + 4] = ny / len;
            _trunkVertices[(i + segments) * 6 + 5] = nz / len;
        }

        _trunkIndices = new uint[segments * 6];
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            _trunkIndices[i * 6] = (uint)i;
            _trunkIndices[i * 6 + 1] = (uint)(i + segments);
            _trunkIndices[i * 6 + 2] = (uint)next;
            _trunkIndices[i * 6 + 3] = (uint)next;
            _trunkIndices[i * 6 + 4] = (uint)(i + segments);
            _trunkIndices[i * 6 + 5] = (uint)(next + segments);
        }

        _trunkVao = GL.GenVertexArray();
        _trunkVbo = GL.GenBuffer();
        _trunkEbo = GL.GenBuffer();

        GL.BindVertexArray(_trunkVao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _trunkVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _trunkVertices.Length * sizeof(float), _trunkVertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _trunkEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _trunkIndices.Length * sizeof(uint), _trunkIndices, BufferUsageHint.StaticDraw);

        var posLocTrunk = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLocTrunk, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLocTrunk);

        var normLocTrunk = GL.GetAttribLocation(_shaderProgram, "aNormal");
        GL.VertexAttribPointer(normLocTrunk, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normLocTrunk);

        // Крона (сфера)
        const int latSegments = 12;
        const int lonSegments = 12;
        float radius = 0.45f; 
        float centerY = 0.45f; 

        List<float> crownVerts = new List<float>();
        List<uint> crownInds = new List<uint>();
        for (int lat = 0; lat <= latSegments; lat++)
        {
            float theta = MathF.PI * lat / latSegments;
            float sinTheta = MathF.Sin(theta);
            float cosTheta = MathF.Cos(theta);
            for (int lon = 0; lon < lonSegments; lon++)
            {
                float phi = 2.0f * MathF.PI * lon / lonSegments;
                float sinPhi = MathF.Sin(phi);
                float cosPhi = MathF.Cos(phi);

                float x = cosPhi * sinTheta;
                float y = cosTheta;
                float z = sinPhi * sinTheta;

                crownVerts.Add(centerX + radius * x);
                crownVerts.Add(centerY + radius * y);
                crownVerts.Add(centerZ + radius * z);
                crownVerts.Add(x);
                crownVerts.Add(y);
                crownVerts.Add(z);
            }
        }

        for (int lat = 0; lat < latSegments; lat++)
        {
            for (int lon = 0; lon < lonSegments; lon++)
            {
                uint current = (uint)(lat * lonSegments + lon);
                uint next = (uint)(lat * lonSegments + (lon + 1) % lonSegments);
                uint below = (uint)((lat + 1) * lonSegments + lon);
                uint belowNext = (uint)((lat + 1) * lonSegments + (lon + 1) % lonSegments);

                crownInds.Add(current);
                crownInds.Add(below);
                crownInds.Add(next);

                crownInds.Add(next);
                crownInds.Add(below);
                crownInds.Add(belowNext);
            }
        }

        _crownVertices = crownVerts.ToArray();
        _crownIndices = crownInds.ToArray();

        _crownVao = GL.GenVertexArray();
        _crownVbo = GL.GenBuffer();
        _crownEbo = GL.GenBuffer();

        GL.BindVertexArray(_crownVao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _crownVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _crownVertices.Length * sizeof(float), _crownVertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _crownEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _crownIndices.Length * sizeof(uint), _crownIndices, BufferUsageHint.StaticDraw);

        var posLocCrown = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLocCrown, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLocCrown);

        var normLocCrown = GL.GetAttribLocation(_shaderProgram, "aNormal");
        GL.VertexAttribPointer(normLocCrown, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normLocCrown);
    }
    private void SetupMirrorLantern()
    {
        _mirrorLanternVertices = new Vector3[]
        {

            new Vector3(-0.1f, -0.1f,  0.1f), new Vector3( 0.1f, -0.1f,  0.1f),
            new Vector3( 0.1f,  0.1f,  0.1f), new Vector3(-0.1f,  0.1f,  0.1f),

            new Vector3(-0.1f, -0.1f, -0.1f), new Vector3(-0.1f,  0.1f, -0.1f),
            new Vector3( 0.1f,  0.1f, -0.1f), new Vector3( 0.1f, -0.1f, -0.1f),

            new Vector3(-0.1f, -0.1f, -0.1f), new Vector3(-0.1f, -0.1f,  0.1f),
            new Vector3(-0.1f,  0.1f,  0.1f), new Vector3(-0.1f,  0.1f, -0.1f),

            new Vector3( 0.1f, -0.1f,  0.1f), new Vector3( 0.1f, -0.1f, -0.1f),
            new Vector3( 0.1f,  0.1f, -0.1f), new Vector3( 0.1f,  0.1f,  0.1f),

            new Vector3(-0.1f,  0.1f,  0.1f), new Vector3( 0.1f,  0.1f,  0.1f),
            new Vector3( 0.1f,  0.1f, -0.1f), new Vector3(-0.1f,  0.1f, -0.1f),

            new Vector3(-0.1f, -0.1f, -0.1f), new Vector3( 0.1f, -0.1f, -0.1f),
            new Vector3( 0.1f, -0.1f,  0.1f), new Vector3(-0.1f, -0.1f,  0.1f)
        };

        _mirrorLanternNormals = new Vector3[]
        {
            new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1),
            new Vector3(0, 0, -1), new Vector3(0, 0, -1), new Vector3(0, 0, -1), new Vector3(0, 0, -1),
            new Vector3(-1, 0, 0), new Vector3(-1, 0, 0), new Vector3(-1, 0, 0), new Vector3(-1, 0, 0),
            new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 0),
            new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0),
            new Vector3(0, -1, 0), new Vector3(0, -1, 0), new Vector3(0, -1, 0), new Vector3(0, -1, 0)
        };

        _mirrorLanternIndices = new uint[]
        {
            0, 1, 2, 2, 3, 0,
            4, 5, 6, 6, 7, 4,
            8, 9, 10, 10, 11, 8,
            12, 13, 14, 14, 15, 12,
            16, 17, 18, 18, 19, 16,
            20, 21, 22, 22, 23, 20
        };

        _mirrorLanternVao = GL.GenVertexArray();
        _mirrorLanternVbo = GL.GenBuffer();
        _mirrorLanternNormalVbo = GL.GenBuffer();
        _mirrorLanternEbo = GL.GenBuffer();

        GL.BindVertexArray(_mirrorLanternVao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _mirrorLanternVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _mirrorLanternVertices.Length * Vector3.SizeInBytes, _mirrorLanternVertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _mirrorLanternNormalVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _mirrorLanternNormals.Length * Vector3.SizeInBytes, _mirrorLanternNormals, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(1);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _mirrorLanternEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _mirrorLanternIndices.Length * sizeof(uint), _mirrorLanternIndices, BufferUsageHint.StaticDraw);

        int posLoc = GL.GetAttribLocation(_shaderProgram, "aPosition");
        int normLoc = GL.GetAttribLocation(_shaderProgram, "aNormal");

        GL.BindVertexArray(0);
    }
    private void SetupFenceAndBushes()
    {
        const int postCount = 12;
        float postWidth = 0.05f;
        float postHeight = 0.5f;
        float postDepth = 0.05f;
        float fenceRadius = 1.5f;

        List<float> fenceVerts = new List<float>();
        List<uint> fenceInds = new List<uint>();
        uint vertexOffset = 0;

        Vector3[] postCenters = new Vector3[postCount];
        for (int i = 0; i < postCount; i++)
        {
            float angle = 2.0f * MathF.PI * i / postCount;
            postCenters[i] = new Vector3(MathF.Cos(angle) * fenceRadius, -1.0f + postHeight / 2, MathF.Sin(angle) * fenceRadius);
        }

        // Столбы
        for (int i = 0; i < postCount; i++)
        {
            float angle = 2.0f * MathF.PI * i / postCount;
            float centerX = MathF.Cos(angle) * fenceRadius;
            float centerZ = MathF.Sin(angle) * fenceRadius;
            float yBottom = -1.0f;
            float yTop = yBottom + postHeight;

            float[] postVertices = {
            centerX - postWidth, yBottom, centerZ - postDepth,  0.0f,  0.0f, -1.0f,
            centerX + postWidth, yBottom, centerZ - postDepth,  0.0f,  0.0f, -1.0f,
            centerX + postWidth, yTop,    centerZ - postDepth,  0.0f,  0.0f, -1.0f,
            centerX - postWidth, yTop,    centerZ - postDepth,  0.0f,  0.0f, -1.0f,
            centerX - postWidth, yBottom, centerZ + postDepth,  0.0f,  0.0f,  1.0f,
            centerX + postWidth, yBottom, centerZ + postDepth,  0.0f,  0.0f,  1.0f,
            centerX + postWidth, yTop,    centerZ + postDepth,  0.0f,  0.0f,  1.0f,
            centerX - postWidth, yTop,    centerZ + postDepth,  0.0f,  0.0f,  1.0f,
            centerX - postWidth, yBottom, centerZ - postDepth, -1.0f,  0.0f,  0.0f,
            centerX - postWidth, yBottom, centerZ + postDepth, -1.0f,  0.0f,  0.0f,
            centerX - postWidth, yTop,    centerZ + postDepth, -1.0f,  0.0f,  0.0f,
            centerX - postWidth, yTop,    centerZ - postDepth, -1.0f,  0.0f,  0.0f,
            centerX + postWidth, yBottom, centerZ - postDepth,  1.0f,  0.0f,  0.0f,
            centerX + postWidth, yBottom, centerZ + postDepth,  1.0f,  0.0f,  0.0f,
            centerX + postWidth, yTop,    centerZ + postDepth,  1.0f,  0.0f,  0.0f,
            centerX + postWidth, yTop,    centerZ - postDepth,  1.0f,  0.0f,  0.0f,
            centerX - postWidth, yBottom, centerZ - postDepth,  0.0f, -1.0f,  0.0f,
            centerX + postWidth, yBottom, centerZ - postDepth,  0.0f, -1.0f,  0.0f,
            centerX + postWidth, yBottom, centerZ + postDepth,  0.0f, -1.0f,  0.0f,
            centerX - postWidth, yBottom, centerZ + postDepth,  0.0f, -1.0f,  0.0f,
            centerX - postWidth, yTop, centerZ - postDepth,  0.0f,  1.0f,  0.0f,
            centerX + postWidth, yTop, centerZ - postDepth,  0.0f,  1.0f,  0.0f,
            centerX + postWidth, yTop, centerZ + postDepth,  0.0f,  1.0f,  0.0f,
            centerX - postWidth, yTop, centerZ + postDepth,  0.0f,  1.0f,  0.0f,
        };

            uint[] postIndices = {
                0, 1, 2, 2, 3, 0,
                4, 5, 6, 6, 7, 4,
                8, 9, 10, 10, 11, 8,
                12, 13, 14, 14, 15, 12,
                16, 17, 18, 18, 19, 16,
                20, 21, 22, 22, 23, 20
            };

            fenceVerts.AddRange(postVertices);
            fenceInds.AddRange(postIndices.Select(idx => idx + vertexOffset));
            vertexOffset += 24;
        }

        // Перегородки
        float panelHeight = 0.4f;
        float panelThickness = 0.05f; // Увеличенная толщина
        for (int i = 0; i < postCount; i++)
        {
            Vector3 start = postCenters[i];
            Vector3 end = postCenters[(i + 1) % postCount];
            float yBottom = -1.0f;
            float yTop = yBottom + panelHeight;

            // Вершины панели (между start и end)
            float[] panelVertices = {

                start.X, yBottom, start.Z - panelThickness/2,  0.0f,  0.0f, -1.0f,
                end.X,   yBottom, end.Z - panelThickness/2,    0.0f,  0.0f, -1.0f,
                end.X,   yTop,    end.Z - panelThickness/2,    0.0f,  0.0f, -1.0f,
                start.X, yTop,    start.Z - panelThickness/2,  0.0f,  0.0f, -1.0f,

                start.X, yBottom, start.Z + panelThickness/2,  0.0f,  0.0f,  1.0f,
                end.X,   yBottom, end.Z + panelThickness/2,    0.0f,  0.0f,  1.0f,
                end.X,   yTop,    end.Z + panelThickness/2,    0.0f,  0.0f,  1.0f,
                start.X, yTop,    start.Z + panelThickness/2,  0.0f,  0.0f,  1.0f,

                start.X, yBottom, start.Z - panelThickness/2, -1.0f,  0.0f,  0.0f,
                start.X, yBottom, start.Z + panelThickness/2, -1.0f,  0.0f,  0.0f,
                start.X, yTop,    start.Z + panelThickness/2, -1.0f,  0.0f,  0.0f,
                start.X, yTop,    start.Z - panelThickness/2, -1.0f,  0.0f,  0.0f,

                end.X, yBottom, end.Z - panelThickness/2,  1.0f,  0.0f,  0.0f,
                end.X, yBottom, end.Z + panelThickness/2,  1.0f,  0.0f,  0.0f,
                end.X, yTop,    end.Z + panelThickness/2,  1.0f,  0.0f,  0.0f,
                end.X, yTop,    end.Z - panelThickness/2,  1.0f,  0.0f,  0.0f,

                start.X, yBottom, start.Z - panelThickness/2,  0.0f, -1.0f,  0.0f,
                end.X,   yBottom, end.Z - panelThickness/2,    0.0f, -1.0f,  0.0f,
                end.X,   yBottom, end.Z + panelThickness/2,    0.0f, -1.0f,  0.0f,
                start.X, yBottom, start.Z + panelThickness/2,  0.0f, -1.0f,  0.0f,

                start.X, yTop, start.Z - panelThickness/2,  0.0f,  1.0f,  0.0f,
                end.X,   yTop, end.Z - panelThickness/2,    0.0f,  1.0f,  0.0f,
                end.X,   yTop, end.Z + panelThickness/2,    0.0f,  1.0f,  0.0f,
                start.X, yTop, start.Z + panelThickness/2,  0.0f,  1.0f,  0.0f,
            };

            uint[] panelIndices = {
                0, 1, 2, 2, 3, 0,
                4, 5, 6, 6, 7, 4,
                8, 9, 10, 10, 11, 8,
                12, 13, 14, 14, 15, 12,
                16, 17, 18, 18, 19, 16,
                20, 21, 22, 22, 23, 20
            };

            fenceVerts.AddRange(panelVertices);
            fenceInds.AddRange(panelIndices.Select(idx => idx + vertexOffset));
            vertexOffset += 24;
;
        }


        _fenceVertices = fenceVerts.ToArray();
        _fenceIndices = fenceInds.ToArray();

        _fenceVao = GL.GenVertexArray();
        _fenceVbo = GL.GenBuffer();
        _fenceEbo = GL.GenBuffer();

        GL.BindVertexArray(_fenceVao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _fenceVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _fenceVertices.Length * sizeof(float), _fenceVertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _fenceEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _fenceIndices.Length * sizeof(uint), _fenceIndices, BufferUsageHint.StaticDraw);

        var posLocFence = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLocFence, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLocFence);

        var normLocFence = GL.GetAttribLocation(_shaderProgram, "aNormal");
        GL.VertexAttribPointer(normLocFence, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normLocFence);

        const int latSegments = 8;
        const int lonSegments = 8;
        float bushRadius = 0.2f;
        Vector3[] bushCenters = {
            new Vector3(-0.5f, -0.8f, 0.8f),
            new Vector3(0.5f, -0.8f, 0.8f),
        }; ;

        List<float> bushVerts = new List<float>();
        List<uint> bushInds = new List<uint>();
        vertexOffset = 0;

        foreach (var center in bushCenters)
        {
            for (int lat = 0; lat <= latSegments; lat++)
            {
                float theta = MathF.PI * lat / latSegments;
                float sinTheta = MathF.Sin(theta);
                float cosTheta = MathF.Cos(theta);
                for (int lon = 0; lon < lonSegments; lon++)
                {
                    float phi = 2.0f * MathF.PI * lon / lonSegments;
                    float sinPhi = MathF.Sin(phi);
                    float cosPhi = MathF.Cos(phi);

                    float x = cosPhi * sinTheta;
                    float y = cosTheta;
                    float z = sinPhi * sinTheta;

                    bushVerts.Add(center.X + bushRadius * x);
                    bushVerts.Add(center.Y + bushRadius * y);
                    bushVerts.Add(center.Z + bushRadius * z);
                    bushVerts.Add(x);
                    bushVerts.Add(y);
                    bushVerts.Add(z);
                }
            }

            for (int lat = 0; lat < latSegments; lat++)
            {
                for (int lon = 0; lon < lonSegments; lon++)
                {
                    uint current = (uint)(lat * lonSegments + lon) + vertexOffset;
                    uint next = (uint)(lat * lonSegments + (lon + 1) % lonSegments) + vertexOffset;
                    uint below = (uint)((lat + 1) * lonSegments + lon) + vertexOffset;
                    uint belowNext = (uint)((lat + 1) * lonSegments + (lon + 1) % lonSegments) + vertexOffset;

                    bushInds.Add(current);
                    bushInds.Add(below);
                    bushInds.Add(next);

                    bushInds.Add(next);
                    bushInds.Add(below);
                    bushInds.Add(belowNext);
                }
            }
            vertexOffset += (uint)((latSegments + 1) * lonSegments);
        }

        _bushVertices = bushVerts.ToArray();
        _bushIndices = bushInds.ToArray();

        _bushVao = GL.GenVertexArray();
        _bushVbo = GL.GenBuffer();
        _bushEbo = GL.GenBuffer();

        GL.BindVertexArray(_bushVao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _bushVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _bushVertices.Length * sizeof(float), _bushVertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bushEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _bushIndices.Length * sizeof(uint), _bushIndices, BufferUsageHint.StaticDraw);

        var posLocBush = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLocBush, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLocBush);

        var normLocBush = GL.GetAttribLocation(_shaderProgram, "aNormal");
        GL.VertexAttribPointer(normLocBush, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normLocBush);

        GL.BindVertexArray(0);
    }
    private void SetupLantern()
    {
        //Столб фонаря (цилиндр)
        const int segments = 16;
        float postRadius = 0.05f;
        float postHeight = 0.7f;
        Vector3 postCenter = new Vector3(-0.75f, -0.5f, 0.5f);

        List<float> postVerts = new List<float>();
        List<uint> postInds = new List<uint>();

        for (int i = 0; i < segments; i++)
        {
            float theta = 2.0f * MathF.PI * i / segments;
            float x = postRadius * MathF.Cos(theta);
            float z = postRadius * MathF.Sin(theta);

            postVerts.Add(postCenter.X + x);
            postVerts.Add(postCenter.Y);
            postVerts.Add(postCenter.Z + z);
            postVerts.Add(x / postRadius);
            postVerts.Add(0.0f);
            postVerts.Add(z / postRadius);

            postVerts.Add(postCenter.X + x);
            postVerts.Add(postCenter.Y + postHeight);
            postVerts.Add(postCenter.Z + z);
            postVerts.Add(x / postRadius);
            postVerts.Add(0.0f);
            postVerts.Add(z / postRadius);
        }

        for (int i = 0; i < segments; i++)
        {
            uint i0 = (uint)(i * 2);
            uint i1 = (uint)((i * 2 + 1));
            uint i2 = (uint)(((i + 1) % segments) * 2);
            uint i3 = (uint)(((i + 1) % segments) * 2 + 1);

            postInds.Add(i0);
            postInds.Add(i1);
            postInds.Add(i3);
            postInds.Add(i0);
            postInds.Add(i3);
            postInds.Add(i2);
        }

        uint centerBottom = (uint)postVerts.Count / 6;
        postVerts.Add(postCenter.X);
        postVerts.Add(postCenter.Y);
        postVerts.Add(postCenter.Z);
        postVerts.Add(0.0f);
        postVerts.Add(-1.0f);
        postVerts.Add(0.0f);

        uint centerTop = (uint)(centerBottom + 1);
        postVerts.Add(postCenter.X);
        postVerts.Add(postCenter.Y + postHeight);
        postVerts.Add(postCenter.Z);
        postVerts.Add(0.0f);
        postVerts.Add(1.0f);
        postVerts.Add(0.0f);

        for (int i = 0; i < segments; i++)
        {
            uint i0 = (uint)(i * 2);
            uint i1 = (uint)(((i + 1) % segments) * 2);
            postInds.Add(centerBottom);
            postInds.Add(i1);
            postInds.Add(i0);

            uint i2 = (uint)(i * 2 + 1);
            uint i3 = (uint)(((i + 1) % segments) * 2 + 1);
            postInds.Add(centerTop);
            postInds.Add(i2);
            postInds.Add(i3);
        }

        _lanternPostVertices = postVerts.ToArray();
        _lanternPostIndices = postInds.ToArray();

        _lanternPostVao = GL.GenVertexArray();
        _lanternPostVbo = GL.GenBuffer();
        _lanternPostEbo = GL.GenBuffer();

        GL.BindVertexArray(_lanternPostVao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _lanternPostVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _lanternPostVertices.Length * sizeof(float), _lanternPostVertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _lanternPostEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _lanternPostIndices.Length * sizeof(uint), _lanternPostIndices, BufferUsageHint.StaticDraw);

        var posLocPost = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLocPost, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLocPost);

        var normLocPost = GL.GetAttribLocation(_shaderProgram, "aNormal");
        GL.VertexAttribPointer(normLocPost, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normLocPost);

        //Стеклянная часть (куб)
        float glassSize = 0.2f;
        float glassY = postCenter.Y + postHeight;

        float[] glassVertices = {

            postCenter.X - glassSize/2, glassY, postCenter.Z + glassSize/2,  0.0f,  0.0f,  1.0f,
            postCenter.X + glassSize/2, glassY, postCenter.Z + glassSize/2,  0.0f,  0.0f,  1.0f,
            postCenter.X + glassSize/2, glassY + glassSize, postCenter.Z + glassSize/2,  0.0f,  0.0f,  1.0f,
            postCenter.X - glassSize/2, glassY + glassSize, postCenter.Z + glassSize/2,  0.0f,  0.0f,  1.0f,

            postCenter.X - glassSize/2, glassY, postCenter.Z - glassSize/2,  0.0f,  0.0f, -1.0f,
            postCenter.X + glassSize/2, glassY, postCenter.Z - glassSize/2,  0.0f,  0.0f, -1.0f,
            postCenter.X + glassSize/2, glassY + glassSize, postCenter.Z - glassSize/2,  0.0f,  0.0f, -1.0f,
            postCenter.X - glassSize/2, glassY + glassSize, postCenter.Z - glassSize/2,  0.0f,  0.0f, -1.0f,

            postCenter.X - glassSize/2, glassY, postCenter.Z - glassSize/2, -1.0f,  0.0f,  0.0f,
            postCenter.X - glassSize/2, glassY, postCenter.Z + glassSize/2, -1.0f,  0.0f,  0.0f,
            postCenter.X - glassSize/2, glassY + glassSize, postCenter.Z + glassSize/2, -1.0f,  0.0f,  0.0f,
            postCenter.X - glassSize/2, glassY + glassSize, postCenter.Z - glassSize/2, -1.0f,  0.0f,  0.0f,

            postCenter.X + glassSize/2, glassY, postCenter.Z - glassSize/2,  1.0f,  0.0f,  0.0f,
            postCenter.X + glassSize/2, glassY, postCenter.Z + glassSize/2,  1.0f,  0.0f,  0.0f,
            postCenter.X + glassSize/2, glassY + glassSize, postCenter.Z + glassSize/2,  1.0f,  0.0f,  0.0f,
            postCenter.X + glassSize/2, glassY + glassSize, postCenter.Z - glassSize/2,  1.0f,  0.0f,  0.0f,

            postCenter.X - glassSize/2, glassY, postCenter.Z - glassSize/2,  0.0f, -1.0f,  0.0f,
            postCenter.X + glassSize/2, glassY, postCenter.Z - glassSize/2,  0.0f, -1.0f,  0.0f,
            postCenter.X + glassSize/2, glassY, postCenter.Z + glassSize/2,  0.0f, -1.0f,  0.0f,
            postCenter.X - glassSize/2, glassY, postCenter.Z + glassSize/2,  0.0f, -1.0f,  0.0f,

            postCenter.X - glassSize/2, glassY + glassSize, postCenter.Z - glassSize/2,  0.0f,  1.0f,  0.0f,
            postCenter.X + glassSize/2, glassY + glassSize, postCenter.Z - glassSize/2,  0.0f,  1.0f,  0.0f,
            postCenter.X + glassSize/2, glassY + glassSize, postCenter.Z + glassSize/2,  0.0f,  1.0f,  0.0f,
            postCenter.X - glassSize/2, glassY + glassSize, postCenter.Z + glassSize/2,  0.0f,  1.0f,  0.0f,
        };

        uint[] glassIndices = {
            0, 1, 2, 2, 3, 0,
            4, 5, 6, 6, 7, 4,
            8, 9, 10, 10, 11, 8,
            12, 13, 14, 14, 15, 12,
            16, 17, 18, 18, 19, 16,
            20, 21, 22, 22, 23, 20
        };

        _lanternGlassVertices = glassVertices;
        _lanternGlassIndices = glassIndices;

        _lanternGlassVao = GL.GenVertexArray();
        _lanternGlassVbo = GL.GenBuffer();
        _lanternGlassEbo = GL.GenBuffer();

        GL.BindVertexArray(_lanternGlassVao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _lanternGlassVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _lanternGlassVertices.Length * sizeof(float), _lanternGlassVertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _lanternGlassEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _lanternGlassIndices.Length * sizeof(uint), _lanternGlassIndices, BufferUsageHint.StaticDraw);

        var posLocGlass = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLocGlass, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLocGlass);

        var normLocGlass = GL.GetAttribLocation(_shaderProgram, "aNormal");
        GL.VertexAttribPointer(normLocGlass, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normLocGlass);

        GL.BindVertexArray(0);
    }
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        GL.ClearColor(0.5f, 0.8f, 0.92f, 1.0f);

        _shaderProgram = CreateShaderFromFiles("../../../Shaders/shader.vert", "../../../Shaders/shader.frag");

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _groundVertices.Length * sizeof(float), _groundVertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _groundIndices.Length * sizeof(uint), _groundIndices, BufferUsageHint.StaticDraw);

        var positionLocation = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(positionLocation);

        _interiorVao = GL.GenVertexArray();
        _interiorVbo = GL.GenBuffer();
        _interiorEbo = GL.GenBuffer();

        GL.BindVertexArray(_interiorVao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _interiorVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _interiorVertices.Length * sizeof(float), _interiorVertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _interiorEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _interiorIndices.Length * sizeof(uint), _interiorIndices, BufferUsageHint.StaticDraw);

        var posLoc_i = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLoc_i, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLoc_i);

        _cubeVao = GL.GenVertexArray();
        _cubeVbo = GL.GenBuffer();
        _cubeEbo = GL.GenBuffer();

        GL.BindVertexArray(_cubeVao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _cubeVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _cubeVertices.Length * sizeof(float), _cubeVertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _cubeEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _cubeIndices.Length * sizeof(uint), _cubeIndices, BufferUsageHint.StaticDraw);

        var posLoc = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLoc, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLoc);

        var normLoc = GL.GetAttribLocation(_shaderProgram, "aNormal");
        GL.VertexAttribPointer(normLoc, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normLoc);


        _roofVao = GL.GenVertexArray();
        _roofVbo = GL.GenBuffer();
        _roofEbo = GL.GenBuffer();

        GL.BindVertexArray(_roofVao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _roofVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _roofVertices.Length * sizeof(float), _roofVertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _roofEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _roofIndices.Length * sizeof(uint), _roofIndices, BufferUsageHint.StaticDraw);

        var posLoc_r = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLoc_r, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLoc_r);

        var normLoc_r = GL.GetAttribLocation(_shaderProgram, "aNormal");
        GL.VertexAttribPointer(normLoc_r, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normLoc_r);



        _windowVao = GL.GenVertexArray();
        _windowVbo = GL.GenBuffer();
        _windowEbo = GL.GenBuffer();

        GL.BindVertexArray(_windowVao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _windowVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _windowVertices.Length * sizeof(float), _windowVertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _windowEbo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _windowIndices.Length * sizeof(uint), _windowIndices, BufferUsageHint.StaticDraw);

        var posLoc_w = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLoc_w, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLoc_w);

        var normLoc_w = GL.GetAttribLocation(_shaderProgram, "aNormal");
        GL.VertexAttribPointer(normLoc_w, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(normLoc_w);




        _cubemapFbo = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _cubemapFbo);

        _cubemapTexture = GL.GenTexture();
        GL.BindTexture(TextureTarget.TextureCubeMap, _cubemapTexture);
        for (int i = 0; i < 6; i++)
        {
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, _cubemapSize, _cubemapSize, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        }
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

        _cubemapDepthRbo = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _cubemapDepthRbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, _cubemapSize, _cubemapSize);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, _cubemapDepthRbo);

        if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
        {
            Console.WriteLine("Framebuffer is not complete!");
        }

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);








        _snowflakes = new Snowflake[SnowflakeCount];
        Random rand = new Random();
        for (int i = 0; i < SnowflakeCount; i++)
        {
            _snowflakes[i] = new Snowflake
            {
                Position = new Vector3(
                    (float)(rand.NextDouble() * 20 - 10), 
                    (float)(rand.NextDouble() * 5 + 5), 
                    (float)(rand.NextDouble() * 20 - 10) 
                ),
                FallSpeed = (float)(rand.NextDouble() * 0.5 + 0.2), 
                SwayPhase = (float)(rand.NextDouble() * Math.PI * 2) 
            };
        }

        _snowflakeVertices = new float[SnowflakeCount * 3]; 
        _snowflakeVao = GL.GenVertexArray();
        _snowflakeVbo = GL.GenBuffer();

        GL.BindVertexArray(_snowflakeVao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _snowflakeVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _snowflakeVertices.Length * sizeof(float), _snowflakeVertices, BufferUsageHint.DynamicDraw);

        var posLocSnow = GL.GetAttribLocation(_shaderProgram, "aPosition");
        GL.VertexAttribPointer(posLocSnow, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(posLocSnow);
        GL.BindVertexArray(0);

        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100f);
        _view = Matrix4.LookAt(
            new Vector3(-1, 1.3f, 4), 
            new Vector3(-0.3f, 0, 0), 
            Vector3.UnitY    
        );

        SetupFenceAndBushes();
        SetupLantern();
        SetupMirrorLantern();
        SetupTree();
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);
        if ((int)e.Key == 32)
        {
            _currentSeason = (Season)(((int)_currentSeason + 1) % 4);
            Console.WriteLine($"Season changed to: {_currentSeason} (Manual)");
            _seasonTimer = 0.0f;
        }
        if ((int)e.Key == 49) // Клавиша 1
        {
            _lanternIsLit = !_lanternIsLit;
        }
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Enable(EnableCap.DepthTest);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(_shaderProgram);

        var viewLocation = GL.GetUniformLocation(_shaderProgram, "uView");
        var projLocation = GL.GetUniformLocation(_shaderProgram, "uProjection");
        var colorLocation = GL.GetUniformLocation(_shaderProgram, "uColor");
        var modelLocation = GL.GetUniformLocation(_shaderProgram, "uModel");
        var lightPosLocation = GL.GetUniformLocation(_shaderProgram, "uLightPos");
        var viewPosLocation = GL.GetUniformLocation(_shaderProgram, "uViewPos");
        var useLightingLocation = GL.GetUniformLocation(_shaderProgram, "uUseLighting");
        var specularStrengthLocation = GL.GetUniformLocation(_shaderProgram, "uSpecularStrength");
        var shininessLocation = GL.GetUniformLocation(_shaderProgram, "uShininess");
        var cubemapLocation = GL.GetUniformLocation(_shaderProgram, "uCubeMap");


        Vector3 cubePosition = new Vector3(0.75f, 0.135f, 0.75f);

        GL.Viewport(0, 0, _cubemapSize, _cubemapSize);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _cubemapFbo);

        Matrix4 cubeProj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), 1.0f, 0.1f, 100.0f);

        Matrix4[] cubeViews = new Matrix4[]
        {
            Matrix4.LookAt(cubePosition, cubePosition + new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)),  // +X
            Matrix4.LookAt(cubePosition, cubePosition + new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)), // -X
            Matrix4.LookAt(cubePosition, cubePosition + new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f)),  // +Y
            Matrix4.LookAt(cubePosition, cubePosition + new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f)), // -Y
            Matrix4.LookAt(cubePosition, cubePosition + new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, -1.0f, 0.0f)),  // +Z
            Matrix4.LookAt(cubePosition, cubePosition + new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, -1.0f, 0.0f))  // -Z
        };

        GL.UseProgram(_shaderProgram);
        GL.UniformMatrix4(projLocation, false, ref cubeProj);
        Vector3 lightPos = new Vector3(0f, 1.5f, 1.5f);
        GL.Uniform3(lightPosLocation, lightPos);
        Vector3 viewPos = new Vector3(-1.0f, 1.3f, 4.0f);
        GL.Uniform3(viewPosLocation, viewPos);

        for (int face = 0; face < 6; face++)
        {
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.TextureCubeMapPositiveX + face, _cubemapTexture, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UniformMatrix4(viewLocation, false, ref cubeViews[face]);

            Matrix4 model_ = Matrix4.Identity;
            GL.UniformMatrix4(modelLocation, false, ref model_);

            // Земля
            GL.Uniform4(colorLocation, _groundColors[_currentSeason]);
            GL.Uniform1(useLightingLocation, 0);
            GL.Uniform1(specularStrengthLocation, 0.0f);
            GL.Uniform1(shininessLocation, 16.0f);
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _groundIndices.Length, DrawElementsType.UnsignedInt, 0);

            // Дом
            GL.Uniform4(colorLocation, new Vector4(0.6f, 0.3f, 0.2f, 1f));
            GL.Uniform1(useLightingLocation, 2);
            GL.Uniform1(specularStrengthLocation, 0.3f);
            GL.Uniform1(shininessLocation, 16.0f);
            GL.BindVertexArray(_cubeVao);
            GL.DrawElements(PrimitiveType.Triangles, _cubeIndices.Length, DrawElementsType.UnsignedInt, 0);

            // Крыша
            GL.Uniform4(colorLocation, _currentSeason == Season.Winter ? new Vector4(0.9f, 0.9f, 0.9f, 1f) : new Vector4(0.7f, 0.1f, 0.1f, 1f));
            GL.Uniform1(useLightingLocation, 2);
            GL.Uniform1(specularStrengthLocation, 0.3f);
            GL.Uniform1(shininessLocation, 16.0f);
            GL.BindVertexArray(_roofVao);
            GL.DrawElements(PrimitiveType.Triangles, _roofIndices.Length, DrawElementsType.UnsignedInt, 0);

            // Ствол дерева
            GL.Uniform4(colorLocation, new Vector4(0.4f, 0.2f, 0.1f, 1f));
            GL.Uniform1(useLightingLocation, 2);
            GL.Uniform1(specularStrengthLocation, 0.3f);
            GL.Uniform1(shininessLocation, 16.0f);
            GL.BindVertexArray(_trunkVao);
            GL.DrawElements(PrimitiveType.Triangles, _trunkIndices.Length, DrawElementsType.UnsignedInt, 0);

            // Крона дерева
            GL.Uniform4(colorLocation, _crownColors[_currentSeason]);
            GL.Uniform1(useLightingLocation, 2);
            GL.Uniform1(specularStrengthLocation, 0.3f);
            GL.Uniform1(shininessLocation, 16.0f);
            GL.BindVertexArray(_crownVao);
            GL.DrawElements(PrimitiveType.Triangles, _crownIndices.Length, DrawElementsType.UnsignedInt, 0);

            // Забор
            GL.Uniform4(colorLocation, new Vector4(0.5f, 0.3f, 0.2f, 1f));
            GL.Uniform1(useLightingLocation, 2);
            GL.Uniform1(specularStrengthLocation, 0.3f);
            GL.Uniform1(shininessLocation, 16.0f);
            GL.BindVertexArray(_fenceVao);
            GL.DrawElements(PrimitiveType.Triangles, _fenceIndices.Length, DrawElementsType.UnsignedInt, 0);

            // Кусты
            GL.Uniform4(colorLocation, _bushColors[_currentSeason]);
            GL.Uniform1(useLightingLocation, 2);
            GL.Uniform1(specularStrengthLocation, 0.3f);
            GL.Uniform1(shininessLocation, 16.0f);
            GL.BindVertexArray(_bushVao);
            GL.DrawElements(PrimitiveType.Triangles, _bushIndices.Length, DrawElementsType.UnsignedInt, 0);

            // Фонарь
            GL.Uniform4(colorLocation, new Vector4(0.2f, 0.1f, 0.2f, 1f));
            GL.Uniform1(useLightingLocation, 2);
            GL.Uniform1(specularStrengthLocation, 0.3f);
            GL.Uniform1(shininessLocation, 16.0f);
            GL.BindVertexArray(_lanternPostVao);
            GL.DrawElements(PrimitiveType.Triangles, _lanternPostIndices.Length, DrawElementsType.UnsignedInt, 0);

        }


        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        GL.Viewport(0, 0, Size.X, Size.Y);

        GL.UniformMatrix4(viewLocation, false, ref _view);
        GL.UniformMatrix4(projLocation, false, ref _projection);

        // Позиция света 
        Vector3 lightPos_ = new Vector3(-0.3f, 1.5f, 1.5f);
        GL.Uniform3(lightPosLocation, lightPos_);

        // Позиция камеры 
        Vector3 viewPos_ = new Vector3(-1.0f, 1.3f, 4.0f);
        GL.Uniform3(viewPosLocation, viewPos_);

        Matrix4 model = Matrix4.Identity;
        GL.UniformMatrix4(modelLocation, false, ref model);

        // Земля
        GL.Uniform4(colorLocation, _groundColors[_currentSeason]);
        GL.Uniform1(useLightingLocation, 0);
        GL.Uniform1(specularStrengthLocation, 0.0f);
        GL.Uniform1(shininessLocation, 16.0f);
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _groundIndices.Length, DrawElementsType.UnsignedInt, 0);

        // Дом
        GL.Uniform4(colorLocation, new Vector4(0.6f, 0.3f, 0.2f, 1f));
        GL.Uniform1(useLightingLocation, 2);
        GL.Uniform1(specularStrengthLocation, 0.3f);
        GL.Uniform1(shininessLocation, 16.0f);
        GL.BindVertexArray(_cubeVao);
        GL.DrawElements(PrimitiveType.Triangles, _cubeIndices.Length, DrawElementsType.UnsignedInt, 0);

        // Крыша
        GL.Uniform4(colorLocation, _currentSeason == Season.Winter ? new Vector4(0.9f, 0.9f, 0.9f, 1f) : new Vector4(0.7f, 0.1f, 0.1f, 1f));
        GL.Uniform1(useLightingLocation, 2);
        GL.Uniform1(specularStrengthLocation, 0.3f);
        GL.Uniform1(shininessLocation, 16.0f);
        GL.BindVertexArray(_roofVao);
        GL.DrawElements(PrimitiveType.Triangles, _roofIndices.Length, DrawElementsType.UnsignedInt, 0);

        // Ствол дерева
        GL.Uniform4(colorLocation, new Vector4(0.4f, 0.2f, 0.1f, 1f));
        GL.Uniform1(useLightingLocation, 2);
        GL.Uniform1(specularStrengthLocation, 0.3f);
        GL.Uniform1(shininessLocation, 16.0f);
        GL.BindVertexArray(_trunkVao);
        GL.DrawElements(PrimitiveType.Triangles, _trunkIndices.Length, DrawElementsType.UnsignedInt, 0);

        // Крона дерева
        GL.Uniform4(colorLocation, _crownColors[_currentSeason]);
        GL.Uniform1(useLightingLocation, 2);
        GL.Uniform1(specularStrengthLocation, 0.3f);
        GL.Uniform1(shininessLocation, 16.0f);
        GL.BindVertexArray(_crownVao);
        GL.DrawElements(PrimitiveType.Triangles, _crownIndices.Length, DrawElementsType.UnsignedInt, 0);

        // Забор
        GL.Uniform4(colorLocation, new Vector4(0.5f, 0.3f, 0.2f, 1f));
        GL.Uniform1(useLightingLocation, 2);
        GL.Uniform1(specularStrengthLocation, 0.3f);
        GL.Uniform1(shininessLocation, 16.0f);
        GL.BindVertexArray(_fenceVao);
        GL.DrawElements(PrimitiveType.Triangles, _fenceIndices.Length, DrawElementsType.UnsignedInt, 0);

        // Кусты
        GL.Enable(EnableCap.Blend);
        GL.Uniform4(colorLocation, _bushColors[_currentSeason]);
        GL.Uniform1(useLightingLocation, 2);
        GL.Uniform1(specularStrengthLocation, 0.3f);
        GL.Uniform1(shininessLocation, 16.0f);
        GL.BindVertexArray(_bushVao);
        GL.DrawElements(PrimitiveType.Triangles, _bushIndices.Length, DrawElementsType.UnsignedInt, 0);
        GL.Disable(EnableCap.Blend);

        // Фонарь: столб
        GL.Uniform4(colorLocation, new Vector4(0.2f, 0.1f, 0.2f, 1f));
        GL.Uniform1(useLightingLocation, 2);
        GL.Uniform1(specularStrengthLocation, 0.3f);
        GL.Uniform1(shininessLocation, 16.0f);
        GL.BindVertexArray(_lanternPostVao);
        GL.DrawElements(PrimitiveType.Triangles, _lanternPostIndices.Length, DrawElementsType.UnsignedInt, 0);
        // Фонарь: стекло
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.Uniform4(colorLocation, _lanternIsLit ? new Vector4(1.0f, 1.0f, 0.8f, 0.6f) : new Vector4(0.8f, 0.8f, 0.9f, 0.3f));
        GL.Uniform1(useLightingLocation, 2);
        GL.BindVertexArray(_lanternGlassVao);
        GL.DrawElements(PrimitiveType.Triangles, _lanternGlassIndices.Length, DrawElementsType.UnsignedInt, 0);
        GL.Disable(EnableCap.Blend);

        // Интерьер
        GL.Disable(EnableCap.DepthTest);
        GL.Enable(EnableCap.Blend);
        GL.Uniform4(colorLocation, new Vector4(0.9f, 0.85f, 0.7f, 0.9f));
        GL.Uniform1(useLightingLocation, 0);
        GL.BindVertexArray(_interiorVao);
        GL.DrawElements(PrimitiveType.Triangles, _interiorIndices.Length, DrawElementsType.UnsignedInt, 0);
        GL.Disable(EnableCap.Blend);

        // Окно 
        GL.Disable(EnableCap.DepthTest);
        GL.Enable(EnableCap.Blend);
        GL.Uniform4(colorLocation, new Vector4(0.2f, 0.2f, 0.8f, 0.5f));
        GL.Uniform1(useLightingLocation, 2);
        GL.BindVertexArray(_windowVao);
        GL.DrawElements(PrimitiveType.Triangles, _windowIndices.Length, DrawElementsType.UnsignedInt, 0);
        GL.Disable(EnableCap.Blend);
        GL.Enable(EnableCap.DepthTest);

        if (_currentSeason == Season.Winter)
        {
            GL.Uniform4(colorLocation, new Vector4(1f, 1f, 1f, 0.8f));
            GL.Uniform1(useLightingLocation, 0);
            GL.PointSize(5f); // Размер снежинок
            GL.BindVertexArray(_snowflakeVao);
            GL.DrawArrays(PrimitiveType.Points, 0, SnowflakeCount);
        }

        // Зеркальный фонарь: куб
        float rotationAngle = _rotationTime * 0.01f; // Скорость вращения (0.5 радиан/сек)
        model = Matrix4.CreateRotationY(rotationAngle) * Matrix4.CreateTranslation(cubePosition); // Matrix4.CreateTranslation(cubePosition);
        GL.UniformMatrix4(modelLocation, false, ref model);
        GL.Uniform4(colorLocation, new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
        GL.Uniform1(useLightingLocation, 3);
        GL.Uniform1(specularStrengthLocation, 0.5f);
        GL.Uniform1(shininessLocation, 32.0f);
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.TextureCubeMap, _cubemapTexture);
        GL.Uniform1(cubemapLocation, 0);
        GL.BindVertexArray(_mirrorLanternVao);
        GL.DrawElements(PrimitiveType.Triangles, _mirrorLanternIndices.Length, DrawElementsType.UnsignedInt, 0);
        
        GL.BindVertexArray(_mirrorLanternVao);
        GL.DrawElements(PrimitiveType.Triangles, _mirrorLanternIndices.Length, DrawElementsType.UnsignedInt, 0);

        Matrix4 _model_ = Matrix4.CreateTranslation(new Vector3(1.5f, -0.18f, 0.25f)); // Позиция второго столба
        GL.UniformMatrix4(modelLocation, false, ref _model_);
        GL.Uniform4(colorLocation, new Vector4(0.2f, 0.1f, 0.1f, 1f));
        GL.Uniform1(useLightingLocation, 2);
        GL.Uniform1(specularStrengthLocation, 0.3f);
        GL.Uniform1(shininessLocation, 16.0f);
        GL.BindVertexArray(_lanternPostVao);
        GL.DrawElements(PrimitiveType.Triangles, _lanternPostIndices.Length, DrawElementsType.UnsignedInt, 0);

        int error = (int)GL.GetError();
        if (error != 0)
            Console.WriteLine($"OpenGL Error in OnRenderFrame: {error}");

        SwapBuffers();
    }
    private float _rotationTime = 0.0f;
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        _rotationTime += (float)args.Time;

        float deltaTime = (float)args.Time;
        for (int i = 0; i < SnowflakeCount; i++)
        {
            var snowflake = _snowflakes[i];
            snowflake.Position.Y -= snowflake.FallSpeed * deltaTime;
            snowflake.Position.X += (float)Math.Sin(snowflake.SwayPhase + Environment.TickCount * 0.001) * 0.02f * deltaTime;
            if (snowflake.Position.Y < -1f)
            {
                snowflake.Position.Y += 10f; 
                snowflake.Position.X = (float)(new Random().NextDouble() * 20 - 10);
                snowflake.Position.Z = (float)(new Random().NextDouble() * 20 - 10);
            }
            _snowflakes[i] = snowflake;

            _snowflakeVertices[i * 3] = snowflake.Position.X;
            _snowflakeVertices[i * 3 + 1] = snowflake.Position.Y;
            _snowflakeVertices[i * 3 + 2] = snowflake.Position.Z;
        }

        GL.BindBuffer(BufferTarget.ArrayBuffer, _snowflakeVbo);
        GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, _snowflakeVertices.Length * sizeof(float), _snowflakeVertices);

        _seasonTimer += (float)args.Time;
        if (_seasonTimer >= SeasonChangeInterval)
        {
            _currentSeason = (Season)(((int)_currentSeason + 1) % 4);
            Console.WriteLine($"Season changed to: {_currentSeason} (Auto)");
            _seasonTimer = 0.0f;
        }
    }
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), (float)e.Width / e.Height, 0.1f, 100f);
    }
    protected override void OnUnload()
    {
        base.OnUnload();
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_cubeVbo);
        GL.DeleteBuffer(_cubeEbo);
        GL.DeleteVertexArray(_cubeVao);
        GL.DeleteBuffer(_roofVbo);
        GL.DeleteBuffer(_roofEbo);
        GL.DeleteVertexArray(_roofVao);
        GL.DeleteBuffer(_windowVbo);
        GL.DeleteBuffer(_windowEbo);
        GL.DeleteVertexArray(_windowVao);
        GL.DeleteBuffer(_interiorVbo);
        GL.DeleteBuffer(_interiorEbo);
        GL.DeleteVertexArray(_interiorVao);
        GL.DeleteBuffer(_trunkVbo);
        GL.DeleteBuffer(_trunkEbo);
        GL.DeleteVertexArray(_trunkVao);
        GL.DeleteBuffer(_crownVbo);
        GL.DeleteBuffer(_crownEbo);
        GL.DeleteVertexArray(_crownVao);
        GL.DeleteBuffer(_snowflakeVbo);
        GL.DeleteVertexArray(_snowflakeVao);
        GL.DeleteBuffer(_fenceVbo);
        GL.DeleteBuffer(_fenceEbo);
        GL.DeleteVertexArray(_fenceVao);
        GL.DeleteBuffer(_bushVbo);
        GL.DeleteBuffer(_bushEbo);
        GL.DeleteVertexArray(_bushVao);
        GL.DeleteBuffer(_lanternPostVbo);
        GL.DeleteBuffer(_lanternPostEbo);
        GL.DeleteVertexArray(_lanternPostVao);
        GL.DeleteBuffer(_lanternGlassVbo);
        GL.DeleteBuffer(_lanternGlassEbo);
        GL.DeleteVertexArray(_lanternGlassVao);
        GL.DeleteProgram(_shaderProgram);
    }

    private string LoadShaderSource(string path)
    {
        return File.ReadAllText(path);
    }

    private int CreateShaderFromFiles(string vertexPath, string fragmentPath)
    {
        string vertexShaderSource = LoadShaderSource(vertexPath);
        string fragmentShaderSource = LoadShaderSource(fragmentPath);
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string info = GL.GetShaderInfoLog(vertexShader);
            throw new Exception($"Ошибка компиляции вершинного шейдера: {info}");
        }

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            string info = GL.GetShaderInfoLog(fragmentShader);
            throw new Exception($"Ошибка компиляции фрагментного шейдера: {info}");
        }

        int shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);

        GL.GetProgram(shaderProgram, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            string info = GL.GetProgramInfoLog(shaderProgram);
            throw new Exception($"Ошибка линковки шейдера: {info}");
        }

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        return shaderProgram;
    }

    public static void Main()
    {
        var nativeSettings = new NativeWindowSettings()
        {
            Size = new Vector2i(1280, 720),
            Title = "Дом и времена года",
        };
        using var window = new SeasonWindow();
        window.Run();
    }
}
