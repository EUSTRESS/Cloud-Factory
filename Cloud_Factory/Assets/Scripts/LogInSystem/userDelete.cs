using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userDelete : LogInSystemManager
{
    string URL = "http://localhost/mydb/userDelete.php";

    // 회원 삭제 버튼
    public void DeleteBtn()
    {
        DeleteUser(WhereField_Delete, WhereCondition_Delete);
    }

    // 회원 삭제
    public void DeleteUser(string wF, string wC)
    {
        WWWForm form = new WWWForm();

        // AddField
        form.AddField("whereField", wF); // username, email, password 중에 하나를 적는다.
        form.AddField("whereCondition", wC); // 적은 Condition에서 할당된 값을 적는다.

        WWW www = new WWW(URL, form);
    }
}
