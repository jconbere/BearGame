﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    class GameView
    {

        SpriteBatch _uiBatch;
        SpriteFont _uiFont;

        World _world;

        Texture2D _backgroundTexture;
        Texture2D _meterTexture;
        Texture2D _skullTexture;
        Texture2D _happyTexture;
        Texture2D _healthbarTexture;

        Texture2D _personhealthyTexture;
        Texture2D _personhurtTexture;
        Texture2D _personhurtbadTexture;
        Texture2D _persondeadTexture;

        Texture2D _uiitemhoneyTexture;
        Texture2D _uiitemtrikeTexture;

        Texture2D _uicontextTexture;
        Texture2D _uispacebarTexture;

        Texture2D _uibearcuteTexture;
        Texture2D _uibearnormTexture;
        Texture2D _uibearangryTexture;

        Texture2D _vignetteTexture;

        public GameView(World world)
        {
            
            _world = world;
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            _uiBatch = new SpriteBatch(device);

            //Vignette
            _vignetteTexture = content.Load<Texture2D>("UI\\Vignette");

            //Load health bar Textures
            //_meterTexture = content.Load<Texture2D>("UI\\healthmeter");
            _healthbarTexture = content.Load<Texture2D>("UI\\healthbar");
            //_skullTexture = content.Load<Texture2D>("UI\\skull");
            //_happyTexture = content.Load<Texture2D>("UI\\happy");
            
            _backgroundTexture = content.Load<Texture2D>("UI\\UI_Background");

            //Load Villager Health Textures
            _personhealthyTexture = content.Load<Texture2D>("UI\\facesHealthy");
            _personhurtTexture = content.Load<Texture2D>("UI\\facesMedium");
            _personhurtbadTexture = content.Load<Texture2D>("UI\\facesBad");
            _persondeadTexture = content.Load<Texture2D>("UI\\facesDead");

            //Load Fonts
#if IOS
#else
            _uiFont = content.Load<SpriteFont>("UI\\UIFont");
#endif

            //Load UI_Items
            _uiitemhoneyTexture = content.Load<Texture2D>("UI\\Honey");
            _uiitemtrikeTexture = content.Load<Texture2D>("UI\\Trike");

            //Load UI_Context
            _uicontextTexture = content.Load<Texture2D>("UI\\contextsheet");
            _uispacebarTexture = content.Load<Texture2D>("UI\\UI_SPACE");

            //Load UI_BearState
            _uibearcuteTexture = content.Load<Texture2D>("UI\\UI_BearCute");
            _uibearangryTexture = content.Load<Texture2D>("UI\\UI_BearAngry");
            _uibearnormTexture = content.Load<Texture2D>("UI\\UI_BearNorm");

            //Achievement
            DaredevilAchievement.DaredevilTexture = content.Load<Texture2D>("Achievements\\Ach_DareDevil");
            JerkAchievement.JerkTexture = content.Load<Texture2D>("Achievements\\Ach_Jerk");
        }

        public void Update(GameTime time)
        {
        }

        public void Draw()
        {
            //const int peopleLegendTextSpacing = 20;
            var vw = 800;
            var vh = 600;
            var ws = 400;

            
            var gameViewLeft = (vw-ws)/2;
            var gameViewTop = (vh-ws)/2;
            
            //Health bar Logic
            float healthPercentage = (_world.Bear.Health / _world.Settings.Bear_HealthMax);
            Color barColor = Color.Red;
            if (_world.Bear.IsHugging)
            {
                barColor = Color.Green;
            }
            
            _uiBatch.Begin();
            _uiBatch.Draw(_healthbarTexture, new Rectangle(205, 0, (int)(400 * healthPercentage), 200), Color.White);
            
            _uiBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.White);
            //Draw Health Meter
            /*_uiBatch.Draw(_skullTexture, new Rectangle(gameViewLeft, gameViewTop - 90, _skullTexture.Width, _skullTexture.Height), Color.White);
            _uiBatch.Draw(_meterTexture, new Rectangle(gameViewLeft + _skullTexture.Width, gameViewTop - 90, (int)(_meterTexture.Width * healthPercentage), _meterTexture.Height), barColor);    
            _uiBatch.Draw(_healthbarTexture, new Rectangle(gameViewLeft + _skullTexture.Width, gameViewTop - 90, _healthbarTexture.Width, _healthbarTexture.Height), Color.White);
            _uiBatch.Draw(_happyTexture, new Rectangle(gameViewLeft + _healthbarTexture.Width + _skullTexture.Width, gameViewTop - 90, _happyTexture.Width, _happyTexture.Height), Color.White);
            */
            //Draw People Legend
            //_uiBatch.DrawString(_uiFont, "Legend", new Vector2(650, 150), Color.White);
            int yPeople = 130;
            int xPeople = 620;
            Color fontColor = Color.White;
            foreach (Villager person in _world.AllVillagers)
            {
                Texture2D healthTexture = _personhealthyTexture;
                switch (person.Health)
                {
                    case (3):
                        healthTexture = _personhealthyTexture;
                        fontColor = Color.Green;
                        break;
                    case (2):
                        healthTexture = _personhurtTexture;
                        fontColor = Color.Orange;
                        break;
                    case (1):
                        healthTexture = _personhurtbadTexture;
                        fontColor = Color.Red;
                        break;
                    case (0):
                        healthTexture = _persondeadTexture;
                        fontColor = Color.DarkRed;
                        break;

                }
                _uiBatch.Draw(healthTexture, new Rectangle(xPeople, yPeople + (70 * (_world.AllVillagers.IndexOf(person))), 64, 64), Color.White);
				if (_uiFont != null) {
	                _uiBatch.DrawString(_uiFont, person.Name.ToString(), new Vector2(xPeople + 70, yPeople + 20 + (70 * (_world.AllVillagers.IndexOf(person)))), fontColor);
				}
            }

            //Draw Inventory Item
            if (_world.Bear.Inventory != null)
            {
                Texture2D itemTexture = _uiitemhoneyTexture;
                if (_world.Bear.Inventory is Honey)
                {
                    itemTexture = _uiitemhoneyTexture;
                }
                else if (_world.Bear.Inventory is Tricycle)
                {
                    itemTexture = _uiitemtrikeTexture;
                }

                _uiBatch.Draw(itemTexture, new Rectangle(50, 400, 120, 120), Color.White);
            }

            //Draw Context
            if (_world.Bear.PossibleInteraction != null)
            {
                int leftAlign = 265;
                _uiBatch.Draw(_uispacebarTexture, new Rectangle(leftAlign, 520, _uispacebarTexture.Width, _uispacebarTexture.Height), Color.White);
                _uiBatch.Draw(_uicontextTexture, new Vector2(leftAlign + 233, 525), new Rectangle(_world.Bear.PossibleInteraction.spritesheetIndex * 50, 0, 50, 50), Color.White);
                
            }

            //Draw Bear World State
            float worldLove = _world._loveSystem.AverageLove;
            Texture2D textureLove = _uibearnormTexture;
            if (worldLove < 2.0)
            {
                textureLove = _uibearangryTexture;
            }
            else if (2.0 >= worldLove && worldLove < 3.5)
            {
                textureLove = _uibearnormTexture;
            }
            else if (worldLove >= 3.5)
            {
                textureLove = _uibearcuteTexture;
            }

            _uiBatch.Draw(textureLove, new Rectangle(0, 0, textureLove.Width, textureLove.Height), Color.White);

            var worldRect = new Rectangle(gameViewLeft, gameViewTop, ws, ws);

            _uiBatch.End();


            _world.Draw(worldRect);

            _uiBatch.Begin();

            _uiBatch.Draw(_vignetteTexture, worldRect, Color.White);

            foreach (var a in _world.Bear.Achievements)
            {
                a.Draw(_uiBatch);
            }

            _uiBatch.End();
        }
    }
}
