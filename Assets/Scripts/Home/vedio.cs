using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
/*
 * 
 * 
 */


[RequireComponent(typeof(VideoPlayer))]
//���غ���Զ���VideoPlayer�����ӵ�game object

public class Vedio : MonoBehaviour
{
    private VideoPlayer videoPlayer;//������Ƶ���������
    public VideoClip ��ƵԴ;//������Ƶ��Դ
    private RawImage rawImage;//����ԭʼͼ��
    /*===ע�⣺ʹ��RawImageʱ����ʹ��ÿ�����ڵ�RawImage����һ������Ļ��Ƶ��ã�������ֻ�������ڱ�������ʱ�ɼ���ͼ��===*/

    //˽�б�����¶���༭��
    [SerializeField]
    [Range(0f, 1f)] public float �����ٶ� = 1.5f;//�����ٶ�

    private void Awake()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();//��ȡ��Ƶ���������
        rawImage = this.GetComponent<RawImage>();//��ȡԭʼͼ�����
    }

    void Start()
    {
        videoPlayer.isLooping = true;//ѭ������,���ֻ����һ������Ϊfalse
        videoPlayer.clip = ��ƵԴ;//��ƵԴ
    }

    void Update()
    {
        //ֻ����һ����ȡ��ע��
        /*
		if (videoPlayer.texture == null)
        {
			���videoPlayerû�ж�Ӧ����Ƶtexture,����Ƶ���Ž���,�򷵻�
            return;
        }
		*/

        //��VideoPlayerd����Ƶ��Ⱦ��UGUI��RawImage
        rawImage.texture = videoPlayer.texture;//RawImage������ʾ�κ�������Image���ֻ����ʾSprite����||videoPlayer.texture:��Ƶ���ݵ��ڲ�����ֻ����
        VideoFade();
    }

    //һ�������Ч�����о���û��ʲô����
    public void VideoFade()
    {
        videoPlayer.Play();//��ʼ����
        rawImage.color = Color.Lerp(rawImage.color, Color.white, �����ٶ� * Time.deltaTime);
        /*
         * Color: RGBA��ɫ��ʾ��ʽ�����ڴ�����ɫ�� ÿ����ɫ��������0��1��Χ�ڵĸ���ֵ������(r,g,b)����RGB��ɫ�ռ��е���ɫ��Alpha����(a)����͸���� ,alphaΪ1��ʾ��ȫ��͸����alphaΪ0��ʾ��ȫ͸��
         * Lerp: �ڲ���1��ɫ�����2��ɫ֮�䰴����3�������Բ�ֵ, ����3������0��1֮�䡣������3Ϊ0ʱ���ز���3��������3Ϊ1ʱ���� /����3/
         * Color.white������ɫ��RGBA Ϊ (1, 1, 1, 1)
         * Time.deltaTime:  �����һ��֡����ǰ֡�ļ��������Ϊ��λ(ֻ��)��OnGUI���ɿ�����Ϊÿ��֡���ܻ��ε�����
         */
    }

}