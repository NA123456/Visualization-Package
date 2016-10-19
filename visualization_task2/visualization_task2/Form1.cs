using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.OpenGl;
using VisPack.Geometry;
using Visualization;

namespace visualization_task2
{
    public partial class Form1 : Form
    {
        public Mesh M = null;
        public Color color = Color.FromArgb(0, 0, 0);

        double xMin = 99999.0;
        double xMax = -0.99999;

        double yMin = 99999.0;
        double yMax = -99999;

        double zMin = 99999.0;
        double zMax = -99999;

        double xCenter;
        double yCenter;
        double zCenter;
        int index;
        float Adata = 0;
        float Bdata = 0;
        float Edge_Val = 0;
        float Face_Val = 0;

        double min = 0;
        double max = 0;
        double value = 0;

        public Form1()
        {
            InitializeComponent();
            initgraphics();
        }

        public void initgraphics()
        {
            simpleOpenGlControl1.InitializeContexts();
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(60, this.Width / this.Height, 0.0, 100.0);
            //Glu.gluPerspective(45, 500 / 300, 0.0, 60.0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        public void Mesh_Draw(Mesh Mmesh)
        {
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            foreach (Zone z in Mmesh.Zones)
            {
                foreach (Face f in z.Faces)
                {
                    if (checkBox1.Checked)
                        Gl.glBegin(Gl.GL_POLYGON);
                    else
                        Gl.glBegin(Gl.GL_LINES);
                    foreach (uint ee in f.Edges)
                    {
                        Edge _edge = z.Edges[ee];
                        Gl.glColor3d(0, 0.5, 1);

                        ///////////////////////////////////////////////
                        if (z.Vertices[_edge.Start].Position.x > xMax)
                            xMax = z.Vertices[_edge.Start].Position.x;

                        if (z.Vertices[_edge.Start].Position.x < xMin)
                            xMin = z.Vertices[_edge.Start].Position.x;

                        if (z.Vertices[_edge.Start].Position.y > yMax)
                            yMax = z.Vertices[_edge.Start].Position.y;

                        if (z.Vertices[_edge.Start].Position.y < yMin)
                            yMin = z.Vertices[_edge.Start].Position.y;

                        if (z.Vertices[_edge.Start].Position.z > zMax)
                            zMax = z.Vertices[_edge.Start].Position.y;

                        if (z.Vertices[_edge.Start].Position.z < zMin)
                            zMin = z.Vertices[_edge.Start].Position.z;
                        //////////////////////////////////////////////

                        xCenter = (xMax + xMin) / 2;
                        yCenter = (yMax + yMin) / 2;
                        zCenter = (zMax + zMin) / 2;

                        /////////////////////////////

                        if (checkBox2.Checked)
                        {
                            Mmesh.GetMinMaxValues((uint)index, out min, out max);

                            Adata = (float)z.Vertices[_edge.Start].Data[index];
                            Bdata = (float)z.Vertices[_edge.End].Data[index];

                            Edge_Val = (Adata + Bdata) / 2;

                            color = valueColor1.discrete_value_color(Edge_Val, (float)min, (float)max);
                            Gl.glColor3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);

                        }// end if

                        // ////////////////////////////////////////////////
                        z.Vertices[_edge.Start].Position.glTell();
                        z.Vertices[_edge.End].Position.glTell();
                    }// end loop edges
                    Gl.glEnd();
                    Gl.glFlush();
                }// end loop faces
            }//end loop zones


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "LoadFile";

            checkBox1.Text = "Filled";
            checkBox1.CheckState = CheckState.Unchecked;
            checkBox2.CheckState = CheckState.Unchecked;
            checkBox3.CheckState = CheckState.Unchecked;
            checkBox2.Text = "Edge Coding";
            checkBox3.Text = "Face Coding";
            checkBox4.Text = "Contour Line";
            checkBox5.Text = "Flooded Contour";
            checkBox6.Text = "Iso Surface";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path;
            DialogResult dr = openFileDialog1.ShowDialog();
            path = openFileDialog1.FileName;
            M = new Mesh(path);
            //if (M != null)
            //{

            Mesh_Draw(M);
            Gl.glTranslated(-xCenter, -yCenter, -zCenter);

            //}
            ////////////////////////////////////////////

            //-----------///-------------///--------///----------//
            listBox1.Items.Clear();

            int count = 0;
            int NumOfValues = 0;
            NumOfValues = M.VarToIndex.Count;

            foreach (string str in M.VarToIndex.Keys)
            {
                NumOfValues = int.Parse(M.VarToIndex[str].ToString());
                listBox1.Items.Add(str);
                count++;
            }
            listBox1.SelectedItem = 0;
            //--------///-----------///-----------///------------//
            simpleOpenGlControl1.Invalidate();

        }


