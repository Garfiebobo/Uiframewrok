using UnityEngine;
using System.Collections;

public class MainMenuPanel : BasePanel {

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPushPanel(string panelTypeString)
    {
        UIPanelType panelType = (UIPanelType) System.Enum.Parse(typeof(UIPanelType), panelTypeString);
        UIManager.Instance.PushPanel(panelType);
    }

}
