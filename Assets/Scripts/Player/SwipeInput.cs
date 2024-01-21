using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class SwipeInput : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public bool SwipedLeft { get; private set; }
        public bool SwipedRight { get; private set; }

        private Vector2 dragStartPosition;

        public void OnDrag(PointerEventData eventData)
        {
            // Определение направления свайпа во время перетаскивания
            float dragX = eventData.position.x - dragStartPosition.x;

            if (Mathf.Abs(dragX) > 50f) // Проверка минимальной длины свайпа
            {
                SwipedLeft = dragX < 0;
                SwipedRight = dragX > 0;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Сброс флагов по завершении свайпа
            SwipedLeft = false;
            SwipedRight = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Запомнить начальную позицию свайпа
            dragStartPosition = eventData.position;
        }
    }
}
