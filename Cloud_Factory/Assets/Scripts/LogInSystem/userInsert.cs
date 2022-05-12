using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userInsert : LogInSystemManager
{
    string URL = "http://localhost/mydb/userInsert.php";

    public InputField InputID, InputPW, InputNickName;
    public Button signUpButtonUI;

    private userSelect userSelect_findData;

    private bool signUpPositive;

    void Awake()
    {
        userSelect_findData = GetComponent<userSelect>();

    }
    public void SignUpCheckBtn() // 중복체크 텍스트 할당, 중복확인 버튼
    { 
        InputUsername_SignUp = InputID.text;

        // 이 자식들 자동으로 할당하게 못하나??
        InputPassword_SignUp = InputPW.text;
        InputNickName_SignUp = InputNickName.text;

        // 이미 있는 아이디가 아니라면 추가 가능!
        if (userSelect_findData.FindUserData(InputUsername_SignUp) == -1)
        {
            signUpPositive = true;

            // 가입하기 버튼 사용 가능 처리
            signUpButtonUI.interactable = true;
        }
        else
        {
            signUpPositive = false;
            // 안내 UI 나오게하기
            Debug.Log("중복된 아이디가 있음");

            // 가입하기 버튼 사용 불가 처리
            signUpButtonUI.interactable = false;
        }
            
    }

    public void SignUpAllocationBtn() // 아이디 서버에 추가, 가입하기 버튼
    {
        AddUser(InputUsername_SignUp, InputPassword_SignUp, InputNickName_SignUp);
    }

    public void AddUser(string username, string password, string nickname)
    {
        WWWForm form = new WWWForm();

        // AddField
        // 테이블에 추가할 이름, 이메일, 비밀번호를 적는다

        // 이미 있는 아이디가 아니라면 추가 가능!
        if (signUpPositive)
        {
            form.AddField("addUsername", username);
            form.AddField("addNickname", nickname);
            form.AddField("addPassword", password);

            WWW www = new WWW(URL, form);

            // 회원가입한 유저를 서버에 업데이트(자동)하고 유니티에도 업데이트(수동)하게함
            userSelect_findData.StartCoroutine(userSelect_findData.UsersDataUpdate());
        }

    }
}
