using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;


public class SpriteMerger : MonoBehaviour
{
    [SerializeField]
    private List<Transform> mRawList; //스프라이트 분리 안된, 게임오브젝트 리스트

    [SerializeField]
    private List<Sprite> mspriteToMerge;

    public Image finalSpriteRenderer = null;
    // Start is called before the first frame update
    void Start()
    {
        mRawList = new List<Transform>();
        finalSpriteRenderer = transform.GetComponent<Image>();
    }

    private void initMergeList(GameObject _Fincloud)
    {
        //List 초기화
        mRawList.Add(_Fincloud.transform);

        for(int i = 0; i <_Fincloud.transform.childCount; i++)
        {
            mRawList.Add(_Fincloud.transform.GetChild(i));
        }

        for(int i = 0; i < mRawList.Count; i++)
        {
            mspriteToMerge.Add(mRawList[i].GetComponent<Image>().sprite);
        }

    }

    public Texture2D Merge(GameObject _Fincloud)
    {
        initMergeList( _Fincloud);

        Resources.UnloadUnusedAssets();
        var newText = new Texture2D( (int)mRawList[0].GetComponent<RectTransform>().rect.width+150, (int)mRawList[0].GetComponent<RectTransform>().rect.height);

        for(int x = 0; x < newText.width; x++)
        {
            for(int y = 0; y< newText.height; y++)
            {
                newText.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }

        for(int i = 0; i < mspriteToMerge.Count; i++)
        {
            Vector2 startPoint = new Vector2(0, 0);
            if (i > 0)
            {
                int tmp_x = (int)mRawList[0].GetComponent<RectTransform>().rect.width/2;
                int tmp_y = (int)mRawList[0].GetComponent<RectTransform>().rect.height/2;
                startPoint = new Vector2(tmp_x + (int)mRawList[i].transform.localPosition.x
                    , tmp_y + (int)mRawList[i].transform.localPosition.y);
            }

            Debug.Log("보정된 벡터2 값"+ startPoint.x +"     " + startPoint.y);


            for (int x = 0; x <mspriteToMerge[i].texture.width; x++)
            {
                for(int y = 0; y < mspriteToMerge[i].texture.height; y++)
                {
                    var color = mspriteToMerge[i].texture.GetPixel(x, y).a == 0 ?
                        newText.GetPixel((int)startPoint.x+x, (int)startPoint.y + y) :
                        mspriteToMerge[i].texture.GetPixel(x, y);

                    newText.SetPixel((int)startPoint.x + x, (int)startPoint.y + y, color);
                }
            }
        }

        newText.Apply();
        var finalSprite = Sprite.Create(newText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));
        finalSprite.name = "New Sprite";
        finalSpriteRenderer.sprite = finalSprite;

        return newText;
    }

    private string overLapFileNameCheck(string filename,int num)
    {
        if (false == Directory.Exists("Assets/Resources/DecoCloudResult/" + num.ToString() + ".png")) return filename;

        return overLapFileNameCheck(filename, ++num);
    }
    public Texture2D SaveTextureToPNGFile(Texture texture, CloudData cloudDt)
    {
        string directoryPath = "Assets/Resources/DecoCloudResult/";
        string fileName = cloudDt.mEmotions[0].Key.ToString() + cloudDt.mEmotions[1].Key.ToString();

        if (true == string.IsNullOrEmpty(directoryPath))
        {
            return null;
        }

        if (false == Directory.Exists(directoryPath))
        {
            return null;
        }

        if (Directory.Exists(directoryPath + fileName + "0".ToString() + ".png"))
            fileName = overLapFileNameCheck(fileName, 0);

        fileName += "0";

        int width = texture.width;
        int height = texture.height;

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture copiendRenderTexture = new RenderTexture(width, height, 0);

        Graphics.Blit(texture, copiendRenderTexture);

        RenderTexture.active = copiendRenderTexture;

        Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRenderTexture;

        byte[] texturePNGBytes = texture2D.EncodeToPNG();

        string filePath = directoryPath + fileName + ".png";

        File.WriteAllBytes(filePath, texturePNGBytes);

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(texturePNGBytes);
        tex.alphaIsTransparency = true;
        return tex;
    }


}
