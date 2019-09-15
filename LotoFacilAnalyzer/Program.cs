using Carubbi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LotoFacilAnalyzer
{
    public class Program
    {
        private static IEnumerable<Concurso> _concursos = new List<Concurso>();
        private static IEnumerable<IEnumerable<int>> _jogos = new List<IEnumerable<int>>();
        private static IEnumerable<Linha> _linhas = new List<Linha>();
        private static readonly string[] operadoresValidos = new string[] { "=", ">", "<", "entre" };

        class OcorrenciaGrupo
        {
            public string key { get; set; }

            public int Count { get; set; }
        }

        static void Main(string[] args)
        {
            bool quit = false;
            do
            {
                Console.Write("LotoFacil>");
                var input = Console.ReadLine().Trim();
                var partesInput = input.Split(' ');
                var comando = partesInput[0];
                var parametros = partesInput.Skip(1).ToList();
                try
                {
                    var output = Interpretar(comando, parametros, ref quit);
                    Console.WriteLine(output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (!quit);
        }

        //    List<OcorrenciaGrupo> ocorrencias = new List<OcorrenciaGrupo>();
        //    var concursos = ImportarConcursos();
        //    var filtro = concursos.Where(c => c.Data >= new DateTime(2019, 7, 11));
        //    var qtdConcursos = filtro.Count();
        //    Console.WriteLine($"Numero de concursos: {qtdConcursos}");
        //    foreach (var item in filtro)
        //    {
        //        var naoSairam = Parametros.Numeros.Except(item.Bolas);
        //        var grupos = GetPermutations(naoSairam, 5);

        //        foreach (var grupo in grupos)
        //        {
        //            var key = string.Join(",", grupo);
        //            var ocorrencia = ocorrencias.SingleOrDefault(x => x.key == key);
        //            if (ocorrencia == null)
        //            {
        //                ocorrencias.Add(new OcorrenciaGrupo
        //                {
        //                    key = key,
        //                    Count = 1
        //                });
        //            }
        //            else
        //            {
        //                ocorrencias.Find(o => o.key == key).Count++;
        //            }
        //        }

        //    }

        //    var menoresOcorrencias = ocorrencias.OrderByDescending(x => x.Count).Take(10);
        //    foreach (var menorOcorrencia in menoresOcorrencias)
        //    {
        //        Console.WriteLine($"{menorOcorrencia.key} não saiu {menorOcorrencia.Count} vezes");
        //    }
        //    Console.ReadKey();
        //}

        private static string Interpretar(string comando, List<string> parametros, ref bool quit)
        {

            switch (comando)
            {
                case "importar":
                    return InterpretarImportar(parametros);
                case "gerar":
                    return InterpretarGerar(parametros);
                case "calcular":
                    return InterpretarCalcular(parametros);
                case "exportar":
                    return InterpretarExportar(parametros);
                case "listar":
                    return InterpretarListar(parametros);
                case "forcaBruta":
                    return InterpretarForcaBruta(parametros);
                case "sair":
                    quit = true;
                    return string.Empty;
                case "ajuda":
                    return InterpretarAjuda(parametros);
                default:
                    return "Comando inválido";
            }

        }

        private static string InterpretarForcaBruta(List<string> parametros)
        {
            var temp15 = Parametro.Valor15;
            Parametro.Valor15 = Parametro.Valor14;
            var palpites = 100;
            var parametroPalpites = LerParametroComArgumentos(parametros, "palpites",
                (arg) => palpites = LerValorArgumento<int>(arg,
                "Quantidade de palpites deve ser numérica",
                "Informe a quantidade de palpites"));

            var geradorPalpites = new Gerador(false, false, false, false);
            var sequencias = geradorPalpites.GerarNumerosErrados(palpites);
            var calculadora = new Calculadora();
            var melhorSaldo = decimal.MinValue;
            var melhorSequencia = new int[] { };
            var gerador = new Gerador(true, false, false, true);
            foreach (var numerosErrados in sequencias)
            {
                var jogos = gerador.GerarJogos(numerosErrados.ToArray(), Parametro.QtdJogos);
                var linhas = calculadora.CalculoSintetico(_concursos, jogos).ToList();
                var saldo = linhas.Sum(x => x.Saldo);
                if (melhorSaldo < saldo)
                {
                    melhorSequencia = numerosErrados.ToArray();
                    melhorSaldo = saldo;
                }
            }

            var numeros = string.Join(",", melhorSequencia);
            Parametro.Valor15 = temp15;
            return $"Melhor sequencia: {numeros}";

        }

        private static string InterpretarListar(List<string> parametros)
        {
            var tipo = "calculo";
            var parametroTipo = LerParametroComArgumentos(parametros, "tipo",
                (arg) => tipo = LerValorArgumentoDominio(arg,
                new string[] { "concurso", "calculo", "jogo" },
                "Tipo de lista inválida. Opções: concurso, calculo ou jogo",
                "Tipo de lista não informado"));

            switch (tipo)
            {
                case "concurso":
                    Console.WriteLine("Lista de concursos");
                    Console.WriteLine("|Data\t\t|Numero\t|Bolas\t|");
                    foreach (var item in _concursos)
                    {
                        var bolas = string.Join(",", item.Bolas.Select(x => x.ToString()));
                        Console.WriteLine("|{0}\t|{1}\t|{2}\t|", item.Data.ToShortDateString(), item.Numero, bolas);
                    }
                    return "Fim da lista";

                case "jogo":
                    Console.WriteLine("Lista de jogos");
                    Console.WriteLine("|Número do jogo\t|Bolas\t|");
                    int i = 1;
                    foreach (var item in _jogos)
                    {
                        var jogo = item.ToArray();
                        var bolas = string.Join(",", jogo.Select(x => x.ToString()));
                        Console.WriteLine("|{0}\t|{1}\t|", i++, bolas);
                    }
                    return "Fim da lista";

                case "calculo":
                default:
                    Console.WriteLine("Lista de cálculos");
                    Console.WriteLine("|Data\t|Número\t|Pontos\t|Ganho\t|Gasto\t|Saldo\t|Bolas sorteadas\t|Bolas jogadas\t|");

                    foreach (var item in _linhas)
                    {
                        var bolasSorteadas = item.N1Jogo == 0
                            ? "-"
                            : string.Concat(item.N1Jogo, ",",
                            item.N2Jogo, ",",
                            item.N3Jogo, ",",
                            item.N4Jogo, ",",
                            item.N5Jogo, ",",
                            item.N6Jogo, ",",
                            item.N7Jogo, ",",
                            item.N8Jogo, ",",
                            item.N9Jogo, ",",
                            item.N10Jogo, ",",
                            item.N11Jogo, ",",
                            item.N12Jogo, ",",
                            item.N13Jogo, ",",
                            item.N14Jogo, ",",
                            item.N15Jogo);

                        var bolasJogadas = item.N1Resultado == 0
                          ? "-"
                          : string.Concat(item.N1Resultado, ",",
                          item.N2Resultado, ",",
                          item.N3Resultado, ",",
                          item.N4Resultado, ",",
                          item.N5Resultado, ",",
                          item.N6Resultado, ",",
                          item.N7Resultado, ",",
                          item.N8Resultado, ",",
                          item.N9Resultado, ",",
                          item.N10Resultado, ",",
                          item.N11Resultado, ",",
                          item.N12Resultado, ",",
                          item.N13Resultado, ",",
                          item.N14Resultado, ",",
                          item.N15Resultado);

                        Console.WriteLine("|{0}\t|{1}\t|{2}\t|{3:C2}\t|{4:C2}\t|{5:C2}\t|{6}\t|{7}\t|",
                            item.DataConcurso.ToShortDateString(),
                            item.NumeroConcurso,
                            item.Pontos,
                            item.Ganho,
                            item.Gasto,
                            item.Saldo,
                            bolasSorteadas,
                            bolasJogadas);
                    }
                    return "Fim da lista";
            }
        }

        private static string InterpretarExportar(List<string> parametros)
        {
            var tipo = "calculo";
            var nomeArquivo = "dados.csv";

            var parametroNomeArquivo = LerParametroComArgumentos(parametros, "arquivo", (arg) => nomeArquivo = LerValorArgumentoString(arg, "Informe o nome do arquivo"));
             
            var parametroTipo = LerParametroComArgumentos(parametros, "tipo",
                (arg) => tipo = LerValorArgumentoDominio(arg,
                new string[] { "concurso", "calculo", "jogo" },
                "Tipo de exportação inválida. Opções: concurso, calculo ou jogo",
                "Tipo de exportação não informado"));

            switch (tipo)
            {
                case "concurso":
                    Exportador.Exportar(nomeArquivo, _concursos);
                    return $"{_concursos} concursos exportados para csv";
                case "jogo":
                    Exportador.Exportar(nomeArquivo, _jogos);
                    return $"{_jogos.Count()} jogos exportados para csv";
                case "calculo":
                default:
                    Exportador.Exportar(nomeArquivo, _linhas);
                    return $"{_linhas.Count()} cálculos exportados para csv";
            }

        }

        private static string LerValorArgumentoString(string arg, string mensagemArgumentoNaoEncontrado)
        {
            if (string.IsNullOrEmpty(arg)) throw new ApplicationException(mensagemArgumentoNaoEncontrado);
            return arg;
        }

        private static string InterpretarCalcular(List<string> parametros)
        {
            var tipo = "completo";
            var argumentoTipo = LerParametroComArgumentos(parametros, "tipo",
                (arg) => tipo = LerValorArgumentoDominio(arg,
                new string[] { "detalhado", "sintetico", "completo" },
                "Opção de cálculo inválida. Opções: detalhado, sintetico ou completo",
                "Informe o tipo de cálculo"));

            var calculadora = new Calculadora();
            calculadora.OnCalculoStart += Calculadora_OnCalculoStart;

            switch (tipo)
            {
                case "detalhado":
                    _linhas = calculadora.CalculoDetalhado(_concursos, _jogos).ToList();
                    break;
                case "sintetico":
                    _linhas = calculadora.CalculoSintetico(_concursos, _jogos).ToList();
                    break;
                case "completo":
                    _linhas = calculadora.CalculoCompleto(_concursos, _jogos).ToList();
                    break;
            }
            return $"{_linhas.Count()} linhas calculadas";
        }

        private static string InterpretarAjuda(List<string> parametros)
        {
            if (parametros.Count > 0)
            {
                var comando = parametros[0];
                switch (comando)
                {
                    case "importar":
                        return @"Exemplos: importar data > 10/10/2018
importar numero > 1822
importar data entre 10/05/2019 22/07/2018
importar numero = 1822";
                    case "gerar":
                        return $@"Exemplos: gerar [jogos 10] [primo] [somaDezenas] [parImpar] [numerosErrados 1,2,3,4,5]
Argumentos entre [] são opcionais
Caso não informe a quantidade de jogos, será gerada a quantidade padrão de {Parametro.QtdJogos} jogos
caso não informe os números errados todos os números válidos serão permutados";
                    case "calcular":
                        return "Exemplo: calcular tipo [sintetico|detalhado|completo]";
                    case "exportar":
                        return "Exemplo: exportar arquivo nomeArquivo.csv tipo [jogo|concurso|calculo]";
                    case "listar":
                        return "Exemplo: listar tipo [jogo|concurso|calculo]";
                    case "forcaBruta":
                        return "Exemplo: forcaBruta [palpites 500]";
                    case "sair":
                        return "Exemplo: sair";
                    case "ajuda":
                        return "Exemplo: ajuda [importar|gerar|calcular|listar|sair|ajuda]";
                    default:
                        return $"O comando {comando} não existe";
                }
            }
            else
            {
                return @"1.)importar: Importa a base da caixa
2.)gerar: Gera jogos com permutações
3.)calcular: Cruza ganhos e gastos dos jogos gerados com os concursos importados
4.)exportar: exporta calculos, jogos e concursos para CSV
5.)listar: mostra calculos, jogos e concursos em tela
6.)forcaBruta: encontra a sequencia de 5 números errados mais lucrativa
7.)sair: sai deste programa
8.)ajuda: exibe ajuda de um determinado comando ex: ajuda gerar";
            }
        }

        private static string InterpretarGerar(List<string> parametros)
        {
            var qtdJogos = Parametro.QtdJogos;
            var parametroQtdJogos = LerParametroComArgumentos(parametros, "jogos",
                (arg) => qtdJogos = LerValorArgumento<int>(arg, "Quantidade de jogos inválida. Ex: gerar jogos:10", "Quantidade de jogos deve ser numérica"));

            var numerosErrados = new int[] { };
            var parametroNumerosErrados = LerParametroComArgumentos(parametros, "numerosErrados",
                (arg) =>
                {
                    if (arg == null) throw new ApplicationException("Números errados inválidos. Ex: gerar numerosErrados:1,2,3,4,5");
                    numerosErrados = arg.Split(',').Select(n => LerValorArgumento<int>(n, "Os números da lista de números errados devem ser todos numéricos", string.Empty)).ToArray();
                });


            var parImpar = LerParametro(parametros, "parImpar");
            var dezenas = LerParametro(parametros, "somaDezenas");
            var primo = LerParametro(parametros, "primos");
            var planilha = LerParametro(parametros, "planilha");

            var gerador = new Gerador(parImpar, primo, dezenas, planilha);
            gerador.OnGerarStart += Gerador_OnGerarStart;

            _jogos = gerador.GerarJogos(numerosErrados, qtdJogos);
            return $"{_jogos.Count()} jogos gerados";
        }

        private static string InterpretarImportar(List<string> parametros)
        {
            var dataInicio = DateTime.MinValue;
            var dataFim = DateTime.MaxValue;
            var numeroInicio = 0;
            var numeroFim = int.MaxValue;
            var operadorData = string.Empty;
            var operadorNumero = string.Empty;

            var filtrarData = LerParametroComArgumentos(parametros, "data",
                (arg) => operadorData = LerValorArgumentoDominio(arg, operadoresValidos, "Operador inválido. Opções =, >, < ou entre", "Informe o operador. Opções =, >, < ou entre"),
                (arg) => dataInicio = LerValorArgumento<DateTime>(arg, "Data de concurso deve ser uma data válida (Ex: 22/10/2019)",
                operadorData == "entre"
                ? "Informe a data de início"
                : "Informe a data de filtro"),
                (arg) =>
                {
                    if (operadorData == "entre")
                    {
                        dataFim = LerValorArgumento<DateTime>(arg, "Data final deve ser uma data válida", "Informe a data final");
                    }
                });

            var filtrarNumero = LerParametroComArgumentos(parametros, "numero",
                  (arg) => operadorNumero = LerValorArgumentoDominio(arg, operadoresValidos, "Operador inválido. Opções =, >, < ou entre", "Informe o operador. Opções =, >, < ou entre"),
                (arg) => numeroInicio = LerValorArgumento<int>(arg, "Número de concurso deve ser numérico",
                operadorNumero == "entre"
                ? "Informe o número de início"
                : "Informe o número de filtro"),
                (arg) =>
                {
                    if (operadorNumero == "entre")
                    {
                        numeroFim = LerValorArgumento<int>(arg, "Número final deve ser numérico", "Informe o número final");
                    }
                });

            _concursos = new Importador().Importar();

            if (filtrarData)
            {
                switch (operadorData)
                {
                    case "=":
                        _concursos = _concursos.Where(c => c.Data == dataInicio);
                        break;
                    case ">":
                        _concursos = _concursos.Where(c => c.Data > dataInicio);
                        break;
                    case "<":
                        _concursos = _concursos.Where(c => c.Data < dataInicio);
                        break;
                    case "entre":
                        _concursos = _concursos.Where(c => c.Data >= dataInicio && c.Data <= dataFim);
                        break;
                }
            }
            else if (filtrarNumero)
            {
                switch (operadorNumero)
                {
                    case "=":
                        _concursos = _concursos.Where(c => c.Numero == numeroInicio);
                        break;
                    case ">":
                        _concursos = _concursos.Where(c => c.Numero > numeroInicio);
                        break;
                    case "<":
                        _concursos = _concursos.Where(c => c.Numero < numeroInicio);
                        break;
                    case "entre":
                        _concursos = _concursos.Where(c => c.Numero >= numeroInicio && c.Numero <= numeroFim);
                        break;
                }
            }

            return $"{_concursos.Count()} concursos importados";
        }

        private static void Calculadora_OnCalculoStart(object sender, EventArgs e)
        {
            Console.WriteLine("Calculando...");
        }

        private static void Gerador_OnGerarStart(object sender, EventArgs e)
        {
            Console.WriteLine("Gerando jogos...");
        }

        private static bool LerParametro(List<string> parametros, string nomeParametro)
        {
            return parametros.FirstOrDefault(x => x.Equals(nomeParametro)) != null;
        }
         
        private static T LerValorArgumento<T>(string arg, string mensagemArgumentoInvalido, string mensagemArgumentoNaoEncontrado) where T : struct
        {
            if (string.IsNullOrEmpty(arg)) throw new ApplicationException(mensagemArgumentoNaoEncontrado);
            var valor = arg.To<T>();
            if (!valor.HasValue) throw new ApplicationException(mensagemArgumentoInvalido);
            return valor.Value;
        }

        private static string LerValorArgumentoDominio(string arg, string[] dominio, string mensagemArgumentoInvalido, string mensagemArgumentoNaoEncontrado)
        {
            if (string.IsNullOrEmpty(arg)) throw new ApplicationException(mensagemArgumentoNaoEncontrado);
            if (!dominio.Contains(arg)) throw new ApplicationException(mensagemArgumentoInvalido);
            return arg;
        }

        private static bool LerParametroComArgumentos(List<string> parametros, string nomeParametro, params Action<string>[] lerArgumentosHandlers)
        {
            var parametro = parametros.FirstOrDefault(p => p.StartsWith(nomeParametro));
            if (parametro != null)
            {
                var index = parametros.IndexOf(parametro);
                var i = index + 1;
                foreach (var handler in lerArgumentosHandlers)
                {
                    var arg = parametros.Skip(i++).Take(1).FirstOrDefault();
                    handler(arg);
                }
            }
            return parametro != null;
        }
    }

}
