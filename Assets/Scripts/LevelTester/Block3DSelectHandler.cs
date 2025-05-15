using BuilderTool.Helpers;
using UnityEngine;

public class Block3DSelectHandler : Singleton<Block3DSelectHandler>{
    public Block3D SelectedBlock {get; private set;}

    private Camera _mainCam;
    private Vector3 _clickPos;
    private Vector3 _offset;

    private Block3D GetSelectedBlock(Vector3 clickPos){
        Ray ray = _mainCam.ScreenPointToRay(clickPos);
    
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Block"))) {
            var block = hit.collider.GetComponentInParent<Block3D>();
            return block;
        }

        return null;
    }

    public bool CheckBlockCollide(Vector3 collidePoint){
        Vector3 mouseWorld = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = collidePoint.z;

        Vector3 dir = (mouseWorld - collidePoint).normalized;
        float distance = 0.1f;

        return Physics.Raycast(collidePoint, dir, distance, LayerMask.GetMask("Wall"));
    }

    private void MoveBlock(Vector3 mousePos){
        Ray ray = _mainCam.ScreenPointToRay(mousePos);
        Vector3 targetPos = ray.origin + _offset;
        targetPos.z = SelectedBlock.transform.position.z;

        if(SelectedBlock.CheckCollidedWhenMove(targetPos))
        {
            SelectedBlock.Body.MovePosition(targetPos);
        }
    }

    private void SnapBlockToTile(){
        Vector3 blockPos = SelectedBlock.transform.position;
        // Ray ray = _mainCam.ScreenPointToRay(blockPos);
        Ray ray = new(blockPos, Vector3.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Map"))) {
            Transform tile = hit.collider.transform.parent;
            Vector3 tilePos = tile.position;
            Vector3 newPos = new(tilePos.x, tilePos.y, SelectedBlock.transform.position.z);
            SelectedBlock.Body.MovePosition(newPos);
        }
    }

    private void HandleMouseInput(){
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            var mousePos = Input.mousePosition;
            SelectedBlock = GetSelectedBlock(mousePos);

            if (SelectedBlock != null) {
                Ray ray = _mainCam.ScreenPointToRay(mousePos);
                _offset = SelectedBlock.transform.position - ray.origin;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && SelectedBlock != null) {
            MoveBlock(Input.mousePosition);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && SelectedBlock != null) {
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
        
    }
}
