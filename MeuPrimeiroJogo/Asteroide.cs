using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MeuPrimeiroJogo
{
	public class Asteroide
	{

		public Texture2D texturaAsteroide;
		public Rectangle delimitacaAsteroide;
		public Vector2 posicao, origem;
		public float anguloRotacao;
		public int velocidade;
		public bool visivel;
		private Random random = new Random();
		public float randX, randY;

		//Metodo Construtor
		public Asteroide (Texture2D texturaAsteroide, Vector2 posicao)
		{
			this.posicao = posicao;
			this.texturaAsteroide = texturaAsteroide;
			velocidade = 3;

			randX = random.Next (0, 700);
			randY = random.Next (-400, -50);

			if (posicao.X >= 700 - texturaAsteroide.Width)
				posicao.X = 700 - texturaAsteroide.Width;

			visivel = true;

			origem.X = texturaAsteroide.Width / 2;
			origem.Y = texturaAsteroide.Height / 2;
		}

		//Atualizar
		public void Update(GameTime gameTime)
		{
			//Estabelecer a delimitaçao para as colisoes
			delimitacaAsteroide = new Rectangle ((int)posicao.X, (int)posicao.Y, texturaAsteroide.Width, texturaAsteroide.Height);

			//Atualizar a movimentao
			posicao.Y = posicao.Y + velocidade;

			if (posicao.X <= 0)
				posicao.X = 0;
			if (posicao.X >= 700 - texturaAsteroide.Width)
				posicao.X = 700 - texturaAsteroide.Width;

			if (posicao.Y >= 750)
				posicao.Y = -50;

			//Rotaçao do asteroid
			float transicao = (float)gameTime.ElapsedGameTime.TotalSeconds;
			anguloRotacao += transicao;
			float circulo = MathHelper.Pi * 2;	
			anguloRotacao %= circulo;

		}

		//Desenhar
		public void Draw(SpriteBatch spriteBatch)
		{
			if (visivel)
				//spriteBatch.Draw (texturaAsteroide, posicao, Color.White);
				spriteBatch.Draw (texturaAsteroide, posicao, null, Color.White, anguloRotacao, origem, 1.0f, SpriteEffects.None, 0f);
		}

	}
}

