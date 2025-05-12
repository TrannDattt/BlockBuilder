using BuilderTool.Interfaces;
using BuilderTool.Mechanic;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderTool.LevelEditor
{
    public class MechanicIcon : Image{
        private GameObject _obj;
        private bool _isObjStatic;

        public void InitIcon(Sprite iconSprite, AMechanic mechanic, ICanHaveMechanic obj){
            sprite = iconSprite;
            _obj = obj.GetObject();

            if(obj.GetObject().GetComponent<EditorBlock>()){
                SetIconDynamic(mechanic);
            }
            else{
                SetIconStatic(mechanic);
            }

            gameObject.SetActive(true);
        }
        
        private void SetIconStatic(AMechanic mechanic){
            _isObjStatic = true;
            var doorTile = _obj.GetComponent<DoorTile>().GetDoor(mechanic.Dir).gameObject;
            transform.localScale = .3f * Vector2.one;
            SetPosition(doorTile.transform.position);
        }

        private void SetIconDynamic(AMechanic mechanic){
            _isObjStatic = false;
            transform.localScale = Vector2.one;
            SetPosition(_obj.transform.position);
        }

        private void SetPosition(Vector3 position){
            var pos = MechanicIconPooling.Instance.GetCanvasPosFromWorld(position);
            GetComponent<RectTransform>().anchoredPosition = pos;
        }

        public void ReturnToPool(){
            transform.localScale = Vector2.one;

            gameObject.SetActive(false);
        }

        void Update()
        {
            if(!_isObjStatic)
            {
                SetPosition(_obj.transform.position);
            }
        }
    }
}
