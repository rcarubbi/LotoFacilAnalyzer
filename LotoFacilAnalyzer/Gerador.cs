using System;
using System.Collections.Generic;
using System.Linq;

namespace LotoFacilAnalyzer
{
    public class Gerador
    {
        private readonly bool _proporcaoParImpar;
        private readonly bool _proporcaoPrimos;
        private readonly bool _somatoriaDezenas;
        private readonly bool _planilha;

        public event EventHandler OnGerarStart;

        public Gerador(bool proporcaoParImpar, bool proporcaoPrimos, bool somatoriaDezenas, bool planilha)
        {
            _proporcaoParImpar = proporcaoParImpar;
            _proporcaoPrimos = proporcaoPrimos;
            _somatoriaDezenas = somatoriaDezenas;
            _planilha = planilha;
        }

        public IEnumerable<IEnumerable<int>> GerarJogos(int[] numerosErrados, int qtdJogos)
        {
            OnGerarStart?.Invoke(this, EventArgs.Empty);
            var numerosCertos = Parametro.Numeros.Except(numerosErrados).ToArray();
            IEnumerable<IEnumerable<int>> resultado = null;
            if (!_planilha)
            {
                resultado = GetPermutations(numerosCertos, 15);
            }
            else
            {
                resultado = GerarPlanilha(numerosCertos);
            }
            if (_proporcaoParImpar)
            {
                resultado = resultado.Where(jogo => jogo.Where(n => n % 2 == 0).Count() == 7 && jogo.Where(n => n % 2 != 0).Count() == 8);
            }

            if (_proporcaoPrimos)
            {
                resultado = resultado.Where(jogo => jogo.Where(n => isPrimo(n)).Count() == 5);
            }

            if (_somatoriaDezenas)
            {
                resultado = resultado.Where(jogo => jogo.Sum() >= 181 && jogo.Sum() <= 195);
            }

            return resultado.Take(qtdJogos);
        }

