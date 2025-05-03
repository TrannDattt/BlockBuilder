using BuilderTool.Enums;
using BuilderTool.Helpers;
using UnityEngine;

public class Door3D : Wall3D{
    public EColor Color {get; private set;}

    private MeshRenderer _renderer;

    public void InitDoor(EColor color){
        _renderer = GetComponentInChildren<MeshRenderer>();
        ChangeColor(color);
    }

    private void ChangeColor(EColor color){
        Color = color;
        _renderer.material.color = ColorMapper.GetColor(color);
    }
}
