using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Private Variables
    [Tooltip("���� �������ͽ� �����ִ� �г�")]
    [SerializeField]
    private GameObject StatusPanel;



    [Tooltip("���ۿ� �г�")]
    [SerializeField]
    private GameObject ControlPanel;

    [Tooltip("����ȭ�� �г�")]
    [SerializeField]
    private GameObject SimplePanel;


    [Tooltip("������Ʈ�� �α��� ȭ���� �ӽ÷� ������ �г�")]
    [SerializeField]
    private GameObject WebsitePanel;

    [Tooltip("��ü ĵ����")]
    [SerializeField]
    private GameObject Canvas;

    #endregion

    #region Public Methods

    public void ShowCanvas()
    {
        Canvas.SetActive(true);
    }
    public void ShowControlPanel()
    {
        ControlPanel.SetActive(true);
    }

    public void ShowSimplePanel()
    {
        SimplePanel.SetActive(true);
    }
    public void HideSimplePanel()
    {
        SimplePanel.SetActive(false);
        WebsitePanel.SetActive(false);
    }
    public void StartGame()
    {
        ControlPanel.SetActive(false);
    }


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
