using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    private LevelGrid levelGrid;
    private Snake snake;

    public GameObject snakeGameObject;

    private void Awake() {
        snake = snakeGameObject.GetComponent<Snake>();

        levelGrid = new LevelGrid(20, 20, snake);
    }

    void Start() {

        /*
        GameObject snakeHeadGameObject = new GameObject("Snake Head");
        SpriteRenderer snakeSpriteRenderer = snakeHeadGameObject.AddComponent<SpriteRenderer>();
        snakeSpriteRenderer.sprite = GameAssets.instance.snakeHeadSprite;
        */
    }

    void Update() {

    }
}
