using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

namespace SpaceControl
{
    /// <summary>
    /// Класс корабль
    /// </summary>
    class Ship
    {
        //целостность корпуса
        int live;

        public int Live
        {
            get { return live; }
            set { live = value; }
        }

        //на орбите?
        bool in_orbit;

        private object thisLock = new object();

        public bool In_orbit
        {
            get { return in_orbit; }
            set { in_orbit = value; }
        }

        //связанная планета
        Planet Connected_planet;

        internal Planet Connected_Planet
        {
            get { return Connected_planet; }
            set { Connected_planet = value; }
        }

        //статус корабля (true - синий, false - красный)
        bool state;

        public bool State
        {
            get { return state; }
            set { state = value; }
        }

        //сила атаки
        int attack;

        //начальное положение на орбите
        float t;

        //отображение корабля
        SpriteFont sf;

        //координаты корабля
        Vector2 coords;

        //цвет корабля
        Color cl;

        Random rn;

        bool first = true;

        float delta_x;
        float delta_y;
        
        /// <summary>
        /// конструктор корабля
        /// </summary>
        /// <param name="sf">шрифт для отображения корабля</param>
        /// <param name="rect">положение в пространстве</param>
        /// <param name="state">true - синий, false - красный</param>
        /// <param name="connected">планета вокруг которой вращается корабль</param>
        /// <param name="t">начальное положение корабля на орбите</param>
        public Ship(SpriteFont sf, Rectangle rect, bool state, Planet connected, float t)
        {
            live = 100;
            in_orbit = true;
            this.state = state;
            Connected_planet = connected;
            if (state == true)
            {
                Con_planet(null).Blue_count += 1;
            }
            else
            {
                Con_planet(null).Red_count += 1;
            }
            this.t = t;
            attack = 2;

            rn = new Random();

            this.sf = sf;
            coords.X = rect.X;
            coords.Y = rect.Y;

            Thread td = new Thread(Work);
            td.IsBackground = true;
            td.Start();
        }

        /// <summary>
        /// основоной рабочий цикл
        /// </summary>
        public void Work()
        {

            while(live > 0)
            {
                //если корабль находится на орбите
                if (in_orbit)
                {
                    if (t >= 6.283f)
                        t = 0.0f;

                    coords.X = Con_planet(null).Rect.X + 45 + 70 * (float)Math.Cos(t);
                    coords.Y = Con_planet(null).Rect.Y + 45 + 70 * (float)Math.Sin(t);

                    //coords.X = Connected_planet.Rect.X + 45 + 70 * (float)Math.Cos(t);
                    //coords.Y = Connected_planet.Rect.Y + 45 + 70 * (float)Math.Sin(t);

                    t += 0.01f;
                }
                else
                {
                    if (first)
                    {
                        delta_x = (float)Con_planet(null).Rect.X - coords.X;
                        delta_y = (float)Con_planet(null).Rect.Y - coords.Y;
                        //delta_x = (float)Connected_planet.Rect.X - coords.X;
                        //delta_y = (float)Connected_planet.Rect.Y - coords.Y;
                        delta_x = delta_x / 100.0f;
                        delta_y = delta_y / 100.0f;

                        first = false;
                    }

                    coords.X += delta_x;
                    coords.Y += delta_y;

                    //уже долетели
                    if (Math.Abs(coords.X - Con_planet(null).Rect.X) < 50 && Math.Abs(coords.Y - Con_planet(null).Rect.Y) < 50)
                    {
                        in_orbit = true;
                        first = true;
                    }
                }

                //битва
                if (state == true)
                {
                    live -= attack * Con_planet(null).Red_count;
                    if (live <= 0) Con_planet(null).Blue_count -= 1;
                }
                else
                {
                    live -= attack * Con_planet(null).Blue_count;
                    if (live <= 0) Con_planet(null).Red_count -= 1;
                }
                Thread.Sleep(20);
            }

        }

        /// <summary>
        /// блокировка
        /// </summary>
        /// <param name="pl">Изменяемая планета</param>
        /// <returns></returns>
        public Planet Con_planet(Planet pl)
        {
            lock(thisLock)
            {
                if (pl != null)
                {
                    this.Connected_Planet = pl;

                    
                }
                return this.Connected_Planet;
            }
        }

        /// <summary>
        /// отрисовка корабля
        /// </summary>
        /// <param name="sb">объект XNA для рисованя</param>
        public void Draw(SpriteBatch sb)
        {
            //чей корабль?
            if (state)
                cl = Color.DarkBlue;
            else
                cl = Color.Red;

            //живой или нет?
            if(live > 0)
            {
                 sb.DrawString(sf,"+" + live,coords, cl);
            }
            else
                sb.DrawString(sf, "X", coords, cl);
        }

        

    }
}
