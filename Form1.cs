using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aula26
{


    public class No
    {
        public int x;
        public int y;
        public int vertice;
        /// <summary>
        /// função de construção.
        /// </summary>
        /// <param name="x">int abscissa do nó</param>
        /// <param name="y">int ordenada do nó</param>
        /// <param name="vertice">int número do nó</param>
        public No(int x, int y, int vertice){ 
            this.x = x;
            this.y = y;
            this.vertice = vertice;
        }
    }

    public partial class Form1 : Form{
        public Form1()
        {
            InitializeComponent();
        }

        public List<No> nos = new List<No>(); // nos, lista de objetos da classe No.

        private void button1_Click(object sender, EventArgs e){
            
            nos.Add(new No(100, 159, 1));  
            nos.Add(new No(184, 93, 2));
            nos.Add(new No(219, 339, 3));
            nos.Add(new No(369, 116, 4));
            nos.Add(new No(55, 283, 5));
            nos.Add(new No(364, 240, 6));

            //Matriz das distâncias
            int Dimensao = nos.Count;
            double[,] MatrizDistancias = new double[Dimensao, Dimensao];

            for (int i=0;i<nos.Count;i++){
                for (int j=0;j<nos.Count;j++){
                    if (i==j){
                        MatrizDistancias[i,j]=0;
                        richTextBox1.AppendText("Distância entre" + i.ToString() +" e "+ j.ToString() +" = 0"+ "\r\n"); richTextBox1.ScrollToCaret();
                    }
                    else{

                        MatrizDistancias[i,j] = CalcularDistanciaEntreNos(nos[i],nos[j]);
                        richTextBox1.AppendText("Distância entre" + i.ToString() + " e " + j.ToString() + " = " + MatrizDistancias[i, j].ToString() + "\r\n"); richTextBox1.ScrollToCaret();

                    }
                }
            }

            double menorDistancia = 0;
            int idx1 = 0;//i
            int idx2 = 1;//j
            menorDistancia = MatrizDistancias[0,1]; // ou maior valor. 

            for (int i = 0; i < nos.Count; i++){
                for (int j = 0; j < nos.Count; j++){
                    if (i != j){
                        if(MatrizDistancias[i,j] < menorDistancia){
                            menorDistancia = MatrizDistancias[i,j];
                            idx1 = i;
                            idx2 = j;
                        }
                    }
                }
            }

            List<int> links = new List<int>();
            //Primeiro link é o nó.idx1 com o nó idx2 e a distância entre eles é a menor distância
            richTextBox1.AppendText("Menor distância é entre " + idx1.ToString() + " e " + idx2.ToString() + " com " + menorDistancia.ToString() + "\r\n"); richTextBox1.ScrollToCaret();
            links.Add(idx1); //índice dos nos {0,1,2...}
            links.Add(idx2);
            links.Add(idx1);

            imprimirCaminho(links);

            //Criar uma lista com os números dos vértices para inserir: {0,1,2,3,4,5}
            List<int> numeros = new List<int>();
            for(int i=0;i<Dimensao;i++){ //trabalhando com os índices
                numeros.Add(i);
            }
            numeros.Remove(idx1);
            numeros.Remove(idx2);

            int contadorCombinacoes = 2;
            int controleDeInfinito = 0;
            int tamanhoMaximoLinha = 100;

            // Guardar somente a de menor custo.
            List<int[]> possibilidades = new List<int[]>();
            int[] linha = new int[tamanhoMaximoLinha];
            double[] custo = new double[tamanhoMaximoLinha];
            double menorCustoLinha = 10000000;
            int indiceMenorCustoLinha = 0;
            int[] numeroUsadoNaPossibilidade = new int[1000]; //Um dia vai estourar.
            int indiceDoNumeroUsadoNaPossibilidade = 0;

            int casaDeComeco = 1;
            int casaDeFim = links.Count;
            int colunaAtual = 0;

            while (numeros.Count > 0 && controleDeInfinito < 10000){

                possibilidades = new List<int[]>();
                linha = new int[tamanhoMaximoLinha];
                custo = new double[tamanhoMaximoLinha];
                menorCustoLinha = 10000000;
                indiceMenorCustoLinha = 0 ;
                numeroUsadoNaPossibilidade = new int[1000]; //Um dia vai estourar.
                indiceDoNumeroUsadoNaPossibilidade =0;           


                for (int i=0;i<numeros.Count;i++){ //repetir 4 vezes, dois a dois. R1.
                    //Inserir um número de numeros em link 
                    casaDeComeco = 1;
                    casaDeFim = links.Count;
                    colunaAtual = 0;

                    //Compara dois a dois
                    for (int j=0;j<contadorCombinacoes; j++){
                        linha = new int[tamanhoMaximoLinha];
                        linha = zerarLista(linha);
                        foreach (int l in links){
                            if (l != -1){
                                if(colunaAtual == casaDeComeco){
                                    linha[colunaAtual] = numeros[i];
                                    colunaAtual++;
                                }
                                linha[colunaAtual] = l;
                                colunaAtual++;
                            }
                        }
                        possibilidades.Add(linha);
                        numeroUsadoNaPossibilidade[indiceDoNumeroUsadoNaPossibilidade] = numeros[i];
                        indiceDoNumeroUsadoNaPossibilidade++;
                        colunaAtual = 0;
                        casaDeComeco++;
                    }
                }

                for (int k = 0; k < possibilidades.Count-1; k++){ 
                
                        custo[k] = calcularTamanhoPossibilidade(possibilidades[k]);
                        if (custo [k] < menorCustoLinha){
                            menorCustoLinha = custo[k];
                            indiceMenorCustoLinha = k;
                        
                        }
                }
                links = possibilidades[indiceMenorCustoLinha].ToList<int>();
                //links = transformarEmLista(possibilidades[indiceMenorCustoLinha]);

                //MessageBox.Show(indiceMenorCustoLinha.ToString());
                //MessageBox.Show(numeroUsadoNaPossibilidade[indiceMenorCustoLinha].ToString());
                numeros.Remove(numeroUsadoNaPossibilidade[indiceMenorCustoLinha]);
                

                contadorCombinacoes++;

                controleDeInfinito++;
                imprimirCaminho(links);

            } // Fim do While.

            //Mostrar a distância final.
            richTextBox1.AppendText("Custo Caminho: " + calcularTamanhoPossibilidade(transformarArray(links)).ToString()); richTextBox1.ScrollToCaret();


        } // final botão.


        /* transformarEmLista 
         * Transforma um array de inteiros em uma lista de inteiros
         * @param int[] array que será convertido
         * @returns List<int> a lista convertida
         */ 
        public List<int> transformarEmLista(int[] v){
            List<int> r = new List<int>();
            foreach( int elemento in v){
                r.Add(elemento);
            }
            return r;

        }

        /* transformarArray
         * Transforma uma lista de inteiros em um array de inteiros
         * @param List<int> lista que será convertida
         * @returns int[] o array convertido
         */
        public int[] transformarArray(List<int> v){
            int[] r = new int[v.Count];
            for(int i=0; i<v.Count; i++){
                r[i]= v[i];
            }
            return r;
        }

        /* imprimirCaminho
         * Imprime os números do caminho que estão em links.
         * @param List<int> lista de inteiros que será impressa.
         */
        public void imprimirCaminho(List<int> links){
            richTextBox1.AppendText("Caminho: "); richTextBox1.ScrollToCaret();
            foreach (int n in links){
                if(n != -1){
                    richTextBox1.AppendText(n.ToString() + ", "); richTextBox1.ScrollToCaret();
                }
            }
            richTextBox1.AppendText("\r\n"); richTextBox1.ScrollToCaret();
        }

        /* calcularTamanhoPossibilidade
         * Calcula a soma das distâncias entre os nós do caminho
         * @param int[] array de índices de nós
         * @returns double soma das distâncias entre os nós do caminho
         */
        public double calcularTamanhoPossibilidade(int[] caminho){
            double trajeto = 0;
            int idx1 = 0;
            int idx2 = 0;
            for(int i=1;i<caminho.Length;i++){
                idx1 = caminho [i-1];
                idx2 = caminho [i];
                if(idx1!=-1 && idx2 !=-1){
                    trajeto = trajeto + CalcularDistanciaEntreNos(nos[idx1],nos[idx2]);
                }
            }
            return trajeto;
        }

        /* zerarLista
         * Preenche os arrays de números com -1, ao invés de 0.
         * @param int[] array de inteiros
         * @returns int[] o array inteiro preenchido com -1.
         */
        public int[] zerarLista(int[] lista){
            for(int i=0;i<lista.Length;i++){
                lista[i]=-1;
            }
            return lista;
        }

        /* CalcularDistanciaEntreNos
         * Calcula a distância euclidiana entre dois nós
         * @param No primeiro nó
         * @param No segundo nó
         * @returns double distância entre os nós.
         */
        public double CalcularDistanciaEntreNos(No n1, No n2){
            double DeltaX = n1.x - n2.x;
            double DeltaY = n1.y - n2.y;
            return Math.Sqrt(DeltaX * DeltaX + DeltaY * DeltaY);

        }

    }
}
