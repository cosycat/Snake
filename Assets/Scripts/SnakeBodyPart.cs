using UnityEngine;

public partial class Snake {
    private class SnakeBodyPart {

        private SnakeBodyPart Next { get; set; }
        private Vector2Int position;
        public Vector2Int Position { get => position; }
        private Vector2Int lastDirection;
        public Vector2Int LastDirection { get => lastDirection; set => lastDirection = value; }

        private readonly int index;

        public bool IsHead => index == 0;


        private readonly GameObject gameObject;

        /// <summary>
        /// Constructor for the head.
        /// </summary>
        /// <param name="headGameObject"></param>
        public SnakeBodyPart(GameObject headGameObject, Vector2Int startPosition, Vector2Int startDirection) {
            index = 0;
            gameObject = headGameObject;
            lastDirection = startDirection;
            SetPositionAndRotation(startPosition, LastDirection);
            //headGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeHeadSprite;
        }

        /// <summary>
        /// Constructor for the body.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="previous"></param>
        public SnakeBodyPart(int index) {
            this.index = index;

            GameObject bodyPartGO = new GameObject("Body Part");
            SpriteRenderer bodyPartSpriteRenderer = bodyPartGO.AddComponent<SpriteRenderer>();
            bodyPartSpriteRenderer.sprite = GameAssets.instance.snakeTailSprite;
            bodyPartSpriteRenderer.sortingOrder = index;
            gameObject = bodyPartGO;
        }

        /// <summary>
        /// Moves this <code>SnakeBodyPart</code> one step forward in the given direction.
        /// </summary>
        /// <param name="directionToMove">The direction in which the body part should be moved now.</param>
        /// <param name="nextDirection">The direction it will move next. Used for rotating corner pieces. <code>null</code>, if it is the head.</param>
        public void MoveInDirection(Vector2Int directionToMove, Vector2Int? nextDirection = null) {
            if (nextDirection == null) {
                nextDirection = directionToMove;
            }

            Vector2Int newPosition = position + directionToMove;

            SetPositionAndRotation(newPosition, directionToMove, nextDirection.Value);

            if (Next != null) {
                Next.MoveInDirection(lastDirection, directionToMove);
            }
            lastDirection = directionToMove;
        }

        internal void SetPositionAndRotation(Vector2Int position, Vector2Int directionToMove, Vector2Int? nextDirection = null) {
            gameObject.transform.position = new Vector3(position.x, position.y);
            gameObject.transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(nextDirection.HasValue ? directionToMove + nextDirection.Value : directionToMove) - 90);
            this.position = new Vector2Int(position.x, position.y);
        }

        internal void AddNewBodyPart() {
            if (Next == null) {
                Next = new SnakeBodyPart(index + 1);
                Next.SetPositionAndRotation(this.position - this.lastDirection, this.lastDirection, this.lastDirection);
            } else {
                Next.AddNewBodyPart();
            }
        }

        private float GetAngleFromVector(Vector2Int direction) {
            float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            while (n < 0) n += 360;
            return n;
        }

        /// <summary>
        /// Returns the number of body parts without or without the head.
        /// </summary>
        /// <param name="withHead">Whether the head should be counted.</param>
        /// <returns>The number of body parts.</returns>
        public int getBodyPartCount(bool withHead) {
            if (Next == null) {
                if (withHead) {
                    return index + 1;
                }
                return index;
            } else {
                return Next.getBodyPartCount(withHead);
            }
        }

    }


}
