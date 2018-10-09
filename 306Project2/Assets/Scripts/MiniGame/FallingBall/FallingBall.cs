﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingBall : MonoBehaviour {
    public GameObject ballPrefab;
    public GameObject backet;
    public Text count;

    public Slider slider;
    public GameObject readyPrefab;
    public GameObject goPrefab;

    // List of possible sprite appearances.
    public List<Sprite> possibleSprites = new List<Sprite>();

    Slider bar;
    GameObject ready;
    GameObject go;

    float timeLimit;
    float currentTime;
    float readyTime = 0.9f;
    float goTime = 0.5f;

    int numOfBalls = 5;
    int currentBalls = 0;
    int goal;
    float xRange;
    float y;

    float leftWall;
    float rightWall;
    bool gameStart;

	// Use this for initialization
	void Start () {
        goal = 98;
        timeLimit = 10f;
        count.text = goal.ToString();
        
        xRange = gameObject.GetComponentInParent<Canvas>().pixelRect.width / 2;
        y = gameObject.GetComponentInParent<Canvas>().pixelRect.height / 2;

        rightWall = gameObject.GetComponentInParent<Canvas>().pixelRect.width / 2;
        leftWall = -rightWall;


        //Initialise time bar
        RectTransform parentRectTransform = gameObject.GetComponent<RectTransform>();
        bar = Instantiate(slider);
        RectTransform barRectTransform = bar.GetComponent<RectTransform>();
        barRectTransform.sizeDelta = new Vector2(gameObject.GetComponentInParent<Canvas>().pixelRect.width * 0.95f,
            gameObject.GetComponentInParent<Canvas>().pixelRect.height * 0.02f);
        float sliderYPosition = gameObject.GetComponentInParent<Canvas>().pixelRect.height / 2 - barRectTransform.rect.height;
        barRectTransform.SetParent(parentRectTransform);
        barRectTransform.localPosition = new Vector2(0, -sliderYPosition);
        bar.value = 0;

        //Generate Ready/Go and set default properties
        ready = Instantiate(readyPrefab);
        ready.GetComponent<RectTransform>().SetParent(parentRectTransform);
        ready.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        ready.SetActive(false);

        go = Instantiate(goPrefab);
        go.GetComponent<RectTransform>().SetParent(parentRectTransform);
        go.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        go.SetActive(false);

        currentTime = -readyTime - goTime;
        gameStart = false;
    }
    


    void OnGUI()
    {
        //Finish game when tehre is no more arrows to press
        if (goal == 0)
        {
            Finish();
        }
        if (gameStart)
        {
            if (currentTime > timeLimit)
            {
                Fail();
            }
            //Listen to key press event
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                float x = backet.GetComponent<RectTransform>().localPosition.x;
                float y = backet.GetComponent<RectTransform>().localPosition.y;
                if (e.keyCode.ToString().Equals("LeftArrow") && x > leftWall)
                {
                    backet.GetComponent<RectTransform>().localPosition = new Vector2 (x - 10, y);
                }
                else if (e.keyCode.ToString().Equals("RightArrow") && x < rightWall)
                {
                    backet.GetComponent<RectTransform>().localPosition = new Vector2(x + 10, y);
                }
            }
        }
    }


    // Update is called once per frame
    void Update () {
        if (!gameStart)
        {
            if (currentTime >= 0) //Start game. Set arrows visible
            {
                go.SetActive(false);
                ready.SetActive(false);
                gameStart = true;
                currentTime = 0;
            }
            else if (Mathf.Abs(currentTime) < goTime) //Show "Go!"
            {
                if (go.activeSelf)
                {
                    float time = Mathf.Sin(Mathf.Lerp(0f, 1f, Mathf.Abs(currentTime) / goTime));
                    go.GetComponent<Text>().color = new Color(time, time, 0);
                }
                else
                {
                    go.SetActive(true);
                    ready.SetActive(false);
                }
            }
            else //Show "Read?"
            {
                if (ready.activeSelf)
                {
                    float percentage = Mathf.Abs(currentTime) - goTime;
                    float time = Mathf.Sin(Mathf.Lerp(0f, 1f, percentage / readyTime));
                    ready.GetComponent<Text>().color = new Color(0, time, 0);
                }
                else
                {
                    go.SetActive(false);
                    ready.SetActive(true);
                }
            }
        }
        else
        {
            if (currentBalls < numOfBalls)
            {
                // Create a new ball.
                GameObject ball = Instantiate(ballPrefab);
                // Set it to have a random appearance.
                int i = Random.Range(0, possibleSprites.Count);
                ball.GetComponent<Image>().sprite = possibleSprites[i];
                // Get the ball's parent and position.
                ball.GetComponent<RectTransform>().SetParent(gameObject.GetComponent<RectTransform>());
                // Set it to a random position.
                ball.GetComponent<RectTransform>().localPosition = new Vector2(Random.Range(0, xRange) * Random.Range(-1f, 1f), y);
                currentBalls++;
            }
            bar.value = Mathf.Lerp(0f, 1f, currentTime / timeLimit);
        }
        currentTime += Time.deltaTime;
    }

    public void catchBall(GameObject ball)
    {
        Destroy(ball);
        goal--;
        currentBalls--;
        count.text = goal.ToString();
    }

    private void Finish()
    {
        //Notify the game manager that the player has successfully finished the game
        //GameObject.FindGameObjectWithTag("MiniGameManager").GetComponent<MiniGameManager>().FinishGame(true);
        Debug.Log("finished");
    }

    private void Fail()
    {
        //Notify the game manager that the player has failed the game
        //GameObject.FindGameObjectWithTag("MiniGameManager").GetComponent<MiniGameManager>().FinishGame(false);
        Debug.Log("failed");
    }
}
