using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInSystemManager : MonoBehaviour
{
    // 회원가입 UI
    [HideInInspector]
    public GameObject SignUpUI;

    // 회원가입에서 입력할 유저 ID, PW, NickName
    [HideInInspector]
    public string InputUsername_SignUp, InputPassword_SignUp, InputNickName_SignUp;

    // 회원삭제에서 입력할 유저 정보의 조건과 조건의 열
    [HideInInspector]
    public string WhereField_Delete, WhereCondition_Delete;

    // 회원정보 수정에서 입력할 유저 ID, PW, NickName, 조건과 그 조건의 열
    [HideInInspector]
    public string InputUsername_Update, InputPassword_Update, InputNickName_Update, WhereField_Update, WhereCondition_Update;

    // 회원가입 UI를 활성화 시키는 함수 (버튼UI에 할당)
    public void SignUpUIActive()
    {
        SignUpUI.SetActive(true);
    }

    // 회원가입 UI를 비활성화 시키는 함수 (버튼UI에 할당)
    public void SignUpUIUnActive()
    {
        SignUpUI.SetActive(false);
    }

}

