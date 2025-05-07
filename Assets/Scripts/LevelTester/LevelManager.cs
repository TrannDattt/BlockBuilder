using BuilderTool.Helpers;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>{
    public int Move {get; private set;}
    private bool _isMoving;

    private void GetInput(){
        if(!_isMoving && Input.GetKeyDown(KeyCode.Mouse0) && Block3DSelectHandler.Instance.SelectedBlock != null){
            _isMoving = true;
            Move++;
        }

        if(Block3DSelectHandler.Instance.SelectedBlock != null && Input.GetKeyUp(KeyCode.Mouse0)){
            _isMoving = false;
        }
    }

    void Update()
    {
        GetInput();
    }
}
