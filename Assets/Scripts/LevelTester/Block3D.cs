using UnityEngine;
using BuilderTool.Enums;
using BuilderTool.Helpers;
using UnityEditor;

public class Block3D : MonoBehaviour{
    public EColor Color {get; private set;}
    public bool IsCollide {get; private set;}

    private MeshRenderer[] _renderers;

    public void InitBlock(EColor color){
        _renderers = GetComponentsInChildren<MeshRenderer>();

        ChangeColor(color);
    }

    public void ChangeColor(EColor color){
        foreach(var renderer in _renderers){
            renderer.material.color = ColorMapper.GetColor(color);
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        // foreach (ContactPoint2D contact in collision.contacts){
        //     var contactPoint = contact.point;
        //     if(Block3DSelectHandler.Instance.CheckBlockCollide(contactPoint)){
        //         IsCollide = true;
        //         return;
        //     }
        // }
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall")){
            IsCollide = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        IsCollide = false;
    }
}
