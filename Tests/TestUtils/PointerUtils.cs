using System.Collections;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
// using InSys = UnityEngine.InputSystem;

namespace ReupVirtualTwinTests.utils
{
    public static class PointerUtils
    {
        public static IEnumerator MoveMouse(
            InputTestFixture input,
            Mouse mouse,
            Vector2 startMousePoint,
            Vector2 endMousePoint,
            int steps
        )
        {
            return MovePointer(
                startMousePoint,
                endMousePoint,
                steps,
                (Vector2 startPosition) => input.Move(mouse.position, startPosition),
                (Vector2 currentPosition, Vector2 delta) =>
                {
                    input.Move(mouse.position, currentPosition, delta);
                    input.Set(mouse.delta, delta);
                },
                (Vector2 endPosition) => { }
            );
        }
        public static IEnumerator DragMouseLeftButton(
            InputTestFixture input,
            Mouse mouse,
            Vector2 startMousePoint,
            Vector2 endMousePoint,
            int steps
        )
        {
            return MovePointer(
                startMousePoint,
                endMousePoint,
                steps,
                (Vector2 startPosition) => { input.Move(mouse.position, startPosition); input.Press(mouse.leftButton); },
                (Vector2 currentPosition, Vector2 delta) =>
                {
                    input.Move(mouse.position, currentPosition, delta);
                    input.Set(mouse.delta, delta);
                },
                (Vector2 endPosition) => input.Release(mouse.leftButton)
            );
        }

        public static IEnumerator MoveFinger(
            InputTestFixture input,
            Touchscreen touch,
            int touchId,
            Vector2 startScreenPosition,
            Vector2 endScreenPosition,
            int steps,
            bool endTouch = true,
            bool startTouch = true
        )
        {
            return MovePointer(
                startScreenPosition,
                endScreenPosition,
                steps,
                (Vector2 startPosition) =>
                {
                    if (startTouch)
                    {
                        input.BeginTouch(touchId, startPosition, true, touch);
                    }
                },
                (Vector2 currentPosition, Vector2 delta) => input.MoveTouch(touchId, currentPosition, delta, true, touch),
                (Vector2 endPosition) =>
                {
                    if (endTouch)
                    {
                        input.EndTouch(touchId, endPosition, Vector2.zero, true, touch);
                    }
                }
            );
        }

        public static IEnumerator MovePointer(
            Vector2 startScreenPosition,
            Vector2 endScreenPosition,
            int steps,
            Action<Vector2> startAction,
            Action<Vector2, Vector2> stepAction,
            Action<Vector2> endAction
        )
        {
            Vector2 startPosition = Camera.main.ViewportToScreenPoint(startScreenPosition);
            Vector2 endPosition = Camera.main.ViewportToScreenPoint(endScreenPosition);
            startAction(startPosition);
            for (int i = 1; i < steps + 1; i++)
            {
                float prevT = (float)(i - 1) / steps;
                float t = (float)i / steps;
                Vector2 prevPostion = Vector2.Lerp(startPosition, endPosition, prevT);
                Vector2 currentPosition = Vector2.Lerp(startPosition, endPosition, t);
                Vector2 delta = currentPosition - prevPostion;
                stepAction(currentPosition, delta);
                yield return null;
            }
            endAction(endPosition);
            yield return null;
        }

        public static IEnumerator AbsolutePositionTouchGesture(
            InputTestFixture input,
            Touchscreen touch,
            Vector2 startFinger1Position,
            Vector2 startFinger2Position,
            Vector2 endFinger1Position,
            Vector2 endFinger2Position,
            int steps)
        {
            // Debug.Log($"122: startFinger1Position >>>\n{startFinger1Position}");
            // Debug.Log($"123: startFinger2Position >>>\n{startFinger2Position}");
            // Debug.Log($"124: endFinger1Position >>>\n{endFinger1Position}");
            // Debug.Log($"125: endFinger2Position >>>\n{endFinger2Position}");
            input.BeginTouch(0, startFinger1Position, true, touch);
            // Debug.Log("first touch done");
            yield return null;
            input.BeginTouch(1, startFinger2Position, true, touch);
            // Debug.Log("second touch done");
            yield return null;
            // Debug.Log("one extra frame before starting to move");
            yield return null;

            for (int i = 1; i <= steps; i++)
            {
                float prevT = (float)(i - 1) / steps;
                float t = (float)i / steps;
                Vector2 prevFinger1Position = Vector2.Lerp(startFinger1Position, endFinger1Position, prevT);
                Vector2 currentFinger1Position = Vector2.Lerp(startFinger1Position, endFinger1Position, t);
                Vector2 deltaFinger1 = currentFinger1Position - prevFinger1Position;

                Vector2 prevFinger2Position = Vector2.Lerp(startFinger2Position, endFinger2Position, prevT);
                Vector2 currentFinger2Position = Vector2.Lerp(startFinger2Position, endFinger2Position, t);
                Vector2 deltaFinger2 = currentFinger2Position - prevFinger2Position;

                input.MoveTouch(0, currentFinger1Position, deltaFinger1, true, touch);
                input.MoveTouch(1, currentFinger2Position, deltaFinger2, true, touch);
                yield return null;
            }
            input.EndTouch(0, endFinger1Position, Vector2.zero, true, touch);
            input.EndTouch(1, endFinger2Position, Vector2.zero, true, touch);
            yield return null;
        }

        public static IEnumerator TouchGesture(
            InputTestFixture input,
            Touchscreen touch,
            Vector2 relativeStartFinger1Position,
            Vector2 relativeStartFinger2Position,
            Vector2 relativeEndFinger1Position,
            Vector2 relativeEndFinger2Position,
            int steps)
        {
            yield return AbsolutePositionTouchGesture(
                input,
                touch,
                Camera.main.ViewportToScreenPoint(relativeStartFinger1Position),
                Camera.main.ViewportToScreenPoint(relativeStartFinger2Position),
                Camera.main.ViewportToScreenPoint(relativeEndFinger1Position),
                Camera.main.ViewportToScreenPoint(relativeEndFinger2Position),
                steps);
        }

        public static void Tap(InputTestFixture input, Touchscreen touch, Vector2 tapPosition, int touchId = 0)
        {
            input.BeginTouch(touchId, tapPosition, true, touch);
            input.EndTouch(touchId, tapPosition);
        }
    }
}
