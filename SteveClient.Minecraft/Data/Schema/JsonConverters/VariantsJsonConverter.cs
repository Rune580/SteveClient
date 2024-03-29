﻿using System.Text.Json;
using System.Text.Json.Serialization;
using SteveClient.Minecraft.BlockStructs;
using SteveClient.Minecraft.Data.Schema.BlockStates;

namespace SteveClient.Minecraft.Data.Schema.JsonConverters;

public class VariantsJsonConverter : JsonConverter<VariantModels>
{
    public override VariantModels? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        VariantModels variants = new VariantModels();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;
            
            string keyText = reader.GetString()!;
            BlockProperties key = new BlockProperties(keyText);

            List<VariantModelJson> modelJsons = new List<VariantModelJson>();

            string? propertyName = null;
            bool next = false;
            
            VariantModelJson currentVariant = new VariantModelJson();

            reader.Read();

            bool inArray = reader.TokenType == JsonTokenType.StartArray;

            do
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.EndObject:
                        modelJsons.Add(currentVariant);
                        currentVariant = new VariantModelJson();
                        next = !inArray;
                        break;
                    case JsonTokenType.EndArray:
                        next = true;
                        break;
                    case JsonTokenType.PropertyName:
                        propertyName = reader.GetString();
                        continue;
                }

                if (next)
                    break;

                if (propertyName == null)
                    continue;
                
                switch (propertyName)
                {
                    case "model":
                        currentVariant.Model = reader.GetString()!;
                        propertyName = null;
                        continue;
                    case "x":
                        currentVariant.X = reader.GetInt32();
                        propertyName = null;
                        continue;
                    case "y":
                        currentVariant.Y = reader.GetInt32();
                        propertyName = null;
                        continue;
                    case "uvlock":
                        currentVariant.UvLock = reader.GetBoolean();
                        propertyName = null;
                        continue;
                    case "weight":
                        currentVariant.Weight = reader.GetInt32();
                        propertyName = null;
                        continue;
                }
            } while (reader.Read());
            
            variants.Add(key, modelJsons.ToArray());
        }
        
        return variants;
    }

    public override void Write(Utf8JsonWriter writer, VariantModels value, JsonSerializerOptions options)
    {
        
    }
}