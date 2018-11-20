using Baidu.Aip;
using Baidu.Aip.ImageClassify;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Homework_ImageClassify_App
{
    public class ImageClassifyHelper
    {
        // 设置APPID/AK/SK
        private const string APP_ID = "14817956";
        private const string API_KEY = "HBKWxOKk7wlqvoY6ZdBY9xZE";
        private const string SECRET_KEY = "BOXpiPQGXwVzO4zpDqUt3kxKhuzeBrI2";

        private static ImageClassify _imageClient = new ImageClassify(API_KEY, SECRET_KEY) { Timeout = 6000 };

        //用户向服务请求检测图像中的主体位置。
        public static JObject ObjectDetectDemo(byte[] image, Dictionary<string,object> options)
        {
            try
            {
                // 带参数调用图像主体检测
                return _imageClient.ObjectDetect(image, options);
            }
            catch (AipException exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        //通用物体识别
        //该请求用于通用物体及场景识别，即对于输入的一张图片（可正常解码，且长宽比适宜），输出图片中的多个物体及场景标签。
        public static JObject AdvancedGeneralDemo(byte[] image)
        {
            try
            {
                // 调用通用物体识别，可能会抛出网络等异常，请使用try/catch捕获
                //return _imageClient.AdvancedGeneral(image);
                // 如果有可选参数
                var options = new Dictionary<string, object>{
        {"baike_num", 5}
    };
                // 带参数调用通用物体识别
                return _imageClient.AdvancedGeneral(image, options);
            }
            catch (AipException exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        public static JObject DishDetectDemo(byte[] image)
        {
            try
            {
                // 调用通用物体识别，可能会抛出网络等异常，请使用try/catch捕获
                //return _imageClient.DishDetect(image);
                // 如果有可选参数
                var options = new Dictionary<string, object>{
        {"top_num", 3},
        {"filter_threshold", "0.7"},
        {"baike_num", 5}
    };

                // 带参数调用菜品识别
                return _imageClient.DishDetect(image, options);
            }
            catch (AipException exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public static JObject CarDetectDemo(byte[] image)
        {
            try
            {
                // 调用通用物体识别，可能会抛出网络等异常，请使用try/catch捕获
                //return _imageClient.CarDetect(image);
                // 如果有可选参数
                var options = new Dictionary<string, object>{
        {"top_num", 3},
        {"baike_num", 5}
    };
                // 带参数调用车辆识别

                return _imageClient.CarDetect(image, options);
            }
            catch (AipException exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        public static JObject LogoDetectDemo(byte[] image)
        {
            try
            {
                // 调用通用物体识别，可能会抛出网络等异常，请使用try/catch捕获
                //return _imageClient.LogoSearch(image);
                // 如果有可选参数
                var options = new Dictionary<string, object>{
        {"custom_lib", "true"}
    };
                // 带参数调用logo商标识别
                return _imageClient.LogoSearch(image, options);
            }
            catch (AipException exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public static JObject AnimalDetectDemo(byte[] image)
        {
            try
            {
                // 调用通用物体识别，可能会抛出网络等异常，请使用try/catch捕获
                //return _imageClient.AnimalDetect(image);
                // 如果有可选参数
                var options = new Dictionary<string, object>{
        {"top_num", 3},
        {"baike_num", 5}
    };
                // 带参数调用动物识别
                return _imageClient.AnimalDetect(image, options);
            }
            catch (AipException exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        /// <summary>
        /// 植物识别
        /// </summary>
        /// <param name="flePath"></param>
        /// <returns></returns>
        public static JObject PlantDetectDemo(byte[] image)
        {
            try
            {
                // 调用通用物体识别，可能会抛出网络等异常，请使用try/catch捕获
                //return _imageClient.PlantDetect(image);
                // 如果有可选参数
                var options = new Dictionary<string, object>{
        {"baike_num", 5}
    };
                // 带参数调用植物识别
                return _imageClient.PlantDetect(image, options);
            }
            catch (AipException exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
    }

    public class LocationInfo
    {
        public float left { get; set; }
        public float top { get; set; }
        public float width { get; set; }
        public float height { get; set; }
    }

    public class ImageClassifyResult<T>
    {
        public long log_id { get; set; }
        public int result_num { get; set; }
        public List<T> result { get; set; }
    }

    public class DishItem
    {
        public string name { get; set; }
        public double calorie { get; set; }
        public double probability { get; set; }
    }

    public class CarItem
    {
        public string name { get; set; }
        public double score { get; set; }
        public string year { get; set; }
        public string keyword { get; set; }
        public BaikeInfo baike_info { get; set; }
    }

    public class LogoItem
    {
        public string name { get; set; }
        public int type { get; set; }
        public double probability { get; set; }
        public LocationInfo location { get; set; }
    }

    public class BaikeInfo {
        public string baike_url { get; set; }
        public string image_url { get; set; }
        public string description { get; set; }
    }

}
