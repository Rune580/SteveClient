using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering;

public class Shader
{
    public readonly int Handle;

    private readonly Dictionary<string, int> _uniformLocations;

    public Shader(string vertPath, string fragPath)
    {
        var shaderSource = Assets.ReadEmbeddedShader(vertPath);
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, shaderSource);
        CompileShader(vertexShader);

        shaderSource = Assets.ReadEmbeddedShader(fragPath);
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, shaderSource);
        CompileShader(fragmentShader);

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
    }

    public void Use()
    {
        GL.UseProgram(Handle);
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
        GL.UseProgram(Handle);
        GL.UniformMatrix4(_uniformLocations[name], true, ref data);
    }

    public void SetMatrix4(string name, bool transpose, ref Matrix4 data)
    {
        GL.UseProgram(Handle);
        GL.UniformMatrix4(_uniformLocations[name], transpose, ref data);
    }

    /// <summary>
    /// Set a uniform Vector4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetVector4(string name, Vector4 data)
    {
        GL.UseProgram(Handle);
        GL.Uniform4(_uniformLocations[name], data);
    }

    /// <summary>
    /// Set a uniform Color4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="color">The data to set</param>
    public void SetColor(string name, Color4 color)
    {
        GL.UseProgram(Handle);
        GL.Uniform4(_uniformLocations[name], color);
    }

    public void SetUniform1(string name, int data)
    {
        GL.UseProgram(Handle);
        GL.Uniform1(_uniformLocations[name], data);
    }

    private static void CompileShader(int shader)
    {
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
            throw new Exception($"Error occurred whilst linking Program({program})");
    }
}