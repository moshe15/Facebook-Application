using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace C20_Ex03_Mor_316046093_Moshe_308459841
{
    public class AppSettings
    {
        public Point LastWindowLocation { get; set; }

        public Size LastWindowSize { get; set; }

        public bool RememberUser { get; set; }

        public string LastAccessToken { get; set; }

        public AppSettings()
        {
            LastWindowLocation = new Point(20, 50);
            LastWindowSize = new Size(500, 1570);
            RememberUser = false;
            LastAccessToken = null;
        }

        public static AppSettings LoadFromFile()
        {
            AppSettings savedPreviosSettings = new AppSettings();
            if (File.Exists(@"C:\Temp\AppSettings.xml"))
            {
                using (Stream stream = new FileStream(@"C:\Temp\AppSettings.xml", FileMode.Open))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppSettings));
                    savedPreviosSettings = xmlSerializer.Deserialize(stream) as AppSettings;
                }
            }

            return savedPreviosSettings;
        }

        public void SaveToFile()
        {
            using (Stream stream = new FileStream(@"C:\Temp\AppSettings.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stream, this);
            }
        }

        public void BackToDefault()
        {
            RememberUser = false;
            LastAccessToken = null;
        }
    }
}