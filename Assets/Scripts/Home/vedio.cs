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
//挂载后会自动把VideoPlayer组件添加到game object

public class Vedio : MonoBehaviour
{
    private VideoPlayer videoPlayer;//声明视频播放器组件
    public VideoClip 视频源;//声明视频资源
    private RawImage rawImage;//声明原始图像
    /*===注意：使用RawImage时，将使用每个存在的RawImage创建一个额外的绘制调用，因此最好只将其用于背景或临时可见的图形===*/

    //私有变量暴露到编辑器
    [SerializeField]
    [Range(0f, 1f)] public float 淡入速度 = 1.5f;//播放速度

    private void Awake()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();//获取视频播放器组件
        rawImage = this.GetComponent<RawImage>();//获取原始图像组件
    }

    void Start()
    {
        videoPlayer.isLooping = true;//循环播放,如果只播放一次设置为false
        videoPlayer.clip = 视频源;//视频源
    }

    void Update()
    {
        //只播放一次则取消注释
        /*
		if (videoPlayer.texture == null)
        {
			如果videoPlayer没有对应的视频texture,即视频播放结束,则返回
            return;
        }
		*/

        //把VideoPlayerd的视频渲染到UGUI的RawImage
        rawImage.texture = videoPlayer.texture;//RawImage可以显示任何纹理，而Image组件只能显示Sprite纹理||videoPlayer.texture:视频内容的内部纹理（只读）
        VideoFade();
    }

    //一个淡入的效果，感觉并没有什么卵用
    public void VideoFade()
    {
        videoPlayer.Play();//开始播放
        rawImage.color = Color.Lerp(rawImage.color, Color.white, 淡入速度 * Time.deltaTime);
        /*
         * Color: RGBA颜色表示形式，用于传递颜色。 每个颜色分量都是0到1范围内的浮点值。分量(r,g,b)定义RGB颜色空间中的颜色。Alpha分量(a)定义透明度 ,alpha为1表示完全不透明，alpha为0表示完全透明
         * Lerp: 在参数1颜色与参数2颜色之间按参数3进行线性插值, 参数3限制在0与1之间。当参数3为0时返回参数3，当参数3为1时返回 /参数3/
         * Color.white：纯白色。RGBA 为 (1, 1, 1, 1)
         * Time.deltaTime:  从最后一个帧到当前帧的间隔，以秒为单位(只读)。OnGUI不可靠，因为每个帧可能会多次调用它
         */
    }

}