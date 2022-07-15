using UnityEngine;
using UnityEngine.UI;

public class TurnPages : MonoBehaviour
{
    public Button leftBtn;
    public Button rightBtn;
    public GameObject lastPage;
    public GameObject nextPage;

    private void OnEnable()
    {
        leftBtn.onClick.AddListener(OnLastPage);
        rightBtn.onClick.AddListener(OnNextPage);
    }

    private void OnDisable()
    {
        leftBtn.onClick.RemoveListener(OnLastPage);
        rightBtn.onClick.RemoveListener(OnNextPage);
    }

    private void OnLastPage()
    {
        gameObject.SetActive(false);
        lastPage.SetActive(true);
    }
    private void OnNextPage()
    {
        gameObject.SetActive(false);
        nextPage.SetActive(true);
    }
}
