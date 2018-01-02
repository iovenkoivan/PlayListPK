using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayListPK
{
    class MP3Tag
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName;
        /// <summary>
        /// Полный путь в файлу
        /// </summary>
        public string FullFileName;

        public MP3Tag()
        {

        }

        public MP3Tag(string fileName, string fullFileName)
        {
            this.FileName = fileName;
            this.FullFileName = fullFileName;
        }
    }
}
