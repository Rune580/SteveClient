using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Shaders.Processor;

namespace SteveClient.Engine.Rendering.Shaders;

public class Shader
{
    public readonly int Handle;

    private readonly Dictionary<string, int> _uniformLocations;
    private readonly Dictionary<string, int> _uniformBlockLocations;
    private readonly BakedShaderAttribute[] _shaderAttributes;

    public Shader(string vertPath, string fragPath, params ShaderAttribute[] shaderAttributes)
    {
        var shaderSource = Assets.ReadEmbeddedShader(vertPath);
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        CompileShader(vertexShader, shaderSource);

        shaderSource = Assets.ReadEmbeddedShader(fragPath);
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        CompileShader(fragmentShader, shaderSource);

        Handle = GL.CreateProgram();
        
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        
        LinkProgram(Handle);
        
        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numOfUniforms);
        _uniformLocations = new Dictionary<string, int>();
        
        for (int i = 0; i < numOfUniforms; i++)
        {
            var key = GL.GetActiveUniform(Handle, i, out _, out _);

            var location = GL.GetUniformLocation(Handle, key);
            
            _uniformLocations.Add(key, location);
        }
        
        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniformBlocks, out var numOfUniformBlocks);
        _uniformBlockLocations = new Dictionary<string, int>();

        for (int i = 0; i < numOfUniformBlocks; i++)
        {
            GL.GetActiveUniformBlock(Handle, i, ActiveUniformBlockParameter.UniformBlockDataSize, out var size);
            GL.GetActiveUniformBlockName(Handle, i, size, out _, out var key);
            GL.GetActiveUniformBlock(Handle, i, ActiveUniformBlockParameter.UniformBlockNameLength, out var nameLength);
            GL.GetActiveUniformBlock(Handle, i, ActiveUniformBlockParameter.UniformBlockActiveUniforms, out var uniforms);

            int[] indices = new int[uniforms];
        }

        _shaderAttributes = new BakedShaderAttribute[shaderAttributes.Length];

        int offset = 0;
        int stride = GetStride(shaderAttributes);

        for (int i = 0; i < _shaderAttributes.Length; i++)
        {
            var attribute = shaderAttributes[i];
            var location = GetAttribLocation(attribute.Name);

            _shaderAttributes[i] = new BakedShaderAttribute(location, attribute.Size,
                attribute.VertexAttribPointerType, attribute.Normalized, stride, offset);

            offset += GetAttributeByteSize(attribute);
        }
    }
    
    public Shader(string shaderName, params ShaderAttribute[] shaderAttributes) :
        this($"{shaderName}.vert", $"{shaderName}.frag", shaderAttributes) { }

    public Shader(string computeShaderName)
    {
        string shaderSource = Assets.ReadEmbeddedShader($"{computeShaderName}.glsl");
        int computeShader = GL.CreateShader(ShaderType.ComputeShader);
        CompileShader(computeShader, shaderSource);
        
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, computeShader);
        LinkProgram(Handle);
        
        GL.DetachShader(Handle, computeShader);
        GL.DeleteShader(computeShader);
        
        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numOfUniforms);

        _uniformLocations = new Dictionary<string, int>();

        for (int i = 0; i < numOfUniforms; i++)
        {
            var key = GL.GetActiveUniform(Handle, i, out _, out _);

            var location = GL.GetUniformLocation(Handle, key);
            
            _uniformLocations.Add(key, location);
        }

        _shaderAttributes = Array.Empty<BakedShaderAttribute>();
    }

    private int GetStride(ShaderAttribute[] shaderAttributes)
    {
        int stride = shaderAttributes.Sum(GetAttributeByteSize);

        return stride;
    }

    private int GetAttributeByteSize(ShaderAttribute attribute)
    {
        int byteSize = attribute.VertexAttribPointerType switch
        {
            VertexAttribPointerType.Byte => sizeof(sbyte),
            VertexAttribPointerType.UnsignedByte => sizeof(byte),
            VertexAttribPointerType.Short => sizeof(short),
            VertexAttribPointerType.UnsignedShort => sizeof(ushort),
            VertexAttribPointerType.Int => sizeof(int),
            VertexAttribPointerType.UnsignedInt => sizeof(uint),
            VertexAttribPointerType.Float => sizeof(float),
            VertexAttribPointerType.Double => sizeof(double),
            _ => throw new ArgumentOutOfRangeException()
        };

        return byteSize * attribute.Size;
    }

    public void Use()
    {
        GL.UseProgram(Handle);

        foreach (var attribute in _shaderAttributes)
        {
            GL.EnableVertexAttribArray(attribute.Location);

            if (attribute.IsIPointer)
            {
                GL.VertexAttribIPointer(attribute.Location,
                    attribute.Size,
                    attribute.VertexAttribIntegerType,
                    attribute.Stride,
                    (IntPtr)attribute.Offset);
            }
            else
            {
                GL.VertexAttribPointer(attribute.Location,
                    attribute.Size,
                    attribute.VertexAttribPointerType,
                    attribute.Normalized,
                    attribute.Stride,
                    attribute.Offset);
            }
        }
    }
    
    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Handle, attribName);
    }
    
    /// <summary>
    /// Set a uniform Matrix4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    /// <remarks>
    ///   <para>
    ///   The matrix is transposed before being sent to the shader.
    ///   </para>
    /// </remarks>
    public void SetMatrix4(string name, Matrix4 data)
    {
        if (!_uniformLocations.TryGetValue(name, out int loc))
            return;
        
        GL.UniformMatrix4(loc, true, ref data);
    }

    public void SetMatrix4(string name, bool transpose, ref Matrix4 data)
    {
        if (!_uniformLocations.TryGetValue(name, out int loc))
            return;
        
        GL.UniformMatrix4(loc, transpose, ref data);
    }

    /// <summary>
    /// Set a uniform Vector4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetVector4(string name, Vector4 data)
    {
        if (!_uniformLocations.TryGetValue(name, out int loc))
            return;
        
        GL.Uniform4(loc, data);
    }

    public void SetVector2(string name, Vector2 data)
    {
        if (!_uniformLocations.TryGetValue(name, out int loc))
            return;
        
        GL.Uniform2(loc, data);
    }
    
    public void SetVector3(string name, Vector3 data)
    {
        if (!_uniformLocations.TryGetValue(name, out int loc))
            return;
        
        GL.Uniform3(loc, data);
    }

    /// <summary>
    /// Set a uniform Color4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="color">The data to set</param>
    public void SetColor(string name, Color4 color)
    {
        if (!_uniformLocations.TryGetValue(name, out int loc))
            return;
        
        GL.Uniform4(loc, color);
    }

    public void SetInt(string name, int data)
    {
        if (!_uniformLocations.TryGetValue(name, out int loc))
            return;
        
        GL.Uniform1(loc, data);
    }

    public void SetFloat(string name, float data)
    {
        if (!_uniformLocations.TryGetValue(name, out int loc))
            return;
        
        GL.Uniform1(loc, data);
    }

    public void SetUniformArray(string name, int[] data)
    {
        if (!name.EndsWith("[0]"))
            name += "[0]";
        
        if (!_uniformLocations.TryGetValue(name, out int loc))
            return;
        
        GL.Uniform1(loc, data.Length, data);
    }

    private static void CompileShader(int shader, string shaderSource)
    {
        GL.ShaderSource(shader, ShaderProcessor.Process(shaderSource));

        GL.CompileShader(shader);
        
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
        if (code != (int) All.True)
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{GL.GetShaderInfoLog(shader)}");
    }

    private static void LinkProgram(int program)
    {
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int) All.True)
            throw new Exception($"Error occurred whilst linking Program({GL.GetProgramInfoLog(program)})");
    }
}