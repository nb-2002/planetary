using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ToggleIcon: MonoBehaviour
{
    public Sprite icon1;
    public Sprite icon2;
    private Button button;
    private bool toggle = false;
    private GameObject child;

    void Start()
    {
        if(0 == transform.childCount) child = null;
        else child = transform.GetChild(0).gameObject;
        if (child != null) child.SetActive(false);
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            SwitchIcon();
        });
    }

    void Update()
    {

    }

    public void SwitchIcon()
    { 
        if (toggle)
        {
            button.image.sprite = icon1;
        }
        else
        {
            button.image.sprite = icon2;
        }
        toggle = !toggle;
        if(child != null)
        {
            child.SetActive(toggle);
        }
    }
}
