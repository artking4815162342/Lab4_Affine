using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lesson_6_Affine_Transformation
{

    public partial class Form1 : Form
    {

        bool drawCheck = false;

        Polygon copyPolygon;

        Polygon polygon;
        MyPoint newPlace;
        MyPoint originDot = new MyPoint(0,0);

        Graphics graphics;
        Bitmap bitmap;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            pictureBox1.Image = bitmap;
            graphics = Graphics.FromImage(bitmap);
            polygon = new Polygon();
            copyPolygon = new Polygon();
            pictureBox1.MouseClick += pictureBox1_Set_Dot;

            pictureBox1.MouseWheel += pictureBox1_Scale_Polygon;
            pictureBox1.MouseClick += pictureBox1_Set_Origin;

            pictureBox1.PreviewKeyDown += pictureBox1_Move;
            pictureBox1.PreviewKeyDown += pictureBox1_Rotate;
        }

        private void pictureBox1_Move(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                polygon.Move(-5f, 0);
            else if (e.KeyCode == Keys.Right)
                polygon.Move(5f, 0);
            else if (e.KeyCode == Keys.Up)
                polygon.Move(0, -5f);
            else if (e.KeyCode == Keys.Down)
                polygon.Move(0, 5f);
            else
                return;
            polygon.Refresh(graphics, pictureBox1.BackColor);
            if (originDot != null)
            {
                DrawOriginDot(originDot.x, originDot.y);
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_Rotate(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad8)
            {
                polygon.Move(-originDot.x, -originDot.y);
                polygon.Rotate(3f);
                polygon.Move(originDot.x, originDot.y);
            }
            else if (e.KeyCode == Keys.NumPad2)
            {
                polygon.Move(-originDot.x, -originDot.y);
                polygon.Rotate(-3f);
                polygon.Move(originDot.x, originDot.y);
            }
            else
                return;
            graphics.Clear(pictureBox1.BackColor);
            polygon.Refresh(graphics, pictureBox1.BackColor);
            copyPolygon.Refresh(graphics, pictureBox1.BackColor);
            if (originDot != null)
            {
                DrawOriginDot(originDot.x, originDot.y);
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_Scale_Polygon(object sender, MouseEventArgs e)
        {
            float scale = 1f;
            polygon.Move(-originDot.x, -originDot.y);
            if (e.Delta > 0)
                polygon.Scale(scale / 100.0f + 1);
            else
                polygon.Scale(1 - scale / 100.0f);
            polygon.Move(originDot.x, originDot.y);
            polygon.Refresh(graphics, pictureBox1.BackColor);
            if (originDot != null)
            {
                DrawOriginDot(originDot.x, originDot.y);
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_Set_Dot(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
            if (polygon.Dots() > 0)
                graphics.DrawLine(new Pen(Color.DarkRed), polygon.LastDot().x, polygon.LastDot().y, e.X, e.Y);

            polygon.Add(new MyPoint(e.X, e.Y));
            copyPolygon.Add(new MyPoint(e.X, e.Y));
            polygon.RedrawDots(graphics);
            if (originDot != null)
            {
                DrawOriginDot(originDot.x, originDot.y);
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_Select_Dot(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
            polygon.SelectDot(e.X, e.Y);
            polygon.HoverSelected(graphics);
            if (originDot != null)
            {
                DrawOriginDot(originDot.x, originDot.y);
            }
            pictureBox1.Refresh();
        }

                                                                                                                                                                                           int mamicSum = 0;

        private void pictureBox1_Set_Origin(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
                return;
            polygon.Refresh(graphics, pictureBox1.BackColor);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DrawOriginDot(e.X, e.Y);
                originDot = new MyPoint(e.X, e.Y);
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_Move_Dot(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
            if (polygon.Selected == null)
                return;
            newPlace = new MyPoint(e.X, e.Y);
            polygon.Refresh(graphics, pictureBox1.BackColor);
            int r = MyPoint.radius;
            graphics.DrawEllipse(new Pen(new SolidBrush(Color.White)), e.X - r, e.Y - r, r * 2, r * 2);
            if (originDot != null)
            {
                DrawOriginDot(originDot.x, originDot.y);
            }
            pictureBox1.Refresh();
        }

        private void DrawOriginDot(float p1, float p2)
        {
            graphics.FillEllipse(new SolidBrush(Color.ForestGreen), p1 - 5, p2 - 5, 5 * 2, 5 * 2);
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!(sender as RadioButton).Checked)
                return;
            pictureBox1.MouseClick -= pictureBox1_Move_Dot;
            pictureBox1.MouseClick -= pictureBox1_Select_Dot;
            pictureBox1.MouseClick += pictureBox1_Set_Dot;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender as RadioButton).Checked)
                return;
            pictureBox1.MouseClick -= pictureBox1_Move_Dot;
            pictureBox1.MouseClick -= pictureBox1_Set_Dot;
            pictureBox1.MouseClick += pictureBox1_Select_Dot;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender as RadioButton).Checked)
                return;
            pictureBox1.MouseClick -= pictureBox1_Set_Dot;
            pictureBox1.MouseClick -= pictureBox1_Select_Dot;
            pictureBox1.MouseClick += pictureBox1_Move_Dot;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            polygon.Close(graphics);
            polygon.RedrawDots(graphics);
            pictureBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            polygon.DeleteDot();
            polygon.Refresh(graphics, pictureBox1.BackColor);
            pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            polygon.MoveDot(newPlace);
            polygon.Refresh(graphics, pictureBox1.BackColor);
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            (sender as PictureBox).Focus();
        }


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            mamicSum++;

            System.Random r = new Random();
            int a = r.Next(5, 8);

            if (mamicSum >= a)
            {
                pictureBox1.Image = (Image)(new Bitmap("source.binprop"));
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }

    public class MyPoint
    {
        public float x, y, z;

        public static int radius = 5;

        public MyPoint(int x, int y, int z)
        {
            this.x = (float)x;
            this.y = (float)y;
            this.z = (float)z;
        }

        public MyPoint(int x, int y)
        {
            this.x = (float)x;
            this.y = (float)y;
            this.z = 1f;
        }
    }

    public class Polygon
    {
        public Queue<MyPoint> data;

        public Stack<MyPoint> _data;

        public MyPoint selected;

        public MyPoint Selected
        {
            get { return selected; }
        }

        public Polygon ()
        {
            this.data = new Queue<MyPoint>();
            this._data = new Stack<MyPoint>();
        }

        public Polygon(Polygon p)
        {
            this.data = new Queue<MyPoint>(p.data);
            this._data = new Stack<MyPoint>(p._data);
        }

        public MyPoint LastDot()
        {
            return _data.Peek();
        }

        public int Dots() { return data.Count; }

        public void Add(MyPoint p)
        {
            data.Enqueue(p);
            _data.Push(p);  
        }

        internal void RedrawLines(Graphics graphics)
        {
            if (data.Count == 0)
                return;
            MyPoint temp = data.Dequeue();
            int c = data.Count;
            data.Enqueue(temp);
            for (int i = 0; i < c; ++i)
            {
                graphics.DrawLine(new Pen(Color.DarkRed), temp.x, temp.y, data.Peek().x, data.Peek().y);
                temp = data.Dequeue();
                data.Enqueue(temp);
            }
            Close(graphics);
        }

        internal void RedrawDots(Graphics graphics)
        {
            int r = MyPoint.radius;
            foreach (var v in data)
                graphics.FillEllipse(new SolidBrush(Color.LightSeaGreen), v.x - r, v.y - r, r * 2, r * 2);
        }

        internal void Close(Graphics graphics)
        {
            graphics.DrawLine(new Pen(Color.DarkRed), LastDot().x, LastDot().y, data.Peek().x, data.Peek().y);
        }

        internal void SelectDot(int p1, int p2)
        {
            foreach (var v in data)
                if (Math.Sqrt(Math.Pow((v.x - p1), 2) + Math.Pow((v.y - p2), 2)) <= MyPoint.radius)
                {
                    selected = v;
                }
        }

        internal void HoverSelected(Graphics graphics)
        {
            if (selected == null)
                return;
            int r = MyPoint.radius;
            RedrawDots(graphics);
            graphics.FillEllipse(new SolidBrush(Color.Crimson), selected.x - r, selected.y - r, r * 2, r * 2);
        }

        internal void DeleteDot()
        {
            if (selected == null)
                return;
            int c = data.Count;
            MyPoint temp;
            for (int i = 0; i < c; ++i)
            {
                temp = data.Dequeue();
                if (temp == selected)
                    if (i == c-1)
                    {
                        _data.Pop();
                        continue;
                    }
                    else
                        continue;
                data.Enqueue(temp);
            }
            selected = null;
        }

        internal void Scale(float scale)
        {
            foreach (MyPoint mp in data)
            {
                mp.x = (mp.x * scale);
                mp.y = (mp.y * scale);
            }
        }

        internal void MoveDot(MyPoint newPlace)
        {
            if (selected == null)
                return;
            foreach (var v in data)
                if (v.x == selected.x && v.y == selected.y)
                {
                    v.x = newPlace.x;
                    v.y = newPlace.y;
                }
            selected = null;
        }

        internal void Refresh(Graphics graphics, Color color)
        {
            RedrawLines(graphics);
            RedrawDots(graphics);
            HoverSelected(graphics);

        }

        internal void Move(float p1, float p2)
        {
            foreach (MyPoint mp in data)
            {
                mp.x += p1;
                mp.y += p2;
            }
        }

        internal void Rotate(float p)
        {
            foreach (MyPoint mp in data)
            {
                float old_y = mp.y;
                float old_x = mp.x;
                float cos = (float)Math.Cos(p * Math.PI / 180f);
                float sin = (float)Math.Sin(p * Math.PI / 180f);
                mp.x = old_x * cos - old_y * sin;
                mp.y = old_y * cos + old_x * sin;
            }
        }
    }

    //TODO : реализовать класс Polygon на основе связного списка
    public class SpecialQueue<T>
    {
        LinkedList<T> list = new LinkedList<T>();

        public void Enqueue(T t)
        {
            list.AddLast(t);
        }

        public T Dequeue()
        {
            var result = list.First.Value;
            list.RemoveFirst();
            return result;
        }

        public T Peek()
        {
            return list.First.Value;
        }

        public bool Remove(T t)
        {
            return list.Remove(t);
        }

        public int Count { get { return list.Count; } }
    }
}
