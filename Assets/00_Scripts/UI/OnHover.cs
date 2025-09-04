using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UnityEvent OnEnter;
    [SerializeField] UnityEvent OnExit;
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit?.Invoke();
    }
}
