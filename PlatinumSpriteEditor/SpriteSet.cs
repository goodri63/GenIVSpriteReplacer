using System;
using System.Drawing;
using System.Drawing.Imaging;

public class SpriteSet
{
	public Bitmap[] Sprites;
	public ColorPalette Normal;
	public ColorPalette Shiny;
	
	public SpriteSet()
	{
		Sprites = new Bitmap[4];
		for (int i=0; i<4; i++)
		{
			Sprites[i]=null;
		}
		Normal = null;
		Shiny = null;
	}
}
