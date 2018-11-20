using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using SWF = System.Windows.Forms;
using SD = System.Drawing;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Linq;

namespace Homework_ImageClassify_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte[] ImageBytes;
        private LocationInfo location;
        public MainWindow()
        {
            InitializeComponent();
            featureSelect_LV.ItemsSource = Enum.GetValues(typeof(FeatureType)).Cast<FeatureType>().ToList();
            Image_PB.Paint += Picturebox_Paint;
            DectectFace_CB.Checked += DectectFace_CB_Checked;
            DectectFace_CB.Unchecked += DectectFace_CB_Checked;
        }

        private void Picturebox_Paint(object sender, SWF.PaintEventArgs e)
        {
            if (location != null)
                e.Graphics.DrawRectangle(SD.Pens.Red, location.left, location.top, location.width, location.height);
        }

        private void UploadImage_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            SWF.OpenFileDialog dlg = new SWF.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "Image Files|*.jpg;*.png;*.bmp;*.jpeg;*.gif;";

            // Get the selected file name and display in a TextBox 
            if (dlg.ShowDialog() == SWF.DialogResult.OK)
            {
                Image_PB.Image = SD.Image.FromFile(dlg.FileName);
                ImageBytes = File.ReadAllBytes(dlg.FileName);

                ObjectDected();
                
                //填写
                var result = ImageClassifyHelper.AdvancedGeneralDemo(ImageBytes);
                var info = JsonConvert.DeserializeObject<ImageClassifyResult<CarItem>>(result.ToString());
                List<string> items = new List<string>();
                info.result.ForEach(item => { items.Add(String.Format("{0}\n{1}\n", item.keyword, item?.baike_info?.description)); });

                SetRichTextBoxValue(relationInfo_Rtb, items);
            }
        }

        private void ObjectDected()
        {
            String with_face = "0";
            if (DectectFace_CB.IsChecked ?? false)
                with_face = "1";
            var options = new Dictionary<string, object>()
            {
                {"with_face", with_face}
            };
            var imageResult = ImageClassifyHelper.ObjectDetectDemo(ImageBytes, options);
            location = JsonConvert.DeserializeObject<LocationInfo>(imageResult["result"].ToString());
            ResetLocation(location, Image_PB.Image.Width, Image_PB.Image.Height);
        }

        private void ResetLocation(LocationInfo location, float imageWidth, float imageHeight)
        {
            if (Image_PB.Image != null)
            {
                var scaleX = Image_PB.Width / imageWidth;
                var scaleY = Image_PB.Height / imageHeight;
                float scale = scaleX > scaleY ? (float)scaleY : (float)scaleX;
                location.width *= scale;
                location.height *= scale;
                location.left *= scale;
                location.top *= scale;
                location.left += (Image_PB.Width - imageWidth * scale) / 2;
                location.top += (Image_PB.Height - imageHeight * scale ) / 2;
            }
            Image_PB.Refresh();
        }
        
        private void feature_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button btn)
            {
                List<string> items = new List<string>();
                switch (btn.DataContext)
                {
                   case FeatureType.菜品识别:
                        var dishResult = ImageClassifyHelper.DishDetectDemo(ImageBytes);
                        var dishDectectResult = JsonConvert.DeserializeObject<ImageClassifyResult<DishItem>>(dishResult.ToString());
                        dishDectectResult.result.ForEach(item => { items.Add(String.Format("菜名：{0}\n 卡路里：{1}\n 置信度：{2}", item.name, item.calorie, item.probability)); });
                        break;
                    case FeatureType.车型识别:
                        var carResult = ImageClassifyHelper.CarDetectDemo(ImageBytes);
                        var carDectectResult = JsonConvert.DeserializeObject<ImageClassifyResult<CarItem>>(carResult.ToString());
                        carDectectResult.result.ForEach(item => { items.Add(String.Format("车型：{0}\n 年份：{1}\n 置信度：{2}", item.name, item.year, item.score)); });
                        break;
                    case FeatureType.商标识别:
                        var logoResult = ImageClassifyHelper.LogoDetectDemo(ImageBytes);
                        var logoDectectResult = JsonConvert.DeserializeObject<ImageClassifyResult<LogoItem>>(logoResult.ToString());
                        logoDectectResult.result.ForEach(item => { items.Add(String.Format("商标：{0}\n 置信度：{1}", item.name, item.probability)); });
                        break;
                    case FeatureType.动物识别:
                        var animalResult = ImageClassifyHelper.AnimalDetectDemo(ImageBytes);
                        var animalDectectResult = JsonConvert.DeserializeObject<ImageClassifyResult<CarItem>>(animalResult.ToString());
                        animalDectectResult.result.ForEach(item => { items.Add(String.Format("动物：{0}\n 置信度：{1}", item.name, item.score)); });
                        break;
                    case FeatureType.植物识别:
                        var plantResult = ImageClassifyHelper.PlantDetectDemo(ImageBytes);
                        var plantDectectResult = JsonConvert.DeserializeObject<ImageClassifyResult<CarItem>>(plantResult.ToString());
                        plantDectectResult.result.ForEach(item => { items.Add(String.Format("植物：{0}\n 置信度：{1}\n", item.name, item.score)); });
                        break;
                }
                SetRichTextBoxValue(detectResult_Rtb, items);
            }
        }

        private void SetRichTextBoxValue(object sender, List<String> texts)
        {
            if(sender is RichTextBox rtb)
            {
                rtb.Document.Blocks.Clear();//清空对象text
                texts.ForEach(item =>
                {
                    Run runx = new Run(item);
                    Paragraph paragraph = new Paragraph(runx);
                    rtb.Document.Blocks.Add(paragraph);
                });

            }
            
        }
        private void DectectFace_CB_Checked(object sender, RoutedEventArgs e)
        {
            var flag = DectectFace_CB.IsChecked;
                //= !DectectFace_CB.IsChecked;
            if (Image_PB.Image != null)
                ObjectDected();
        }
    }

    public enum FeatureType
    {
        菜品识别 = 1,
        车型识别 = 2,
        商标识别 = 3,
        动物识别 = 4,
        植物识别 = 5,
    }
}
