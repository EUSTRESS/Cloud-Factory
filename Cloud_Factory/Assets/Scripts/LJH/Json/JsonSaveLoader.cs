using System.Collections;
using System.Collections.Generic;
// 파일 입출력과 문자열 처리하기 위해서
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class JsonSaveLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /* SAVE     */

        // 파일을 저장할 경로와 파일 이름으로 FileStream을 만듬
        FileStream stream = new FileStream(Application.dataPath + "/Data/test.json", FileMode.OpenOrCreate);
        JsonTestClass jTest1 = new JsonTestClass();
        string jsonData = JsonConvert.SerializeObject(jTest1);
        // 문자열인 json데이터를 Encoding.UTF8의 GetBytes 함수로 byte배열로 만들어줌
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        // 그 데이터를 파일스트림에 써준다
        stream.Write(data, 0, data.Length);
        stream.Close();

       

        /* LOAD */
        //FileStream stream = new FileStream(Application.dataPath + "/Data/test.json", FileMode.Open);
        //byte[] data = new byte[stream.Length];
        //stream.Read(data, 0, data.Length);
        //stream.Close();
        //string jsonData = Encoding.UTF8.GetString(data);
        //JsonTestClass jTest2 = JsonConvert.DeserializeObject<JsonTestClass>(jsonData);
        //jTest2.Print();

    }
}

/*
원래는 서버에 테이블 활용하지만,
작업이 빠르게 진행되어야 하는 경우에 그냥 json 데이터로 던지고 
클라이언트에서 json 데이터의 구조만 보고
직접 클래스를 짜야하는 경우도 발생한다.
 
물론 받은 json데이터를 일일이 분석해서 클래스를 짜도 되겠지만 훨씬
간단하고 원터치로 json데이터를 클래스로 만드는 방법이 있습니다.

구글에서 json 2 C sharp하면 json 데이터를 c#클래스로 변환해주는 사이트들이 나옴

적당한 사이트 골라서 json데이터를 입력한뒤 convert버튼하면 구조
알아서 분석해서 자동으로 C# 클래스로 만들어줌
 */
