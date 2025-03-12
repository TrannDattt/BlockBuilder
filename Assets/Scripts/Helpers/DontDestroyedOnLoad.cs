using UnityEngine;

namespace BuilderTool.Helpers
{
    public class DontDestroyedOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
