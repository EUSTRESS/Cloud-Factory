using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userUpdate : LogInSystemManager
{
    string URL = "http://localhost/mydb/userUpdate.php";

    public void UpdateBtn()
    {
        UpdateUser(InputUsername_Update, InputPassword_Update, InputNickName_Update
                , WhereField_Update, WhereCondition_Update);
    }

    public void UpdateUser(string username, string password, string nickname, string wF, string wC)
    {
        WWWForm form = new WWWForm();

        // AddField
        // 수정할 이름, 이메일, 비밀번호를 적는다
        form.AddField("editUsername", username); 
        form.AddField("editPassword", password);
        form.AddField("editNickname", nickname);

        form.AddField("whereField", wF); // username, email, password 중에 하나를 적는다.
        form.AddField("whereCondition", wC); // 적은 Condition에서 할당된 값을 적는다.

        WWW www = new WWW(URL, form);

        
    }
}
