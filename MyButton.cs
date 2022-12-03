using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

public class MyButton : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{
    public UnityEvent OnClick;
    public UnityEvent OnExit;
    Image i;
    Color originalColor;
    private void Start()
    {
        i = GetComponent<Image>();
        originalColor = i.color;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        i.color = originalColor;
        OnExit.Invoke();
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        i.color = Color.black;
        OnClick.Invoke();
    }
    public bool IsPressed()
    {
        return (i.color == Color.black);
    }
}
