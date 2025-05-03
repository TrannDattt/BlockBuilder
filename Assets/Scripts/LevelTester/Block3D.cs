using UnityEngine;
using BuilderTool.Enums;
using BuilderTool.Helpers;

public class Block3D : MonoBehaviour{
    public EColor Color {get; private set;}

    private MeshRenderer[] _renderers;
    private Camera _mainCam;
    private Vector3 offset;
    private bool _isSelected;

    public void InitBlock(EColor color){
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _mainCam = Camera.main;
        ChangeColor(color);
    }

    public void ChangeColor(EColor color){
        foreach(var renderer in _renderers){
            renderer.material.color = ColorMapper.GetColor(color);
        }
    }

    void OnMouseDown()
    {
        _isSelected = true;

        // TODO: Collider is on child object, not on block
        var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePos;
    }

    void OnMouseUp()
    {
        _isSelected = false;
    }

    void Update()
    {
        if(_isSelected){
            var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = offset + mousePos;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // TODO: Event when block collide with door
    }
}
