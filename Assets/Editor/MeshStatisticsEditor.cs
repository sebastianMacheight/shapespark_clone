using ReUpVirtualTwin.Helpers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshFilter))]
public class MeshStatisticsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MeshFilter meshFilter = (MeshFilter)target;
        Mesh mesh = meshFilter.sharedMesh;

        if (mesh != null)
        {

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mesh Data", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Triangle Count: " + mesh.triangles.Length / 3);
            EditorGUILayout.LabelField("Vertex Count: " + mesh.vertexCount);
            EditorGUILayout.LabelField("Normal Count: " + mesh.normals.Length);
            EditorGUILayout.LabelField("Tangent Count: " + mesh.tangents.Length);
            EditorGUILayout.LabelField("Uv Lenght: " + mesh.uv.Length);


            var meshInfo = new MeshInfo(meshFilter.gameObject);
            EditorGUILayout.LabelField("Mesh Info Script", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Is Mesh Serialized: " + meshInfo.isMeshSerialized);
            EditorGUILayout.LabelField("Areal Vertex Density: " + meshInfo.arealVertexDensity);
            EditorGUILayout.LabelField("Vol Vertex Density: " + meshInfo.volVertexDensity);
            EditorGUILayout.LabelField("Mesh size: " + meshInfo.size);
            EditorGUILayout.LabelField("Mesh area: " + meshInfo.area);
            EditorGUILayout.LabelField("Mesh volumen: " + meshInfo.volumen);

            //var serializedMesh = ((GameObject)target).GetComponent<SerializeMesh>();
            if (meshInfo.isMeshSerialized)
            {
                var serializedMesh = target.GetComponent<SerializeMesh>().Rebuild();
                EditorGUILayout.LabelField("Serialized Mesh Data", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Triangle Count: " + serializedMesh.triangles.Length / 3);
                EditorGUILayout.LabelField("Vertex Count: " + serializedMesh.vertexCount);
                EditorGUILayout.LabelField("Normal Count: " + serializedMesh.normals.Length);
                EditorGUILayout.LabelField("Tangent Count: " + serializedMesh.tangents.Length);
                EditorGUILayout.LabelField("Uv Lenght: " + serializedMesh.uv.Length);
            }
        }
    }
}

