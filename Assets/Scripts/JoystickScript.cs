using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform background;
    private RectTransform handle;
    private Vector2 inputVector;

    public Vector2 InputVector => inputVector;

    private void Start()
    {
        background = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(null, background.position);
        Vector2 radius = background.sizeDelta / 2;
        inputVector = (eventData.position - position) / (radius * 2);
        inputVector = inputVector.magnitude > 1.0f ? inputVector.normalized : inputVector;

        handle.anchoredPosition = new Vector2(inputVector.x * radius.x, inputVector.y * radius.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(ResetValues());
    }

    IEnumerator ResetValues()
    {
        yield return new WaitForSeconds(0.01f);
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
