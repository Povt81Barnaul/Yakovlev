using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceControl
{
    /// <summary>
    /// Класс планета
    /// </summary>
    class Planet : GraphicObject
    {
        //чья планета
        int status;

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        //цвет текста
        Color cl;
        //коордианты текста
        Vector2 text;
        Vector2 text2;
        //объект текст
        SpriteFont sf;
        //строка состояния планеты
        String state;

        //количество кораблей на планете
        int blue_count = 0;

        public int Blue_count
        {
            get { return blue_count; }
            set { blue_count = value; }
        }

        int red_count = 0;

        public int Red_count
        {
            get { return red_count; }
            set { red_count = value; }
        }

        //постройки
        bool MilFact = false; //военный завод

        public bool Milfact
        {
            get { return MilFact; }
            set { MilFact = value; }
        }

        bool ResFact = false; //добывающая фабрика

        public bool Resfact
        {
            get { return ResFact; }
            set { ResFact = value; }
        }

        /// <summary>
        /// Конструктор планеты
        /// </summary>
        /// <param name="tex">текстура</param>
        /// <param name="rec">положение</param>
        /// <param name="sp">шрифт</param>
        /// <param name="stat">статтус планеты (0 - нейтральная, 1 - синяя, 2 - красная)</param>
        public Planet(Texture2D tex, Rectangle rec, SpriteFont sp, int stat) : base(tex, rec)
        {
            text.X = rec.X;
            text.Y = rec.Y - 20;
            text2.X = rec.X;
            text2.Y = rec.Y - 40;

            this.sf = sp;
            this.status = stat;
        }

        /// <summary>
        /// Отрисовка планеты
        /// </summary>
        /// <param name="sb">Объект XNA для отрисовки</param>
        public override void Draw(SpriteBatch sb)
        {
            state = "";
            //установка статуса планеты
            switch (status)
	        {
                case 0: { state = "Empty "; cl = Color.Gray; break; }
                case 1: { state = "UNSC "; cl = Color.DarkBlue; break; }
                case 2: { state = "Covenant "; cl = Color.Red; break; }
		        default: break;
	        }
            //есть ли постройки
            if (MilFact)
                state += "MF ";
            if (ResFact)
                state += "RF ";
            String state2 = "B: " + blue_count;
            state2 += " R: " + red_count;

            base.Draw(sb);
            sb.DrawString(sf,state,text,cl);
            sb.DrawString(sf,state2,text2,cl);
        }
    }
}
