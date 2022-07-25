using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/*
 * 외부 라이브러리인 Newtonsoft의 json 라이브러리
 */

public class NewtonsoftJsonExample : MonoBehaviour
{
    void Start()
    {
        // JsonTestClass의 오브젝트를 생성해서 Json 데이터로 만들기        
        JsonTestClass jTest1 = new JsonTestClass();
        // 이 과정을 직렬화라고 부른다
        string jsonData = JsonConvert.SerializeObject(jTest1);
        Debug.Log(jsonData);

        // json 데이터를 다시 오브젝트로 바꿀 때는 json데이터를 어떤 오브젝트로
        // 변환하는지 명시적으로 함수에 알려줘야함
        // 만약, json데이터가 가진 구조가 함수에 알려준 클래스의 구조와 다르다면
        // 변환 도중에 에러가 발생하기 때문에 잘 확인하고 진행해야함.
        JsonTestClass jTest2 = JsonConvert.DeserializeObject<JsonTestClass>(jsonData);
        jTest2.Print();

        // 유니티에서 json을 사용할 때 주의할 점!!
        // 1. 자기 참조 문제
        // 이 코드의 의도는 클래스에 들어있는 int 타입의 i를 json 데이터로
        // 만들어서 저장하려는 것
        // 이 코드 그대로 사용하면 jsonSerialiszationException 오류 발생 (자기 참조 루프)
        // GameObject obj = new GameObject();
        // obj.AddComponent<TestMono>();
        // // Gameobject.gameobject...
        // Debug.Log(JsonConvert.SerializeObject(obj.GetComponent<TestMono>()));
        // Newtonsoft의 json 라이브러리로는 모노비헤이비어를 상속받는 클래스의 오브젝트를
        // json 데이터로 시리얼라이즈할 수는 없다.
        // 그렇기 때문에 모노비헤이비어를 상속받는 클래스의 오브젝트를 시리얼라이즈하는 대신에
        // 스크립트가 가지고 있는 프로퍼티 중에서 필요한 프로퍼티를 클래스로 묶어서
        // 해당 클래스만 시리얼라이즈 하거나
        // 유니티가 기본 제공하는 jsonUtility 기능 사용해서 시리얼라이즈 해야 함.

        // 2. Vector3를 시리얼라이즈하는 것, 이것 또한 자기 참조        
        JsonVector jVector = new JsonVector();
        // Debug.Log(JsonConvert.SerializeObject(jVector));
        // jVector.vector3.normalized.normalized.normalized ...
        // 해결방안-> 핸들링 무시
        JsonSerializerSettings setting = new JsonSerializerSettings();
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        Debug.Log(JsonConvert.SerializeObject(jVector, setting));
        // 하지만 이렇게 무시하게 해도 normalized 벡터나 벡터의 길이 등의 불필요한 값들이
        // 시리얼라이즈 되기 때문에 쓸데없이 json데이터의 길이나 용량이 늘어나는 문제가 생김.
        // 외부 라이브러리를 이용해서 Vector3에서 좌표 값만을 json데이터로 시리얼라이즈 하기 원하면
        /*
         * public class SerializableVector3
         * {
         *  public float x;
         *  public float y;
         *  public float z;
         * }
         */
        // 이런식으로 시리얼라이즈 용 벡터 클래스를 만들어서 시리얼라이즈 해야한다.
    }
}

public class JsonVector
{
    public Vector3 vector3 = new Vector3(3, 3, 3);
}

public class JsonTestClass
{
    // 사용가능한 멤버변수
    public int i;
    public float f;
    public bool b;
    public string str;
    public int[] iArray;
    public List<int> iList = new List<int>();
    public Dictionary<string, float> fDictionary = new Dictionary<string, float>();
    public IntVector2 iVector;

    // 생성자에서 멤버변수 초기화
    public JsonTestClass()
    {
        i = 10;
        f = 99.9f;
        b = true;
        str = "JSON TEST String";
        iArray = new int[] { 1, 1, 2, 3, 4, 8, 12, 21, 34, 55 };

        for (int idx = 0; idx < 5; idx++)
        {
            iList.Add(2 * idx);
        }

        fDictionary.Add("PIE", Mathf.PI);
        fDictionary.Add("Epsilon", Mathf.Epsilon);
        fDictionary.Add("Sprt(2)", Mathf.Sqrt(2));

        iVector = new IntVector2(3, 2);
    }

    // 프린트해서 하나씩 출력
    public void Print()
    {
        Debug.Log("i : " + i);
        Debug.Log("f : " + f);
        Debug.Log("b : " + b);
        Debug.Log("str : " + str);

        for (int idx = 0; idx < iArray.Length; idx++)
        {
            Debug.Log(string.Format("iArray[{0}] = {1}", idx, iArray[idx]));
        }

        for (int idx = 0; idx < iList.Count; idx++)
        {
            Debug.Log(string.Format("iList[{0}] = {1}", idx, iList[idx]));
        }

        foreach (var data in fDictionary)
        {
            Debug.Log(string.Format("iDictionary[{0}] = {1}", data.Key, data.Value));
        }

        Debug.Log("iVector = " + iVector.x + ", " + iVector.y);
    }

    [System.Serializable]
    public class IntVector2
    {
        public int x;
        public int y;
        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}