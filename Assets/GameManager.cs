using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0; // �����Ϳ��� VSync�� ��Ȱ��ȭ�մϴ�.
        Application.targetFrameRate = 60; // ���ϴ� ������ �ӵ��� �����մϴ�.
#endif
        //NetworkManager.instance.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
