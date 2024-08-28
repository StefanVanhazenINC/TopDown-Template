using Cinemachine;
using UnityEngine;

[AddComponentMenu("")] // Hide in menu
[SaveDuringPlay]
[ExecuteAlways]
[DisallowMultipleComponent]
public class CinemachineCameraSizeAdjuster : CinemachineExtension
{
    [SerializeField] Vector2Int _baseAspectRatio = new Vector2Int(9, 16);
    [SerializeField][Range(0, 179)] float _baseCameraFOV = 60;
    [SerializeField] float _baseCameraSize = 5;

    float baseAspectRatio => _baseAspectRatio.x / (float)_baseAspectRatio.y;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            LensSettings lens = state.Lens;

            if (state.Lens.Orthographic)
            {
                if (state.Lens.Aspect < baseAspectRatio)
                {
                    // letterboxing
                    var baseHorizontalSize = _baseCameraSize * baseAspectRatio;
                    var verticalSize = baseHorizontalSize / state.Lens.Aspect;

                    lens.OrthographicSize = verticalSize;
                }
                else
                {
                    // pillarboxing
                    lens.OrthographicSize = _baseCameraSize;
                }
            }
            else
            {
                if (state.Lens.Aspect < baseAspectRatio)
                {
                    // letterboxing
                    var baseVerticalSize = Mathf.Tan(_baseCameraFOV * 0.5f * Mathf.Deg2Rad);
                    var baseHorizontalSize = baseVerticalSize * baseAspectRatio;
                    var verticalSize = baseHorizontalSize / state.Lens.Aspect;
                    var verticalFov = Mathf.Atan(verticalSize) * Mathf.Rad2Deg * 2;

                    lens.FieldOfView = verticalFov;
                }
                else
                {
                    // pillarboxing
                    lens.FieldOfView = _baseCameraFOV;
                }
            }

            state.Lens = lens;
        }
    }
}