        float y1 = 0.0f;
        float y2 = 0.0f;
        float x1 = 0.0f;
        float x2 = 0.0f;
        float s1 = 0.0f;
        float s2 = 0.0f;
        float RX1 = 0;
        float RX2 = 0;
        float A = 0.0f;
        //float z = 0.0f;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Down:
                    {
                        y1 += 0.05f;
                        Gl.glTranslatef(0.0f, -y1, 0.0f);
                        simpleOpenGlControl1.Refresh();
                        break;
                    }
                case Keys.Up:
                    {
                        y2 += 0.05f;
                        Gl.glTranslatef(0.0f, y2, 0.0f);
                        simpleOpenGlControl1.Refresh();
                        break;
                    }
                case Keys.Right:
                    {
                        x1 += 0.05f;
                        Gl.glTranslatef(x1, 0.0f, 0.0f);
                        simpleOpenGlControl1.Refresh();
                        break;
                    }
                case Keys.Left:
                    {
                        x2 += 0.05f;
                        Gl.glTranslatef(-x2, 0.0f, 0.0f);
                        simpleOpenGlControl1.Refresh();
                        break;
                    }
                //zoom in
                case Keys.W:
                    {
                        s1 += 1.0f;
                        Gl.glTranslated(0, 0, s1);
                        simpleOpenGlControl1.Invalidate();
                        break;
                    }
                //zoom out
                case Keys.S:
                    {
                        s2 -= 1.0f;
                        Gl.glTranslated(0, 0, s2);
                        simpleOpenGlControl1.Invalidate();
                        break;
                    }

                case Keys.E:
                    {
                        A += 10;
                        Gl.glTranslatef((float)xCenter, (float)yCenter, (float)zCenter);
                        Gl.glRotatef(A, 0.0f, 1.0f, 0.0f);
                        Gl.glTranslatef((float)-xCenter, (float)-yCenter, -(float)zCenter);
                        simpleOpenGlControl1.Refresh();
                        break;
                    }

                case Keys.D:
                    {
                        A += 10;
                        Gl.glTranslatef((float)xCenter, (float)yCenter, (float)zCenter);
                        Gl.glRotatef(-A, 0.0f, 1.0f, 0.0f);
                        Gl.glTranslatef((float)-xCenter, (float)-yCenter, -(float)zCenter);
                        simpleOpenGlControl1.Refresh();
                        break;
                    }

                case Keys.R:
                    {
                        RX1 += 10;
                        Gl.glTranslatef((float)xCenter, (float)yCenter, (float)zCenter);
                        Gl.glRotatef(RX1, 1.0f, 0.0f, 0.0f);
                        Gl.glTranslatef((float)-xCenter, (float)-yCenter, -(float)zCenter);
                        simpleOpenGlControl1.Refresh();
                        break;
                    }

                case Keys.F:
                    {
                        RX2 += 10;
                        Gl.glTranslatef((float)xCenter, (float)yCenter, (float)zCenter);
                        Gl.glRotatef(-RX2, 1.0f, 0.0f, 0.0f);
                        Gl.glTranslatef((float)-xCenter, (float)-yCenter, -(float)zCenter);
                        simpleOpenGlControl1.Refresh();
                        break;
                    }
                default:
                    break;
            }
            Invalidate();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            simpleOpenGlControl1.Invalidate();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            if (M != null)
                Mesh_Draw(M);

