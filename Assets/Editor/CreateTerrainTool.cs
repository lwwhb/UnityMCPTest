using UnityEngine;
using UnityEditor;

public class CreateTerrainTool
{
    [MenuItem("Tools/Create Terrain")]
    public static void CreateTerrain()
    {
        var terrainData = new TerrainData();
        terrainData.heightmapResolution = 257;
        terrainData.size = new Vector3(100, 20, 100);

        int res = terrainData.heightmapResolution;
        float[,] heights = new float[res, res];
        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                float nx = x / (float)res * 3f;
                float ny = y / (float)res * 3f;
                heights[x, y] = Mathf.PerlinNoise(nx, ny) * 0.5f
                              + Mathf.PerlinNoise(nx * 2f, ny * 2f) * 0.25f
                              + Mathf.PerlinNoise(nx * 4f, ny * 4f) * 0.125f;
            }
        }
        terrainData.SetHeights(0, 0, heights);

        if (!System.IO.Directory.Exists("Assets/Terrain"))
            AssetDatabase.CreateFolder("Assets", "Terrain");

        AssetDatabase.CreateAsset(terrainData, "Assets/Terrain/MainTerrainData.asset");
        AssetDatabase.SaveAssets();

        var terrainGO = Terrain.CreateTerrainGameObject(terrainData);
        terrainGO.name = "MainTerrain";
        terrainGO.transform.position = new Vector3(-50, 0, -50);

        Debug.Log("Terrain created successfully");
    }
}
