using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 유니티에서 제공하는 jsonUtility
 */

public class JsonUtilityExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // jsonutility의 단점은 기본적인 데이터 타입과 배열,
        // 리스트에 대한 시리얼라이즈만 지원한다
        // 딕셔너리와 직접 생성한 클래스가 보이지 않음
        // 직접 생성한 클래스의 경우에는 [System.Serializable] 어트리뷰트를
        // 붙여줘야만 Json데이터로 변환되며, 딕셔너리는 아예 지원하지 않음.
        JsonTestClass jTest1 = new JsonTestClass();
        string jsonData = JsonUtility.ToJson(jTest1);
        Debug.Log(jsonData);

        JsonTestClass jTest2 = JsonUtility.FromJson<JsonTestClass>(jsonData);
        jTest2.Print();

        // jsonUtility를 사용하면 불필요한 값을 제외하고 필요한 좌표만 저장된다
        JsonVector jVector = new JsonVector();
        string jsonData2 = JsonUtility.ToJson(jVector);
        Debug.Log(jsonData2);

        // 또한 모노비헤이비어를 상속받는 클래스의 오브젝트도 시리얼라이즈 가능
        GameObject obj = new GameObject();
        var test1 = obj.AddComponent<TestMono>();
        test1.i = 100;
        test1.v3 /= 10;
        // getcomponent등으로 직접가지고온 클래스의 이름으로 시리얼라이즈해야함
        string jsonData3 = JsonUtility.ToJson(obj.GetComponent<TestMono>());
        Debug.Log(jsonData3);

        // 오브젝트로 다시 변환
        // JsonUtility.FromJson<TestMono>(jsonData3);
        // 새로운 인스턴스를 생성할 수 없다는 에러발생 후 디시리얼라이즈 실패
        // 해결방안 ->
        GameObject obj2 = new GameObject();
        JsonUtility.FromJsonOverwrite(jsonData3, obj2.AddComponent<TestMono>());

        // 한 마디로 모노비헤이비어를 상속받는 오브젝트의 Json 데이터를
        // 다시 오브젝트로 만드려면 같은 형태를 갖는 오브젝트를 미리 생성한 뒤에
        // 디시리얼라이즈를 해야한다는 뜻

        // 이 기능을 사용하여 게임내의 데이터를 json데이터로 저장해서
        // 게임 진행 상황을 저장하거나, 다시 불러와서 원래대로 세팅하는
        // 세이브 로드 기능 구현할수있음
    }
}
