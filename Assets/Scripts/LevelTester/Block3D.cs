using UnityEngine;
using BuilderTool.Enums;
using BuilderTool.Helpers;
using UnityEditor;
using BuilderTool.Interfaces;

public class Block3D : MonoBehaviour, ICanHaveMechanic{
    public Rigidbody2D Body {get; private set;}

    public EColor Color {get; private set;}
    public bool IsCollide {get; private set;}

    private MeshRenderer[] _renderers;

    public void InitBlock(EColor color){
        _renderers = GetComponentsInChildren<MeshRenderer>();
        Body = GetComponent<Rigidbody2D>();

        ChangeColor(color);
    }

    public void ChangeColor(EColor color){
        foreach(var renderer in _renderers){
            renderer.material.color = ColorMapper.GetColor(color);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall")){
            IsCollide = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        IsCollide = false;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
