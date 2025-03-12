using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuilderTool.Test
{
    public class DesignCurLevel : MonoBehaviour
    {
        private const string LEVEL_EDITOR_SCENE = "LevelEditor";

        public void DesignLevel()
        {
            SceneManager.LoadScene(LEVEL_EDITOR_SCENE);
        }
    }
}
