using UnityEngine;

namespace BuilderTool.LevelEditor
{
    public class UserInputHandler : MonoBehaviour
    {
        private void GetUserInput()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                                     Vector2.zero,
                                                     Mathf.Infinity,
                                                     LayerMask.GetMask("Map", "Block"));

                if (!hit.collider)
                {
                    if(hit.collider.gameObject.TryGetComponent<EditorField>(out EditorField editorField)){

                    }

                    if (hit.collider.gameObject.TryGetComponent<EditorBlock>(out EditorBlock editorBlock)){
                        SelectBlock(editorBlock);
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //UpdateOnfieldBlocks();
            }
        }

        private void SelectTile()
        {

        }

        private void SelectBlock(EditorBlock block)
        {

        }
    }
}
