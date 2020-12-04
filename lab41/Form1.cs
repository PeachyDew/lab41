using System;
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
                Circle.GetNow().Draw(e); //отрисовка нового круга 
                Circle.GetNext(); // указатель на следующий
            }
            Circle.Get0(); 
        }

        void Delete_Process(EventArgs e) //процесс удаления элемента/ов
        {
            int k = 0;
            for (int i = 0; i < (Circle.GetTotalElements() - 1); i++) //сдвиг счетчика на последний элемент
            {
                Circle.GetNext(); 
            }

            for (int i = Circle.GetTotalElements() - 1; i >= 0; i--)
            {
                if (Circle.GetNow().Getselect1() == true) //если объект выделен
                {
                    Circle.Delete(i); //удаление элемента
                    k++;
                }
                Circle.GetPrevious();
            }
            if (k == 0) //если объекты не выделены удаляет последний 
            {
                Circle.Delete(Circle.GetTotalElements() - 1);
                Circle.GetPrevious();

            }

            Circle.Get0();
            pictureBox.Refresh(); // перерисовка pictureBox
        }

        void Delete_Click(object sender, EventArgs e) //
        {
            Delete_Process(null); //запуск удаления 
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e) //нажатие мышки на pictureBox
        {
            
                if (e.Button == MouseButtons.Left)
                {
                    int a = 0;
                    for (int i = 0; i < Circle.GetTotalElements(); i++)
                    {
                   
                        if (Circle.GetNow().Border(e.X, e.Y) == true) //если попали в круг 
                        {
                        if (!ModifierKeys.HasFlag(Keys.Control)) //если нажат ctrl
                        {
                            Circle.GetNow().SelectChange(); //меняем выделение с true на false
                        }
                            a++;
                        }
                        Circle.GetNext(); //сдвигаем указатель на следующий 
                    }
                    Circle.Get0();

                    if (a == 0) //если не попали в круг
                    {
                        CCircle Lap = new CCircle(e.X, e.Y); //создаем новый круг по полученным координатам
                        Circle.Add(Lap); // добавляем круг в хранилище
                       
                        for (int i = 0; i < (Circle.GetTotalElements() - 1); i++)
                        {
                            Circle.GetNext();
                        }
                        Circle.GetNow().SelectChange2(); //делаем созданный объект единственно выделенным

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
        private bool select; //выделение объекта
        private int x; // координата x круга
        private int y; // координата y круга 
        private const int r = 30; // радиус
        public CCircle() // конструктор по умолчанию
        {
            y = x = 0;
            select = false;
        }
        public CCircle(int _x, int _y) // конструктор с параметрами
        {
            x = _x;
            y = _y;
            select = false;
        }
        public bool Getselect1()// возвращает выделен ли объект
        {
            return select;
        }

        public void SelectChange()  //метод снимающий выделение, если объект выделен
        { 
            if (select == true)
            {
                select = false;
            }
            
        }
        public void SelectChange2()  //метод выделяющий объект, если он не выделен
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
            if ((int)Math.Sqrt(_x * _x + _y * _y) <= r) // попадание координат в круг
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

        public void Draw(PaintEventArgs e) //отрисовка объекта в pictureBox
        {
            Pen Pen1 = new Pen(Brushes.Pink, 4);
            Pen Pen2 = new Pen(Brushes.Black, 4);
      
            if (select == true) // если он выделен
            {
                e.Graphics.DrawEllipse(Pen1, x - r, y - r, r * 2, r * 2); // отрисовка круга розовым цветом
            }
            else 
            {
                e.Graphics.DrawEllipse(Pen2, x - r, y - r, r * 2, r * 2);// отрисовка круга черным цветом
            }
        }
    }


class MyStorage // класс хранилище
{
    CCircle[] array; //массив объектов CCIrcle
    int totalElements; //количество элементов , находящихся в хранилище
    int size; //размер хранилища
    int index;
    public MyStorage() // конструктор по умолчанию
    {
        index = 0;
        totalElements = 0;
        size = 0;
        array = null;
    }
    public MyStorage(int size) // конструктор с параметрами
    {
        index = 0;
        totalElements = 0;
        this.size = size;
        array = new CCircle[size]; 
    }
    ~MyStorage()
    {
       
    }
    public void ExpandarrElements()//расширение массива
    {
        CCircle[] newarray = array; //создание массива newarray идентичного array 
            array = new CCircle[size * 2]; // на месте array создается новый массив в 2 раза больше
        for (int i = 0; i < size; i++)
        {
            array[i] = newarray[i]; //копирование элементов
        }
        size = size * 2; 
    }
    public void Add(CCircle obj)//добавление объекта 
    {
        totalElements++; // увеличиваем общее количество элементво
        if (totalElements == size) // если количество элементов равно размеру, увеличиваем
                ExpandarrElements();
            array[totalElements - 1] = obj; // добавляем объект в массив
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
    public int GetTotalElements() //возращение количество элементов в хранилище
    {
        return totalElements;
    }
    public int GetSize()// возвращение размера
    {
        return size;
    }
    public void GetNext() //метод возвращающий указатель не следующий
    {
        index++;
    }
    public void GetPrevious()//метод возвращающий указатель не предыдущий
    {
        index--;
    }
    public void Get0() //метод присваивающий index 0
        {
        index = 0;
    }

    public CCircle GetNow() //возвращает элемент в храниоище
    {
        if (array[index] != null)
            return array[index];
        else return null;
    }
}
}
