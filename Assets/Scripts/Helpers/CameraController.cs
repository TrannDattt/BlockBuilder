using BuilderTool.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private float _minZoomMult;
    [SerializeField] private float _maxZoomMult;

    private const float _defaultZoomMult = 10;

    private Camera _mainCam;

    public void ChangeToTestMode(){
        Grid3D.Instance.GetFloorSize(out float height, out float width, out Vector2 center);

        Tilting(new Vector3(-30, 0, 0));
        Zooming(GetZoomMult(height, width));

        Vector2 offset = new(-7, -7);
        Moving(center + offset);
    }

    public void ChangeToDesignMode(){
        Tilting(Vector3.zero);
        Zooming(10);
        Moving(Vector3.zero);
    }

    private void Tilting(Vector3 targetAngle){
        transform.rotation = Quaternion.Euler(targetAngle);
    }

    private float GetZoomMult(float floorHeight, float floorWidth){
        var zoomRange = _maxZoomMult - _minZoomMult;
        var floorMaxHeight = Grid3D.Instance.FloorMaxHeight;
        var floorMaxWidth = Grid3D.Instance.FloorMaxWidth;

        if(floorHeight > floorWidth){
            return zoomRange * floorHeight / floorMaxHeight + _minZoomMult;
        }
        else{
            return zoomRange * floorWidth / floorMaxWidth + _minZoomMult;
        }
    }

    private void Zooming(float mult){
        _mainCam.orthographicSize = mult;
    }

    private void Moving(Vector3 targetPos){
        transform.position = targetPos;
    }

    void OnEnable()
    {
        _mainCam = Camera.main;
    }
}
