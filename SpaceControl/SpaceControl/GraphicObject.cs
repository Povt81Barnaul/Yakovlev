using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceControl
{
    /// <summary>
    /// Графический примитив
    /// </summary>
    class GraphicObject
    {
        protected Texture2D texture;
        protected Rectangle rect;

        protected Rectangle global;

        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }


        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public GraphicObject()
        {

        }

        /// <summary>
        /// Конструктор графического примитива
        /// </summary>
        /// <param name="text">Текстура</param>
        /// <param name="rect">положение объекта</param>
        public GraphicObject(Texture2D text, Rectangle rect)
        {
            this.texture = text;
            this.rect = rect;
            this.global.X = rect.X;
            this.global.Y = rect.Y;
        }

        /// <summary>
        /// Обновление координат
        /// </summary>
        /// <param name="x">Новый X</param>
        /// <param name="y">Новый Y</param>
        public void UpdateCoords(int x, int y)
        {
            rect.X = global.X + x;
            rect.Y = global.Y + y;
        }

        /// <summary>
        /// Отрисовка графического примитива
        /// </summary>
        /// <param name="sb">Объект XNA для отрисовки</param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rect, Color.White);
        }
    }
}
