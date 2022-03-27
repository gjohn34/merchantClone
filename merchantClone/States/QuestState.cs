using merchantClone.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace merchantClone.States
{
    public class QuestState : State
    {
        private Quest _quest;
        public QuestState(Game1 game, GraphicsDevice graphics, ContentManager content, Quest quest) : base(game, graphics, content)
        {
            _quest = quest;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new System.NotImplementedException();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}