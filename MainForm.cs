using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace PlatinumSpriteEditor
{
	public partial class MainForm : Form
	{
		System.ComponentModel.IContainer components = null;
		
		CheckBox DPCheck;
		ComboBox IndexBox;
		ComboBox SaveBox;
		Button OpenPngs;
		Button LoadSheet;
		Button LoadNarc;
		Button SaveSingle;
		Button MakeShiny;
		
		NarcReader nr;
		PictureBox[,] Display;
		
		MainMenu mainMenu1;
		MenuItem menuFile;
		MenuItem menuItem10;
		MenuItem menuOpenPng;
		MenuItem menuSavePng;
		MenuItem menuLoadSheet;
		MenuItem menuAbout;
		MenuItem menuOpenNarc;
		MenuItem menuWriteToNarc;
		MenuItem menuOptions;
		MenuItem AutoColor;
		MenuItem AutoConvert;
		MenuItem UseShrinking;

		Label lblMale;
		Label lblFemale;
		Label BackN;
		Label BackS;
		Label FrontN;
		Label FrontS;
		Label lblNormal;
		Label lblShiny;
		
		bool[] used;
		Rectangle rect;
		IndexedBitmapHandler Handler;
		
		SpriteSet CurrentSprites;

		static string[] names = {"Female backsprite", "Male backsprite", "Female frontsprite", "Male frontsprite", "Shiny"};
		
		public MainForm()
		{
			InitializeComponent();
		}

		void MainFormLoad(object sender, EventArgs e)
        {
        }

		void InitializeComponent()
		{
			used = null;
			Handler = new IndexedBitmapHandler();
			CurrentSprites = new SpriteSet();
			BuildPictureBoxes(96, 56);
			
			mainMenu1 = new MainMenu();
			menuFile = new MenuItem();
			menuOpenNarc = new MenuItem();
			menuWriteToNarc = new MenuItem();
			menuOpenPng = new MenuItem();
			menuSavePng = new MenuItem();
			menuLoadSheet = new MenuItem();
			menuItem10 = new MenuItem();
			menuAbout = new MenuItem();
			menuOptions = new MenuItem();
			AutoColor = new MenuItem();
			AutoConvert = new MenuItem();
			UseShrinking = new MenuItem();
			
			AutoColor.Checked = true;
			AutoConvert.Checked = true;

			OpenPngs = new Button();
			LoadSheet = new Button();
			LoadNarc = new Button();
			SaveSingle = new Button();
			MakeShiny = new Button();
			DPCheck = new CheckBox();
			
			lblMale = new Label();
			lblFemale = new Label();
			BackN = new Label();
			BackS = new Label();
			FrontN = new Label();
			FrontS = new Label();
			lblNormal = new Label();
			lblShiny = new Label();
			
			SuspendLayout();

			mainMenu1.MenuItems.AddRange(new MenuItem[3]{menuFile, menuOptions, menuItem10});
			menuFile.Index = 0;
			menuFile.MenuItems.AddRange(new MenuItem[5]{menuOpenNarc, menuWriteToNarc, menuOpenPng, menuSavePng, menuLoadSheet});
			menuFile.Text = "&File";
			menuOpenNarc.Index = 0;
			menuOpenNarc.Text = "&Open narc...";
			menuOpenNarc.Click += (menuItem2_Click);
			menuOpenNarc.Shortcut = Shortcut.CtrlO;
			menuWriteToNarc.Index = 1;
			menuWriteToNarc.Text = "&Write to narc...";
			menuWriteToNarc.Click += (menuItem8_Click);
			menuWriteToNarc.Shortcut = Shortcut.CtrlW;
			menuOpenPng.Index = 2;
			menuOpenPng.Text = "&Load Sprite Set";
			menuOpenPng.Click += (OpenPng_Click);
			menuOpenPng.Shortcut = Shortcut.CtrlL;
			menuSavePng.Index = 3;
			menuSavePng.Text = "&Save Sprite Set";
			menuSavePng.Click += (btnSaveAs_Click);
			menuSavePng.Shortcut = Shortcut.CtrlS;
			menuLoadSheet.Index = 4;
			menuLoadSheet.Text = "Load Spr&ite Sheet";
			menuLoadSheet.Click += (btnLoadSheet_Click);
			menuLoadSheet.Shortcut = Shortcut.CtrlI;
			menuOptions.Index = 1;
			menuOptions.MenuItems.AddRange(new MenuItem[3]{AutoColor, AutoConvert, UseShrinking});
			menuOptions.Text = "Options";
			AutoColor.Index = 0;
			AutoColor.Text = "Fix Non-standard Colors";
			AutoColor.Click += (menuCheck_Click);
			AutoConvert.Index = 1;
			AutoConvert.Text = "Convert Wrong Format Images";
			AutoConvert.Click += (menuCheck_Click);
			UseShrinking.Index = 2;
			UseShrinking.Text = "Allow Shrinking of Expanded Images";
			UseShrinking.Click += (menuCheck_Click);			
			menuItem10.Index = 2;
			menuItem10.MenuItems.AddRange(new MenuItem[1]{menuAbout});
			menuItem10.Text = "Help";
			menuAbout.Index = 0;
			menuAbout.Text = "Credits";
			menuAbout.Click += (menuItem13_Click);
			
			IndexBox = new ComboBox();
			IndexBox.DropDownWidth = 160;
			IndexBox.Location = new Point(340, 8);
			IndexBox.MaxDropDownItems = 16;
			IndexBox.Name = "IndexBox";
			IndexBox.Size = new Size(160, 21);
			IndexBox.TabIndex = 6;
			IndexBox.SelectedIndexChanged += (IndexBox_SelectedIndexChanged);
			
			OpenPngs.Location = new Point(646, 8);
			OpenPngs.Name = "OpenPng";
			OpenPngs.Size = new Size(100, 25);
			OpenPngs.TabIndex = 3;
			OpenPngs.Text = "Load Sprite Set";
			OpenPngs.Click += (OpenPng_Click);

			LoadSheet.Location = new Point(538, 8);
			LoadSheet.Name = "LoadSheet";
			LoadSheet.Size = new Size(100, 25);
			LoadSheet.Text = "Load Sprite Sheet";
			LoadSheet.Click += (btnLoadSheet_Click);
				
			SaveBox = new ComboBox();
			SaveBox.DropDownWidth = 160;
			SaveBox.Location = new Point(512, 728);
			SaveBox.MaxDropDownItems = 8;
			SaveBox.Name = "SaveBox";
			SaveBox.Size = new Size(160, 21);

			SaveBox.Items.Add("Normal Female Backsprite");
			SaveBox.Items.Add("Normal Male Backsprite");
			SaveBox.Items.Add("Normal Female Frontsprite");
			SaveBox.Items.Add("Normal Male Frontprite");
			SaveBox.Items.Add("Shiny Female Backsprite");
			SaveBox.Items.Add("Shiny Male Backsprite");
			SaveBox.Items.Add("Shiny Female Frontsprite");
			SaveBox.Items.Add("Shiny Male Frontprite");
			SaveBox.SelectedIndex = 0;
			
			SaveSingle.Location = new Point(674, 728);
			SaveSingle.Name = "SaveSingle";
			SaveSingle.Size = new Size(70, 21);
			SaveSingle.Text = "Save PNG";
			SaveSingle.Click += (SaveSingle_Click);

			MakeShiny.Location = new Point(96, 728);
			MakeShiny.Name = "MakeShiny";
			MakeShiny.Size = new Size(120, 21);
			MakeShiny.Text = "Create Shiny Palette";
			MakeShiny.Click += (MakeShiny_Click);
			
			LoadNarc.Location = new Point(96, 8);
			LoadNarc.Name = "LoadNarc";
			LoadNarc.Size = new Size(70, 25);
			LoadNarc.TabIndex = 2;
			LoadNarc.Text = "Open narc";
			LoadNarc.Click += (menuItem2_Click);
			
			DPCheck.Location = new Point(170, 8);
			DPCheck.Name = "DPCheck";
			DPCheck.Text = "Diamond/Pearl";
			
			lblFemale.Text = "Female";
			lblFemale.Location = new Point(232, 36);
			Controls.Add(lblFemale);
			lblMale.Text = "Male";
			lblMale.Location = new Point(564, 36);
			Controls.Add(lblMale);
			BackN.Text = "Back";
			BackN.Location = new Point(52, 126);
			Controls.Add(BackN);
			BackS.Text = "Back";
			BackS.Location = new Point(52, 462);
			Controls.Add(BackS);
			FrontN.Text = "Front";
			FrontN.Location = new Point(52, 294);
			Controls.Add(FrontN);
			FrontS.Text = "Front";
			FrontS.Location = new Point(52, 630);
			Controls.Add(FrontS);
			lblNormal.Text = "Normal";
			lblNormal.Location = new Point(8, 210);
			Controls.Add(lblNormal);
			lblShiny.Text = "Shiny";
			lblShiny.Location = new Point(8, 546);
			Controls.Add(lblShiny);

			ResumeLayout(false);

			Menu = mainMenu1;
			Controls.Add(IndexBox);
			Controls.Add(SaveBox);
			Controls.Add(OpenPngs);
			Controls.Add(LoadSheet);
			Controls.Add(LoadNarc);
			Controls.Add(SaveSingle);
			Controls.Add(DPCheck);
			Controls.Add(MakeShiny);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Width = 760;
			Height = 808;
			Text = "Gen IV Sprite Editor";
			Name = "MainForm";
			AutoScaleBaseSize = new Size(5, 13);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			OpenPngs.Enabled = false;
		}
		
		void IndexBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			CurrentSprites = new SpriteSet();
			int num = (IndexBox.Items.IndexOf(IndexBox.Text) * 6);
			for (int i=0; i<4; i++)
			{	
				if (nr.fe[num+i].Size == 6448)
				{
					nr.OpenEntry(num+i);
					CurrentSprites.Sprites[i] = MakeImage(nr.fs);
					nr.Close();
				}
			}
			if (nr.fe[num + 4].Size == 72)
			{
				nr.OpenEntry(num + 4);
				CurrentSprites.Normal = SetPal(nr.fs);
				nr.Close();
			}
			if (nr.fe[num + 5].Size == 72)
			{
				nr.OpenEntry(num + 5);
				CurrentSprites.Shiny = SetPal(nr.fs);
				nr.Close();
			}
			LoadImages();
			OpenPngs.Enabled = true;
		}

		void BuildPictureBoxes(int x_start, int y_start)   
		{
			Display = new PictureBox[2,4];
			for (int i=0; i<Display.GetLength(0); i++)
			{
				for (int j=0; j<Display.GetLength(1); j++)
				{
					Display[i,j] = new PictureBox();
					Display[i,j].Size = new Size(320, 160);
					Display[i,j].Location = new Point((x_start+328*i), (y_start+168*j));
					Display[i,j].Name = "" + (2*j+i);
					Display[i,j].Click += (Picturebox_Click);
					Controls.Add(Display[i,j]);
				}
			}
		}
		
		void LoadImages()
		{
			for (int i=0; i<Display.GetLength(0); i++)
			{
				for (int j=0; j<Display.GetLength(1); j++)
				{
					Display[i,j].Image = null;
				}
			}
			if (CurrentSprites.Normal == null)
				return;
			if (CurrentSprites.Shiny == null)
				CurrentSprites.Shiny = CurrentSprites.Normal;
			Bitmap image = new Bitmap(160, 80, PixelFormat.Format8bppIndexed);
			for (int i=0; i<4; i++)
			{
				if (CurrentSprites.Sprites[i] != null)
				{
					CurrentSprites.Sprites[i].Palette = CurrentSprites.Shiny;
					Display[(i%2),((i/2)+2)].Image = new Bitmap(CurrentSprites.Sprites[i], 320, 160);
					CurrentSprites.Sprites[i].Palette = CurrentSprites.Normal;
					Display[(i%2),(i/2)].Image = new Bitmap(CurrentSprites.Sprites[i], 320, 160);
				}
			}
		}
		
		Bitmap CheckSize(Bitmap image, string filename, string name, int spritenumber = 2)
		{
			DialogResult yesno;
			IndexedBitmapHandler Handler = new IndexedBitmapHandler();
			if (image.PixelFormat != PixelFormat.Format8bppIndexed)
			{
				if (AutoConvert.Checked)
					yesno = DialogResult.Yes;
				else
					yesno = MessageBox.Show(filename + " is not 8bpp Indexed!  Attempt conversion?", "Incompatible image format", MessageBoxButtons.YesNo);
				if(yesno != DialogResult.Yes)
					return null;
				image = Handler.Convert(image, PixelFormat.Format8bppIndexed);
				if (image == null)
					return null;
				if ((image.PixelFormat != PixelFormat.Format8bppIndexed)||(image.Palette == null))
				{
					MessageBox.Show("Conversion failed.", "Failed");
					return null;
				}
			}
			if (((image.Height != 64) && (image.Height != 80)) || ((image.Width != 64) && (image.Width != 80) && (image.Width != 160)))
			{
				int imagescale = 0;
				if (UseShrinking.Checked)
				{
					if ((image.Width/64==image.Height/64) && (image.Width%64==0) && (image.Height%64==0))
						imagescale = image.Width/64;
					if ((image.Width/80==image.Height/80) && (image.Width%80==0) && (image.Height%80==0))
						imagescale = image.Width/80;
					if ((image.Width/160==image.Height/80) && (image.Width%160==0) && (image.Height%80==0))
						imagescale = image.Width/160;
					if (imagescale > 1)
					{
						yesno = MessageBox.Show(filename + " is too large.  Attempt to shrink?", "Too large", MessageBoxButtons.YesNo);
						if(yesno == DialogResult.Yes)
							image = Handler.ShrinkImage(image, imagescale, imagescale);
						else
							imagescale = 0;
					}
				}
				if (imagescale == 0)
				{
					yesno = MessageBox.Show(filename + " size not recognized. Use Canvas Splitter?", "Unrecognized size", MessageBoxButtons.YesNo);
					if(yesno != DialogResult.Yes)
						return null;
					SizeChooser Chooser = new SizeChooser();
					DialogResult success = Chooser.ShowDialog();
					int sizeChoice = Chooser.choice;
					Chooser.Dispose();
					if (success == DialogResult.Cancel)
						return null;
					int a = 80;
					int b = 80;
					if (sizeChoice == 0)
					{
						a = 64;
						b = 64;
					}
					if (sizeChoice == 2)
						a = 160;
					if ((image.Width < a) || (image.Height < b))
					{
						MessageBox.Show("Image is too small");
						return null;
					}
					Bitmap[] tiles = Handler.Split(image, a, b);
					SpriteCropper Cropper = new SpriteCropper(tiles, name);
					success = Cropper.ShowDialog();
					if (success == DialogResult.Cancel)
						return null;
					image = Cropper.Chosen;
					Cropper.Dispose();
				}
			}
			if (AutoColor.Checked)
				image.Palette = StandardizeColors(image);
			byte check = Handler.PaletteSize(image);
			if (check > 16)
			{
				yesno = MessageBox.Show("Image's palette contains more than sixteen colors.  Attempt to shrink?", "Improper palette size", MessageBoxButtons.YesNo);
				if(yesno == DialogResult.Yes)
				{
					image = Handler.ShrinkPalette(image);
					check = Handler.PaletteSize(image);
					if (check > 16)
						MessageBox.Show("Palette still too large.  Image will not save correctly.", "Failed");
				}
			}
			if (image.Height == 64 && image.Width == 64)
				image =	Handler.Resize(image, 8, 8, 8, 8);
			if (image.Height == 80 && image.Width == 80)
			{
				if ((spritenumber < 2) && (DPCheck.Checked == true))
					image = Handler.Resize(image, 0, 0, 0, 80);
				else
					image = Handler.Concat(image, image);
			}
			if (image.Height == 80 && image.Width == 160)
				return image;
			return null;
		}
						
		Bitmap MakeImage(FileStream fs)
		{
			fs.Seek(48L, SeekOrigin.Current);
			BinaryReader binaryReader = new BinaryReader(fs);
			ushort[] array = new ushort[3200];
			for (int i = 0; i < 3200; i++)
			{
				array[i] = binaryReader.ReadUInt16();
			}
			uint num = array[0];
			if (DPCheck.Checked == false)
			{
				for (int j = 0; j < 3200; j++)
				{
					unchecked
					{
						ushort[] array2;
						IntPtr value;
						(array2 = array)[(int)(value = (IntPtr)j)] = (ushort)(array2[(int)value] ^ (ushort)(num & 0xFFFF));
						num *= 1103515245;
						num += 24691;
					}
				}
			}
			else
			{
				num = array[3199];
				for (int num2 = 3199; num2 >= 0; num2--)
				{
					unchecked
					{
						ushort[] array2;
						IntPtr value;
						(array2 = array)[(int)(value = (IntPtr)num2)] = (ushort)(array2[(int)value] ^ (ushort)(num & 0xFFFF));
						num *= 1103515245;
						num += 24691;
					}
				}
			}
			
			Bitmap r_bitmap = new Bitmap(160, 80, PixelFormat.Format8bppIndexed);
			rect = new Rectangle(0, 0, 160, 80);
			byte[] array3 = new byte[12800];
			for (int k = 0; k < 3200; k++)
			{
				array3[k * 4] = (byte)(array[k] & 0xF);
				array3[k * 4 + 1] = (byte)((array[k] >> 4) & 0xF);
				array3[k * 4 + 2] = (byte)((array[k] >> 8) & 0xF);
				array3[k * 4 + 3] = (byte)((array[k] >> 12) & 0xF);
			}
			BitmapData bitmapData = r_bitmap.LockBits(rect, ImageLockMode.WriteOnly, r_bitmap.PixelFormat);
			IntPtr scan = bitmapData.Scan0;
			Marshal.Copy(array3, 0, scan, 12800);
			r_bitmap.UnlockBits(bitmapData);
			Bitmap bitmap = new Bitmap(1, 1, PixelFormat.Format4bppIndexed);
			ColorPalette palette = bitmap.Palette;
			for (int l = 0; l < 16; l++)
			{
				palette.Entries[l] = Color.FromArgb(l << 4, l << 4, l << 4);
			}
			r_bitmap.Palette = palette;
						
			if (r_bitmap == null)
			{
				MessageBox.Show("MakeImage Failed");
				return null;
			}
			return r_bitmap;
		}
		
		ColorPalette SetPal(FileStream fs)
		{
			fs.Seek(40L, SeekOrigin.Current);
			ushort[] array = new ushort[16];
			BinaryReader binaryReader = new BinaryReader(fs);
			for (int i = 0; i < 16; i++)
			{
				array[i] = binaryReader.ReadUInt16();
			}
			Bitmap bitmap = new Bitmap(1, 1, PixelFormat.Format4bppIndexed);
			ColorPalette palette = bitmap.Palette;
			for (int j = 0; j < 16; j++)
			{
				palette.Entries[j] = Color.FromArgb((array[j] & 0x1F) << 3, ((array[j] >> 5) & 0x1F) << 3, ((array[j] >> 10) & 0x1F) << 3);
			}
			return palette;
		}
		
		void menuItem2_Click(object sender, EventArgs e)
		{
			OpenPngs.Enabled = false;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "*.narc|*.narc|*.*|*.*";
			string filter = openFileDialog.Filter;
			openFileDialog.Title = "Open narc file";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				IndexBox.Items.Clear();
				nr = new NarcReader(openFileDialog.FileName);
				for (int i = 0; i < nr.Entrys; i+=6)
				{
					IndexBox.Items.Add("Item" + i + "(" + nr.fe[i].Size + ")");
				}
				IndexBox.SelectedIndex = 1;
			}
		}
		
		void Picturebox_Click(object sender, EventArgs e)
		{
			if (OpenPngs.Enabled == false)
				return;
			OpenPngs.Enabled = false;
			PictureBox source = sender as PictureBox;
			int index = Convert.ToInt32(source.Name);
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Choose an image";
			openFileDialog.CheckPathExists = true;
			openFileDialog.Filter = "Supported fomats: *.bmp, *.gif, *.png | *.bmp; *.gif; *.png";
			openFileDialog.ShowHelp = true;
			Bitmap image;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				OpenPngs.Enabled = true;
				return;
			}
			image = new Bitmap(openFileDialog.FileName);
			IndexedBitmapHandler Handler = new IndexedBitmapHandler();
			if (index > 3)
			{
				image = CheckSize(image, openFileDialog.FileName, "Shiny");
				if (image == null)
				{
					OpenPngs.Enabled = true;
					return;
				}
				ColorPalette temp = Handler.AlternatePalette(CurrentSprites.Sprites[index%4], image);
				if (temp != null)
					CurrentSprites.Shiny = temp;
				else
					CurrentSprites.Shiny = image.Palette;
			}
			else
			{
				image = CheckSize(image, openFileDialog.FileName, names[index], index);
				if (image == null)
				{
					OpenPngs.Enabled = true;
					return;
				}
				bool match = Handler.PaletteEquals(CurrentSprites.Normal, image);
				if (!match)
				{
					DialogResult yesno = MessageBox.Show("Image's palette does not match the current palette.  Use PaletteMatch?", "Palette mismatch", MessageBoxButtons.YesNo);
					if(yesno == DialogResult.Yes)
					{
						image = Handler.PaletteMatch(CurrentSprites.Normal, image, used);
						used = Handler.IsUsed(image, used);
					}
					else
						used = Handler.IsUsed(image);
					CurrentSprites.Normal = image.Palette;
				}
				CurrentSprites.Sprites[index] = image;
			}
			OpenPngs.Enabled = true;
			LoadImages();
		}
		
		void OpenPng_Click(object sender, EventArgs e)
		{
			if (OpenPngs.Enabled == false)
				return;
			OpenPngs.Enabled = false;
			LoadingForm Open = new LoadingForm();
			var result = Open.ShowDialog();
			if (result == DialogResult.Cancel)
			{
				OpenPngs.Enabled = true;
				return;
			}
			string[] filenames = Open.files;
			bool Autofill = Open.result;
			int shinymatch = Open.shinymatch;
			bool paletteMatch = Open.paletteMatch;
			Open.Dispose();
			SpriteSet temp = new SpriteSet();
			Bitmap image;
			for (int i=0; i<4; i++)
			{
				if (filenames[i] == "")
					continue;
				image = new Bitmap(filenames[i]);
				temp.Sprites[i] = CheckSize(image, filenames[i], names[i], i);
			}
			bool[] tempUsed = null;
			if (paletteMatch)
			{
				temp.Normal = CurrentSprites.Normal;
				tempUsed = used;
			}
			for (int i=0; i<4; i++)
			{
				if (temp.Sprites[i] == null)
					continue;
				if (temp.Normal == null)
				{
					temp.Normal = temp.Sprites[i].Palette;
					tempUsed = Handler.IsUsed(temp.Sprites[i]);
				}
				else
				{
					bool match = Handler.PaletteEquals(temp.Normal, temp.Sprites[i]);
					if (!match)
					{
						temp.Sprites[i] = Handler.PaletteMatch(temp.Normal, temp.Sprites[i], tempUsed);
						temp.Normal = temp.Sprites[i].Palette;
					}
					tempUsed = Handler.IsUsed(temp.Sprites[i], tempUsed);
				}
			}
			used = tempUsed;
			if (filenames[4] != "")
			{
				image = new Bitmap(filenames[4]);
				image = CheckSize(image, filenames[4], names[4], 4);
				if ((shinymatch < 4) && (temp.Sprites[shinymatch] != null))
					temp.Shiny = Handler.AlternatePalette(temp.Sprites[shinymatch], image);
				else
					temp.Shiny = image.Palette;
			}
			
			if (Autofill)
			{
				if (temp.Sprites[0] == null)
					temp.Sprites[0] = temp.Sprites[1];
				if (temp.Sprites[1] == null)
					temp.Sprites[1] = temp.Sprites[0];
				if (temp.Sprites[2] == null)
					temp.Sprites[2] = temp.Sprites[3];
				if (temp.Sprites[3] == null)
					temp.Sprites[3] = temp.Sprites[2];
				if (filenames[4] == "")
					temp.Shiny = temp.Normal;
			}

			for (int i=0; i<4; i++)
			{
				if (temp.Sprites[i] != null)
					CurrentSprites.Sprites[i] = temp.Sprites[i];
			}
			if (temp.Normal != null)
				CurrentSprites.Normal = temp.Normal;
			if (temp.Shiny != null)
				CurrentSprites.Shiny = temp.Shiny;
			
			LoadImages();
			OpenPngs.Enabled = true;
		}

		void menuItem8_Click(object sender, EventArgs e)
		{
			if (OpenPngs.Enabled == false)
				return;
			int num = (IndexBox.Items.IndexOf(IndexBox.Text) * 6);
			for (int i=0; i<4; i++)
			{	
				if (nr.fe[num+i].Size == 6448)
				{
					nr.OpenEntry(num+i);
					SaveBin(nr.fs, CurrentSprites.Sprites[i]);
					nr.Close();
				}
			}
			if (nr.fe[num + 4].Size == 72)
			{
				nr.OpenEntry(num + 4);
				SavePal(nr.fs, CurrentSprites.Normal);
				nr.Close();
			}
			if (nr.fe[num + 5].Size == 72)
			{
				nr.OpenEntry(num + 5);
				SavePal(nr.fs, CurrentSprites.Shiny);
				nr.Close();
			}
		}
		
		void menuItem13_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Credit to loadingNOW and SCV for the original PokeDsPic and PokeDsPicPlatinum, without which this would never have happened.", "Credits");
		}
		
		protected void btnSaveAs_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = "Save Image Set";
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.Filter = "*.png|*.png";
			saveFileDialog.ShowHelp = true;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = saveFileDialog.FileName;
				fileName = fileName.Replace(".png", "");
				bool ShinySaved = false;
				if (CurrentSprites.Sprites[2] != null)
				{
					if (CurrentSprites.Shiny != null)
					{
						CurrentSprites.Sprites[2].Palette = CurrentSprites.Shiny;
						SavePNG(CurrentSprites.Sprites[2], (fileName + "Shiny.png"));
						ShinySaved = true;
					}
					CurrentSprites.Sprites[2].Palette = CurrentSprites.Normal;
					SavePNG(CurrentSprites.Sprites[2], (fileName + "FFront.png"));
				}
				if (CurrentSprites.Sprites[3] != null)
				{
					if ((CurrentSprites.Shiny != null) && (!ShinySaved))
					{
						CurrentSprites.Sprites[3].Palette = CurrentSprites.Shiny;
						SavePNG(CurrentSprites.Sprites[3], (fileName + "Shiny.png"));
						ShinySaved = true;
					}
					CurrentSprites.Sprites[3].Palette = CurrentSprites.Normal;
					SavePNG(CurrentSprites.Sprites[3], (fileName + "MFront.png"));
				}
				if (CurrentSprites.Sprites[0] != null)
				{
					if ((CurrentSprites.Shiny != null) && (!ShinySaved))
					{
						CurrentSprites.Sprites[0].Palette = CurrentSprites.Shiny;
						SavePNG(CurrentSprites.Sprites[0], (fileName + "Shiny.png"));
						ShinySaved = true;
					}
					CurrentSprites.Sprites[0].Palette = CurrentSprites.Normal;
					SavePNG(CurrentSprites.Sprites[0], (fileName + "FBack.png"));
				}
				if (CurrentSprites.Sprites[1] != null)
				{
					if ((CurrentSprites.Shiny != null) && (!ShinySaved))
					{
						CurrentSprites.Sprites[1].Palette = CurrentSprites.Shiny;
						SavePNG(CurrentSprites.Sprites[1], (fileName + "Shiny.png"));
					}
					CurrentSprites.Sprites[1].Palette = CurrentSprites.Normal;
					SavePNG(CurrentSprites.Sprites[1], (fileName + "MBack.png"));
				}
			}
		}

		void SaveSingle_Click(object sender, EventArgs e)
		{
			int index = SaveBox.SelectedIndex;
			if (CurrentSprites.Sprites[index % 4] == null)
			{
				MessageBox.Show("Image is empty.");
				return;
			}
			string selected = SaveBox.Text;
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = "Save As PNG";
			saveFileDialog.OverwritePrompt = true;
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.Filter = "*.png|*.png";
			saveFileDialog.ShowHelp = true;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = saveFileDialog.FileName;
				Bitmap image = CurrentSprites.Sprites[index % 4];
				if (index > 3)
					image.Palette = CurrentSprites.Shiny;
				else
					image.Palette = CurrentSprites.Normal;
				SavePNG(image, fileName);
			}
		}
		
		void btnLoadSheet_Click(object sender, EventArgs e)
		{
			if (OpenPngs.Enabled == false)
				return;
			OpenPngs.Enabled = false;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Select a sprite sheet";
			openFileDialog.CheckPathExists = true;
			openFileDialog.Filter = "Supported fomats: *.bmp, *.gif, *.png | *.bmp; *.gif; *.png";
			openFileDialog.ShowHelp = true;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				OpenPngs.Enabled = true;
				return;
			}
			Bitmap image = new Bitmap(openFileDialog.FileName);
			if ((image.Width != 256) || (image.Height != 64))
			{
				MessageBox.Show("The sprite sheet should be 256x64.");
				return;
			}
			IndexedBitmapHandler Handler = new IndexedBitmapHandler();
			image = Handler.Convert(image, PixelFormat.Format8bppIndexed);
			image.Palette = StandardizeColors(image);
			Bitmap[] tiles = Handler.Split(image, 64, 64);
			SpriteSet sprites = new SpriteSet();
			bool[] used = Handler.IsUsed(tiles[0]);
			used = Handler.IsUsed(tiles[2], used);
			Bitmap temp = Handler.ShrinkPalette(tiles[0], used);
			sprites.Normal = temp.Palette;
			temp =	Handler.Resize(temp, 8, 8, 8, 8);
			temp = Handler.Concat(temp, temp);
			sprites.Sprites[2] = temp;
			sprites.Sprites[3] = temp;
			temp = Handler.ShrinkPalette(tiles[2], used);
			temp =	Handler.Resize(temp, 8, 8, 8, 8);
			if (DPCheck.Checked)
				temp = Handler.Resize(temp, 0, 0, 0, 80);
			else
				temp = Handler.Concat(temp, temp);
			sprites.Sprites[0] = temp;
			sprites.Sprites[1] = temp;
			temp = Handler.ShrinkPalette(tiles[1], used);
			temp =	Handler.Resize(temp, 8, 8, 8, 8);
			temp = Handler.Concat(temp, temp);
			sprites.Shiny = Handler.AlternatePalette(sprites.Sprites[2], temp);
			CurrentSprites = sprites;
			OpenPngs.Enabled = true;
			LoadImages();
		}
		
		void MakeShiny_Click(object sender, EventArgs e)
		{
			if (OpenPngs.Enabled == false)
				return;
			OpenPngs.Enabled = false;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Choose the base image";
			openFileDialog.CheckPathExists = true;
			openFileDialog.Filter = "Supported fomats: *.bmp, *.gif, *.png | *.bmp; *.gif; *.png";
			openFileDialog.ShowHelp = true;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				OpenPngs.Enabled = true;
				return;
			}
			string filename = openFileDialog.FileName;
			openFileDialog.Title = "Choose the shiny image";
			openFileDialog.CheckPathExists = true;
			openFileDialog.Filter = "Supported fomats: *.bmp, *.gif, *.png | *.bmp; *.gif; *.png";
			openFileDialog.ShowHelp = true;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				OpenPngs.Enabled = true;
				return;
			}
			Bitmap parent = new Bitmap(filename);
			Bitmap child = new Bitmap(openFileDialog.FileName);
			IndexedBitmapHandler Handler = new IndexedBitmapHandler();
			ColorPalette temp = Handler.AlternatePalette(parent, child);
			if (temp != null)
				CurrentSprites.Shiny = temp;
			else
				MessageBox.Show("Failed!", "Failed");
			OpenPngs.Enabled = true;
			LoadImages();
		}
		
		void menuCheck_Click(object sender, EventArgs e)
		{
			MenuItem item = sender as MenuItem;
			item.Checked = !item.Checked;
		}
		
		ColorPalette StandardizeColors(Bitmap image)
		{
			ColorPalette pal = image.Palette;
			bool OffColor = false;
			for (int i=0; i<pal.Entries.Length; i++)
			{
				if ((pal.Entries[i].R % 8 != 0) || (pal.Entries[i].G % 8 != 0) || (pal.Entries[i].B % 8 != 0))
					OffColor = true;
			}
			if (OffColor)
			{
//				yesno = MessageBox.Show("Colors are not appropriately formatted for storage.  Fix?", "Incompatible colors", MessageBoxButtons.YesNo);
//				if(yesno != DialogResult.Yes)
//					MessageBox.Show("Colors will not store correctly.  Image may look different in-game.", "Failed");
				for (int i=0; i<pal.Entries.Length; i++)
				{
					byte r = (byte)(pal.Entries[i].R - (pal.Entries[i].R % 8));
					byte g = (byte)(pal.Entries[i].G - (pal.Entries[i].G % 8));
					byte b = (byte)(pal.Entries[i].B - (pal.Entries[i].B % 8));
					pal.Entries[i] = Color.FromArgb(r, g, b);
				}
			}
			return pal;
		}
		
		void SavePNG(Bitmap image, string filename)
		{
			IndexedBitmapHandler Handler = new IndexedBitmapHandler();
			byte[] array = Handler.GetArray(image);
			Bitmap temp = Handler.MakeImage(image.Width, image.Height, array, image.PixelFormat);
			ColorPalette cleaned = Handler.CleanPalette(image);
			temp.Palette = cleaned;
			temp.Save(filename, ImageFormat.Png);
		}
				
		void SaveBin(FileStream fs, Bitmap source)
		{
			BinaryWriter binaryWriter = new BinaryWriter(fs);
			rect = new Rectangle(0, 0, 160, 80);
			BitmapData bitmapData = source.LockBits(rect, ImageLockMode.ReadOnly, source.PixelFormat);
			IntPtr scan = bitmapData.Scan0;
			byte[] array = new byte[12800];
			Marshal.Copy(scan, array, 0, 12800);
			source.UnlockBits(bitmapData);
			ushort[] array2 = new ushort[3200];
			for (int i = 0; i < 3200; i++)
			{
				array2[i] = (ushort)((array[i * 4] & 0xF) | ((array[i * 4 + 1] & 0xF) << 4) | ((array[i * 4 + 2] & 0xF) << 8) | ((array[i * 4 + 3] & 0xF) << 12));
			}
			uint num = 0u;
			if (DPCheck.Checked == false)
			{
				for (int j = 0; j < 3200; j++)
				{
					unchecked
					{
						ushort[] array3;
						IntPtr value;
						(array3 = array2)[(int)(value = (IntPtr)j)] = (ushort)(array3[(int)value] ^ (ushort)(num & 0xFFFF));
						num *= 1103515245;
						num += 24691;
					}
				}
			}
			else
			{
				num = 31315u;
				for (int num2 = 3199; num2 >= 0; num2--)
				{
					num += array2[num2];
				}
				for (int num3 = 3199; num3 >= 0; num3--)
				{
					unchecked
					{
						ushort[] array3;
						IntPtr value;
						(array3 = array2)[(int)(value = (IntPtr)num3)] = (ushort)(array3[(int)value] ^ (ushort)(num & 0xFFFF));
						num *= 1103515245;
						num += 24691;
					}
				}
			}
			byte[] array4 = new byte[48]
			{82, 71, 67, 78, 255, 254, 0, 1, 48, 25, 0, 0, 16, 0, 1, 0, 82, 65, 72, 67, 32, 25, 0, 0, 10, 0, 20, 0, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 25, 0, 0, 24, 0, 0, 0};
			for (int k = 0; k < 48; k++)
			{
				binaryWriter.Write(array4[k]);
			}
			for (int l = 0; l < 3200; l++)
			{
				binaryWriter.Write(array2[l]);
			}
		}

		void SavePal(FileStream fs, ColorPalette palette)
		{
			byte[] buffer = new byte[40]
			{82, 76, 67, 78, 255, 254, 0, 1, 72, 0, 0, 0, 16, 0, 1, 0, 84, 84, 76, 80, 56, 0, 0, 0, 4, 0, 10, 0, 0, 0, 0, 0, 32, 0, 0, 0, 16, 0, 0, 0};
			BinaryWriter binaryWriter = new BinaryWriter(fs);
			binaryWriter.Write(buffer, 0, 40);
			ushort[] array = new ushort[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = (ushort)(((palette.Entries[i].R >> 3) & 0x1F) | (((palette.Entries[i].G >> 3) & 0x1F) << 5) | (((palette.Entries[i].B >> 3) & 0x1F) << 10));
			}
			for (int j = 0; j < 16; j++)
			{
				binaryWriter.Write(array[j]);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
	}
}
