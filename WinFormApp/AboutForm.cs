using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormApp
{
    public partial class AboutForm : Form
    {
        private Label label1 = null;

        public AboutForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.DarkGray;
            this.ClientSize = new Size(540, 520);            
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Text = "About";

            label1 = new Label();
            label1.AutoSize = true;
            label1.Font = new Font( "Microsoft Sans Serif",14.25F, 
                FontStyle.Bold, GraphicsUnit.Point,((byte)(0)));
            label1.Location = new Point(11, 21);
            label1.Dock = DockStyle.Fill;
            label1.AutoSize = false;
            label1.Name = "label1";
            label1.TabIndex = 1;
            label1.Text = "about...";
            label1.ForeColor = Color.White;
            label1.BackColor = Color.Transparent; // da se vidi slika ispod

            //label1.BackgroundImage = Image.FromFile(@"c:\RapidTDD_update\RapidAbout\300pixels.png ");

            var asm = Assembly.GetExecutingAssembly(); 
            using (var stream = asm.GetManifestResourceStream("WinFormApp.Resources.About.png")) 
            {     
                label1.BackgroundImage = Image.FromStream(stream); 
            }  

            label1.BackgroundImageLayout = ImageLayout.Stretch;

            this.Controls.Add(label1);

            this.Load += new EventHandler(this.AboutForm_Load);
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            label1.Text =
                "\n"
                +"                  Rapid TDD application .NET 9\n\n"
                + "     Compile, run c# code in memory and execute tests"
                + "\n\n\n\n\n\n\n\n\n\n\n\n\n\n"
                + "           Copyleft © 2022-2026 by Darko Bondjerovic\n\n\n"
                + $"                        Version: {version} \n\n\n";
        }
    }
}
