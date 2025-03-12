using BuilderTool.Enums;
using UnityEngine;

namespace BuilderTool.LevelEditor
{
    [System.Serializable]
    public abstract class ATileAttribute : MonoBehaviour
    {
        [field: SerializeField] public Color TileColor { get; private set; }
        [field: SerializeField] public bool CanContainBlock { get; private set; }
        [field: SerializeField] public ETileType TileType { get; private set; }

        public virtual void ResetAttribute()
        {
        }
    }
}
