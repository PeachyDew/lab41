﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab41
{
    public partial class Form1 : Form
    {
        MyStorage Circle = new MyStorage(50);
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < Circle.GetTotalElements(); i++)
            {
                Circle.GetNow().Draw(e);
                Circle.GetNext();
            }
            Circle.Get0();
        }

        
        void Delete_Process(EventArgs e)
        {
            int k = 0;
            for (int i = 0; i < (Circle.GetTotalElements() - 1); i++)
            {
                Circle.GetNext();
            }
            for (int i = Circle.GetTotalElements() - 1; i >= 0; i--)
            {
                if (Circle.GetNow().Getselect1() == true)
                {
                    Circle.Delete(i);
                    k++;
                }
                Circle.GetPrevious();
            }
            if (k == 0)
            {
                Circle.Delete(Circle.GetTotalElements() - 1);
                Circle.GetPrevious();

            }

            Circle.Get0();
            pictureBox.Refresh();
        }

        void Delete_Click(object sender, EventArgs e)
        {
            Delete_Process(null);
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == ((char)Keys.Delete))
            {
                Delete_Process(null);
            }
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            
                if (e.Button == MouseButtons.Left)
                {
                    int a = 0;
                    for (int i = 0; i < Circle.GetTotalElements(); i++)
                    {
                   
                        if (Circle.GetNow().Border(e.X, e.Y) == true)
                        {
                        if (!ModifierKeys.HasFlag(Keys.Control)) {
                            Circle.GetNow().SelectChange();
                        }
                            a++;
                        }
                        Circle.GetNext();
                    }
                    Circle.Get0();

                    if (a == 0)
                    {
                        CCircle Lap = new CCircle(e.X, e.Y);
                        Circle.Add(Lap);
                       
                        for (int i = 0; i < (Circle.GetTotalElements() - 1); i++)
                        {
                            Circle.GetNext();
                        }
                        Circle.GetNow().SelectChange2();
                        for (int i = Circle.GetTotalElements() - 1; i >= 0; i--)
                        {
                            if (Circle.GetNow().Getselect1() == true)
                            {
                                Circle.GetNow().SelectChange();
                            }
                            Circle.GetPrevious();
                        }
                        Circle.Get0();

                        for (int i = 0; i < (Circle.GetTotalElements() - 1); i++)
                        {
                            Circle.GetNext();
                        }
                        Circle.GetNow().SelectChange2();
                        Circle.Get0();
                    }
                    pictureBox.Refresh();
                }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
        }

        }
    public class CCircle
    {
        private bool select;
        private int x;
        private int y;
        private const int r = 30;
        public CCircle()
        {
            y = x = 0;
            select = false;
        }
        public CCircle(int _x, int _y)
        {
            x = _x;
            y = _y;
            select = false;
        }
        public bool Getselect1()
        {
            return select;
        }

        public void SelectChange() {
            if (select == true)
            {
                select = false;
            }
            
        }
        public void SelectChange2()
        {
            if (select == false)
            {
                select = true;
            }

        }
        public bool Border(int xS, int yS) // проверка попадания в круг 
        {
            bool bord = false;
            int _x = Math.Abs(xS - x);
            int _y = Math.Abs(yS - y);
            if ((int)Math.Sqrt(_x * _x + _y * _y) <= r)
            {
                if (select == true)
                {
                    select = false;
                }
                else
                {
                    select = true;
                }
                bord = true;
            }
            return bord;
        }

        public void Draw(PaintEventArgs e)
        {
            Pen Pen1 = new Pen(Brushes.Pink, 4);
            Pen Pen2 = new Pen(Brushes.Black, 4);
      
            if (select == true)
            {
                e.Graphics.DrawEllipse(Pen1, x - r, y - r, r * 2, r * 2);
            }
            else 
            {
                e.Graphics.DrawEllipse(Pen2, x - r, y - r, r * 2, r * 2);
            }
        }
    }


class MyStorage
{
    CCircle[] array;
    int totalElements;
    int size;
    int index;
    public MyStorage()
    {
        index = 0;
        totalElements = 0;
        size = 0;
        array = null;
    }
    public MyStorage(int size)
    {
        index = 0;
        totalElements = 0;
        this.size = size;
        array = new CCircle[size];
    }
    ~MyStorage()
    {
        Console.Write("Destructor Storage \n");
    }
    public void ExpandarrElements()//расширение массива
    {
        CCircle[] newarray = array;
            array = new CCircle[size * 2];
        for (int i = 0; i < size; i++)
        {
            array[i] = newarray[i];
        }
        size = size * 2;
    }
    public void Add(CCircle obj)//добавление
    {
        totalElements++;
        if (totalElements == size)
                ExpandarrElements();

            array[totalElements - 1] = obj;
    }
    public void Delete(int a)//удаление 
    {
        if (totalElements == 0)
        {

        }
        else
        {
            for (int i = a; i < totalElements - 1; i++)
            {
                    array[i] = array[i + 1];
            }
                array[totalElements] = null;
            totalElements--;
        }
    }
    public int GetTotalElements()
    {
        return totalElements;
    }
    public int GetSize()
    {
        return size;
    }
    public void GetNext()
    {
        index++;
    }
    public void GetPrevious()
    {
        index--;
    }
    public void Get0()
    {
        index = 0;
    }

    public CCircle GetNow()
    {
        if (array[index] != null)
            return array[index];
        else return null;
    }
}
}