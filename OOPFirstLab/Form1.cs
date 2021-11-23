using OOPFirstLab.Common;
using OOPFirstLab.GameObjectDescriptors;
using OOPFirstLab.GameObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOPFirstLab
{
    public partial class Form1 : Form
    {
        private GameEngine gameEngine;
        private static Random s_r = new Random();

        public Form1()
        {
            InitializeComponent();
        }
        private void Start()
        {
            if (timer1.Enabled)
                return;
        

            gameEngine = new GameEngine
                (
                    s_r,
                    rows: 500,
                    cols: 500
                );

            gameEngine.NewGame();

            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = null;

            timer1.Start();
        }

        private void DrawNext()
        {
            gameEngine.NextStep();
            DrawCurrentState();
        }

        private void DrawCurrentState()
        {
            Text = gameEngine.IsZasuha ? "Засуха" : "Нормальная погода";

            if (pictureBox1.Image == null)
            {
                int bitmap_wight = 500;
                int bitmap_height = 500;
                pictureBox1.Image = new Bitmap(bitmap_wight * Resolution, bitmap_height * Resolution);
            }
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.Green);

            // Рисуем в таком приоритете: Человек, Хищник, всеядный, травоядный, фрукт
            Brush[] brushes = {
                Brushes.DarkOrange,                // морковь
                Brushes.GreenYellow,               // трава
                Brushes.Red,                       // фрукт
                Brushes.Sienna,                    // лось
                Brushes.SandyBrown,                // олень
                Brushes.Silver,                    // кролик
                Brushes.Brown,                     // медведь
                Brushes.Gray,                      // енот
                Brushes.Pink,                      // пиг
                Brushes.Orange,                    // тигр
                Brushes.Gold,                      // гепард
                Brushes.DarkSlateGray,             // волк
                Brushes.White,                     // чел
                Brushes.Black                      // дом
            };         // человек

            var RealMap = gameEngine.GetCurrentMap();
            for (int h = 0; h < RealMap.Height; h++)
            {
                for (int w = 0; w < RealMap.Width; w++)
                {
                    List<IGameObject> gameObjects = RealMap.GetObjectsAtPos(w, h);
                    if (gameObjects != null)
                    {
                        int brushIndex = -1;
                        bool isMutant = false;
                        foreach (IGameObject go in gameObjects)
                        {
                            int idx = Convert.ToInt32(go.Type);
                            if (idx > brushIndex)
                            {
                                isMutant = go.IsMutant;
                                brushIndex = idx;
                            }
                        }

                        if (brushIndex >= 0)
                        {
                            graphics.FillRectangle(brushes[brushIndex], w * Resolution, h * Resolution, Resolution - 1, Resolution - 1);
                            if (isMutant)
                            {
                                graphics.DrawRectangle(Pens.Black, w * Resolution, h * Resolution, Resolution - 1, Resolution - 1);
                            }
                        }  
                    }                  
                }
            }

            pictureBox1.Refresh();
        }

        public int Resolution { get { return (int)nudResolution.Value; } }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameEngine != null)
            {
                int x = e.X / Resolution;
                int y = e.Y / Resolution;
                List<IGameObject> gameObjects = gameEngine.GetCurrentMap().GetObjectsAtPos(x, y);
                string descr = gameObjects != null ? GetObjectsDesctiption(gameObjects) : string.Empty;
                if (descr.Length > 0)
                {
                    MessageBox.Show(descr);
                }
            }
        }
        //инфо
        private string GetObjectsDesctiption(List<IGameObject> gameObjects)
        {
            // TODO: запрашивать строку с описанием из IGameObject
            StringBuilder sb = new StringBuilder();

            foreach (IGameObject go in gameObjects)
            {
                if (go != null)
                {
                    switch (go.Type)
                    {
                        case GameObjectType.Fruit1:
                            sb.AppendLine("Морковь");
                            break;
                        case GameObjectType.Fruit2:
                            sb.AppendLine("Трава");
                            break;
                        case GameObjectType.Fruit3:
                            sb.AppendLine("Фрукт");
                            break;
                        case GameObjectType.HerbivoreAnimal1:
                            sb.AppendFormat("Лось со здоровьем {0}", ((GameObject<HerbivoreDescriptor1>)go).Health).AppendLine();
                            break;
                        case GameObjectType.HerbivoreAnimal2:
                            sb.AppendFormat("Олень со здоровьем {0}", ((GameObject<HerbivoreDescriptor2>)go).Health).AppendLine();
                            break;
                        case GameObjectType.HerbivoreAnimal3:
                            sb.AppendFormat("Кролик со здоровьем {0}", ((GameObject<HerbivoreDescriptor3>)go).Health).AppendLine();
                            break;
                        case GameObjectType.OmnivoreAnimal1:
                            sb.AppendFormat("Медведь со здоровьем {0}", ((GameObject<OmnivoreDescriptor1>)go).Health).AppendLine();
                            break;
                        case GameObjectType.OmnivoreAnimal2:
                            sb.AppendFormat("Енот со здоровьем {0}", ((GameObject<OmnivoreDescriptor2>)go).Health).AppendLine();
                            break;
                        case GameObjectType.OmnivoreAnimal3:
                            sb.AppendFormat("Пиг со здоровьем {0}", ((GameObject<OmnivoreDescriptor3>)go).Health).AppendLine();
                            break;
                        case GameObjectType.PredatoryAnimal1:
                            sb.AppendFormat("Тигр со здоровьем {0}", ((GameObject<PredatoryDescriptor1>)go).Health).AppendLine();
                            break;
                        case GameObjectType.PredatoryAnimal2:
                            sb.AppendFormat("Гепард со здоровьем {0}", ((GameObject<PredatoryDescriptor2>)go).Health).AppendLine();
                            break;
                        case GameObjectType.PredatoryAnimal3:
                            sb.AppendFormat("Волк со здоровьем {0}", ((GameObject<PredatoryDescriptor3>)go).Health).AppendLine();
                            break;
                        case GameObjectType.Human:
                            Human h = go as Human;
                            string strHender = h.Gender == Gender.Male ? "Мужчина" : "Женщина";
                            sb.AppendFormat("{0} со здоровьем {1}", strHender, h.Health).AppendLine();
                            break;
                        case GameObjectType.House:
                            sb.AppendFormat("Дом, в котором хранится {0} фруктов", ((House)go).FruitCount).AppendLine();
                            break;
                    }
                }
            }

            return sb.ToString();
        }
        private void Stop()
        {
            if (!timer1.Enabled)
                return;

            timer1.Stop();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNext();
        }
        private void Start_Click(object sender, EventArgs e)
        {
            Start();
        }
        private void bEnd_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void nudResolution_ValueChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
