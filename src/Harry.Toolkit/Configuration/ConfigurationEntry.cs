using System;
using System.IO;
using System.Xml.Serialization;

namespace Harry.Configuration
{
    public abstract class ConfigurationEntry<Entry> where Entry : ConfigurationEntry<Entry>, new()
    {
        private static string _contentRootPath;
        private static bool _initialized = false;

        public string FilePath { get; private set; }

        /// <summary>
        /// 从XML文件加载配置信息
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static Entry LoadFromFile(string fullFileName)
        {
            if (fullFileName == null)
            {
                throw new ArgumentNullException(nameof(fullFileName));
            }

            Entry result = null;

            if (File.Exists(fullFileName))
            {
                try
                {
                    using (var stream = File.Open(fullFileName, FileMode.Open))
                    {
                        XmlSerializer _serializer = new XmlSerializer(typeof(Entry));
                        result = _serializer.Deserialize(stream) as Entry;
                    }
                }
                catch (FileNotFoundException)
                {
                    result = new Entry();
                }
            }
            else
            {
                result = new Entry();
            }

            result.FilePath = fullFileName;

            return result;
        }

        public static Entry LoadFromFile(string fileName, string contentPath)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            if (contentPath == null)
            {
                throw new ArgumentNullException(nameof(contentPath));
            }
            _contentRootPath = contentPath;


            var fullFileName = GetRealPath(fileName, contentPath);

            return LoadFromFile(fullFileName);
        }


        /// <summary>
        /// 保存当前配置
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contentPath"></param>
        public void SaveTo(string fileName, string contentPath = null)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            fileName = GetRealPath(fileName, contentPath);

            using (var stream = File.Open(fileName, FileMode.Create))
            {
                XmlSerializer _serializer = new XmlSerializer(typeof(Entry));
                _serializer.Serialize(stream, this);
            }
        }

        public void Save()
        {
            SaveTo(this.FilePath);
        }

        //获取实际物理地址
        public static string GetRealPath(string fileName, string contentPath = null)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (Path.IsPathRooted(fileName))
            {
                return fileName;
            }

            if (contentPath == null)
            {
                throw new ArgumentNullException(nameof(contentPath));
            }
#if  NET35
            return Path.Combine(contentPath, String.Join(new string(Path.DirectorySeparatorChar,1), fileName.Split(new char[] { '/', '~', '\\' }, StringSplitOptions.RemoveEmptyEntries)));
#else
            return Path.Combine(contentPath, Path.Combine(fileName.Split(new char[] { '/', '~', '\\' }, StringSplitOptions.RemoveEmptyEntries)));
#endif
        }


    }
}
