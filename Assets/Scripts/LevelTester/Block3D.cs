using UnityEngine;
using BuilderTool.Enums;
using UnityEditor;
using BuilderTool.Interfaces;
using System.Collections.Generic;
using System.Linq;

public class Block3D : MonoBehaviour, ICanHaveMechanic{
    public Rigidbody Body {get; private set;}

    public EColor Color {get; private set;}
    public bool IsCollide {get; private set;}

    private List<BlockPart3D> _blockParts;

    public void InitBlock(EColor color)
    {
        _blockParts = GetComponentsInChildren<BlockPart3D>().ToList();
        _blockParts.ForEach(part => part.SetBaseObject(gameObject));
        Body = GetComponent<Rigidbody>();

        ChangeColor(color);
    }

    public void ChangeColor(EColor color){
        Color = color;
        _blockParts.ForEach(part => part.ChangeColor(color));
    }

    public bool CheckCollidedWhenMove(Vector3 targetPos)
    {
        var moveDir = targetPos - transform.position;

        foreach (var part in _blockParts)
        {
            if (part.CheckCollidedPart(moveDir, out Collider collider))
            {
                var layer = collider.gameObject.layer;

                if (layer != LayerMask.NameToLayer("Door"))
                {
                    return false;
                }

                Debug.Log(1);
                var door = collider.gameObject.GetComponentInParent<Door3D>();
                if (!door.CheckMatchColor(this))
                {
                    return false;
                }

                Debug.Log(2);
                if (!CheckCanGoThroughDoor(moveDir))
                {
                    return false;
                }

                Debug.Log(3);
                GoThroughDoor();

                //TODO: Handle logic to let this block go through door
            }
        }

        return true;
    }

    public bool CheckCanGoThroughDoor(Vector3 moveDir)
    {
        return _blockParts.All(part => part.CheckCanGoThroughDoor(moveDir));
    }

    private void GoThroughDoor()
    {
        Destroy(gameObject);
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
