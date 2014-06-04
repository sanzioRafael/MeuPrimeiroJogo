using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeuPrimeiroJogo
{
	public class Inimigo
	{

		public Rectangle delimitacaInimigo;
		public Texture2D inimigoTextura, tiroTextura;
		public Vector2 posicao;
		public int velocidade, velocidadeTiro, delayTiro;
		public bool visivel;
		public List<Tiro> tiros;

		//Metodo construtor
		public Inimigo (Texture2D inimigoTextura, Vector2 posicao, Texture2D tiroTextura)
		{
			tiros = new List<Tiro> ();
			this.inimigoTextura = inimigoTextura;
			this.posicao = posicao;
			this.tiroTextura = tiroTextura;
			delayTiro = 50;
			velocidade = 5;
			velocidadeTiro = 7;
			visivel = true;
		}

		//Atualizar
		public void Update(GameTime gameTime)
		{

			//Atualizar colisoes do retangulo
			delimitacaInimigo = new Rectangle ((int)posicao.X, (int)posicao.Y, inimigoTextura.Width, inimigoTextura.Height);

			//Atualizar a movimentaçao do inimigo
			posicao.Y += velocidade;

			//Movimentar o inimigo para o topo da tela se ele voar fora da parte inferior
			if (posicao.Y >= 750)
				posicao.Y = -75;
			if (posicao.X >= 700 - inimigoTextura.Width)
				posicao.X = 700 - inimigoTextura.Width;

			tiroInimigo ();
			atualizarTiros ();

		}

		//Desenhar
		public void Draw(SpriteBatch spriteBatch)
		{
			//Desenhar a nave do inimigo
			spriteBatch.Draw (inimigoTextura, posicao, Color.White);
			//Desenhar os tiros dos inimigos
			foreach (Tiro t in tiros)
				t.Draw (spriteBatch);
		}

		//Atualizar tiros
		public void atualizarTiros()
		{
			/*			
			 * Para cada tiro na lista(tiros),
			 * atualiza o movimento e se o tiro chegar ao topo da tela,
			 * ele sera removido da lista
			*/
			foreach (Tiro t in tiros) {
				//Delimitaçao dos tiros pentencente da lista de tiros
				t.delimitacaoTiro = new Rectangle ((int)t.posicao.X, (int)t.posicao.Y, t.texturaTiro.Width, t.texturaTiro.Height);
				//Estabelece como a bala ira se movimentar
				t.posicao.Y += velocidadeTiro;
				//Se o tiro sair fora da tela ele se tornara invisivel
				if (t.posicao.Y >= 750)
					t.visivel = false;
			}
			/*						
			 * A interaçao atraves da lista(tiros),
			 * verifica se nao sao mais visiveis,
			 * se forem eles sao removidos da lista e a lista dminui de tamanho
			*/
			for (int i = 0; i < tiros.Count; i++) {
				if (!tiros [i].visivel) {
					tiros.RemoveAt (i);
					i--;
				}
			}
		}

		//Tiro do Inimido
		public void tiroInimigo()
		{
			//Atirar somente se o delay do tiro resetar
			if (delayTiro >= 0)
				delayTiro--;

			if (delayTiro <= 0) {
				//Cria um novo tiro e posiciona na frente do centro da textura do inimigo
				Tiro novoTiro = new Tiro (tiroTextura);
				novoTiro.posicao = new Vector2 (posicao.X + inimigoTextura.Width / 2 - tiroTextura.Width / 2, posicao.Y + 30);
				novoTiro.visivel = true;

				if (tiros.Count < 20)
					tiros.Add (novoTiro);

				//Resetando o delay do tiro
				if (delayTiro == 0)
					delayTiro = 50;

			}
		}

	}
}

