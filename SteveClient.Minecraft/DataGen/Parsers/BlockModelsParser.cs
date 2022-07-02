﻿using System.Text.Json;
using OpenTK.Mathematics;
using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Data.JsonSchema;
using SteveClient.Minecraft.ModelLoading;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.DataGen.Parsers;

public class BlockModelsParser : IMinecraftAssetParser
{
    public string JarPath => "assets/minecraft/models/block/";
    public string LocalPath => "models/block/";
    
    public bool DataExists()
    {
        var path = Path.GetFullPath(LocalPath);
        
        bool dirExists = Directory.Exists(path);
        bool dirContainsFiles = false;

        if (dirExists)
            dirContainsFiles = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Any();
        
        return dirContainsFiles;
    }

    public void Parse()
    {
        string[] files = Directory.GetFiles(LocalPath, "*.json", SearchOption.AllDirectories);

        Dictionary<string, ModelJson> modelJsons = new Dictionary<string, ModelJson>();
        
        foreach (var file in files)
        {
            var model = JsonSerializer.Deserialize<ModelJson>(File.ReadAllText(file)) ?? throw new InvalidOperationException();

            if (model.Parent is not null)
            {
                model.Parent = model.Parent
                    .Replace("minecraft:", "")
                    .Replace("block/", "");
            }
            
            modelJsons[Path.GetFileNameWithoutExtension(file)] = model;
        }

        foreach (var (key, modelJson) in modelJsons)
        {
            var textureMap = GetTextureMap(modelJson, modelJsons);
            var elements = GetElements(modelJson, modelJsons);

            List<BlockFace> blockFaces = new List<BlockFace>();

            foreach (var element in elements)
                blockFaces.AddRange(GetBlockFaces(element));

            foreach (var face in blockFaces)
            {
                while (textureMap.ContainsKey(face.Texture))
                    face.Texture = textureMap[face.Texture];
            }

            if (blockFaces.Count == 0)
                continue;

            BlockModel model = new BlockModel(blockFaces.ToArray());
            
            BlockModels.Add(key, model);
        }
    }

    private List<BlockFace> GetBlockFaces(ElementJson element)
    {
        List<BlockFace> blockFaces = new List<BlockFace>();

        if (element.Faces is null)
            return blockFaces;

        var faces = element.Faces;
        
        if (faces.Down is not null)
            blockFaces.Add(GetBlockFace(element.From, element.To, Directions.Down, faces.Down));

        if (faces.Up is not null)
            blockFaces.Add(GetBlockFace(element.From, element.To, Directions.Up, faces.Up));
        
        if (faces.North is not null)
            blockFaces.Add(GetBlockFace(element.From, element.To, Directions.North, faces.North));
        
        if (faces.South is not null)
            blockFaces.Add(GetBlockFace(element.From, element.To, Directions.South, faces.South));
        
        if (faces.West is not null)
            blockFaces.Add(GetBlockFace(element.From, element.To, Directions.West, faces.West));
        
        if (faces.East is not null)
            blockFaces.Add(GetBlockFace(element.From, element.To, Directions.East, faces.East));

        return blockFaces;
    }

    private BlockFace GetBlockFace(Vector3 from, Vector3 to, Directions direction, FacesJson.FaceJson faceJson)
    {
        BlockFace face = new BlockFace
        {
            Texture = faceJson.Texture,
            UvMin = faceJson.Uv.Xy / 16,
            UvMax = faceJson.Uv.Zw / 16
        };

        Vector3 min = from / 16;
        Vector3 max = to / 16;
        
        switch (direction)
        {
            case Directions.Down:
                face.TopLeft = min;
                face.TopRight = new Vector3(max.X, min.Y, min.Z);
                face.BottomLeft = new Vector3(min.X, min.Y, max.Z);
                face.BottomRight = new Vector3(max.X, min.Y, max.Z);
                break;
            case Directions.Up:
                face.TopLeft = new Vector3(min.X, max.Y, max.Z);
                face.TopRight = max;
                face.BottomLeft = new Vector3(min.X, max.Y, min.Z);
                face.BottomRight = new Vector3(max.X, max.Y, min.Z);
                break;
            case Directions.North:
                face.TopLeft = new Vector3(min.X, max.Y, min.Z);
                face.TopRight = new Vector3(max.X, max.Y, min.Z);
                face.BottomLeft = min;
                face.BottomRight = new Vector3(max.X, min.Y, min.Z);
                break;
            case Directions.South:
                face.TopLeft = max;
                face.TopRight = new Vector3(min.X, max.Y, max.Z);
                face.BottomLeft = new Vector3(max.X, min.Y, max.Z);
                face.BottomRight = new Vector3(min.X, min.Y, max.Z);
                break;
            case Directions.West:
                face.TopLeft = new Vector3(min.X, max.Y, min.Z);
                face.TopRight = new Vector3(min.X, max.Y, max.Z);
                face.BottomLeft = min;
                face.BottomRight = new Vector3(min.X, min.Y, max.Z);
                break;
            case Directions.East:
                face.TopLeft = new Vector3(max.X, max.Y, min.Z);
                face.TopRight = max;
                face.BottomLeft = new Vector3(max.X, min.Y, min.Z);
                face.BottomRight = new Vector3(max.X, min.Y, max.Z);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        return face;
    }

    private List<ElementJson> GetElements(ModelJson modelJson, Dictionary<string, ModelJson> modelJsons)
    {
        List<ElementJson> elements = new List<ElementJson>();

        if (modelJson.Elements is not null)
        {
            elements.AddRange(modelJson.Elements);
            return elements;
        }

        if (modelJson.Parent is null)
            return elements;

        var result = GetElements(modelJsons[modelJson.Parent], modelJsons);
        
        elements.AddRange(result);
        return elements;
    }

    private Dictionary<string, string> GetTextureMap(ModelJson modelJson, Dictionary<string, ModelJson> modelJsons)
    {
        Dictionary<string, string> textureMap = new Dictionary<string, string>();

        if (modelJson.Textures is not null)
        {
            foreach (var (key, value) in modelJson.Textures)
                textureMap[$"#{key}"] = value;
        }

        if (modelJson.Parent is null)
            return textureMap;

        var result = GetTextureMap(modelJsons[modelJson.Parent], modelJsons);

        foreach (var (key, value) in result)
            textureMap[key] = value;

        return textureMap;
    }
}