            if (checkBox4.Checked)
                Contoure();
            if (checkBox3.Checked)
                faceColoring();
            if (checkBox5.Checked)
                FloodedContour();
            if (checkBox6.Checked)
                IsoSurface();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            simpleOpenGlControl1.Invalidate();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            simpleOpenGlControl1.Invalidate();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString() != "")
            {
                string str2 = listBox1.SelectedItem.ToString();
                index = int.Parse(M.VarToIndex[str2].ToString());
                M.GetMinMaxValues((uint)index, out min, out max);

            }
        }

        private void faceColoring()
        {
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            foreach (Zone z in M.Zones)
            {
                foreach (Face f in z.Faces)
                {
                    int count = 0;
                    double sum = 0;
                    count = f.Edges.Count();
                    foreach (uint ee in f.Edges)
                    {
                        Edge _edge = z.Edges[ee];
                        sum = sum + z.Vertices[_edge.Start].Data[index];
                    }
                    Face_Val = (float)sum / count;

                    color = valueColor1.Continuous_value_color(Face_Val, (float)min, (float)max);
                    Gl.glColor3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);

                    Gl.glBegin(Gl.GL_POLYGON);
                    foreach (uint ee in f.Edges)
                    {
                        Edge _edge = z.Edges[ee];
                        z.Vertices[_edge.Start].Position.glTell();
                        z.Vertices[_edge.End].Position.glTell();
                    }
                    Gl.glEnd();
                    Gl.glFlush();
                }
            }
        }

        private List<Vertex> sorting(List<Vertex> ListtoSort)
        {
            if (ListtoSort.Count > 3)
            {
                List<Vertex> Above = new List<Vertex>();
                List<Vertex> Bellow = new List<Vertex>();
                Vertex Min = new Vertex();
                Vertex Max = new Vertex();

                Min.Position.x = 99999999999.0;
                Max.Position.x = -0.999999999;
                /************************************************/
                foreach (Vertex MM in ListtoSort)
                {
                    if (MM.Position.x < Min.Position.x)
                        Min = MM;
                    else if (MM.Position.x == Min.Position.x)
                    {
                        if (MM.Position.y < Min.Position.y)
                            Min = MM;
                    }
                    if (MM.Position.x > Max.Position.x)
                        Max = MM;
                    else if (MM.Position.x == Max.Position.x)
                    {
                        if (MM.Position.y > Max.Position.y)
                            Max = MM;
                    }
                }

                /************************************************/

                // y = mx + c
                double m = (Max.Position.y - Min.Position.y) / (Max.Position.x - Min.Position.x);
                double c1 = Min.Position.y - (m * Min.Position.x);
                /************************************************/

                foreach (Vertex MM in ListtoSort)
                {
                    double y = (m * MM.Position.x) + c1;
                    if (MM != Min && MM != Max)
                    {
                        if (y > MM.Position.y)
                            Bellow.Add(MM);
                        else if (y < MM.Position.y)
                            Above.Add(MM);
                    }

                }
                /************************************************/
                for (int a = 0; a < Above.Count - 1; a++)
                {
                    for (int b = a + 1; b < Above.Count; b++)
                    {
                        if (Above[a].Position.x < Above[b].Position.x)
                        {
                            Vertex tmp = Above[a];
                            Above[a] = Above[b];
                            Above[b] = tmp;
                        }
                    }
                }
                /************************************************/

                for (int a = 0; a < Bellow.Count - 1; a++)
                {
                    for (int b = a + 1; b < Bellow.Count; b++)
                    {
                        if (Bellow[a].Position.x > Bellow[b].Position.x)
                        {
                            Vertex tmp = Bellow[a];
                            Bellow[a] = Bellow[b];
                            Bellow[b] = tmp;
                        }
                    }
                }
                /************************************************/
                List<Vertex> MinX = new List<Vertex>() { Min };
                List<Vertex> MaxX = new List<Vertex>() { Max };
                ListtoSort = new List<Vertex>(MinX.Concat(Bellow).Concat(MaxX).Concat(Above));

            }
            return ListtoSort;
        }

        private List<Vertex> FilterList(List<Vertex> ListToFilter)
        {
            for (int a = 0; a < ListToFilter.Count - 1; a++)
            {
                for (int b = a + 1; b < ListToFilter.Count; b++)
                {
                    if (ListToFilter[a].Position.x == ListToFilter[b].Position.x && ListToFilter[a].Position.y == ListToFilter[b].Position.y)
                    {
                        ListToFilter.RemoveAt(b);
                        b--;
                    }
                }
            }
            return ListToFilter;
        }
        List<Vertex> vertex = new List<Vertex>();
        Vertex V = new Vertex();

        Vertex V1 = new Vertex();
        Vertex V2 = new Vertex();

        private void Contoure()
        {
            double Alfa = 0;
            double xPosition = 0;
            double yPosition = 0;
            double zPosition = 0;

            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            value = (max - min) / 10;
            for (double i = (value / 2) + min; i < max; i += value)
            {
                foreach (Zone z in M.Zones)
                {
                    foreach (Face f in z.Faces)
                    {
                        Gl.glBegin(Gl.GL_LINES);
                        //foreach (uint ee in f.edges)
                        //{
                        //    edge _edge = z.edges[ee];
                        //    gl.glcolor3d(0, 0.5, 1);

                        //    int size = f.edges.count();
                        //    sum = sum + z.vertices[_edge.start].data[index];

                        //    z.vertices[_edge.start].position.gltell();
                        //    z.vertices[_edge.end].position.gltell();

                        //}
                        foreach (uint ee in f.Edges)
                        {
                            Edge _edge = z.Edges[ee];
                            Gl.glColor3d(0, 0.5, 1);

                            //int size = f.Edges.Count();
                            //if (size == 4)
                            //{
                            //    double avg = sum / 4;
                            //    if (avg < i)
                            //    {
                            //        if (z.Vertices[_edge.Start].Data[index] < i)
                            //            z.Vertices[_edge.Start].Position.glTell();

                            //    }
                            //    else
                            //    {
                            //        if (z.Vertices[_edge.Start].Data[index] > i)
                            //            z.Vertices[_edge.Start].Position.glTell();
                            //    }

                            //} 
                            /////////////////////////////////////////////////////////
                            if ((i >= z.Vertices[_edge.Start].Data[index] && i <= z.Vertices[_edge.End].Data[index]) ||
                                (i <= z.Vertices[_edge.Start].Data[index] && i >= z.Vertices[_edge.End].Data[index]))
                            {
                                Alfa = (i - z.Vertices[_edge.Start].Data[index]) /
                                    (z.Vertices[_edge.End].Data[index] - z.Vertices[_edge.Start].Data[index]);
                                xPosition = Alfa * (z.Vertices[_edge.End].Position.x - z.Vertices[_edge.Start].Position.x) +
                                   z.Vertices[_edge.Start].Position.x;
                                yPosition = Alfa * (z.Vertices[_edge.End].Position.y - z.Vertices[_edge.Start].Position.y) +
                                    z.Vertices[_edge.Start].Position.y;
                                zPosition = Alfa * (z.Vertices[_edge.End].Position.z - z.Vertices[_edge.Start].Position.z) +
                                    z.Vertices[_edge.Start].Position.z;

                                V.Position.x = xPosition;
                                V.Position.y = yPosition;
                                V.Position.z = zPosition;
                                vertex.Add(V);
                                V = new Vertex();
                            }

                            ////////////////////////////////////////////////////////
                            z.Vertices[_edge.Start].Position.glTell();
                            z.Vertices[_edge.End].Position.glTell();

                        }

                        //////////////////////////////////////////////////////////////////////////////
                        Gl.glBegin(Gl.GL_LINES);
                        foreach (Vertex v in vertex)
                        {
                            color = valueColor1.Continuous_value_color((float)i, (float)min, (float)max);
                            Gl.glColor3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);
                            v.Position.glTell();
                        }
                        ////////////////////////////////////////////////////////////////////////////////

                        vertex = new List<Vertex>();

                        Gl.glEnd();
                        Gl.glFlush();
                    }
                }
            }
        }
        private void FloodedContour()
        {
            double Alfa = 0;
            double xPosition = 0;
            double yPosition = 0;
            double zPosition = 0;

            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            value = (max - min) / 10;

            foreach (Zone z in M.Zones)
            {
                foreach (Face f in z.Faces)
                {
                    bool firstContoure = false;
                    for (double i = (value / 2) + min; i < max; i += value)
                    {
                        List<Vertex> L1 = new List<Vertex>();
                        List<Vertex> L2 = new List<Vertex>();
                        List<Vertex> L3 = new List<Vertex>();
                        List<Vertex> L4 = new List<Vertex>();
                        foreach (uint ee in f.Edges)
                        {
                            Edge _edge = z.Edges[ee];

                            if ((i >= z.Vertices[_edge.Start].Data[index] && i <= z.Vertices[_edge.End].Data[index]) ||
                                (i <= z.Vertices[_edge.Start].Data[index] && i >= z.Vertices[_edge.End].Data[index]))
                            {
                                Alfa = (i - z.Vertices[_edge.Start].Data[index]) /
                                    (z.Vertices[_edge.End].Data[index] - z.Vertices[_edge.Start].Data[index]);
                                xPosition = Alfa * (z.Vertices[_edge.End].Position.x - z.Vertices[_edge.Start].Position.x) +
                                   z.Vertices[_edge.Start].Position.x;
                                yPosition = Alfa * (z.Vertices[_edge.End].Position.y - z.Vertices[_edge.Start].Position.y) +
                                    z.Vertices[_edge.Start].Position.y;
                                zPosition = Alfa * (z.Vertices[_edge.End].Position.z - z.Vertices[_edge.Start].Position.z) +
                                    z.Vertices[_edge.Start].Position.z;

                                V.Position.x = xPosition;
                                V.Position.y = yPosition;
                                V.Position.z = zPosition;
                                vertex.Add(V);
                                V = new Vertex();
                                if (z.Vertices[_edge.Start].Data[index] <= i && z.Vertices[_edge.End].Data[index] > i)
                                {
                                    if (z.Vertices[_edge.End].Data[index] == i)
                                    {
                                        L1.Add(z.Vertices[_edge.Start]);
                                        L1.Add(z.Vertices[_edge.End]);
                                    }
                                    else
                                    {
                                        L1.Add(z.Vertices[_edge.Start]);
                                        L2.Add(z.Vertices[_edge.End]);
                                    }
                                }
                                else if (z.Vertices[_edge.Start].Data[index] > i && z.Vertices[_edge.End].Data[index] <= i)
                                {
                                    if (z.Vertices[_edge.Start].Data[index] == i)
                                    {
                                        L1.Add(z.Vertices[_edge.Start]);
                                        L1.Add(z.Vertices[_edge.End]);
                                    }
                                    else
                                    {
                                        L2.Add(z.Vertices[_edge.Start]);
                                        L1.Add(z.Vertices[_edge.End]);
                                    }
                                }
                            }
                            else if (z.Vertices[_edge.End].Data[index] > i && z.Vertices[_edge.End].Data[index] < (i + value)
                                && z.Vertices[_edge.Start].Data[index] > i && z.Vertices[_edge.Start].Data[index] < (i + value))
                            {
                                L3.Add(z.Vertices[_edge.Start]);
                                L3.Add(z.Vertices[_edge.End]);
                            }
                            else if (i == (value / 2) + min)
                            {
                                if (z.Vertices[_edge.End].Data[index] > (i - value) && z.Vertices[_edge.End].Data[index] < i
                                   && z.Vertices[_edge.Start].Data[index] > (i - value) && z.Vertices[_edge.Start].Data[index] < i)
                                {
                                    L4.Add(z.Vertices[_edge.Start]);
                                    L4.Add(z.Vertices[_edge.End]);
                                }
                            }
                            ////////////////////////////////////////////////////////


                        }
                        Gl.glEnd();
                        Gl.glFlush();
                        foreach (Vertex intersection in vertex)
                        {
                            L1.Add(intersection);
                            L2.Add(intersection);
                        }

                        L1 = new List<Vertex>(FilterList(L1));
                        L2 = new List<Vertex>(FilterList(L2));
                        L3 = new List<Vertex>(FilterList(L3));
                        L4 = new List<Vertex>(FilterList(L4));

                        L1 = new List<Vertex>(sorting(L1));
                        L2 = new List<Vertex>(sorting(L2));
                        L3 = new List<Vertex>(sorting(L3));
                        L4 = new List<Vertex>(sorting(L4));


                        ///////////////////////////////////////////////////////////////////////////////// 
                        if (firstContoure == false && L1.Count > 2)
                        {
                            color = new Color();
                            color = valueColor1.Continuous_value_color((float)i, (float)min, (float)max);
                            Gl.glBegin(Gl.GL_POLYGON);
                            foreach (Vertex List1 in L1)
                            {
                                Gl.glColor3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);
                                List1.Position.glTell();
                            }
                            Gl.glEnd();
                            Gl.glFlush();
                            firstContoure = true;
                        }
                        ///////////////////////////////////////////////////////////////////////////////

                        if ((i + value) > max)
                        {
                            color = new Color();
                            color = valueColor1.Continuous_value_color((float)max, (float)min, (float)max);
                        }
                        else
                        {
                            color = new Color();
                            color = valueColor1.Continuous_value_color((float)(i + value), (float)min, (float)max);
                        }
                        Gl.glBegin(Gl.GL_POLYGON);
                        foreach (Vertex List2 in L2)
                        {
                            Gl.glColor3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);
                            List2.Position.glTell();
                        }
                        Gl.glEnd();
                        Gl.glFlush();
                        ///////////////////////////////////////////////////////////////////////////////////
                        if ((i + value) > max)
                        {
                            color = new Color();
                            color = valueColor1.Continuous_value_color((float)max, (float)min, (float)max);
                        }
                        else
                        {
                            color = new Color();
                            color = valueColor1.Continuous_value_color((float)(i + value), (float)min, (float)max);
                        }
                        Gl.glBegin(Gl.GL_POLYGON);
                        foreach (Vertex List3 in L3)
                        {
                            Gl.glColor3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);
                            List3.Position.glTell();
                        }
                        Gl.glEnd();
                        Gl.glFlush();
                        ///////////////////////////////////////////////////////////////////////////////////////
                        color = new Color();
                        color = valueColor1.Continuous_value_color((float)i, (float)min, (float)max);
                        Gl.glBegin(Gl.GL_POLYGON);
                        foreach (Vertex List4 in L4)
                        {
                            Gl.glColor3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);
                            List4.Position.glTell();
                        }
                        Gl.glEnd();
                        Gl.glFlush();
                        vertex = new List<Vertex>();


                    }
                }
            }
        }

        private void IsoSurface()
        {
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            value = double.Parse(textBox2.Text.ToString());
            // value = (max - min) / 1000;
            foreach (Zone z in M.Zones)
            {
                foreach (Face f in z.Faces)
                {
                    Gl.glBegin(Gl.GL_LINES);
                    foreach (uint ee in f.Edges)
                    {
                        Edge _edge = z.Edges[ee];
                        Gl.glColor3d(0, 0.5, 1);

                        z.Vertices[_edge.Start].Position.glTell();
                        z.Vertices[_edge.End].Position.glTell();
                    }// end loop edges

                }//end loop faces
                Gl.glEnd();
                Gl.glFlush();
                foreach (Element e in z.Elements)
                {
                    vertex = new List<Vertex>();
                    double[] vertexData = new double[8];
                    int counter = 0;

                    foreach (uint i in e.vertInOrder)
                    {
                        vertexData[counter] = z.Vertices[i].Data[index];
                        counter++;
                    }//end loop of vertInOrder

                    byte isoCase = 0;
                    int[] triTableCase = new int[16];

                    isoCase = ISOSurface.GetElementCase(vertexData, value);
                    for (int t = 0; t < 16; t++)
                        triTableCase[t] = ISOSurface.triTable[isoCase, t];

                    double Alfa = 0;
                    double xPosition = 0;
                    double yPosition = 0;
                    double zPosition = 0;
                    foreach (int tt in triTableCase)
                    {
                        if (tt != -1)
                        {
                            Edge edge = ISOSurface.GetEdgePoints[tt];
                            double startEdgeData = z.Vertices[e.vertInOrder[edge.Start]].Data[index];
                            double EndEdgeData = z.Vertices[e.vertInOrder[edge.End]].Data[index];

                            if ((value > startEdgeData && value <= EndEdgeData) ||
                                  (value <= startEdgeData && value > EndEdgeData))
                            {
                                Alfa = (value - startEdgeData) / (EndEdgeData - startEdgeData);
                                xPosition = Alfa * (z.Vertices[e.vertInOrder[edge.End]].Position.x - z.Vertices[e.vertInOrder[edge.Start]].Position.x) +
                                   z.Vertices[e.vertInOrder[edge.Start]].Position.x;
                                yPosition = Alfa * (z.Vertices[e.vertInOrder[edge.End]].Position.y - z.Vertices[e.vertInOrder[edge.Start]].Position.y) +
                                    z.Vertices[e.vertInOrder[edge.Start]].Position.y;
                                zPosition = Alfa * (z.Vertices[e.vertInOrder[edge.End]].Position.z - z.Vertices[e.vertInOrder[edge.Start]].Position.z) +
                                    z.Vertices[e.vertInOrder[edge.Start]].Position.z;

                                V.Position.x = xPosition;
                                V.Position.y = yPosition;
                                V.Position.z = zPosition;
                                vertex.Add(V);
                                V = new Vertex();
                            }//end if
                        }// end if

                    }//end tritable loop

                    Gl.glBegin(Gl.GL_TRIANGLE_STRIP);

                    for (int c = 0; c < vertex.Count - 2; c += 1)
                    {
                        color = new Color();
                        color = valueColor1.Continuous_value_color((float)value, (float)min, (float)max);
                        Gl.glColor3d(color.R / 255.0, color.G / 255.0, color.B / 255.0);
                        vertex[c].Position.glTell();
                        vertex[c + 1].Position.glTell();
                        vertex[c + 2].Position.glTell();
                    }
                    Gl.glEnd();
                    Gl.glFlush();

                }//end loop on elements 

            }//end loop on zones


        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            simpleOpenGlControl1.Invalidate();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            simpleOpenGlControl1.Invalidate();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            simpleOpenGlControl1.Invalidate();
        }



    }

}