using UnityEngine;
using BuilderTool.Enums;
using BuilderTool.Helpers;

public class BlockPart3D : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public GameObject BaseObject { get; private set; }

    public void SetBaseObject(GameObject obj)
    {
        BaseObject = obj;
    }

    public void ChangeColor(EColor color)
    {
        _meshRenderer.material.color = ColorMapper.GetColor(color);
    }

    public bool CheckCollidedPart(Vector3 dir, out Collider collider)
    {
        var partCollider = GetComponent<BoxCollider>();
        float halfSize = partCollider.bounds.max.x - partCollider.bounds.center.x;

        collider = null;
        if (Physics.Raycast(transform.position, dir, out var hit, dir.magnitude + halfSize, LayerMask.GetMask("Wall", "Block", "Door")))
        {
            if (!hit.collider.gameObject.TryGetComponent(out BlockPart3D part) || BaseObject != part.BaseObject)
            {
                collider = hit.collider;
                // Debug.Log($"Collided with: {hit.collider.gameObject}");
            }
        }

        return collider != null;
    }
}
