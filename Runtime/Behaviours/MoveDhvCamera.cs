using ReupVirtualTwin.controllerInterfaces;
using ReupVirtualTwin.inputs;
using ReupVirtualTwin.managerInterfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ReupVirtualTwin.behaviours
{
    public class MoveDhvCamera : MonoBehaviour
    {
        [SerializeField] public float KeyboardMoveCameraSpeedMetersPerSecond = 40; //todo inject this from Reup prefab

        InputProvider _inputProvider;
        IDragManager dragManager;
        IZoomPositionRotationDHVController zoomPositionRotationDHVController;

        private void OnEnable()
        {
            _inputProvider.holdStarted += OnHoldStarted;
        }
        private void OnDisable()
        {
            _inputProvider.holdStarted -= OnHoldStarted;
        }

        void OnHoldStarted(InputAction.CallbackContext ctx)
        {
            zoomPositionRotationDHVController.startingFocusScreenPoint = _inputProvider.PointerInput();
        }

        [Inject]
        public void Init(
            IDragManager dragManager,
            InputProvider inputProvider,
            IZoomPositionRotationDHVController zoomPositionRotationDHVController)
        {
            _inputProvider = inputProvider;
            this.dragManager = dragManager;
            this.zoomPositionRotationDHVController = zoomPositionRotationDHVController;
        }

        void Update()
        {
            KeyboardUpdatePosition();
            PointerUpdatePosition();
        }

        void KeyboardUpdatePosition()
        {
            Vector2 inputValue = _inputProvider.KeyboardMoveDhvCamera().normalized;
            zoomPositionRotationDHVController.moveInDirection(inputValue, KeyboardMoveCameraSpeedMetersPerSecond);
        }

        void PointerUpdatePosition()
        {
            if (!dragManager.dragging)
            {
                return;
            }
            zoomPositionRotationDHVController.focusScreenPoint = _inputProvider.PointerInput();
        }

    }
}
