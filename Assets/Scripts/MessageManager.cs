using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;

public class MessageManager : MonoBehaviour {
    public static MessageManager inst;
    public static bool isController => Input.GetJoystickNames().Length > 0;
    
    public MessageScreen[] screens;
    public Text messageBox;
    public float fadeTime = 1.0f;
    public int maxMessages = 3;

    private Queue<TextLine> messages;
    private Image messageBG;
    private RectTransform messageRect;

    private void Awake() {
        inst = this;
        messages = new Queue<TextLine>();

        messageBG = messageBox.transform.parent.GetComponent<Image>();
        messageBG.enabled = false;
        messageRect = messageBG.GetComponent<RectTransform>();
    }

    public void ShowMessage(string message, float duration) {
        while (messages.Count >= maxMessages) {
            messages.Dequeue();
        }

        messages.Enqueue(new TextLine(message, Time.time + duration));
        UpdateTextBox();
    }

    private void Update() {
        bool needsUpdate = false;
        while (messages.Count > 0 && Time.time > messages.Peek().timeToDie) {
            messages.Dequeue();
            needsUpdate = true;
        }

        if (needsUpdate) {
            UpdateTextBox();
        }
    }

    private void UpdateTextBox() {
        var builder = new StringBuilder();
        foreach (var msg in messages) {
            if (builder.Length > 0) {
                builder.Append("\n");
            }
            builder.Append(msg.text);
        }
        messageBox.text = builder.ToString();
        messageBG.enabled = builder.Length > 0;
        messageRect.sizeDelta = Vector2.one;
    }

    public CanvasGroup ShowScreen(int index) {
        var screen = screens[index];
        CanvasGroup group;
        if (isController) {
            group = screen.controller;
        } else {
            group = screen.keyboard;
        }

        StartCoroutine(ScreenCoroutine(group, screen.duration));
        return group;
    }

    private IEnumerator ScreenCoroutine(CanvasGroup group, float duration) {
        group.alpha = 1.0f;
        yield return new WaitForSeconds(duration);
        
        while (group.alpha > 0) {
            group.alpha -= Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    private struct TextLine {
        public readonly string text;
        public readonly float timeToDie;

        public TextLine(string text, float timeToDie) {
            this.text = text;
            this.timeToDie = timeToDie;
        }
    }

    [System.Serializable]
    public struct MessageScreen {
        public CanvasGroup controller;
        public CanvasGroup keyboard;
        public float duration;
    }
}
