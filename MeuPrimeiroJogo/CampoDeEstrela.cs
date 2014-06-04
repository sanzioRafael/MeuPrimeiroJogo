using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MeuPrimeiroJogo
{
	public class CampoDeEstrela
	{
		public Texture2D texturaFundo;
		public Vector2 posicaoFundo1, posicaoFundo2;
		public int velocidade;

		//Metodo construtor
		public CampoDeEstrela ()
		{
			texturaFundo = null;
			posicaoFundo1 = new Vector2 (0, 0);
			posicaoFundo2 = new Vector2 (0, -750);
			velocidade = 2;
		}

		//Carregar conteudo
		public void LoadContent(ContentManager Content)
		{
			texturaFundo = Content.Load<Texture2D> ("space");
		}

		//Atualizar
		public void Update(GameTime gameTime)
		{
			//Configuraçao da velocidade do movimento do fundo
			posicaoFundo1.Y += velocidade;
			posicaoFundo2.Y += velocidade;

			//Movimento do fundo (Repetiçao do fundo)
			if (posicaoFundo1.Y >= 750) {
				posicaoFundo1.Y = 0;
				posicaoFundo2.Y = -750;
			} 
		}

		//Desenhar
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (texturaFundo, posicaoFundo1, Color.White);
			spriteBatch.Draw (texturaFundo, posicaoFundo2, Color.White);
		}
	}
}

