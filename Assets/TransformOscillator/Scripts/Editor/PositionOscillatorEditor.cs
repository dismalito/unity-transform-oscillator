using UnityEditor;
using UnityEngine;

namespace TransformOscillator.Scripts.Editor
{
    [CustomEditor(typeof(PositionOscillator))]
    public class PositionOscillatorEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var pointMover = (PositionOscillator)target;
            DrawHandles(pointMover);
        }

        private void DrawHandles(PositionOscillator positionOscillator)
        {
            var originPosition = positionOscillator.transform.position;
            var handlerPoint = positionOscillator._offsetPoint + originPosition;
            var oppositePoint = positionOscillator._offsetPoint * -1 + originPosition;
            DrawHandlerCircle(positionOscillator, handlerPoint);
            DrawNormalCircle(oppositePoint);
            DrawLine(handlerPoint, originPosition);
            DrawLine(oppositePoint, originPosition);
            DrawLabel(handlerPoint);
        }

        private void DrawNormalCircle(Vector3 handlerPoint)
        {
            Handles.color = Color.yellow;
            GetFreeMoveHandle(handlerPoint, 0.05f, Handles.CircleHandleCap);
        }

        private void DrawLabel(Vector3 point)
        {
            Handles.Label(point + new Vector3(0, HandleUtility.GetHandleSize(point) * 0.2f, 0), "Movement distance point");
        }

        private void DrawHandlerCircle(PositionOscillator positionOscillator, Vector3 handlerPoint)
        {
            Handles.color = Color.cyan;
            var handlerNewPosition = GetFreeMoveHandle(handlerPoint, 0.25f, Handles.SphereHandleCap);
            handlerNewPosition = ApplyConstraints(handlerNewPosition, positionOscillator);
            
            if (positionOscillator._offsetPoint != handlerNewPosition - positionOscillator.transform.position)
            {
                positionOscillator._offsetPoint = handlerNewPosition - positionOscillator.transform.position;
                EditorUtility.SetDirty(positionOscillator);
            }
        }

        private Vector3 ApplyConstraints(Vector3 handlerPosition, PositionOscillator positionOscillator)
        {
            var constraints = positionOscillator._constraints;
            var offsetPoint = positionOscillator._offsetPoint;
            var originPosition = positionOscillator.transform.position;
            var x = constraints.FreezePositionX ? (originPosition.x + offsetPoint.x) : handlerPosition.x;
            var y = constraints.FreezePositionY ? (originPosition.y + offsetPoint.y) : handlerPosition.y;
            var z = constraints.FreezePositionZ ? (originPosition.z + offsetPoint.z) : handlerPosition.z;
            return new Vector3(x, y, z);
        }

        private static void DrawLine(Vector3 handlerPoint, Vector3 origin)
        {
            Handles.color = Color.cyan;
            Handles.DrawLine(origin, handlerPoint);
        }

        private static Vector3 GetFreeMoveHandle(Vector3 point, float size, Handles.CapFunction handleType) =>
            Handles.FreeMoveHandle(point, HandleUtility.GetHandleSize(point) * size, Vector3.zero, handleType);
    }
}