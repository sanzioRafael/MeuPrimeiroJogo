using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeuPrimeiroJogo
{
	public class Jogador
	{

		public Texture2D texturaJogador, texturaTiro, texturaVida;
		public Vector2 posicao, posicaoVida;
		public int velocidade, vida;
		public double delayTiro;
		private KeyboardState teclado;
		//Variaveis responsaveis por colisoes
		public Rectangle delimitacaoJogador, delimitacaoVida;
		public bool colisao;
		public List<Tiro> tiros;
		GerenciadorSom som = new GerenciadorSom();

		//Metodo construtor
		public Jogador ()
		{
			texturaJogador = null;
			tiros = new List<Tiro> ();
			posicao = new Vector2 (300, 300);
			delayTiro = 7;
			velocidade = 5;
			colisao = false;
			vida = 200;
			posicaoVida = new Vector2 (50, 50);
		}

		//Carregar conteudo
		public void LoadContent(ContentManager Content)
		{
			texturaJogador = Content.Load<Texture2D> ("ship");
			texturaTiro = Content.Load<Texture2D> ("playerbullet");
			texturaVida = Content.Load<Texture2D> ("healthbar");
			som.LoadContent (Content);
		}

		//Atualizar
		public void Update(GameTime gameTime)
		{
			//Pegando teclas apertadas do teclado
			teclado = Keyboard.GetState ();

			//Delimitaçao para a nave do jogador
			delimitacaoJogador = new Rectangle ((int)posicao.X, (int)posicao.Y, texturaJogador.Width, texturaJogador.Height);
			//Estabelecer o retangulo para barra de vida
			delimitacaoVida = new Rectangle ((int)posicaoVida.X, (int)posicaoVida.Y, vida, 25);

			if (teclado.IsKeyUp (Keys.Space))
				delayTiro = 1;
			//Atirar
			if (teclado.IsKeyDown (Keys.Space))
				Atirar ();

			AtualizarTiro ();

			//Controle da nave
			if (teclado.IsKeyDown (Keys.Right) || teclado.IsKeyDown (Keys.D))
				posicao.X += velocidade;
			if (teclado.IsKeyDown (Keys.Left) || teclado.IsKeyDown (Keys.A))
				posicao.X -= velocidade;
			if (teclado.IsKeyDown (Keys.Up) || teclado.IsKeyDown (Keys.W))
				posicao.Y -= velocidade;
			if (teclado.IsKeyDown (Keys.Down) || teclado.IsKeyDown (Keys.S))
				posicao.Y += velocidade;

			//Manter o jogador nos limites da tela
			if (posicao.X <= 0)
				posicao.X = 0;
			if (posicao.X >= 700 - texturaJogador.Width)
				posicao.X = 700 - texturaJogador.Width;
			if (posicao.Y <= 0)
				posicao.Y = 0;
			if (posicao.Y >= 750 - texturaJogador.Height)
				posicao.Y = 750 - texturaJogador.Height;

		}

		//Desenhar
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (texturaJogador, posicao, Color.White);
			spriteBatch.Draw (texturaVida, delimitacaoVida, Color.White);

			foreach (Tiro t in tiros)
				t.Draw (spriteBatch);
		}

		//Metodo responsavel pelo jogador poder atirar onde estabelecera a posicao inicial de todas os tiros
		public void Atirar()
		{
			//Atirar somente se o delay reiniciar
			if (delayTiro > 0 || delayTiro == 0)
				delayTiro--;

			/*			
			 * Se o delay do tiro for igual a zero: 
			 * cria-se uma novo tiro na posicao do jogador,
			 * Tornar ele visivel na tela,
			 * entao adicionar na lista tiros
			*/
			if (delayTiro <= 0 || delayTiro == 0) {

				som.somTiroJogador.Play ();

				Tiro novoTiro = new Tiro (texturaTiro);
				novoTiro.posicao = new Vector2 (posicao.X + 32 - novoTiro.texturaTiro.Width / 2, posicao.Y + 30);

				novoTiro.visivel = true;

				if (tiros.Count <= 20)
					tiros.Add (novoTiro);
			}

			//Reiniciar o delay do tiro
			if (delayTiro == 0)
				delayTiro = 7;
		}

		//Atualizar os tiros
		public void AtualizarTiro()
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
				t.posicao.Y -= velocidade;
				//Se o tiro sair fora da tela ele se tornara invisivel
				if (t.posicao.Y <= 0)
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

	}
}

