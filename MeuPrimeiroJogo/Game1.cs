#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace MeuPrimeiroJogo
{
	public class Game1 : Game
	{

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Random random = new Random ();
		public int danoTiroInimigo, pontuacao;
		private KeyboardState teclado;

		//Conteudo do jogo
		Jogador jogador = new Jogador ();
		CampoDeEstrela fundoDoJogo = new CampoDeEstrela ();
		List<Asteroide> asteroides = new List<Asteroide> ();
		List<Inimigo> inimigos = new List<Inimigo>();
		List<Explosao> explosoes = new List<Explosao>();
		GerenciadorSom som = new GerenciadorSom();
		Texture2D texturaMenu, texturaGameOver;

		//Estabelecendo o primeiro Estado
		Estados estadoJogo = Estados.Menu;

		//Metodo construtor
		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferWidth = 700;
			graphics.PreferredBackBufferHeight = 750;
			this.Window.Title = "XNA - Jogo de Nave Tutorial";
			Content.RootDirectory = "Content";
			danoTiroInimigo = 10;
			pontuacao = 0;
			texturaMenu = null;
			texturaGameOver = null;
		}

		//Inicializar
		protected override void Initialize ()
		{
			base.Initialize ();	
		}

		//Carregar Conteudo
		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			jogador.LoadContent (Content);
			fundoDoJogo.LoadContent (Content);
			som.LoadContent (Content);
			texturaMenu = Content.Load<Texture2D> ("MenuInicial");
			texturaGameOver = Content.Load<Texture2D> ("GameOver");

		}

		//Atualizar
		protected override void Update (GameTime gameTime)
		{
			teclado = Keyboard.GetState ();

			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit ();

			//Atualizando os estados do jogo
			switch(estadoJogo) {
			//Atualizando o estado do menu
			case Estados.Menu:
				{
					if (teclado.IsKeyDown (Keys.Enter)) {
						estadoJogo = Estados.Jogando;
						MediaPlayer.Play (som.musicaFundo);
					}
					fundoDoJogo.Update (gameTime);
					fundoDoJogo.velocidade = 1;
					break;
				}
				//Atualizando o estado jogando
			case Estados.Jogando:
				{
					fundoDoJogo.velocidade = 5;

					//Atualizando Inimigos e verificando as colisoes das naves ininimas com a do jogador
					foreach (Inimigo i in inimigos) {
						//Checar se as naves inimigas estao colidindo com o jogador
						if (i.delimitacaInimigo.Intersects (jogador.delimitacaoJogador)) {
							som.somExplosao.Play ();
							explosoes.Add (new Explosao (Content.Load<Texture2D> ("explosion3"), i.posicao));
							jogador.vida -= 40;
							i.visivel = false;
						}
						//Checar se os tiros dos inimigos que colidem com o jogador
						for (int x = 0; x < i.tiros.Count; x++) {
							if (i.tiros [x].delimitacaoTiro.Intersects (jogador.delimitacaoJogador)) {
								jogador.vida -= danoTiroInimigo;
								i.tiros [x].visivel = false;
							}
						}
						//Checar tiros do jogador que colidem com o inimigo
						for (int y = 0; y < jogador.tiros.Count; y++) {
							if (jogador.tiros [y].delimitacaoTiro.Intersects (i.delimitacaInimigo)) {
								som.somExplosao.Play ();
								explosoes.Add (new Explosao (Content.Load<Texture2D> ("explosion3"), i.posicao));
								pontuacao += 20;
								jogador.tiros [y].visivel = false;
								i.visivel = false;
							}
						}
						i.Update (gameTime);
					}

					//Atualizando todos os asteroides da lista e checar as colisoes
					foreach (Asteroide a in asteroides) {
						if (a.delimitacaAsteroide.Intersects (jogador.delimitacaoJogador)) {
							som.somExplosao.Play ();
							explosoes.Add (new Explosao (Content.Load<Texture2D> ("explosion3"), a.posicao));
							a.visivel = false;
							jogador.colisao = true;
							jogador.vida -= 20;
						}

						//Verificaçao de colisoes entre a lista de tiros e a lista de asteroides
						for (int i = 0; i < jogador.tiros.Count; i++) {
							if (a.delimitacaAsteroide.Intersects(jogador.tiros [i].delimitacaoTiro)) {
								som.somExplosao.Play ();
								explosoes.Add (new Explosao (Content.Load<Texture2D> ("explosion3"), a.posicao));
								pontuacao += 5;
								jogador.tiros [i].visivel = false;
								a.visivel = false;
								jogador.tiros.RemoveAt (i);
							}
						}

						a.Update (gameTime);
					}

					//Atualizando as explosoes
					foreach (Explosao e in explosoes) {
						e.Update (gameTime);
					}

					//Se a vida do jogador acabar, entao GameOver
					if (jogador.vida <= 0) {
						estadoJogo = Estados.GameOver;
						som.somGameOver.Play ();
						explosoes.Add (new Explosao (Content.Load<Texture2D> ("explosion3"), jogador.posicao));
					}

					jogador.Update (gameTime);
					fundoDoJogo.Update (gameTime);
					CarregarAsteroide ();
					CarregarInimigos ();
					ManegeExplosion ();
					break;
				}

			case Estados.GameOver:
				{
					if (teclado.IsKeyDown (Keys.Escape)) {
						inimigos.Clear ();
						asteroides.Clear ();
						explosoes.Clear ();
						jogador.tiros.Clear ();
						jogador.posicao = new Vector2 (300, 300);
						estadoJogo = Estados.Menu;
						jogador.vida = 200;
						pontuacao = 0;
					}

					MediaPlayer.Stop ();

					break;
				}
			}

			base.Update (gameTime);
		}

		//Desenhar
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();

			switch (estadoJogo) {

			case Estados.Menu:
				{
					fundoDoJogo.Draw (spriteBatch);
					spriteBatch.Draw (texturaMenu, new Vector2 (35, 0), Color.White);
					break;
				}

			case Estados.Jogando:
				{
					fundoDoJogo.Draw (spriteBatch);
					jogador.Draw (spriteBatch);

					foreach (Explosao e in explosoes)
						e.Draw (spriteBatch);

					foreach (Asteroide a in asteroides)
						a.Draw (spriteBatch);

					foreach (Inimigo i in inimigos)
						i.Draw (spriteBatch);
					break;
				}

			case Estados.GameOver:
				{
					fundoDoJogo.Draw (spriteBatch);
					spriteBatch.Draw (texturaGameOver, Vector2.Zero, Color.White);
					break;
				}

			}

			spriteBatch.End ();

			base.Draw (gameTime);
		}

		//Carregar os asteroides na tela
		public void CarregarAsteroide()
		{
			//Criando variaveis randomicas para os eixos x e y do asteroide
			int randX = random.Next (0, 700);
			int randY = random.Next (-400, -50);

			if (asteroides.Count <= 5)
				asteroides.Add (new Asteroide (Content.Load<Texture2D> ("asteroid"), new Vector2 (randX, randY)));

			//Verifica se possui algum asteroid invisivel, caso tiver ele sera removido da lista de asteroides
			for (int i = 0; i < asteroides.Count; i++) {
				if (!asteroides [i].visivel) {
					asteroides.RemoveAt (i);
					i--;
				}
			}

		}

		//Carregar os inimigos na tela
		public void CarregarInimigos()
		{
			//Craindo variaveis randomicas para o eixo x e y do inimigo
			int randX = random.Next (0, 700);
			int randY = random.Next (-400, -50);

			if (inimigos.Count <= 3)
				inimigos.Add (new Inimigo (Content.Load<Texture2D> ("enemyship"), new Vector2 (randX, randY), Content.Load<Texture2D> ("EnemyBullet")));

			//Verifica se possui algum inimigo invisivel, caso tiver ele sera removido da lista de inimigos
			for (int i = 0; i < inimigos.Count; i++) {
				if (!inimigos [i].visivel) {
					inimigos.RemoveAt (i);
					i--;
				}
			}

		}

		//Gerenciador de explosoes
		public void ManegeExplosion()
		{
			//Verificar se possui alguma explosoao invisivel, se tiver ela sera removida da lista
			for (int i = 0; i < explosoes.Count; i++) {
				if (!explosoes [i].visivel) {
					explosoes.RemoveAt (i);
					i--;
				}
			}
		}

	}
}

