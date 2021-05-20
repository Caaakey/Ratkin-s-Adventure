using UnityEngine;
using UnityEngine.Rendering;

using Unity.Collections;
using Unity.Mathematics;

namespace RksAdventure.Core
{
    public static class MeshUtility
    {
        public static Mesh CreateMesh(float width, float height, Rect uv)
        {
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;

            NativeArray<float3> vertices = new NativeArray<float3>(4, Allocator.Temp);
            vertices[0] = new float3(-halfWidth, -halfHeight, 0);
            vertices[1] = new float3(-halfWidth, halfHeight, 0);
            vertices[2] = new float3(halfWidth, halfHeight, 0);
            vertices[3] = new float3(halfWidth, -halfHeight, 0);

            NativeArray<int> indices = new NativeArray<int>(6, Allocator.Temp);
            indices[0] = 0; indices[1] = 1; indices[2] = 2;
            indices[3] = 0; indices[4] = 2; indices[5] = 3;

            NativeArray<float2> uvs = new NativeArray<float2>(4, Allocator.Temp);
            uvs[0] = new float2(uv.xMin, uv.yMin);
            uvs[1] = new float2(uv.xMin, uv.yMax);
            uvs[2] = new float2(uv.xMax, uv.yMax);
            uvs[3] = new float2(uv.xMax, uv.yMin);

            NativeArray<float4> colors = new NativeArray<float4>(4, Allocator.Temp);
            colors[0] = new float4(1, 1, 1, 1);
            colors[1] = new float4(1, 1, 1, 1);
            colors[2] = new float4(1, 1, 1, 1);
            colors[3] = new float4(1, 1, 1, 1);

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.SetUVs(0, uvs);
            mesh.SetColors(colors);

            vertices.Dispose();
            indices.Dispose();
            uvs.Dispose();
            colors.Dispose();

            return mesh;
        }

    }
}
