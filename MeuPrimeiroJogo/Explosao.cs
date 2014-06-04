using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace MeuPrimeiroJogo
{
	public class Explosao
	{

		public Texture2D texturaExplosao;
		public Rectangle delimitacaoExplosao;
		public Vector2 posicao, origem;
		public float cronometro, intervalo;
		public int frameAtual, larguraFrame, alturaFrame;
		public bool visivel;

		//Metodo Construtor
		public Explosao (Texture2D texturaExplosao, Vector2 posicao)
		{
			this.texturaExplosao = texturaExplosao;
			this.posicao = posicao;
			cronometro = 0f;
			intervalo = 30f;
			frameAtual = 1;
			larguraFrame = 128;
			alturaFrame = 128;
			visivel = true;
		}

		//Carregar Conteudo
		public void LoadContent(ContentManager Content)
		{
			texturaExplosao = Content.Load<Texture2D> ("");
		}

		//Atualizar
		public void Update(GameTime gameTime)
		{
			//Atualiza em milisegundos o tempo de excecuçao do jogo
			cronometro += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

			//Verificar se o tempo do cronometro e maior do que o do intervalo
			if (cronometro > intervalo) {
				//Mostra o proximo quadro
				frameAtual++;
				//Reiniciar o cronometo
				cronometro = 0f;
			}

			/*			
			 * Se for o ultimo frame,
			 * Mas como e uma explosao temos que torna-la ela invisivel pois,
			 * para que parece uma explosao e preciso que ela acabe.
			 * E tornar o resetar o frame atual para o inicio do spritesheet
			*/
			if (frameAtual == 17) {
				visivel = false;
				frameAtual = 0;
			}

			delimitacaoExplosao = new Rectangle (frameAtual * larguraFrame, 0, alturaFrame, larguraFrame);
			origem = new Vector2 (delimitacaoExplosao.Width / 2, delimitacaoExplosao.Height / 2);

		}

		//Desenhar
		public void Draw(SpriteBatch spriteBatch)
		{
			//Se for visivel, entao pinte
			if (visivel)
				spriteBatch.Draw (texturaExplosao, posicao, delimitacaoExplosao, Color.White, 0f, origem, 1.0f, SpriteEffects.None, 0f);
		}

	}
}

