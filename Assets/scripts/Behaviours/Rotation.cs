using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float sensitivity = 1f;

    private CharacterRotationManager _characterRotationManager;
    private InputProvider _inputProvider;
    private DragManager _dragManager;

    private void Awake()
    {
        _inputProvider = new InputProvider();
        _dragManager = GetComponent<DragManager>();
        _characterRotationManager = GetComponent<CharacterRotationManager>();
    }

    void Update()
    {
        if (_dragManager.dragging)
        {
            Vector2 look = _inputProvider.RotateViewInput();
            _characterRotationManager.horizontalRotation += (look.x * sensitivity);
            _characterRotationManager.verticalRotation += (look.y * sensitivity * -1f);
        }
    }

}
