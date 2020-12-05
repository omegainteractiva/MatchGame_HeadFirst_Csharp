using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;


public class GameManager : MonoBehaviour
{
    public GameObject panelGrid, info, lost;
    public TextMeshProUGUI timeText;
    public float maxTime = 10f;

    private bool _stopTime;
    
    private TextMeshProUGUI[] _textEmojis;
    private Button[] _buttons;
    
    private TextMeshProUGUI _lastTextBlockClicked;
    private bool _findingMatch;
    private int _matchesFound;
    

    private void Awake()
    {
        _textEmojis = panelGrid.GetComponentsInChildren<TextMeshProUGUI>();
        _buttons = panelGrid.GetComponentsInChildren<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpGame();
        ButtonBehavior();
    }

    private void Update()
    {
        Timer_Tick();
    }

    private void SetUpGame()
    {
        List<string> animalEmoji = new List<string>()
        {
            "<sprite=1>", "<sprite=1>",
            "<sprite=14>", "<sprite=14>",
            "<sprite=3>", "<sprite=3>",
            "<sprite=11>", "<sprite=11>",
            "<sprite=5>", "<sprite=5>",
            "<sprite=6>", "<sprite=6>",
            "<sprite=7>", "<sprite=7>",
            "<sprite=0>", "<sprite=0>",
        };
        
        Random random = new Random();

        foreach (var textEmoji in _textEmojis)
        {
            int index = random.Next(animalEmoji.Count);
            string nextEmoji = animalEmoji[index];
            textEmoji.text = nextEmoji;
            animalEmoji.RemoveAt(index);
        }
    }

    private void ButtonBehavior()
    {
        foreach (var button in _buttons)
        {
            var textMeshPro = button.transform.GetComponentInChildren<TextMeshProUGUI>();
            button.onClick.AddListener((() =>
            {
                TextBlock_MouseDown(button, textMeshPro);
            }));
        }
    }

    private void TextBlock_MouseDown(Button button, TextMeshProUGUI textMeshPro)
    {
        if (_findingMatch == false)
        {
            button.interactable = false;
            _lastTextBlockClicked = textMeshPro;
            _findingMatch = true;
        }
        else if (textMeshPro.text == _lastTextBlockClicked.text)
        {
            _matchesFound++;
            button.gameObject.SetActive(false);
            _lastTextBlockClicked.transform.parent.gameObject.SetActive(false);
            _findingMatch = false;
        }
        else
        {
            _lastTextBlockClicked.transform.parent.GetComponent<Button>().interactable = true;
            _findingMatch = false;
        }
    }

    private void Timer_Tick()
    {
        if (!_stopTime)
        {
            maxTime -= Time.deltaTime;
        }
        
        timeText.text = (Mathf.RoundToInt(maxTime)).ToString("00s");
        if (maxTime < 1)
        {
            lost.SetActive(true);
        }

        if (_matchesFound == 8)
        {
            _stopTime = true;
            info.SetActive(true);
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
}