        private IEnumerable<IEnumerable<int>> GerarPlanilha(int[] numerosCertos)
        {
            var mapaIndices = new List<int[]>();

            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 13, 14, 15, 16 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 13, 14, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 12, 13, 14, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 13, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12, 13, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 11, 12, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 11, 13, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 12, 14, 15, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 13, 14, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 13, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 12, 14, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9, 11, 12, 13, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9, 11, 13, 14, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 14, 15, 16 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 13, 14, 15, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 9, 10, 13, 14, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 9, 12, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 9, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 10, 11, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 10, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 10, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 9, 11, 12, 13, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 10, 11, 12, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 11, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 13, 14, 16, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 10, 12, 14, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 11, 12, 13, 14, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 11, 13, 14, 15, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 10, 11, 12, 13, 14, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 10, 11, 13, 14, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 10, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 11, 12, 13, 14, 15, 16, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 9, 10, 11, 12, 13, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 9, 10, 12, 13, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 9, 11, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 10, 11, 12, 13, 14, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 10, 11, 12, 13, 14, 15, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 7, 10, 11, 12, 13, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 8, 9, 11, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 8, 9, 11, 12, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 8, 10, 11, 12, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 8, 10, 11, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 5, 9, 10, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 15, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 13, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 9, 11, 12, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 9, 11, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 10, 11, 12, 14, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 10, 11, 13, 14, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 10, 12, 13, 14, 15, 16, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 8, 11, 12, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 9, 10, 12, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 10, 11, 12, 13, 14, 15, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 7, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 8, 9, 10, 11, 12, 13, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 8, 9, 11, 12, 13, 14, 15, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 9, 10, 11, 12, 13, 14, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 6, 9, 10, 11, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 7, 8, 9, 10, 11, 12, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 7, 8, 9, 10, 12, 13, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 7, 8, 9, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 7, 9, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 4, 8, 9, 10, 11, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 12, 14, 15, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 13, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 8, 9, 12, 13, 14, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 8, 9, 12, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 8, 9, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 9, 11, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 9, 11, 12, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 10, 11, 12, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 7, 10, 11, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 8, 9, 10, 11, 12, 13, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 8, 9, 10, 11, 12, 13, 15, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 8, 9, 11, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 8, 10, 11, 12, 13, 14, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 8, 10, 11, 12, 13, 14, 15, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 6, 8, 10, 11, 12, 13, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 7, 8, 9, 10, 11, 12, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 7, 8, 9, 11, 12, 13, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 7, 8, 10, 11, 12, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 7, 8, 11, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 8, 9, 10, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 5, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 6, 7, 8, 10, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 6, 7, 8, 11, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 6, 7, 9, 10, 11, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 6, 8, 9, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 3, 7, 8, 9, 10, 11, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 13, 14, 15, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 12, 13, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 12, 13, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 9, 11, 12, 13, 15, 16, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 9, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 16, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 10, 12, 13, 14, 15, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 10, 12, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 11, 12, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 9, 10, 11, 12, 14, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 7, 10, 11, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 8, 9, 10, 11, 12, 14, 15, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 8, 9, 10, 11, 13, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 8, 9, 11, 12, 13, 14, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 8, 9, 11, 12, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 6, 8, 9, 11, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 7, 8, 9, 10, 11, 12, 14, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 7, 8, 9, 10, 11, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 7, 9, 10, 11, 12, 13, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 7, 9, 10, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 5, 8, 9, 10, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 6, 7, 8, 9, 10, 11, 13, 14, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 6, 7, 8, 9, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 6, 7, 9, 10, 11, 12, 13, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 6, 7, 9, 11, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 6, 8, 10, 11, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 7, 8, 9, 10, 11, 12, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 4, 7, 8, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 17 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 7, 8, 9, 10, 11, 13, 14, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 7, 8, 9, 11, 12, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 7, 8, 9, 11, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 7, 8, 10, 11, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 7, 8, 10, 11, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 7, 8, 11, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 7, 9, 10, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 8, 9, 10, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 6, 9, 10, 11, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 2, 5, 7, 9, 10, 11, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 9, 10, 12, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 9, 10, 13, 14, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 9, 11, 13, 14, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 9, 12, 13, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15, 16 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15, 18 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 15, 16, 18 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 10, 12, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15, 17 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 9, 10, 11, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 9, 11, 12, 13, 14, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 9, 11, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 10, 11, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 10, 11, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 7, 11, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 8, 9, 10, 11, 12, 14, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 8, 9, 10, 11, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 6, 9, 10, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 7, 8, 9, 10, 11, 12, 14, 15, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 7, 8, 9, 10, 11, 13, 14, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 7, 8, 9, 10, 11, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 7, 8, 9, 11, 12, 13, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 7, 8, 9, 11, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 7, 9, 10, 11, 12, 13, 14, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 5, 8, 9, 10, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 6, 7, 8, 9, 10, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 6, 7, 8, 9, 11, 12, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 6, 7, 9, 10, 11, 12, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 6, 8, 9, 10, 11, 12, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 6, 8, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 4, 7, 8, 10, 11, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 3, 5, 6, 7, 8, 10, 11, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 5, 6, 8, 9, 10, 11, 12, 13, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 5, 6, 9, 10, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 5, 7, 8, 9, 10, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 5, 7, 9, 10, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 6, 7, 8, 9, 10, 11, 12, 13, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 3, 6, 7, 8, 9, 11, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 4, 5, 6, 7, 8, 9, 10, 11, 13, 14, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 1, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 1, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 4, 5, 6, 7, 8, 10, 11, 12, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 1, 4, 5, 6, 9, 10, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 4, 5, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 4, 5, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 4, 6, 7, 8, 9, 10, 11, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 4, 6, 7, 8, 10, 11, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 1, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 5, 7, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 1, 5, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 15, 16, 18 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 15, 16, 17 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 11, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 10, 11, 13, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 10, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 11, 12, 13, 14, 15, 17, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 8, 11, 12, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 9, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 10, 11, 12, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 7, 10, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 8, 9, 11, 12, 13, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 6, 9, 10, 11, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 7, 8, 9, 10, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 7, 8, 9, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 7, 9, 10, 11, 12, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 8, 9, 10, 11, 12, 13, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 5, 8, 9, 10, 11, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 6, 7, 8, 9, 10, 11, 13, 14, 15, 17, 18 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 6, 7, 8, 9, 11, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 6, 7, 8, 10, 11, 12, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 6, 7, 8, 10, 11, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 18 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 6, 8, 9, 10, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 6, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 7, 9, 10, 11, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 4, 8, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 18 });
            mapaIndices.Add(new int[] { 0, 2, 3, 5, 6, 7, 8, 10, 11, 12, 13, 14, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 5, 6, 7, 8, 10, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 2, 3, 5, 6, 7, 9, 10, 11, 12, 13, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 2, 3, 5, 6, 7, 9, 10, 11, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 5, 6, 8, 9, 10, 11, 12, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 5, 7, 8, 9, 10, 11, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 6, 7, 8, 9, 10, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 6, 7, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 6, 8, 9, 10, 11, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 2, 4, 5, 6, 7, 8, 9, 10, 11, 13, 14, 15, 16, 19 });
            mapaIndices.Add(new int[] { 0, 2, 4, 5, 6, 7, 8, 9, 10, 12, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 4, 5, 6, 7, 9, 11, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 4, 5, 6, 8, 10, 11, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 2, 4, 5, 7, 8, 9, 11, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 2, 4, 5, 7, 8, 10, 11, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 4, 6, 7, 8, 9, 10, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 4, 6, 8, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 2, 6, 7, 8, 9, 10, 11, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 3, 4, 5, 6, 7, 8, 9, 10, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 0, 3, 4, 5, 6, 8, 9, 11, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 3, 4, 5, 6, 8, 10, 11, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 3, 4, 5, 7, 8, 10, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 3, 4, 6, 7, 8, 9, 10, 11, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 0, 3, 4, 6, 7, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 3, 4, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 3, 5, 6, 7, 8, 9, 11, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 0, 4, 5, 6, 7, 8, 10, 11, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 0, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 14, 15, 17, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 15, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 16, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 15, 17, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 12, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 14, 16, 17, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 11, 12, 13, 14, 15, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 9, 10, 12, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 9, 10, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 10, 11, 12, 13, 14, 15, 16, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 13, 14, 15, 16, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 8, 10, 11, 12, 13, 14, 15, 16, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 9, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 6, 10, 11, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 7, 8, 9, 10, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 7, 8, 10, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 7, 9, 10, 11, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 8, 9, 10, 11, 12, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 5, 8, 9, 11, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 17, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 15, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 17 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 11, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 11, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 6, 7, 9, 10, 11, 12, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 6, 8, 9, 10, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 4, 7, 9, 10, 11, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 16, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 13, 14, 15, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 5, 6, 7, 8, 10, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 5, 6, 7, 9, 10, 11, 12, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 5, 6, 7, 9, 11, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 5, 6, 8, 9, 10, 11, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 5, 7, 8, 9, 10, 11, 12, 13, 14, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 5, 7, 8, 9, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 5, 7, 8, 10, 11, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 3, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 3, 6, 8, 9, 10, 11, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 16, 18 });
            mapaIndices.Add(new int[] { 1, 2, 4, 5, 6, 7, 8, 9, 10, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14, 16, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 4, 5, 6, 7, 8, 10, 11, 12, 13, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 4, 5, 6, 7, 9, 10, 11, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 4, 5, 6, 7, 10, 11, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 1, 2, 4, 5, 6, 8, 10, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 4, 5, 7, 8, 9, 10, 11, 12, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 1, 2, 4, 5, 7, 8, 9, 10, 11, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 4, 6, 7, 8, 9, 10, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 4, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 2, 5, 6, 7, 8, 9, 10, 11, 12, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 2, 6, 7, 8, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 12, 13, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 3, 4, 5, 6, 7, 8, 10, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 1, 3, 4, 5, 6, 8, 9, 10, 11, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 3, 4, 5, 7, 8, 10, 11, 12, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16, 18, 19 });
            mapaIndices.Add(new int[] { 1, 3, 4, 7, 8, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 3, 5, 6, 7, 8, 9, 10, 11, 13, 14, 15, 16, 17, 18 });
            mapaIndices.Add(new int[] { 1, 3, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15, 16, 17, 19 });
            mapaIndices.Add(new int[] { 1, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 1, 4, 5, 6, 7, 8, 9, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16, 17 });
            mapaIndices.Add(new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 5, 6, 7, 8, 11, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 5, 6, 8, 9, 10, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 13, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 5, 7, 9, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 6, 7, 8, 9, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 6, 7, 8, 10, 11, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 4, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 5, 6, 7, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 3, 5, 6, 8, 9, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 4, 5, 6, 7, 8, 9, 12, 13, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 2, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 18, 19 });
            mapaIndices.Add(new int[] { 3, 4, 5, 6, 7, 9, 10, 11, 12, 13, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 3, 4, 5, 6, 7, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19 });
            mapaIndices.Add(new int[] { 3, 4, 5, 7, 8, 9, 10, 11, 12, 13, 14, 15, 17, 18, 19 });
            mapaIndices.Add(new int[] { 3, 4, 6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });

            foreach (var item in mapaIndices)
            {
                var jogo = new List<int>();
                foreach (var indice in item)
                {
                    jogo.Add(numerosCertos[indice]);
                }
                yield return jogo;
            }
        }

        public IEnumerable<IEnumerable<int>> GerarNumerosErrados(int palpites)
        {
            return GetPermutations(Parametro.Numeros, 5).Take(palpites);
        }

        private bool isPrimo(int n)
        {
            var count = 0;
            for (int i = 2; i < n; i++)
            {
                if (n % i == 0)
                {
                    count++;
                    break;
                }
            }

            return (count == 0);
        }

        private IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }
    }
}
