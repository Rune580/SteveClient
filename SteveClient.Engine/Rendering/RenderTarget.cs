using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering;

public class RenderTarget
{
    public static readonly RenderTarget WorldSpace = new(() => CameraState.ViewMatrix, () => CameraState.ProjectionMatrix);
    public static readonly RenderTarget ScreenSpace = new(() => CameraState.ScreenSpaceViewMatrix, () => CameraState.ScreenSpaceProjectionMatrix);

    private readonly Func<Matrix4> _viewMatrixFunc;
    private readonly Func<Matrix4> _projMatrixFunc;

    private RenderTarget(Func<Matrix4> viewMatrixFunc, Func<Matrix4> projMatrixFunc)
    {
        _viewMatrixFunc = viewMatrixFunc;
        _projMatrixFunc = projMatrixFunc;
    }

    public Matrix4 ViewMatrix => _viewMatrixFunc.Invoke();
    public Matrix4 ProjectionMatrix => _projMatrixFunc.Invoke();
}