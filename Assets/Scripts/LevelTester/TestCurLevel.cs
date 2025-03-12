using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuilderTool.Test
{
    public class TestCurLevel : MonoBehaviour
    {
        private const string LEVEL_TESTER_SCENE = "LevelTester";

        public void TestLevel()
        {
            SceneManager.LoadScene(LEVEL_TESTER_SCENE);
        }
    }
}
