using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MeuPrimeiroJogo
{
	public class GerenciadorSom
	{

		public SoundEffect somTiroJogador, somExplosao, somGameOver;
		public Song musicaFundo;

		//Metodo construtor
		public GerenciadorSom ()
		{
			somTiroJogador = null;
			somExplosao = null;
			somGameOver = null;
			musicaFundo = null;
		}

		//Carregar conteudo
		public void LoadContent(ContentManager Content)
		{
			somTiroJogador = Content.Load<SoundEffect> ("playershoot");
			somExplosao = Content.Load<SoundEffect> ("explode");
			somGameOver = Content.Load<SoundEffect> ("GameOver");
			musicaFundo = Content.Load<Song> (@"theme.wav");
		}


	}
}

