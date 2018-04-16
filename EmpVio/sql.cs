using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MetroFramework;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using Transitions;
using System.Windows.Forms.DataVisualization.Charting;

namespace EmpVio
{
    class sql
    {

        //SQL CONNECTION 
        public static SqlCommand sqlcmd = new SqlCommand();
        public static SqlDataAdapter da = new SqlDataAdapter();
        public static SqlConnection sqlcon = new SqlConnection("Data Source = 'KMDI-ACER-E15\\KMDISQLSERVER'; Network Library = 'DBMSSOCN'; Initial Catalog = 'violationdb'; User ID = 'kmdiadmin'; Password='kmdiadmin'");



        MainForm ths;
        //CLASS CONSTRACTOR
        public sql(MainForm frm)
        {
            ths = frm;
        }

        //AUTO ROW NUMBER
        public void autorow(Object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, ths.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
        public static bool IsNumeric(string val)
        {
            int x;
            return int.TryParse(val, out x);
        }






        //LOAD EMPLOYEE'S TABLE
        public void loademployees()
        {
            try
            {
                sql.sqlcon.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                BindingSource BS = new BindingSource();
                string str = "select * from employeetb";
                sqlcmd = new SqlCommand(str, sql.sqlcon);
                da.SelectCommand = sqlcmd;
                da.Fill(ds, "employeetb");
                BS.DataSource = ds;
                BS.DataMember = "employeetb";
                ths.empGRID.DataSource = BS;
                ths.empGRID.Columns["id"].Visible = false;
                loaddepertment();
            }
            catch (SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }

        }
        //load department
        public void loaddepertment()
        {
            try
            {
                BindingSource bs = new BindingSource();
                DataSet ds = new DataSet();
                ds.Clear();
                string str = "select distinct department from employeetb";
                sqlcmd = new SqlCommand(str, sqlcon);
                da.SelectCommand = sqlcmd;
                da.Fill(ds, "employeetb");
                bs.DataSource = ds;
                bs.DataMember = "employeetb";
                ths.department.DataSource = bs;
                ths.department.DisplayMember = "department";
                ths.department.SelectedIndex = -1;
            }
            catch(Exception e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //ADDING NEW EMPLOYEE
        public void addemp()
        {
           try
            {
                sqlcon.Open();

                string find = "select * from employeetb where employee ='" + ths.empname.Text + "'";
                sqlcmd = new SqlCommand(find, sqlcon);
                SqlDataReader read = sqlcmd.ExecuteReader();
                if (read.HasRows == true)
                {
                    read.Close();
                    MetroMessageBox.Show(MainForm.ActiveForm, "Employee already exist! Data not saved", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    read.Close();
                    string str = "declare @id as integer = (select max(id)+1 from employeetb)" +
                   "insert into employeetb (id,employee,department)values(@id,'" + ths.empname.Text + "','" + ths.department.Text + "')";
                    sqlcmd = new SqlCommand(str, sqlcon);
                    sqlcmd.ExecuteNonQuery();
                }
               
            } 
            catch(SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //update employee
        public void updateemp(string str)
        {
            try
            {
                sqlcon.Open();
                sqlcmd = new SqlCommand(str, sqlcon);
                sqlcmd.ExecuteNonQuery(); 
            }
            catch (SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //DELETE EMPLOYEE
        public void deleteemp(string str)
        {
            try
            {
                sqlcon.Open();
                sqlcmd = new SqlCommand(str, sqlcon);
                sqlcmd.ExecuteNonQuery();
            }
            catch(SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
















        //VIOLATION LIST SECTION
        public void loadviolationtb()
        {
            try
            {
                sqlcon.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                BindingSource bs = new BindingSource();
                string str = "select * from violationtb";
                sqlcmd = new SqlCommand(str, sqlcon);
                da.SelectCommand = sqlcmd;
                da.Fill(ds, "violationtb");
                bs.DataSource = ds;
                bs.DataMember = "violationtb";
                ths.violationGrid.DataSource = bs;
                ths.violationGrid.Columns["id"].Visible = false;
            }
            catch(SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //ADD NEW VIOLATION
        public void addviolation()
        {
            try
            {
                sqlcon.Open();

                string find = "select * from violationtb where violation = '" + ths.violation.Text + "'";
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd = new SqlCommand(find, sql.sqlcon);
                SqlDataReader read = sqlcmd.ExecuteReader();
                if (read.HasRows == true)
                {
                    read.Close();
                    MetroMessageBox.Show(MainForm.ActiveForm, "Violation already exist! Data not saved", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    read.Close();
                    string str = "declare @id as integer = (select max(id)+1 from violationtb)" +
                 "insert into violationtb (id,violation,points)" +
                 "values (@id,'" + ths.violation.Text + "','" + ths.points.Text + "')";
                    sqlcmd = new SqlCommand(str, sqlcon);
                    sqlcmd.ExecuteNonQuery();
                }  
            }
            catch(SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //UPDATE VIOLATION
        public void updateviolation(string str)
        {
            try
            {
                sqlcon.Open();
                sqlcmd = new SqlCommand(str, sqlcon);
                sqlcmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //DELETE VIOLATION
        public void deleteviolation(string id)
        {
            try
            {
                sqlcon.Open();
                string str = "delete from violationtb where id = '" + id + "'";
                sqlcmd = new SqlCommand(str, sqlcon);
                sqlcmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
















        //VIOLATORS SECTION
        public void loadviolators()
        {
            try
            {
                sqlcon.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                BindingSource bs = new BindingSource();
                string str = "select a.id,b.EMPLOYEE,c.VIOLATION,a.VDATE,c.POINTS from VIOLATORSTB as a "+
"inner join employeetb as b "+
"on a.empid = b.ID "+
"inner join violationtb as c "+
"on a.vioid = c.ID ";
                sqlcmd = new SqlCommand(str, sqlcon);
                da.SelectCommand = sqlcmd;
                da.Fill(ds, "violatorstb");
                bs.DataSource = ds;
                bs.DataMember = "violatorstb";
                ths.violatorGRID.DataSource = bs;
                ths.violatorGRID.Columns["id"].Visible = false;
            }
            catch (SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //GENERATE VIOLATORS
        public void genviolator()
        {
            try
            {
                sqlcon.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                BindingSource bs = new BindingSource();
                string str = "select employee from employeetb order by employee asc";
                sqlcmd = new SqlCommand(str, sqlcon);
                da.SelectCommand = sqlcmd;
                da.Fill(ds, "employeetb");
                bs.DataSource = ds;
                bs.DataMember = "employeetb";
                ths.selectviolator.DataSource = bs;
                ths.selectviolator.DisplayMember = "employee";
            }
            catch(SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //GENERATE VIOLATIONS
        public void genviolation()
        {
            try
            {
                sqlcon.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                BindingSource bs = new BindingSource();
                string str = "select violation from violationtb order by violation asc";
                sqlcmd = new SqlCommand(str, sqlcon);
                da.SelectCommand = sqlcmd;
                da.Fill(ds, "violationtb");
                bs.DataSource = ds;
                bs.DataMember = "violationtb";
                ths.selectviolation.DataSource = bs;
                ths.selectviolation.DisplayMember = "violation";
            }
            catch(SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //ADD NEW VIOLATORS
        public void addviolators()
        {
            try
            {
                sqlcon.Open();
                string str = "declare @id as integer = (select max(id)+1 from violatorstb)"+
                    "declare @empid as varchar(50) = (select id from employeetb where employee = '" + ths.selectviolator.Text + "')"+
                    "declare @vioid as varchar(50) = (select id from violationtb where violation = '" + ths.selectviolation.Text + "')" +
                    "insert into violatorstb (id,empid,vioid,vdate)" +
                    "values(@id,@empid,@vioid,'" + ths.vdate.Text + "')";
                sqlcmd = new SqlCommand(str, sqlcon);
                sqlcmd.ExecuteNonQuery();
            }
            catch(SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //UPDATE VIOLATORS
        public void updateviolator(string query)
        {
           try
            {
                sqlcon.Open();
                sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        //DELETE VIOLATORS
        public void deleteviolators(string id)
        {
            try
            {
                sqlcon.Open();
                string str = "delete from violatorstb where id = '" + id + "'";
                sqlcmd = new SqlCommand(str, sqlcon);
                sqlcmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }









        //VIOLATION SUMMARY SECTION

//        use VIOLATIONDB
//select c.violation, sum(c.POINTS) from VIOLATORSTB as a
// inner join employeetb as b
// on a.empid = b.ID
// inner join violationtb as c
// on a.vioid = c.ID
// group by c.VIOLATION order by sum(c.POINTS) desc

// select b.EMPLOYEE,sum(c.POINTS) from VIOLATORSTB as a
// inner join employeetb as b
// on a.empid = b.ID
// inner join violationtb as c
// on a.vioid = c.ID
// group by b.employee order by sum(c.POINTS) desc

        public void summarygridcontent(string str)
        {
            try
            {
                sqlcon.Open();
                DataSet ds = new DataSet();
                BindingSource bs = new BindingSource();
                ds.Clear();
                sqlcmd = new SqlCommand(str, sqlcon);
                da.SelectCommand = sqlcmd;
                da.Fill(ds, "violatorstb");
                bs.DataSource = ds;
                bs.DataMember = "violatorstb";
                ths.summaryGrid.DataSource = bs;
                ths.summaryGrid.Columns["points"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        public void usetrans()
        {
            ths.panel4.Location = new Point(910,0);
            Transition t1 = new Transition(new TransitionType_EaseInEaseOut(300));
            t1.add(ths.panel4, "Left", 188);
            t1.run();
            
        }









        //CHARTING
        public  void charting(string query , string xvaluemember,string title)
        {
            try
            {
                sqlcon.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                BindingSource bs = new BindingSource();
                sqlcmd = new SqlCommand(query, sqlcon);
                da.SelectCommand = sqlcmd;
                da.Fill(ds, "VIOLATORSTB");
                bs.DataSource = ds;
                bs.DataMember = "VIOLATORSTB";
                ths.chart1.DataSource = bs;

                ChartArea CArea = ths.chart1.ChartAreas[0];
                CArea.BackColor = Color.Azure;
                CArea.ShadowColor = Color.Red;
                CArea.Area3DStyle.Enable3D = true;

                ths.chart1.Series["Series1"].XValueMember = ""+ xvaluemember +"";
                ths.chart1.Series["Series1"].YValueMembers = "points";

                ths.chart1.Series["Series1"].Font = new System.Drawing.Font("Century Gothic", 11.0F, System.Drawing.FontStyle.Regular);
                ths.chart1.Series["Series1"].YValueType = ChartValueType.Auto;
                ths.chart1.Series["Series1"].ChartType = SeriesChartType.StackedBar;
                ths.chart1.Series["Series1"].IsValueShownAsLabel = true;
                ths.chart1.Series["Series1"].LabelForeColor = Color.Black;
                ths.chart1.Series["Series1"].LabelFormat = "N0";
                ths.chart1.Series["Series1"].LegendText = "Points";

                ths.chart1.Titles.Clear();
                Title T = ths.chart1.Titles.Add("" + title + "");
                T.ForeColor = Color.White;
                T.BackColor = Color.Black;
                T.Font = new System.Drawing.Font("Century Gothic", 15.0F, System.Drawing.FontStyle.Regular);
                T.BorderColor = Color.Black;

    
            }
            catch (SqlException e)
            {
                MetroMessageBox.Show(MainForm.ActiveForm, "" + e + "", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
    }
   
}
