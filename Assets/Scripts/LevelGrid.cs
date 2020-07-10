using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class LevelGrid {

    private readonly int width;
    private readonly int height;

    private Vector2Int foodGridPosition;

    GameObject foodGameObject;

    public Snake Snake { get; }


    public LevelGrid(int width, int height, Snake snake) {
        this.width = width;
        this.height = height;
        this.Snake = snake;

        snake.SnakeMoved += OnSnakeMoved;

        SpawnFood();
    }

    private void SpawnFood() {
        do {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (Snake.GetCompleteSnakeGridPosition().Contains(foodGridPosition));

        foodGameObject = new GameObject("Food");
        SpriteRenderer spriteRenderer = foodGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameAssets.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public void OnSnakeMoved(object source, SnakeEventArgs args) {
        if (foodGridPosition == args.NewGridPosition) {
            Object.Destroy(foodGameObject);
            Snake.HasEatenFood();
            SpawnFood();
        }
    }

}
