using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MeuPrimeiroJogo
{
	public class Tiro
	{
		public Texture2D texturaTiro;
		public Rectangle delimitacaoTiro;
		public Vector2 posicao, origem;
		public bool visivel;
		public float velocidade;

		//Metodo Construtor
		public Tiro (Texture2D texturaTiro)
		{
			this.texturaTiro = texturaTiro;
			velocidade = 10;
			visivel = false;
		}

		//Desenhar
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (texturaTiro, posicao, Color.White);
		}
	}
}

