using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace ValidaLayout
{
    class Program
    {
        static void Main(string[] args)
        {
            string archive="";
            string entry_before = "E";
            int    entry_number_before = 0;

            string entry = "S";
            int    entry_number = 0;

            if(args.Length == 0 )
            {
                archive = "AFDT_00100_170220231225.txt";
            }
            else if(args[0].Equals("help"))
            {
                Console.WriteLine("Validador AFDT Versão 0.0.1");
                Console.WriteLine("Abra o cmd na pasta do Executável e digite:");
                Console.WriteLine("ValidaLayout NomeArquivoAFDT.txt");
                Console.WriteLine("");
                Console.WriteLine("Então o programa gera um arquivo com o nome NomeArquivoAFDT.txt.log no mesmo diretório.");
                Console.ReadLine();
                Environment.Exit(1);
            }
            else
            {
                archive = args[0];
            }             

            bool count_error=false;
            int count=0;
            int count_pis=0;
            string pis= "";
            string pis_anterior= "";
            string dia="";
            string dia_anterior="";
            string[] lines;
        
            lines = System.IO.File.ReadAllLines(archive);
            
            RegistraLog log = new RegistraLog(archive+".log");

            log.add("Inicia leitura do arquivo");
            foreach (string line in lines)
            {
                count++;
                
                if(count.Equals(1) || line.Length < 50 )
                {
                    continue;                    
                }

                //valida contagem
                if(!count.Equals(Convert.ToInt32(line.Substring(0,9))) && !count_error)
                {
                    count_error = true;
                    log.add($"Divergência: Erro de contagem. Linha: {count}. Esperado: {count}. Encontrado: "+Convert.ToInt32(line.Substring(0,9)));
                }
 
                //valida entrada/saída.
                //se o registro for S01 o anterior deve ser E01: inverte de E para S e não muda o número.
                //se o registro for E02 o anteriro deve ser S01: inverte de S para E e diminui um número.
                pis = line.Substring(23,12);
                dia = line.Substring(11,8);
                entry = line.Substring(51,1);
                entry_number = Convert.ToInt16(line.Substring(52,2));
                //Console.WriteLine(pis);
                if(count>2)
                {
                    if(pis.Equals(pis_anterior) && dia.Equals(dia_anterior))
                    {
                        count_pis++;
                    }
                    else
                    {
                        count_pis =0;
                    }
                    
                    if(count_pis > 4)
                    {
                        log.add($"Divergência: Colaborador com mais de quatro apontamentos na linha: {count}. PIS: {pis}");
                    }
                    /*
                    if(entry.Equals("S"))
                    {
                        
                    }
                    
                    if(entry.Equals(entry_before) && count > 1)
                    {
                        log.add($"Divergência: Erro na Entrada/Saída. Linha: {count}");
                    }

                    if(entry_number.Equals(entry_before))
                    {
                        log.add($"Divergência: Erro na Entrada/Saída. Linha: {count}");
                    }*/
                }                
                entry_before = entry;
                entry_number_before = entry_number;
                pis_anterior = pis;
                dia_anterior = dia;
            }
        }
    }
}