﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using OpenTK;


namespace QuickFont
{
    class TexturePage : IDisposable
    {
        int gLTexID;
        int width;
        int height;

        public int GLTexID { get { return gLTexID; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public TexturePage(string filePath)
        {
            var bitmap = new QBitmap(filePath);
            CreateTexture(bitmap.bitmapData);
            bitmap.Free();
        }

        public TexturePage(BitmapData dataSource)
        {
            CreateTexture(dataSource);
        }

        private void CreateTexture(BitmapData dataSource)
        {
            width = dataSource.Width;
            height = dataSource.Height;

            Helper.SafeGLEnable(EnableCap.Texture2D, () =>
            {
                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

                GL.GenTextures(1, out gLTexID);
                GL.BindTexture(TextureTarget.Texture2D, gLTexID);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, dataSource.Scan0);

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            });
        }

        public void Dispose()
        {
            GL.DeleteTexture(gLTexID);
        }
    }
}
