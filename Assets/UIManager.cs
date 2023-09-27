using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Private Variables

    private bool isTestPanelActivated;


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

    [Tooltip("ĳ���� ���ÿ� �г�")]
    [SerializeField]
    private GameObject CharacterSelectPanel;

    [Tooltip("�� ���ÿ� �г�")]
    [SerializeField]
    private GameObject TeamSelectPanel;

    [Tooltip("�κ� ĵ����")]
    [SerializeField]
    private GameObject Canvas;

    [Tooltip("���� ĵ����")]
    [SerializeField]
    private GameObject ForumCanvas;

    [Tooltip("�׽�Ʈ �г�")]
    [SerializeField]
    private GameObject TestPanel;

    [Tooltip("�̹��� �ε��� �Է��ʵ�")]
    [SerializeField]
    private InputField ImageIndex;

    #endregion

    #region Public Variables


    #endregion

    #region Public Methods

    public void ShowLobbyCanvas()
    {
        Canvas.SetActive(true);
    }
    public void HideLobbyCanvas()
    {
        Canvas.SetActive(false);
    }
    public void ShowControlPanel()
    {
        ControlPanel.SetActive(true);
    }

    public void ShowSimplePanel()
    {
        SimplePanel.SetActive(true);
    }

    public void ShowForumCanvas()
    {
        ForumCanvas.SetActive(true);
    }
    public void ClearPanels()
    {
        SimplePanel.SetActive(false);
        WebsitePanel.SetActive(false);
        CharacterSelectPanel.SetActive(false);
        TeamSelectPanel.SetActive(false);
    }
    public void StartGame()
    {
        ControlPanel.SetActive(false);
    }

    public void EmitChangeImage()
    {
        int index = int.Parse(ImageIndex.text);
        Debug.Log(index);
        ImageManager.instance.ChangeTeam(index);
    }


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isTestPanelActivated = !isTestPanelActivated; // ���ü� ���¸� ���
            TestPanel.SetActive(isTestPanelActivated); // �г��� ���ü��� ������ ���·� ����
        }
    }
}
