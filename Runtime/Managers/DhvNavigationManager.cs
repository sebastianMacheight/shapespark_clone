using ReupVirtualTwin.controllerInterfaces;
using Zenject;

namespace ReupVirtualTwin.controllers
{
    public class DhvNavigationManager: IDhvNavigationManager
    {
        public bool isRotating { get; private set; }
        public bool isZooming { get; private set; }

        [Inject]
        public DhvNavigationManager()
        {
            isRotating = false;
            isZooming = false;
        }

        public void Rotate()
        {
            if (!isZooming)
            {
                isRotating = true;
            }
        }
    
        public void Zoom()
        {
            if (!isRotating)
            {
                isZooming = true;
            }
        }

        public void StopRotation()
        {
            isRotating = false;
        }

        public void StopZoom()
        {
            isZooming = false;
        }
    }
}