using System;
using System.Collections.Generic;
using System.Linq;

namespace LotoFacilAnalyzer
{
    public class Calculadora
    {
        public event EventHandler OnCalculoStart;

        private decimal CalcularJogo(Concurso concurso, int[] jogo, out int pontos)
        {
            var acertos = concurso.Bolas.Intersect(jogo).Count();
            pontos = acertos;
            switch (acertos)
            {
                case 11:
                    return Parametro.Valor11;
                case 12:
                    return Parametro.Valor12;
                case 13:
                    return Parametro.Valor13;
                case 14:
                    return Parametro.Valor14;
                case 15:
                    return Parametro.Valor15;
                default:
                    return 0;
            }
        }

        public IEnumerable<Linha> CalculoCompleto(IEnumerable<Concurso> concursos, IEnumerable<IEnumerable<int>> jogos)
        {
            OnCalculoStart?.Invoke(this, EventArgs.Empty);
            var ganhoTotal = 0M;
            foreach (var concurso in concursos)
            {
                var ganhoParcial = 0M;
                foreach (var jogo in jogos)
                {
                    var arrayJogo = jogo.ToArray();
                    var ganhoJogo = CalcularJogo(concurso, arrayJogo, out var pontos);
                    yield return CriarLinhaDetalhe(concurso, arrayJogo, ganhoJogo, pontos);
                    ganhoParcial += ganhoJogo;
                }
                yield return CriarLinhaSubTotal(concurso, jogos.Count(), ganhoParcial);
                ganhoTotal += ganhoParcial;
            }
            yield return CriarLinhaTotal(concursos.Count(), jogos.Count(), ganhoTotal);

        }

        private Linha CriarLinhaTotal(int qtdConcursos, int qtdJogos, decimal ganhoTotal)
        {
            return new Linha
            {
                Ganho = ganhoTotal,
                Gasto = Parametro.CustoJogo * qtdJogos * qtdConcursos,
                Saldo = ganhoTotal - (Parametro.CustoJogo * qtdJogos * qtdConcursos)
            };
        }

        public IEnumerable<Linha> CalculoSintetico(IEnumerable<Concurso> concursos, IEnumerable<IEnumerable<int>> jogos)
        {
            OnCalculoStart?.Invoke(this, EventArgs.Empty);
            foreach (var concurso in concursos)
            {
                var ganhoJogo = 0M;
                foreach (var jogo in jogos)
                {
                    var arrayJogo = jogo.ToArray();
                    ganhoJogo += CalcularJogo(concurso, arrayJogo, out var _); 
                }
                yield return CriarLinhaSubTotal(concurso, jogos.Count(), ganhoJogo);
            }
        }

        private Linha CriarLinhaSubTotal(Concurso concurso, int qtdJogos, decimal ganhoParcial)
        {
            return new Linha
            {
                DataConcurso = concurso.Data,
                NumeroConcurso = concurso.Numero,
                N1Resultado = concurso.Bolas[0],
                N2Resultado = concurso.Bolas[1],
                N3Resultado = concurso.Bolas[2],
                N4Resultado = concurso.Bolas[3],
                N5Resultado = concurso.Bolas[4],
                N6Resultado = concurso.Bolas[5],
                N7Resultado = concurso.Bolas[6],
                N8Resultado = concurso.Bolas[7],
                N9Resultado = concurso.Bolas[8],
                N10Resultado = concurso.Bolas[9],
                N11Resultado = concurso.Bolas[10],
                N12Resultado = concurso.Bolas[11],
                N13Resultado = concurso.Bolas[12],
                N14Resultado = concurso.Bolas[13],
                N15Resultado = concurso.Bolas[14],
                Ganho = ganhoParcial,
                Gasto = Parametro.CustoJogo * qtdJogos,
                Saldo = ganhoParcial - (Parametro.CustoJogo * qtdJogos)
            };
        }

        public IEnumerable<Linha> CalculoDetalhado(IEnumerable<Concurso> concursos, IEnumerable<IEnumerable<int>> jogos)
        {
            OnCalculoStart?.Invoke(this, EventArgs.Empty);
            foreach (var concurso in concursos)
            {
                foreach (var jogo in jogos)
                {
                    var arrayJogo = jogo.ToArray();
                    var ganhoJogo = CalcularJogo(concurso, arrayJogo, out var pontos);
                    yield return CriarLinhaDetalhe(concurso, arrayJogo, ganhoJogo, pontos);
                }
            }
        }

