using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userLogin : LogInSystemManager
{
	public InputField inputUserName, inputPassword;
	public Text loginGuideText;

	private userSelect userSelect_findData;

	string LoginURL = "http://localhost/mydb/login.php";

    void Awake()
    {
		userSelect_findData = GetComponent<userSelect>();

	}

    public void LoginBtn()
    {
		StartCoroutine(LoginToDB(inputUserName.text, inputPassword.text));
	}

	IEnumerator LoginToDB(string username, string password)
	{
		WWWForm form = new WWWForm();
		form.AddField("usernamePost", username);
		form.AddField("passwordPost", password);

		WWW www = new WWW(LoginURL, form);

		yield return www;

		// 아이디가 등록되어 있다면
		if (userSelect_findData.FindUserData(username) != -1)
        {
			// 비밀번호까지 맞다면 로그인 성공!
			if (password == userSelect_findData.GetUserPassword(userSelect_findData.FindUserData(username)))
            {
				loginGuideText.text =
				userSelect_findData.GetUserNickname(userSelect_findData.FindUserData(username))
				+ "님 안녕하세요!";
			}
			else
            {
				loginGuideText.text = "비밀번호가 틀렸습니다.";
			}
		}
        else // 아니라면
        {
			loginGuideText.text = "등록된 회원이 아닙니다.";
        }

		Invoke("initText", 5.0f);
	}

	void initText()
    {
		loginGuideText.text = "###Cloud Factory###";
		// 중복선언 방지
		CancelInvoke("initText");
	}
}
