using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIManager {

   

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    private Dictionary<UIPanelType, string> panelPathDict;
    private Dictionary<UIPanelType, BasePanel> panelDict;
    private Stack<BasePanel> panelStack;

    private UIManager()
    {
        ParseUIPanelTypeJson();
    }

    /// <summary>
    ///Pushpanel
    /// </summary>
    public void PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

      
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
    }
    /// <summary>
    /// Pop  panel
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

      
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        if (panelStack.Count <= 0) return;
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();

    }

    /// <summary>
    /// get panel by paneltype
    /// </summary>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        //BasePanel panel;
        //panelDict.TryGetValue(panelType, out panel);//TODO

        BasePanel panel = panelDict.TryGet(panelType);

        if (panel == null)
        {
          
            //string path;
            //panelPathDict.TryGetValue(panelType, out path);
            string path = panelPathDict.TryGet(panelType);
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            instPanel.transform.SetParent(CanvasTransform,false);
            panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
            return instPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }

    }

    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }
    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<UIPanelType, string>();

        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");

        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

        foreach (UIPanelInfo info in jsonObject.infoList) 
        {
            //Debug.Log(info.panelType);
            panelPathDict.Add(info.panelType, info.path);
        }
    }

    /// <summary>
    /// just for test
    /// </summary>
    public void Test()
    {
        string path ;
        panelPathDict.TryGetValue(UIPanelType.Knapsack,out path);
        Debug.Log(path);
    }
}
