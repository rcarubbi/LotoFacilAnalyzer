using Carubbi.CurrencyWriter;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace LotoFacilAnalyzer
{
    internal class Ditador
    {
        private readonly int _qtdNumeros;
        private readonly int _intervalo;
        private readonly ICurrencyWriter _cw;
        public Ditador(int qtdNumeros, decimal intervalo)
        {
            _cw = CurrencyWriterFactory.GetInstance().GetCurrencyWriter(new System.Globalization.CultureInfo("pt-BR"));

            _qtdNumeros = qtdNumeros;
            _intervalo = (int)(intervalo * 1000);
        }

        public bool Ditar(IEnumerable<IEnumerable<int>> jogos)
        {
            int indiceJogo = 0;
            foreach (var jogo in jogos)
            {
                if (indiceJogo > 0 && indiceJogo % 3 == 0)
                {
                    var ea = new CancelEventArgs();
                    ReproduzirFrase("Próxima cartela");
                    OnCartelaEnded?.Invoke(this, ea);
                    if (ea.Cancel)
                    {
                        return false;
                    }
                }

                if (!DitarJogo(jogo, indiceJogo + 1))
                {
                    return false;
                }
                indiceJogo++;
            }
            return true;
        }

        private bool DitarJogo(IEnumerable<int> jogo, int numeroJogo)
        {
            var numeroJogoExtenso = _cw.Write(numeroJogo, CurrencyType.real).Replace("real", string.Empty).Replace("reais", string.Empty);
            ReproduzirFrase($"Jogo número {numeroJogoExtenso}");
            Thread.Sleep(_intervalo);

            StringBuilder frase = new StringBuilder();
            int numeroBola = 1;
            foreach (var bola in jogo)
            {
                var numeroPorExtenso = _cw.Write(bola, CurrencyType.real).Replace("real", string.Empty).Replace("reais", string.Empty);
                frase.Append($"{numeroPorExtenso}, ");

                if (numeroBola % _qtdNumeros == 0 || numeroBola == jogo.Count())
                {
                    ReproduzirFrase(frase.ToString().Substring(0, frase.Length - 2).ToLower().Trim());
                    Thread.Sleep(_intervalo);
                    var ea = new CancelEventArgs();
                    OnGrupoBolasCantado?.Invoke(this, ea);
                    frase.Clear();
                    if (ea.Cancel)
                    {
                        return false;
                    }
                }
                
                numeroBola++;
            }
            return true;
        }

        private void ReproduzirFrase(string frase)
        {
            var arquivo = Carubbi.Google.TTS.GoogleTTS.GenerateFile(frase, Carubbi.Google.TTS.Language.BrazilianPortuguese);
            using (IWavePlayer waveOutDevice = new WaveOut())
            using (AudioFileReader audioFileReader = new AudioFileReader(arquivo))
            {
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();

                while (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(500);
                }

                waveOutDevice.Dispose();
            }
            File.Delete(arquivo);
        }

        public event EventHandler<CancelEventArgs> OnCartelaEnded;
        public event EventHandler<CancelEventArgs> OnGrupoBolasCantado;

    }
}