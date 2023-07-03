using UnityEngine;
using ReUpVirtualTwin.Helpers;
using UnityEngine.InputSystem;

[RequireComponent(typeof(IRayProvider))]
public abstract class Select : MonoBehaviour
{
    protected InputProvider _inputProvider;
    protected IRayProvider _rayProvider;
    protected DragManager _dragManager;

    protected virtual void Awake()
    {
        _inputProvider = new InputProvider();
        _rayProvider = GetComponent<IRayProvider>();
        _dragManager = ObjectFinder.FindDragManager();
    }

    private void OnEnable()
    {
        _inputProvider.selectCanceled += OnSelect;
    }


    private void OnDisable()
    {
        _inputProvider.selectCanceled -= OnSelect;
    }


    public abstract void OnSelect(InputAction.CallbackContext ctx);
}
