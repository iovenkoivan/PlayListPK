using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayListPK
{
    class MP3TagExecutor
    {
        /// <summary>
        /// Метка заголовка плейлиста
        /// </summary>
        const string LabelFirst = @"#EXTM3U";
        /// <summary>
        /// Метка дополнительной информации
        /// </summary>
        const string LabelSecond = @"#EXTINF:";
        /// <summary>
        /// Список файлов
        /// </summary>
        List<string> fileList = new List<string>();
        /// <summary>
        /// Список плейлиста
        /// </summary>
        List<string> playList = new List<string>();
        /// <summary>
        /// Список MP3-тегов
        /// </summary>
        List<MP3Tag> mp3Tag = new List<MP3Tag>();
        /// <summary>
        /// Рабочая папка
        /// </summary>
        string workPath;
        /// <summary>
        /// Полный путь файла рекламного видеоролика
        /// </summary>
        string advertFileName;
        /// <summary>
        /// Количество треков, после которых должен проигрываться рекламный
        /// </summary>
        int advertRepeat;
        /// <summary>
        /// Установка рабочей папки с музыкой
        /// </summary>
        /// <param name="path">Полный адрес папки</param>
        /// <returns>Полный адрес папки</returns>
        public string SetFolder(string path)
        {
            workPath = path;

            return workPath;
        }
        /// <summary>
        /// Установка параметров рекламного аудиоролика
        /// </summary>
        /// <param name="file">Полный адрес файла</param>
        /// <param name="repeat">Количество пропускаемых треков</param>
        public void SetAdvertFile(string file, int repeat)
        {
            advertFileName = file;
            advertRepeat = repeat;
        }
        /// <summary>
        /// Получение списка MP3-файлов в рабочей папке
        /// </summary>
        /// <returns>Список файлов полученных из рабочей папки</returns>
        public string[] GetFileList()
        {
            // Очистка имеющегося списка
            fileList.Clear();

            // Поиск MP3-файлов
            foreach (string item in Directory.GetFiles(workPath, @"*.mp3"))
            {
                fileList.Add(item);
            }

            return fileList.ToArray();
        }
        /// <summary>
        /// Перемешивание списка файлов
        /// </summary>
        /// <returns>Перемешанный список файлов</returns>
        public string[] RandomizeFileList()
        {
            Random random = new Random();

            // Изъятие первого элемента списка и помещение его на рандомную позицию.
            for (int i = 0; i < fileList.Count; i++)
            {
                string temp = fileList[0];
                fileList.RemoveAt(0);
                fileList.Insert(random.Next(fileList.Count), temp);
            }

            return fileList.ToArray();
        }
        /// <summary>
        /// Создание списка MP3-тегов
        /// </summary>
        public void GetMP3Tag()
        {
            foreach (string item in fileList)
            {
                mp3Tag.Add(new MP3Tag(Path.GetFileName(item), item));
            }
        }
        /// <summary>
        /// Добавление рекламного аудиоролика в список файлов
        /// </summary>
        /// <returns>Список файлов, в котором рекламный аудиоролик повторяется через установленное количество треков</returns>
        public string[] AddAdvertFile()
        {
            int i = 0;

            do
            {
                fileList.Insert(i, advertFileName);
                i = i + advertRepeat;
            } while (i < fileList.Count);

            return fileList.ToArray();
        }
        /// <summary>
        /// Формирование файла плейлиста
        /// </summary>
        /// <returns>Массив строк готового плейлиста</returns>
        public string[] StartGenerate()
        {
            // Очистка текущего массива строк для плейлиста
            playList.Clear();
            // Добавление заголовка плейлиста
            playList.Add(LabelFirst);
            // Создание списка MP3-тегов
            GetMP3Tag();
            // Перебор полученного списка MP3-теков и и добавление информации из них в массив строк плейлиста
            foreach (var item in mp3Tag)
            {
                // Метка дополнительной информации, продолжительность (-1), имя файла
                playList.Add(LabelSecond + "-1," + item.FileName);
                playList.Add(item.FullFileName);
            }

            return playList.ToArray();
        }
        /// <summary>
        /// Запрос массива строк плейлиста
        /// </summary>
        /// <returns>Массив строк готового плейлиста</returns>
        public string[] GetPlayList()
        {
            return playList.ToArray();
        }
    }
}
