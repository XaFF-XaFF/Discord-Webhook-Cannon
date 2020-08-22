﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discord_Webhook_Cannon
{
    public partial class Mainwindow : Form
    {

        int MouPosX = 0, MouPosY = 0;
        bool mouseDown = false;

        public Mainwindow()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = Screen.AllScreens[0].WorkingArea.Location;
        }



        #region labels
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/Aries52");
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/XaFF-XaFF");
        }
        #endregion



        #region panel
        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            MouPosX = e.X;
            MouPosY = e.Y;
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.SetDesktopLocation(MousePosition.X - MouPosX, MousePosition.Y - MouPosY);
            }
        }

        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        #endregion



        #region buttons
        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TXT Text|*.txt";
            ofd.ShowDialog();
            textBox2.Text = ofd.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string webhook = textBox1.Text;
            string proxyList = textBox2.Text;
            string threads = trackBar1.Value.ToString();
            string message = messageTxtBox.Text;

            if (string.IsNullOrEmpty(webhook) || string.IsNullOrEmpty(proxyList) || string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Invalid options", "Error", MessageBoxButtons.OK);
            }
            else
            {
                Cannon cannon = new Cannon();
                cannon.StartThreads(webhook, proxyList, threads, message);
            }
        }

        private void AppExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion 
    }
}
