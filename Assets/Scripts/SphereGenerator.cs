using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    GameObject planet;
    Mesh planetMesh;
    Vector3[] planetVertices;
    int[] planetTriangles;
    MeshRenderer planetMeshRenderer;
    MeshFilter planetMeshFilter;
    MeshCollider planetMeshCollider;

    public GameObject CreatePlanet(Color color, float size)
    {
        Material material = CreateMaterialWithColor(color);
        CreatePlanetGameObject(material, size);
        return planet;
    }

    public GameObject CreatePlanet(Material material, float size)
    {
        CreatePlanetGameObject(material, size);
        RecalculateMesh();
        return planet;
    }

    private Material CreateMaterialWithColor(Color color)
    {
        Material material = new Material(Shader.Find("Specular"));
        material.SetFloat("_Shininess", 1);
        material.SetColor("_Color", color);
        return material;
    }

    private void CreatePlanetGameObject(Material material, float size)
    {
        planet = new GameObject();
        planetMeshFilter = planet.AddComponent<MeshFilter>();
        planetMesh = planetMeshFilter.mesh;
        planetMeshRenderer = planet.AddComponent<MeshRenderer>();

        planetMeshRenderer.material = material;
        planet.transform.localScale = new Vector3(size, size, size);
        CustomSphere.Create(planet);

        RecalculateMesh();
    }

    private void RecalculateMesh()
    {
        planetMesh.RecalculateBounds();
        planetMesh.RecalculateTangents();
        planetMesh.RecalculateNormals();
    }
}
