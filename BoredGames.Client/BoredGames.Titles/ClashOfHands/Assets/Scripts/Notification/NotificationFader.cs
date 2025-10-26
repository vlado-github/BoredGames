using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NotificationFader : MonoBehaviour
{
    public CanvasGroup notificationCanvasGroup;
    public float fadeInDuration = 0.5f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 0.5f;
    public string notificationTextTag = "notification_text";

    void Start()
    {
        // Ensure the notification is initially invisible
        notificationCanvasGroup.alpha = 0f;
    }

    public void ShowNotification(string message, Color? textColor = null, float? duration = null)
    {
        var color = textColor != null ? textColor.Value : Color.white;
        var durationOfDisplay = duration != null ? duration.Value : displayDuration;
        StartCoroutine(FadeNotificationRoutine(message, color, durationOfDisplay));
    }

    private IEnumerator FadeNotificationRoutine(string message, Color textColor, float durationOfDisplay)
    {
        //Set message text
        var notificationObject = GameObject.FindGameObjectWithTag(notificationTextTag);
        if (notificationObject != null)
        {
            var notificationMessage = notificationObject.GetComponent<TextMeshProUGUI>();
            if (notificationMessage != null)
            {
                notificationMessage.text = message.ToUpper();
                notificationMessage.color = textColor;
            }
        }

        // Fade In
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            notificationCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            timer += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        notificationCanvasGroup.alpha = 1f; // Ensure full visibility

        // Display Duration
        yield return new WaitForSeconds(durationOfDisplay);

        // Fade Out
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            notificationCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
            timer += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        notificationCanvasGroup.alpha = 0f; // Ensure full transparency
    }
}
