using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bgm : MonoBehaviour
{
    public AudioSource HomeBGM;
  
    public GameObject SoundButton;
    public bool isSoundOn = true;
    // Start is called before the first frame update
    void Start()
    {

        HomeBGM.loop = true;//设置声音为循环播放 ;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SoundButtonPress()
    {

        if (isSoundOn)
        {
            Button MusicBtn = SoundButton.GetComponent<Button>();
            //关掉声音
            print("关掉声音");
            HomeBGM.Stop();
            //并且把图片换成另一张
            MusicBtn.image.sprite = Resources.Load<Sprite>("Home/mute");
            //设置声音关闭
            isSoundOn = false;

        }
        else
        {
            Button MusicBtn = SoundButton.GetComponent<Button>();
            //打开声音
            print("打开声音");
            HomeBGM.Play();//声音播放
            //并且把图片换成另一张
            MusicBtn.image.sprite = Resources.Load<Sprite>("Home/bgm");
            //设置声音的打开
            isSoundOn = true;
        }

    }
}
