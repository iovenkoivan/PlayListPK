using NAudio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayListPK
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Инициализация выполняющего класса
        /// </summary>
        MP3TagExecutor mp3TagExecutor = new MP3TagExecutor();

        public MainForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработка выбора папки
        /// </summary>
        private void buttonSelectDirectory_Click(object sender, EventArgs e)
        {
            // Создание диалогового окна для выбора папки
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    // Внесение адреса выбранной папки в поле на форме с передачей адреса выполняющему классу
                    labelPath.Text = mp3TagExecutor.SetFolder(fbd.SelectedPath);

                    // Вывод списка файлов на форму
                    richTextBox.Lines = mp3TagExecutor.GetFileList();

                    // Включение пользовательских элементов на форме
                    buttonRandomize.Enabled = true;
                    buttonSave.Enabled = true;
                    label.Enabled = true;
                    numericUpDown.Enabled = true;
                    buttonSelectFile.Enabled = true;
                }
            }
        }
        /// <summary>
        /// Обработка перемешивания списка файлов
        /// </summary>
        private void buttonRandomize_Click(object sender, EventArgs e)
        {
            // Перемешивание списка файлов с последующим выводом на форму
            richTextBox.Lines = mp3TagExecutor.RandomizeFileList();
        }
        /// <summary>
        /// Обработка выбора рекламного аудиоролика
        /// </summary>
        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Файлы MP3 (*.mp3)|*.mp3";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Передача обрабатывающему классу имени файла и количества повторений
                    mp3TagExecutor.SetAdvertFile(ofd.FileName, Convert.ToInt32(numericUpDown.Value));

                    // Добавление рекламного аудиоролика в список файлов
                    richTextBox.Lines = mp3TagExecutor.AddAdvertFile();
                }
            }
        }
        /// <summary>
        /// Обработка сохранения плейлиста в файл
        /// </summary>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Формирование плейлиста с последующим отображением на форме
            richTextBox.Lines = mp3TagExecutor.StartGenerate();

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Файл плейлиста M3U (*.m3u)|*.m3u";
                sfd.RestoreDirectory = true;
                sfd.CreatePrompt = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Запись плейлиста в файл
                    File.WriteAllLines(sfd.FileName, mp3TagExecutor.GetPlayList());
                }
            }
        }
    }
}
