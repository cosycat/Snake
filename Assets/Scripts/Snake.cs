using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEventArgs : EventArgs {
    public Vector2Int NewGridPosition { get; set; }
}

public partial class Snake : MonoBehaviour {

    // Moving
    private Vector2Int HeadGridPosition { get { return headBodyPart.Position; } }

    private Vector2Int nextGridMoveDirection;
    private Vector2Int LastGridMoveDirection { get { return headBodyPart.LastDirection; } }
    private float gridMoveTimeRemaining;
    [SerializeField] private float totalGridMoveTime;
    public event EventHandler<SnakeEventArgs> SnakeMoved;

    // Body Parts
    [SerializeField] private int snakeSize;
    private List<Vector2Int> snakeBodyPartPositionList = new List<Vector2Int>();
    private SnakeBodyPart headBodyPart;


    internal void HasEatenFood() {
        snakeSize++;
    }


    private void Awake() {
        nextGridMoveDirection = new Vector2Int(0, 1);
        gridMoveTimeRemaining = totalGridMoveTime;
        headBodyPart = new SnakeBodyPart(gameObject, new Vector2Int(10, 10), new Vector2Int(0, 1));
    }


    private void Update() {
        HandleInput();

    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleInput() {
        Vector2Int newGridMoveDirection = new Vector2Int(nextGridMoveDirection.x, nextGridMoveDirection.y);
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            newGridMoveDirection.Set(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            newGridMoveDirection.Set(0, -1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            newGridMoveDirection.Set(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            newGridMoveDirection.Set(1, 0);
        }
        if ((newGridMoveDirection + LastGridMoveDirection).magnitude != 0 || LastGridMoveDirection.magnitude == 0 || headBodyPart.getBodyPartCount(true) == 1) {
            // Only set the new direction if the player doesn't try to go in the exact opposite direction as before.
            nextGridMoveDirection = newGridMoveDirection;
        }
    }

    private void HandleMovement() {
        if (gridMoveTimeRemaining <= 0) {
            if (headBodyPart.getBodyPartCount(false) < snakeSize) {
                headBodyPart.AddNewBodyPart();
            }
            headBodyPart.MoveInDirection(nextGridMoveDirection);

            gridMoveTimeRemaining += totalGridMoveTime;

            OnSnakeMoved();
        }

        gridMoveTimeRemaining -= Time.deltaTime;
    }

    



    protected virtual void OnSnakeMoved() {
        if (SnakeMoved != null) {
            SnakeMoved(this, new SnakeEventArgs() { NewGridPosition = GetHeadGridPosition() });
        }
    }

    

    public List<Vector2Int> GetCompleteSnakeGridPosition() {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();

        gridPositionList.Add(HeadGridPosition);
        gridPositionList.AddRange(snakeBodyPartPositionList);

        return gridPositionList;
    }

    public Vector2Int GetHeadGridPosition() {
        return HeadGridPosition;
    }


}
