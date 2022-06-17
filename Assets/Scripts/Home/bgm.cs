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

        HomeBGM.loop = true;//��������Ϊѭ������ ;
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
            //�ص�����
            print("�ص�����");
            HomeBGM.Stop();
            //���Ұ�ͼƬ������һ��
            MusicBtn.image.sprite = Resources.Load<Sprite>("Home/mute");
            //���������ر�
            isSoundOn = false;

        }
        else
        {
            Button MusicBtn = SoundButton.GetComponent<Button>();
            //������
            print("������");
            HomeBGM.Play();//��������
            //���Ұ�ͼƬ������һ��
            MusicBtn.image.sprite = Resources.Load<Sprite>("Home/bgm");
            //���������Ĵ�
            isSoundOn = true;
        }

    }
}
