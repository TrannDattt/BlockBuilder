using UnityEngine;

public class SceneCameraController : MonoBehaviour
{
    [SerializeField] private float baseFOV = 60f;
    [SerializeField] private float fovScaleFactor = 3.0f; // điều chỉnh độ "zoom" theo size grid

    private void Start()
    {
        // if (LevelDataHolder.Instance == null)
        // {
        //     Debug.LogWarning("Không có dữ liệu level.");
        //     return;
        // }

        // // TODO: Làm thành 1 hàm chứ đừng vứt cả vào Start
        // //
        // int width = LevelDataHolder.Instance.GridWidth;
        // int height = LevelDataHolder.Instance.GridHeight;

        // // Tính vị trí trung tâm lưới
        // Vector3 gridCenter = new Vector3(width / 2f, -height / 2f, 0f); // -height để đúng chiều y bạn dùng

        // // Thiết lập vị trí camera (giả sử nhìn từ trên xuống)
        // float cameraHeight = 10f; // khoảng cách trục Z hoặc Y để nhìn từ trên xuống
        // transform.position = new Vector3(gridCenter.x, gridCenter.y-2, -cameraHeight); // camera đặt trước lưới
        // transform.LookAt(gridCenter);

        // // Thiết lập FOV theo size lưới
        // Camera cam = GetComponent<Camera>();
        // if (cam != null && cam.orthographic == false)
        // {
        //     float maxSize = Mathf.Max(width, height);
        //     cam.fieldOfView = baseFOV + maxSize * fovScaleFactor;
        // }
        // //
    }
}
