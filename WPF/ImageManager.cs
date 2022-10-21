﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace WPF
{
	public static class ImageManager
	{
		static Dictionary<string, Bitmap> _imageCache = new();

		public static void Initialize()
		{
			_imageCache = new();
		}

		/*
		 * TODO: Fix documentation
		 */
		public static Bitmap GetImage(string url)
		{
			if (_imageCache.ContainsKey(url))
			{
				return _imageCache[url];
			}
			else
			{
				Bitmap bitmap = new Bitmap(url);
				_imageCache.Add(url, bitmap);
				return bitmap;
			}
		}

		/*
		 * Clears Cache.
		 */
		public static void ClearCache() => _imageCache.Clear();

		/*
		 * TODO: Fix documentation
		 */
		public static Bitmap EmptyTrack(int x, int y)
		{
			Bitmap bitmap = new(x, y);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(System.Drawing.Color.White);
			return bitmap;
		}

		/*
		 * Clones a bitmap 
		 */
		public static Bitmap CloneImage(string image)
		{
			Bitmap newBitmap = GetImage(image);
			Bitmap clone = newBitmap.Clone(new Rectangle(0, 0, newBitmap.Width, newBitmap.Height), PixelFormat.Format32bppArgb);
			return clone;
		} 

		/*
		 * Converts Bitmap to a BitmapSource.
		 */
		public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");

			var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

			var bitmapData = bitmap.LockBits(
				rect,
				ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			try
			{
				var size = (rect.Width * rect.Height) * 4;

				return BitmapSource.Create(
					bitmap.Width,
					bitmap.Height,
					bitmap.HorizontalResolution,
					bitmap.VerticalResolution,
					PixelFormats.Bgra32,
					null,
					bitmapData.Scan0,
					size,
					bitmapData.Stride);
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}
	}
}
