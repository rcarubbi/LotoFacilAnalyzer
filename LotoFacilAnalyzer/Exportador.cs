using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LotoFacilAnalyzer
{
    public static class Exportador
    {

        public static void Exportar(string nomeArquivo, IEnumerable<Concurso> concursos)
        {
            var linhas = concursos.ToArray().Select(x =>
            $"{x.Data};" +
            $"{x.Numero};" +
            $"{x.Bolas[0]};" +
            $"{x.Bolas[1]};" +
            $"{x.Bolas[2]};" +
            $"{x.Bolas[3]};" +
            $"{x.Bolas[4]};" +
            $"{x.Bolas[5]};" +
            $"{x.Bolas[6]};" +
            $"{x.Bolas[7]};" +
            $"{x.Bolas[8]};" +
            $"{x.Bolas[9]};" +
            $"{x.Bolas[10]};" +
            $"{x.Bolas[11]};" +
            $"{x.Bolas[12]};" +
            $"{x.Bolas[13]};" +
            $"{x.Bolas[14]};").ToList();
            linhas.Insert(0, "Data;Numero;Bola 1;Bola 2;Bola 3;Bola 4;Bola 5;Bola 6;Bola 7;Bola 8;Bola 9;Bola 10;Bola 11;Bola 12;Bola 13;Bola 14;Bola 15");
            File.WriteAllLines(nomeArquivo, linhas);
        }

        public static void Exportar(string nomeArquivo, IEnumerable<IEnumerable<int>> jogos)
        {
            var linhas = jogos.ToArray().Select(j =>
            {
                var x = j.ToArray();
                return $"{x[0]};" +
                $"{x[1]};" +
                $"{x[2]};" +
                $"{x[3]};" +
                $"{x[4]};" +
                $"{x[5]};" +
                $"{x[6]};" +
                $"{x[7]};" +
                $"{x[8]};" +
                $"{x[9]};" +
                $"{x[10]};" +
                $"{x[11]};" +
                $"{x[12]};" +
                $"{x[13]};" +
                $"{x[14]};";
            }).ToList();
            linhas.Insert(0, "Bola 1;Bola 2;Bola 3;Bola 4;Bola 5;Bola 6;Bola 7;Bola 8;Bola 9;Bola 10;Bola 11;Bola 12;Bola 13;Bola 14;Bola 15");
            File.WriteAllLines(nomeArquivo, linhas);
        }
        public static void Exportar(string nomeArquivo, IEnumerable<Linha> linhas)
        {
            var lines = linhas.Select(x =>
            $"{x.DataConcurso};" +
            $"{x.NumeroConcurso};" +
            $"{x.N1Resultado};" +
            $"{x.N2Resultado};" +
            $"{x.N3Resultado};" +
            $"{x.N4Resultado};" +
            $"{x.N5Resultado};" +
            $"{x.N6Resultado};" +
            $"{x.N7Resultado};" +
            $"{x.N8Resultado};" +
            $"{x.N9Resultado};" +
            $"{x.N10Resultado};" +
            $"{x.N11Resultado};" +
            $"{x.N12Resultado};" +
            $"{x.N13Resultado};" +
            $"{x.N14Resultado};" +
            $"{x.N15Resultado};" +
            $"{x.N1Jogo};" +
            $"{x.N2Jogo};" +
            $"{x.N3Jogo};" +
            $"{x.N4Jogo};" +
            $"{x.N5Jogo};" +
            $"{x.N6Jogo};" +
            $"{x.N7Jogo};" +
            $"{x.N8Jogo};" +
            $"{x.N9Jogo};" +
            $"{x.N10Jogo};" +
            $"{x.N11Jogo};" +
            $"{x.N12Jogo};" +
            $"{x.N13Jogo};" +
            $"{x.N14Jogo};" +
            $"{x.N15Jogo};" +
            $"{x.Pontos};" +
            $"{x.Gasto};" +
            $"{x.Ganho};" +
            $"{x.Saldo};").ToList();
            lines.Insert(0, "Data;Numero;" +
                "Resultado 1;Resultado 2;Resultado 3;Resultado 4;Resultado 5;Resultado 6;Resultado 7;Resultado 8;Resultado 9;Resultado 10;" +
                "Resultado 11; Resultado 12; Resultado 13; Resultado 14; Resultado 15;" +
                "Jogo 1;Jogo 2;Jogo 3;Jogo 4;Jogo 5;Jogo 6;Jogo 7;Jogo 8;Jogo 9;Jogo 10;Jogo 11;Jogo 12;Jogo 13;Jogo 14;Jogo 15;Pontos;Gasto;Ganho;Saldo");
            File.WriteAllLines(nomeArquivo, lines);
        }
    }
}
