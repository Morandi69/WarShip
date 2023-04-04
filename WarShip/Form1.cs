using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarShip
{
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int x4=0,x3=0,x2=0,x1=0;
        int[,] UserField = new int[10,10];//0-пустая клетка 1-корабль 2-подбитый корабль 3-клетка куда уже походили
        int[,] AiField=new int[10,10];
        List<Ship> AiShips = new List<Ship>();
        List<Ship> UserShips = new List<Ship>();
        List<int[]> LastShots = new List<int[]>();
        bool shotok = false;
        int[] UserDethShip = new int[4];
        int[] AiDethShip = new int[4];


        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 10;
            dataGridView1.RowCount = 10;
            dataGridView2.ColumnCount = 10;
            dataGridView2.RowCount = 10;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToResizeRows = false;


            for (int i = 0; i < 10; i++)
            {
                dataGridView1.Rows[i].Height = dataGridView1.Height / 10;
                dataGridView1.Columns[i].Width = dataGridView1.Width / 10;
                dataGridView2.Rows[i].Height = dataGridView2.Height / 10;
                dataGridView2.Columns[i].Width = dataGridView2.Width / 10;

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
            button2.Visible = true;
            label1.Visible = true;
            x4 = 0; x3 = 0; x2 = 0; x1 = 0;
            AiField = new int[10, 10];
            UserField = new int[10, 10];
            AiShips.Clear();
            UserShips.Clear();
            LastShots.Clear();
            shotok = false;
            SetShip(AiSet(4), AiField, AiShips);
            SetShip(AiSet(3), AiField, AiShips);
            SetShip(AiSet(3), AiField, AiShips);
            SetShip(AiSet(2), AiField, AiShips);
            SetShip(AiSet(2), AiField, AiShips);
            SetShip(AiSet(2), AiField, AiShips);
            SetShip(AiSet(1), AiField, AiShips);
            SetShip(AiSet(1), AiField, AiShips);
            SetShip(AiSet(1), AiField, AiShips);
            SetShip(AiSet(1), AiField, AiShips);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    dataGridView1[i, j].Value = ' ';
                    dataGridView2[i, j].Value = ' ';
                }
            }
            drawAiField();
            drawUserField();
            

        }
        void Win()
        {
            if (UserShips.Count == 0)
            {
                MessageBox.Show("Вы проиграли");
            }
            if (AiShips.Count == 0)
            {
                MessageBox.Show("Вы победили");
            }
        }
        
        private void button2_Click(object sender, EventArgs e)//Растановка кораблей игрока
        {
            List<int[]> SelectCells = SelectedCellsToArray(dataGridView2.SelectedCells);
            if (CanSet(SelectCells,UserField))
            {
                if(SelectCells.Count==4 && x4 < 1)
                {
                    SetShip(SelectCells, UserField,UserShips);
                    drawUserField();
                    x4++;
                }
                if (SelectCells.Count == 3 && x3 < 2)
                {
                    SetShip(SelectCells, UserField, UserShips);
                    x3++;
                    drawUserField();
                }
                if (SelectCells.Count == 2 && x2 < 3)
                {
                    SetShip(SelectCells, UserField, UserShips);
                    x2++;
                    drawUserField();
                }
                if (SelectCells.Count == 1 && x1 < 4)
                {
                    SetShip(SelectCells, UserField, UserShips);
                    x1++;
                    drawUserField();
                }
                if (UserShips.Count == 10)
                {
                    dataGridView1.Enabled = true;
                    dataGridView2.CurrentCell = null;
                    button2.Visible = false;
                    label1.Visible = false;
                    label18.Visible = true;
                }
            }
            dataGridView2.CurrentCell = null;
        }

        List<int[]> SelectedCellsToArray (DataGridViewSelectedCellCollection dataGridCells)//Преобразовывает из селектед целс в массив 
        {
            List<int[]> result=new List<int[]>();
            for (int i = 0; i < dataGridCells.Count; i++)
            {
                result.Add(new int[] { dataGridCells[i].ColumnIndex, dataGridCells[i].RowIndex });
            }
            return result;
        }


        List<int[]> AiSet(int count)//Генерирует координаты нового корабля
        {
            Random ran = new Random();
            List<int[]> result = new List<int[]>();
            do
            {
                result.Clear();
                int i = ran.Next(0, 11 - count);
                int j = ran.Next(0, 10);
                for (int l = 0; l < count; l++)
                {
                    result.Add(new int[] { i, j });
                    i++;
                }
            } while (CanSet(result,AiField)==false);
            return result;
        }


        
        bool CanSet(List<int[]> dataG, int[,] field)//проверяет можно ли поставить корабль
        {
            if (dataG.Count > 4)
            {
                return false;
            }

            bool row = true;
            bool col = true;
            for (int i = 1; i < dataG.Count; i++)
            {
                if (dataG[i][0] != dataG[0][0])
                {
                    col = false;
                }
                if (dataG[i][1] != dataG[0][1])
                {
                    row = false;
                }
            }

            if (col == false && row == false)
            {
                return false;
            }

            for (int i = 0; i < dataG.Count; i++)
            {

                int Celli = dataG[i][0];
                int Cellj = dataG[i][1];
                for (int j = Celli - 1; j <= Celli + 1; j++)
                {
                    for (int l = Cellj - 1; l <= Cellj + 1; l++)
                    {
                        if (j < 10 && l < 10 && l > -1 && j > -1)
                        {
                            if (field[j, l] != 0)
                            {
                                return false;
                            }

                        }

                    }
                }
            }
            return true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)//Обработчик при клике на ячейку
        {
            if (AiField[e.ColumnIndex, e.RowIndex] == 0)
            {
                AiField[e.ColumnIndex, e.RowIndex] = 3;
                drawAiField();
                shotok = false;
                StartPosition:
                if (LastShots.Count == 0)
                {
                    if (AiMove()) { Win(); goto StartPosition; }
                }
                else
                {
                    if (AiSecondShot()) { Win(); goto StartPosition; }
                }
                if (shotok == false)
                {
                    if(AiMoveRandom()) { Win(); goto StartPosition; }
                }
                
                
            }
            if (AiField[e.ColumnIndex, e.RowIndex] == 1)
            {
                AiField[e.ColumnIndex, e.RowIndex] = 2;
                EmptyCells(e.ColumnIndex, e.RowIndex, AiField,AiShips);
                


            }
            drawAiField();
            Win();

        }

        bool AiMove()
        {
            int k = 1;
            do
            {
                int j = k;
                for (int i = 0; i < k+1; i++)
                {
                    if (UserField[i, j] == 0 || UserField[i, j] == 1)
                    {
                        shotok = true;
                        
                        if (UserField[i, j] == 0)
                        {
                            UserField[i, j] = 3;
                            drawUserField();
                            return false;
                        }
                        if (UserField[i, j] == 1)
                        {
                            UserField[i, j] = 2;
                            EmptyCells(i, j,UserField,UserShips);
                            LastShots.Add(new int[] { i, j });
                            drawUserField();
                            return true;

                        }
                        
                    }
                    j--;
                }
                k += 2;
            } while (k < 10);

            k = 2;
            do
            {
                int j = 9;
                for (int i = k; i < 10; i++)
                {
                    if (UserField[i, j] == 0 || UserField[i, j] == 1)
                    {
                        shotok = true;
                        if (UserField[i, j] == 0)
                        {
                            UserField[i, j] = 3;
                            drawUserField();
                            return false;
                        }
                        if (UserField[i, j] == 1)
                        {
                            UserField[i, j] = 2;
                            EmptyCells(i, j, UserField, UserShips);
                            LastShots.Add(new int[] { i, j });
                            drawUserField();
                            return true;

                        }
                        
                    }
                    j--;
                }
                k += 2;
            } while (k < 9);
            return false;
        }

        bool AiSecondShot()
        {
            int i = LastShots[LastShots.Count - 1][0];
            int j = LastShots[LastShots.Count - 1][1];
            for (int k = i-1; k < i+2; k++)
            {
                for (int l = j-1; l < j+2; l++)
                {
                    if(k<10 && l<10 && k>-1 && l>-1 && (UserField[k,l]==0 || UserField[k, l] == 1))
                    {
                        shotok = true;
                        if (UserField[k, l] == 0)
                        {
                            UserField[k, l] = 3;
                            drawUserField();
                            return false;

                        }
                        if (UserField[k, l] == 1)
                        {
                            UserField[k, l] = 2;
                            EmptyCells(k, l, UserField, UserShips);
                            LastShots.RemoveAt(LastShots.Count - 1);
                            LastShots.Add(new int[] { k, l });
                            drawUserField();
                            return true;
                        }
                    }
                }
            }
            LastShots.RemoveAt(LastShots.Count - 1);
            return false;
        }

        

        bool AiMoveRandom()
        {

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (UserField[i, j] == 0 || UserField[i, j] == 1)
                    {
                        shotok = true;

                        if (UserField[i, j] == 0)
                        {
                            UserField[i, j] = 3;
                            drawUserField();
                            return false;
                        }
                        if (UserField[i, j] == 1)
                        {
                            UserField[i, j] = 2;
                            EmptyCells(i, j, UserField, UserShips);
                            LastShots.Add(new int[] { i, j });
                            drawUserField();
                            return true;

                        }

                    }
                }
            }
            return false;
        }

        void EmptyCells(int i, int j,int [,] Field,List<Ship> Ships)
        {
            if (i > 0 && j>0)
            {
                Field[i - 1, j - 1] = 3;
            }
            if(i<9 && j < 9)
            {
                Field[i + 1, j + 1] = 3;
            }
            if(i>0 && j < 9)
            {
                Field[i - 1, j + 1] = 3;
            }
            if(i<9 && j > 0)
            {
                Field[i + 1, j - 1] = 3;
            }
            for (int l = 0; l < Ships.Count; l++)
            {
                for (int k = 0; k < Ships[l].Coords.Count; k++)
                {
                    if(Ships[l].Coords[k][0]==i && Ships[l].Coords[k][1] == j)
                    {
                        Ships[l].Hp--;
                    }
                }
            }
            
        }
        void DeadShip(int[,] Field, List<Ship> Ships,int[] DethShip)
        {
            for (int i = 0; i < Ships.Count; i++)
            {
                if (Ships[i].Hp <= 0)
                {
                    for (int j = 0; j < Ships[i].Coords.Count; j++)
                    {
                        for (int k = Ships[i].Coords[j][0] - 1; k < Ships[i].Coords[j][0]+2; k++)
                        {
                            for (int l = Ships[i].Coords[j][1] - 1; l < Ships[i].Coords[j][1] + 2; l++)
                            {
                                if(l>-1 && k>-1 && l<10 && k<10 && Field[k, l] == 0)
                                {
                                    Field[k, l] = 3;
                                }
                            }
                        }
                    }
                    if (Ships[i].Coords.Count == 1)
                    {
                        DethShip[0]++;
                    }
                    if (Ships[i].Coords.Count == 2)
                    {
                        DethShip[1]++;
                    }
                    if (Ships[i].Coords.Count == 3)
                    {
                        DethShip[2]++;
                    }
                    if (Ships[i].Coords.Count == 4)
                    {
                        DethShip[3]++;
                    }
                    Ships.RemoveAt(i);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        void SetShip(List<int[]> dataGridCells, int[,] field, List<Ship> Ships)//устанавливает корабль
        {
            if (CanSet(dataGridCells,field))
            {
                for (int i = 0; i < dataGridCells.Count; i++)
                {
                    field[dataGridCells[i][0], dataGridCells[i][1]]=1;
                }
            }
            Ships.Add(new Ship(dataGridCells));
        }

        void drawUserField()//Перерисовывает поле игрока
        {
            DeadShip(UserField, UserShips,UserDethShip);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (UserField[i, j] == 0)
                    {
                        dataGridView2[i, j].Style.BackColor = Color.White;
                    }
                    if (UserField[i, j] == 1)
                    {
                        dataGridView2[i, j].Style.BackColor = Color.Blue;
                    }
                    if (UserField[i, j] == 3)
                    {
                        dataGridView2[i, j].Style.BackColor = Color.White;
                        dataGridView2[i, j].Value = '#';
                    }
                    if (UserField[i, j] == 2)
                    {
                        dataGridView2[i, j].Style.BackColor = Color.Orange;
                        dataGridView2[i, j].Value = 'X';
                    }

                }
            }
            label14.Text = AiDethShip[3].ToString();
            label15.Text = AiDethShip[2].ToString();
            label16.Text = AiDethShip[1].ToString();
            label17.Text = AiDethShip[0].ToString();
            dataGridView2.CurrentCell = null;

        }

        void drawAiField()//Перерисовывает поле компьютера
        {
            DeadShip(AiField, AiShips,AiDethShip);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (AiField[i,j] == 0)
                    {
                        dataGridView1[i, j].Style.BackColor = Color.Gray;
                    }
                    if(AiField[i,j] == 1)
                    {
                        dataGridView1[i, j].Style.BackColor = Color.Gray;
                    }
                    if (AiField[i, j] == 3)
                    {
                        dataGridView1[i, j].Style.BackColor = Color.White;
                        dataGridView1[i, j].Value = '#';
                    }
                    if (AiField[i, j] == 2)
                    {
                        dataGridView1[i, j].Style.BackColor = Color.Orange;
                        dataGridView1[i, j].Value = 'X';
                    }
                }
            }
            dataGridView1.CurrentCell = null;
            label10.Text = UserDethShip[3].ToString();
            label11.Text = UserDethShip[2].ToString();
            label12.Text = UserDethShip[1].ToString();
            label13.Text = UserDethShip[0].ToString();
        }

        
    }
    public class Ship
    {
        public int Hp { get; set; }
        public List<int[]> Coords { get; set; }
        public Ship(List<int[]> Coordins)
        {
            Hp = Coordins.Count;
            Coords = Coordins;
        }
    }
}
