using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Definitions;

namespace SteveClient.Engine.Rendering.Font;

public static class FontRenderer
{
    private static BakedTextRender BakeText(FontString text, Vector3 pos, float scale, Vector3 dir, Color4 color)
    {
        List<Character> characters = new List<Character>();
        List<Matrix4> transforms = new List<Matrix4>();

        float rad = (float)Math.Atan2(dir.X, dir.Y); //TODO z
        Matrix4 rotM = Matrix4.CreateRotationZ(rad);
        Matrix4 origin = Matrix4.CreateTranslation(pos);

        float offset = 0f;
        foreach (var c in text)
        {
            float w = c.Size.X * scale;
            float h = c.Size.Y * scale;
            float xRel = offset + c.Bearing.X * scale;
            float yRel = (c.Size.Y - c.Bearing.Y) * scale;

            offset += (c.Advance >> 6) * scale;

            Matrix4 scaleM = Matrix4.CreateScale(new Vector3(w, h, 1.0f));
            Matrix4 translation = Matrix4.CreateTranslation(new Vector3(xRel, -yRel, 0));

            Matrix4 transform = scaleM * translation * rotM * origin;
            
            characters.Add(c);
            transforms.Add(transform);
        }

        return new BakedTextRender(characters.ToArray(), transforms.ToArray(), color);
    }
    
    public static void DrawText(FontString text, Vector3 pos, float scale, Vector3 dir, Color4 color)
    {
        BakedTextRender textRender = BakeText(text, pos, scale, dir, color);
        
        RenderLayerDefinitions.WorldFontLayer.Upload(textRender);
    }

    public static void DrawText(FontString text, Vector3 pos, float scale, Vector3 dir)
    {
        DrawText(text, pos, scale, dir, Color4.White);
    }

    public static void DrawTextScreenSpace(FontString text, Vector3 pos, float scale, Vector3 dir)
    {
        BakedTextRender textRender = BakeText(text, pos, scale, dir, Color4.White);

        RenderLayerDefinitions.ScreenFontLayer.Upload(textRender);
    }

    public static void DrawTextScreenSpace(FontString text, Vector2 pos, float scale)
    {
        DrawTextScreenSpace(text, new Vector3(pos.X, pos.Y, -1), scale, Vector3.UnitZ);
    }

    public static void DrawTextBillBoard(FontString text, Vector3 pos, float scale, Vector3 dir, Color4 color)
    {
        Matrix4 view = CameraState.ViewMatrix;
        Matrix4 projection = CameraState.ProjectionMatrix;
        //Vector3 right = new Vector3(view[0, 0], view[1, 0], view[2, 0]);
        Vector3 up = new Vector3(view[0, 1], view[1, 1], view[2, 1]);

        List<Character> characters = new List<Character>();
        List<Matrix4> transforms = new List<Matrix4>();

        Matrix4 face = Matrix4.CreateFromQuaternion(Face(pos));

        float rad = (float)Math.Atan2(-dir.X, dir.Y); // TODO z
        Matrix4 rotM = Matrix4.CreateRotationZ(rad);
        Matrix4 origin = Matrix4.CreateTranslation(pos);

        float offset = 0f;
        foreach (var c in text)
        {
            float w = c.Size.X * scale;
            float h = c.Size.Y * scale;
            float xRel = offset + c.Bearing.X * scale;
            float yRel = (c.Size.Y - c.Bearing.Y) * scale;

            offset += (c.Advance >> 6) * scale;

            Matrix4 scaleM = Matrix4.CreateScale(new Vector3(w, h, 1.0f));
            Matrix4 translation = Matrix4.CreateTranslation(new Vector3(xRel, yRel, 0));

            Matrix4 transform = scaleM * translation * rotM * origin;

            characters.Add(c);
            transforms.Add(transform);
        }
        
        RenderLayerDefinitions.WorldFontLayer.Upload(new BakedTextRender(characters.ToArray(), transforms.ToArray(), color));
    }

    public static void DrawTextBillBoard(FontString text, Vector3 pos, float scale, Vector3 dir)
    {
        DrawTextBillBoard(text, pos, scale, dir, Color4.White);
    }
    
    private static Quaternion Face(Vector3 pos)
    {
        Vector3 dist = CameraState.Position - pos;
        
        Vector3 dirA = -Vector3.UnitZ;
        Vector3 dirB = dist.Normalized();

        float angle = (float)Math.Acos(Vector3.Dot(dirA, dirB));
        Vector3 axis = Vector3.Cross(dirA, dirB).Normalized();

        Quaternion rotation = Quaternion.FromAxisAngle(axis, angle);

        return rotation;
    }
}