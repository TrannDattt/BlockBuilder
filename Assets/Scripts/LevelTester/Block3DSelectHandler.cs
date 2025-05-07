using BuilderTool.Helpers;
using UnityEngine;

public class Block3DSelectHandler : Singleton<Block3DSelectHandler>{
    public Block3D SelectedBlock {get; private set;}

    private Camera _mainCam;
    private Vector3 _clickPos;
    private Vector3 _offset;

    private Block3D GetSelectedBlock(Vector2 selectPos){
        // Ray ray = new(selectPos, Vector3.forward);
        // if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Block"))){
        //     var block = hit.collider.gameObject.GetComponentInParent<Block3D>();
        //     return block;
        // }
    
        var ray = _mainCam.ScreenPointToRay(selectPos);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Block"));
        if(hit.collider != null){
            var block = hit.collider.gameObject.GetComponentInParent<Block3D>();
            // Debug.Log(block);
            return block;
        }

        // Debug.Log(1);
        return null;
    }

    public bool CheckBlockCollide(Vector2 collidePoint){
        Vector2 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

        var dir = mousePos - collidePoint;
        var distance = .1f;

        if(Physics2D.Raycast(collidePoint, dir, distance, LayerMask.GetMask("Wall"))){
            return true;
        }

        return false;
    }

    // TODO: Make block stop when collide with walls or other blocks
    //      Solution: use 2d block as base for collider => Use composite collider to connect them
    //              => Check collided
    private void MoveBlock(Vector3 mousePos){
        var ray = _mainCam.ScreenPointToRay(mousePos);
        var zPos = SelectedBlock.transform.position.z;
        var targetPos = ray.origin + _offset;
        // SelectedBlock.Body.
        SelectedBlock.Body.MovePosition(targetPos);

        Vector3 pos3D = SelectedBlock.transform.position;
        pos3D.z = zPos;
        SelectedBlock.transform.position = pos3D;

        // if(!SelectedBlock.IsCollide){
        //     SelectedBlock.transform.position = targetPos;
        // }
    }

    private void SnapBlockToTile(){
        Vector2 blockPos = SelectedBlock.transform.position;
        var ray = _mainCam.ScreenPointToRay(blockPos);
        
        var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Map"));

        if(hit.collider != null){
            var tile = hit.collider.gameObject.transform.parent;
            var tilePos = tile.position;
            Vector3 newPos = new(tilePos.x, tilePos.y, SelectedBlock.transform.position.z);
            SelectedBlock.transform.position = newPos;
        }
    }

    private void HandleMouseInput(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            var mousePos = Input.mousePosition;
            // var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            SelectedBlock = GetSelectedBlock(mousePos);

            if(SelectedBlock != null){
                // _clickPos = mousePos;
                var ray = _mainCam.ScreenPointToRay(mousePos);
                _offset = SelectedBlock.transform.position - ray.origin;
            }
        }

        // if(Input.GetKey(KeyCode.Mouse0) && SelectedBlock != null){
        //     var mousePos = Input.mousePosition;
        //     // var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        //     MoveBlock(mousePos);
        // }

        if(Input.GetKeyUp(KeyCode.Mouse0) && SelectedBlock != null){
            SnapBlockToTile();
            SelectedBlock = null;
        }
    }

    void OnEnable()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        HandleMouseInput();
    }

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Mouse0) && SelectedBlock != null){
            var mousePos = Input.mousePosition;
            // var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            MoveBlock(mousePos);
        }
    }
}
