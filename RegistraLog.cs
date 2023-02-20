using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace ValidaLayout
{
    public class RegistraLog
    {
        
        private static string caminhoExe = string.Empty;
        public string fileName;
        public RegistraLog(string fileName = "ValidaLayout.log")
        {
            this.fileName = fileName;
        }

        public bool add(string strMensagem)
        {
            try
            {
                caminhoExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string caminhoArquivo = Path.Combine(caminhoExe, fileName);
                if (!File.Exists(caminhoArquivo))
                {
                    FileStream arquivo = File.Create(caminhoArquivo);
                    arquivo.Close();
                }
                using (StreamWriter w = File.AppendText(caminhoArquivo))
                {
                    AppendLog(strMensagem, w);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        private void AppendLog(string logMensagem, TextWriter txtWriter, string tipo = "txt")
        {
            try
            {
                if(tipo == "txt")
                {
                    txtWriter.WriteLine("------------------------------------");
                    txtWriter.Write("\r\nLog Entrada ");
                    txtWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                }
                txtWriter.WriteLine($"{logMensagem}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
