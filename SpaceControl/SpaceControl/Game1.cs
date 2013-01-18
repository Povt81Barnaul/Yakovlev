using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Collections.Generic;
using System;

namespace SpaceControl
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //ограничение по количеству кораблей
        const int MAX_SHIPS = 20;

        int money_blue;
        int money_red;

        /// <summary>
        /// Задаем глобальные координаты
        /// </summary>
        int global_x = 0;
        int global_y = 0;

        /// <summary>
        /// координаты камеры
        /// </summary>
        int cam_x = 0;
        int cam_y = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //список планет
        List<Planet> planets = new List<Planet>();

        //список кораблей
        List<Ship> ships = new List<Ship>();

        //изображение курсора
        GraphicObject cursor;
        
        //основной шрифт
        SpriteFont sf;
        //отображение планет
        SpriteFont ship;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = true;
            Window.Title = "Space Control";

            //ресурсы на момент начала игры
            money_blue = money_red = 1500;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sf = Content.Load<SpriteFont>("font");
            ship = Content.Load<SpriteFont>("ship");

            // TODO: use this.Content to load your game content here

            //создаем планеты
            planets.Add(new Planet(Content.Load<Texture2D>("Earth"),new Rectangle(300,300,100,100),sf,1));
            planets.Add(new Planet(Content.Load<Texture2D>("Mars"), new Rectangle(30, 65, 100, 100),sf,2));
            planets.Add(new Planet(Content.Load<Texture2D>("Mercury"), new Rectangle(80, 400, 100, 100),sf,0));
            planets.Add(new Planet(Content.Load<Texture2D>("Venus"), new Rectangle(600, 500, 100, 100),sf,0));
            planets.Add(new Planet(Content.Load<Texture2D>("Uranus"), new Rectangle(500, 300, 100, 100),sf,0));

            //загрузим курсор
            cursor = new GraphicObject(Content.Load<Texture2D>("curs"), new Rectangle(0, 0, 20, 20));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            MouseState ms = Mouse.GetState();

            cursor.UpdateCoords(ms.X, ms.Y);

            foreach (var item in planets)
            {
                item.UpdateCoords(cam_x,cam_y);
            }

            //работа с планетами
            Check_Planets();
            //работа с кораблями
            Check_Ships();

            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.Escape))
                this.Exit();
            /*
            if (keys.IsKeyDown(Keys.W))
                cam_y -= 10;
            if (keys.IsKeyDown(Keys.A))
                cam_x -= 10;
            if (keys.IsKeyDown(Keys.S))
                cam_y += 10;
            if (keys.IsKeyDown(Keys.D))
                cam_x += 10;
            */

            base.Update(gameTime);
        }

        /// <summary>
        /// Метод создания кораблей
        /// </summary>
        void CreateShips()
        {
            Random rn = new Random();
            for (int i = 0; i < 10; i++)
            {
                ships.Add(new Ship(ship, new Rectangle(rn.Next(0, 400), rn.Next(0, 400), 10, 10),true,planets[rn.Next(0,2)],(float)(rn.NextDouble() * 6)));
            }
        }

        /// <summary>
        /// Проверяем статус планеты
        /// </summary>
        void Check_Planets()
        {
            Random rn = new Random();

            for(int i = 0; i < planets.Count; i++)
            {
                //захват планеты
                if(planets[i].Blue_count == 0 && planets[i].Red_count > 0)
                {
                    planets[i].Status = 2;
                }
                else if (planets[i].Blue_count > 0 && planets[i].Red_count == 0)
                {
                    planets[i].Status = 1;
                }

                //если планета не пустая
                if (planets[i].Status != 0)
                {
                    if (planets[i].Status == 1)
                    {
                        if (money_blue >= 500)
                        {
                            //строим добывающий завод
                            if (planets[i].Resfact == false && rn.Next(0, 200) < 1)
                            {
                                money_blue -= 500;
                                planets[i].Resfact = true;
                                break;
                            }
                            //строим военный завод
                            if (planets[i].Milfact == false && rn.Next(0, 200) < 1)
                            {
                                money_blue -= 500;
                                planets[i].Milfact = true;
                                break;
                            }
                        }
                        //добываем ресурсы
                        if (planets[i].Resfact && rn.Next(0,200) < 1)
                        {
                            money_blue += 10;
                            break;
                        }

                        //строим корабли
                        if (money_blue > 50 && rn.Next(0,200) < 1 && planets[i].Milfact)
                        {
                            if (ships.Count < MAX_SHIPS)
                            {
                                ships.Add(new Ship(ship, new Rectangle(0, 0, 10, 10), true, planets[i], (float)(rn.NextDouble() * 6)));
                            }
                            money_blue -= 50;
                            break;
                        }
                    }
                    else
                    {
                        if (money_red >= 500)
                        {
                            //строим добывающий завод
                            if (planets[i].Resfact == false && rn.Next(0, 200) < 1)
                            {
                                money_red -= 500;
                                planets[i].Resfact = true;
                                break;
                            }
                            //строим военный завод
                            if (planets[i].Milfact == false && rn.Next(0, 200) < 1)
                            {
                                money_red -= 500;
                                planets[i].Milfact = true;
                                break;
                            }
                        }
                        //добываем ресурсы
                        if (planets[i].Resfact && rn.Next(0, 200) < 1)
                        {
                            money_red += 10;
                            break;
                        }
                        //строим корабли
                        if (money_red > 50 && rn.Next(0, 200) < 1 && planets[i].Milfact && ships.Count < MAX_SHIPS)
                        {
                            ships.Add(new Ship(ship, new Rectangle(0, 0, 10, 10), false, planets[i], (float)(rn.NextDouble() * 6)));
                            money_red -= 50;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Проверяем статус корабля
        /// </summary>
        public void Check_Ships()
        {
            Random rn = new Random();

            for (int i = 0; i < ships.Count; i++ )
            {
                if (ships[i].Live > 0 && rn.Next(0, 500) < 1)
                {
                    Planet n_plan = planets[rn.Next(0, planets.Count)];
                    if(!ships[i].Connected_Planet.Equals(n_plan))
                    {
                        //изменили счётчик кораблей на планете
                        if (ships[i].State == true)
                        {
                            n_plan.Blue_count += 1;
                            ships[i].Con_planet(null).Blue_count -= 1;
                            //ships[i].ChangeOrbit();
                        }
                        else
                        {
                            n_plan.Red_count += 1;
                            ships[i].Con_planet(null).Red_count -= 1;
                            //ships[i].ChangeOrbit();
                        }

                        ships[i].Con_planet(n_plan);


                    }
                    
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);


            // TODO: Add your drawing code here
            spriteBatch.Begin();

            

            //отобразим планеты
            foreach (var item in planets)
            {
                item.Draw(spriteBatch);
            }

            //отобразим корабли
            foreach (var item in ships)
            {
                item.Draw(spriteBatch);
            }

            //отобразим строку состояния
            spriteBatch.DrawString(sf, "Resources UNSC: " + money_blue, Vector2.Zero, Color.BlueViolet);
            spriteBatch.DrawString(sf, "Resources Covenant: " + money_red, new Vector2(0, 15), Color.Red);

            //отобразим курсор
            cursor.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
