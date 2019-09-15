using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LotoFacilAnalyzer
{
    public class Importador
    {
        public IEnumerable<Concurso> Importar()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
            var file = Directory.GetFiles(path, "*.xlsx").FirstOrDefault();
            if (file == null) throw new ApplicationException("Nenhuma planilha encontrada na pasta");

            using (var fs = new FileStream(file, FileMode.Open))
            using (var reader = ExcelReaderFactory.CreateReader(fs))
            {
                var indiceLinha = 0;
                while (reader.Read())
                {
                    indiceLinha++;
                    if (indiceLinha <= Parametro.QtdLinhasCabecalhoPlanilha) continue;

                    yield return LerConcurso(reader);

                }
            }

        }

        private static Concurso LerConcurso(IExcelDataReader reader)
        {
            var a = reader[0];
            List<int> bolas = new List<int>();
            for (int i = 2; i < 17; i++)
            {
                bolas.Add(Convert.ToInt32(reader[i]));
            }

            return new Concurso
            {
                Data = Convert.ToDateTime(reader[1]),
                Numero = Convert.ToInt32(reader[0]),
                Bolas = bolas.ToArray()
            };
        }

    }
}
