using OpenTK.Mathematics;
using static Silk.NET.Assimp.Assimp;

namespace SteveClient.Assimp;

public static unsafe class AssimpLoader
{
    public static Mesh[] Load(string file)
    {
        var scene = GetApi().ImportFile(file, 0U);

        uint meshCount = scene->MNumMeshes;
        Mesh[] meshes = new Mesh[meshCount];
        
        for (uint m = 0; m < meshCount; m++)
        {
            var mMesh = scene->MMeshes[m];
            uint vertexCount = mMesh->MNumVertices;

            Vector3[] vertices = new Vector3[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            
            for (uint v = 0; v < vertexCount; v++)
            {
                vertices[v] = mMesh->MVertices[v].AsOpenTkVector();
                normals[v] = mMesh->MNormals[v].AsOpenTkVector();
            }

            uint faceCount = mMesh->MNumFaces;
            List<uint> indices = new List<uint>();

            for (uint f = 0; f < faceCount; f++)
            {
                var mFace = mMesh->MFaces[f];
                uint indexCount = mFace.MNumIndices;

                for (uint i = 0; i < indexCount; i++)
                    indices.Add(mFace.MIndices[i]);
            }

            meshes[m] = new Mesh(vertices, normals, indices.ToArray());
        }

        return meshes;
    }
}