        private Linha CriarLinhaDetalhe(Concurso concurso, int[] arrayJogo, decimal ganhoJogo, int pontos)
        {
            return new Linha
            {
                DataConcurso = concurso.Data,
                NumeroConcurso = concurso.Numero,
                N1Resultado = concurso.Bolas[0],
                N2Resultado = concurso.Bolas[1],
                N3Resultado = concurso.Bolas[2],
                N4Resultado = concurso.Bolas[3],
                N5Resultado = concurso.Bolas[4],
                N6Resultado = concurso.Bolas[5],
                N7Resultado = concurso.Bolas[6],
                N8Resultado = concurso.Bolas[7],
                N9Resultado = concurso.Bolas[8],
                N10Resultado = concurso.Bolas[9],
                N11Resultado = concurso.Bolas[10],
                N12Resultado = concurso.Bolas[11],
                N13Resultado = concurso.Bolas[12],
                N14Resultado = concurso.Bolas[13],
                N15Resultado = concurso.Bolas[14],
                N1Jogo = arrayJogo[0],
                N2Jogo = arrayJogo[1],
                N3Jogo = arrayJogo[2],
                N4Jogo = arrayJogo[3],
                N5Jogo = arrayJogo[4],
                N6Jogo = arrayJogo[5],
                N7Jogo = arrayJogo[6],
                N8Jogo = arrayJogo[7],
                N9Jogo = arrayJogo[8],
                N10Jogo = arrayJogo[9],
                N11Jogo = arrayJogo[10],
                N12Jogo = arrayJogo[11],
                N13Jogo = arrayJogo[12],
                N14Jogo = arrayJogo[13],
                N15Jogo = arrayJogo[14],
                Pontos = pontos,
                Ganho = ganhoJogo,
                Gasto = Parametro.CustoJogo,
                Saldo = ganhoJogo - Parametro.CustoJogo
            };
        }

        //public void GerarRelatorio(Func<Concurso, bool> filter, int[] numerosErrados)
        //  {


        //var listaNumerosErrados = GetPermutations(Parametros.NumerosValidos, 5).Where(jogo =>
        // jogo.Where(n => n % 2 == 0).Count() == 3 && jogo.Where(n => n % 2 != 0).Count() == 2
        //).ToList();
        //var concursos = ImportarConcursos();

        //var listaNumerosErrados = new[]
        //{
        //    new int [] { 1,10,12,19,21},
        //    new int [] { 1,5,10,14,23},
        //    new int [] { 1,5,10,12,19},
        //    new int [] { 1,10,12,19,23},
        //    new int [] { 5,10,14,19,23},
        //    new int [] { 10,11,12,13,21},
        //    new int [] { 2,3,9,19,22},
        //};

        //        var concursos = _concursos.Where(filter); ;

        //        var jogos = _gerador.GerarJogos(numerosErrados, Parametro.QtdJogos);


        //        foreach (var concurso in concursos)
        //        {
        //            var gastoParcial = Parametro.CustoJogo * jogos.Count();
        //            var ganhoParcial = 0M;

        //            foreach (var jogo in jogos)
        //            {
        //                var ganhoJogo = CalcularGanho(concurso, jogo.ToArray());
        //                linhas.Add(new Linha
        //                {
        //                    Concurso = concurso,
        //                    Jogo = jogo.ToArray(),
        //                    Ganho = ganhoJogo,
        //                    Gasto = Parametro.CustoJogo,
        //                    Saldo = ganhoJogo - Parametro.CustoJogo
        //                });
        //                ganhoParcial += ganhoJogo;
        //            }

        //            var saldoParcial = ganhoParcial - gastoParcial;

        //            if (saldoParcial > 0)
        //            {
        //                Console.WriteLine($"No concurso {concurso.Numero}, você gastou {gastoParcial:C2} e ganhou {ganhoParcial:C2}.");
        //                Console.WriteLine($"saldo parcial: {saldoParcial:C2}");
        //            }
        //            gastoTotal += gastoParcial;
        //            ganhoTotal += ganhoParcial;
        //        }

        //        var saldoTotal = ganhoTotal - gastoTotal;

        //        Console.WriteLine($"N1: {numerosErrados.ToArray()[0]}, N2: {numerosErrados.ToArray()[1]}, N3: {numerosErrados.ToArray()[2]}, N4:{numerosErrados.ToArray()[3]}, N5:{numerosErrados.ToArray()[4]}");
        //        Console.WriteLine($"Você gastou {gastoTotal:C2} e ganhou {ganhoTotal:C2}.");
        //        Console.WriteLine($"Saldo: {saldoTotal:C2}");
        //        if (saldoTotal > 0)
        //        {
        //            Console.ReadKey();
        //        }

        //    }

        //}

    }
}
