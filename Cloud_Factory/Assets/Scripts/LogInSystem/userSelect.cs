using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class userSelect : LogInSystemManager
{
    string URL = "http://localhost/mydb/userSelect.php";
    public string[] usersData;

    IEnumerator Start()
    {
        WWW users = new WWW(URL);
        yield return users;

        string usersDataString = users.text;
        usersData = usersDataString.Split(';');

#region USER INFO DEBUG
        Debug.Log(users.text);
        Debug.Log(usersData.Length);
        for (int i = 0; i < usersData.Length - 1; i++)
        {
            Debug.Log(GetValueData(usersData[i], "username:"));
            Debug.Log(GetValueData(usersData[i], "nickname:"));
            Debug.Log(GetValueData(usersData[i], "password:"));
            Debug.Log("------------------------------------");
        }
#endregion
    }

    // 서버에서 자동 업데이트된 기록을 유니티에서도 업데이트하게함
    public IEnumerator UsersDataUpdate()
    {
        WWW users = new WWW(URL);
        yield return users;

        string usersDataString = users.text;
        usersData = usersDataString.Split(';');

#region USER INFO UPDATE DEBUG
        Debug.Log("UPDATE!!");
        Debug.Log(users.text);
        Debug.Log(usersData.Length);
        for (int i = 0; i < usersData.Length - 1; i++)
        {
            Debug.Log(GetValueData(usersData[i], "username:"));
            Debug.Log(GetValueData(usersData[i], "nickname:"));
            Debug.Log(GetValueData(usersData[i], "password:"));
            Debug.Log("------------------------------------");
        }
#endregion
    }

    // 회원가입된 이름,이메일,비밀번호를 불러온다.
    string GetValueData(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));
        }
        return value;
    }

    // userData가 있는지! 만약에 있다면 이 함수를 활용해서 데이터를 가지고 올 수 있음
    // 있다면 회원 index값을 리턴함, 없다면 -1값을 리턴
    // inputUsername : 로그인에 사용한 유저 아이디
    public int FindUserData(string inputUsername)
    {
        for (int i = 0; i < usersData.Length - 1; i++)
        {
            // 입력한 아이디가 같은 정보를 찾음
            if (inputUsername == GetValueData(usersData[i], "username:"))
            {
                Debug.Log(GetValueData(usersData[i], "username:"));
                Debug.Log(GetValueData(usersData[i], "nickname:"));
                Debug.Log(GetValueData(usersData[i], "password:"));

                return i;
            }
        }
        return -1;
    }

    // USER NICKNAME 가져옴
    public string GetUserNickname(int index)
    {
        return GetValueData(usersData[index], "nickname:");
    }

    // USER PASSWORD 가져옴
    public string GetUserPassword(int index)
    {
        return GetValueData(usersData[index], "password:");
    }


}
