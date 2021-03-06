﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Cursor : MonoBehaviour
{
    //log 출력용 나중에 지울것
    public Text TOUCHPOINT;
    public Text TOUCHCOUNT;
    public Text ScreenSize;
    public Text Error;
    public Text time;
    public Text World;
    
    //이미지 설정
    public Image pad_img;

    //네트워크 통신용 
    DateTime frame;
    NetWorkManager nm;

    //화면 처리 변수
    Camera c;

    //UI 처리 변수
    //public Button stat;//stat 탭 버튼
    GameObject cur;//본 오브젝트
    Vector3 startpos;//커서 시작점
    Vector3 startpos_screen;//시작점 스크린 좌표
    Vector3 now_pos;//커서 현재점
    
    Vector3 v;
    Touch now;//현재 터치 
    int pad_index = 10;//디폴트 터치 카운트
                       // Use this for initialization
    void Start()
    {
        //카메라 및 패드 객체 변수 초기화
        c = Camera.main;
        cur = GameObject.Find("Cursor");

        //이미지 변수 초기화
        

        //좌표 변수들
        startpos = cur.transform.position;
        startpos_screen=c.WorldToScreenPoint(startpos);
        now_pos = cur.transform.position;
        v = new Vector3();//패드 계산용

        //네트워크 통신용 변수 초기화
        
        nm = GameObject.Find("NetWorkManager").GetComponent<NetWorkManager>();//네트워크 매니저 오브젝트에 접근 클래스 불러오기
        
        frame = DateTime.Now;

        string img_path = "Img/ControllUI/pad_"+nm.My_Info.color;
        //Debug.Log("Pad Color path : "+img_path);
        pad_img.sprite = Resources.Load<Sprite>(img_path) as Sprite;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        TOUCHCOUNT.text = " " + Input.touchCount;
        //cur.transform.position.Set()
        if (Input.touchCount > 0)
        {
            int count = Input.touchCount;

            now = Input.touches;

            //터치 부터 처리
            for (int i = 0; i < count; ++i)
            {
                now = Input.GetTouch(i);
                
                TOUCHPOINT.text = "X : " + now.position.x + " Y : " + now.position.y;
                ScreenSize.text = Screen.width + " X " + Screen.height;
                
                if (now.position.x < pad_boundx && now.position.y < pad_boundy)
                {
                    if ((now.phase == TouchPhase.Ended || now.phase == TouchPhase.Canceled))
                    {
                        //패드를 터치했던 손을 땠을 때
                        PadUp();
                        break;
                    }
                    else
                    {
                        //float x, y, z;
                        Error.text = "Pad Touch";
                        PadTouch(now.position.x, now.position.y, startpos.z);
                    }
                }
                else {
                    Error.text = "Not Pad Touch";
                }
            }
        }

        time.text = frame.ToString();
        
    }

    void PadTouch(float x,float y,float z)
    {//패드를 터치했을때 

        
        v.Set(x,y,z);
        
        cur.transform.position = c.ScreenToWorldPoint(v);
        

        World.text= "X : " + cur.transform.position.x + " Y : " + cur.transform.position.y;

        //패드 터치 정보 송신
        if (DateTime.Now.Millisecond - frame.Millisecond >= 33 || DateTime.Now.Millisecond - frame.Millisecond < -33)
        {

            frame = DateTime.Now;
            CS_MOVE_PACKET mov;
            mov.id = nm.My_Info.id;

            
            mov.x = (x - startpos_screen.x)/(Screen.width / 4);
            mov.y = (y - startpos_screen.y)/(Screen.height / 3);


            nm.GameDataSend(mov, NetworkController.CS_MOVE);
        }
    }

    void PadUp()
    {
        cur.transform.position = startpos;

        frame = DateTime.Now;
        CS_MOVE_PACKET mov;
        mov.id = nm.My_Info.id;


        mov.x = 0;
        mov.y = 0;


        nm.GameDataSend(mov, NetworkController.CS_MOVE);

    }

    public void Btn0Touch()
    {//1번 버튼을 터치했을때 
        if (DateTime.Now.Millisecond - frame.Millisecond >= 33 || DateTime.Now.Millisecond - frame.Millisecond < -33) {
            frame = DateTime.Now;
            CS_BUTTON_PACKET bt;
            bt.id = nm.My_Info.id;
            bt.btn_number = (char)0;

            nm.GameDataSend(bt, NetworkController.CS_BTN);
        }

        return;
    }
    public void Btn1Touch()
    {//2번 버튼을 터치했을때 
        //아직 미구현
        if (DateTime.Now.Millisecond - frame.Millisecond >= 33 || DateTime.Now.Millisecond - frame.Millisecond < -33)
        {
            frame = DateTime.Now;
            CS_BUTTON_PACKET bt;
            bt.id = nm.My_Info.id;
            bt.btn_number = (char)1;

            nm.GameDataSend(bt, NetworkController.CS_BTN);
        }
        return;
    }
    public void Btn2Touch()
    {//3번 버튼을 터치했을때 
        //아직 미구현
        if (DateTime.Now.Millisecond - frame.Millisecond >= 33 || DateTime.Now.Millisecond - frame.Millisecond < -33)
        {
            frame = DateTime.Now;
            CS_BUTTON_PACKET bt;
            bt.id = nm.My_Info.id;
            bt.btn_number = (char)2;

            nm.GameDataSend(bt, NetworkController.CS_BTN);
        }
        return;
    }
    public void Btn3Touch()
    {//4번 버튼을 터치했을때 
        //아직 미구현
        if (DateTime.Now.Millisecond - frame.Millisecond >= 33 || DateTime.Now.Millisecond - frame.Millisecond < -33)
        {
            frame = DateTime.Now;
            CS_BUTTON_PACKET bt;
            bt.id = nm.My_Info.id;
            bt.btn_number = (char)3;

            nm.GameDataSend(bt, NetworkController.CS_BTN);
        }
        return;
    }

    public void BtnStat() {
        SceneManager.LoadScene("Status");
    }

    public void BtnUpgrade() {
        SceneManager.LoadScene("Upgrade");
    }
}
