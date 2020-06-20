using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Utworzenie macierzy miast
            int licznik = 0;
            string linia;
            StreamReader sr = new StreamReader(@"C:\\Users\dlugo\source\repos\ConsoleApp1\kroC100.txt");
            int iloscmiast = int.Parse(sr.ReadLine());
            int[,] tablicamiast = new int[iloscmiast, iloscmiast];

            while ((linia = sr.ReadLine()) != null)
            {
                string[] liczby = linia.Split(' ');
                for (int i = 0; i < liczby.Length - 1; i++)
                {
                    int.TryParse(liczby[i], out tablicamiast[licznik, i]);
                }
                licznik++;
            }
            sr.Close();

            for (int i = iloscmiast - 1; i > 0; i--)
            {
                for (int j = 0; j < iloscmiast; j++)
                {
                    tablicamiast[j, i] = tablicamiast[i, j];
                }
            }


            Console.WriteLine();
            int losy = 10;
            int selekcja = 2;  //1. Koło ruletki  2. Selekcja turniejowa
            int krzyzowanie = 1;  //1. OX   2. UDBX
            int mutacja = 1; //1. Przez inwersję  2. Przez zamianę
            int parametrmutacji = 5;  //1. (1-5)   2. niezmiennie 5
            int petlowanie = 10000;
            Console.WriteLine();
            //Losowanie miast
            Random rand = new Random();
            int[,] tablicapomocnicza = new int[iloscmiast + 1, losy];
            int[,] tablicapopulacji = new int[iloscmiast + 1, losy];
            for (int j = 0; j < losy; j++)
            {
                int n = iloscmiast;
                for (int i = 0; i < iloscmiast; i++)
                {
                    tablicapomocnicza[i, j] = i + 1;
                }

                for (int i = 0; i < iloscmiast; i++)
                {
                    int r = rand.Next(n);
                    tablicapopulacji[i, j] = tablicapomocnicza[r, j];
                    tablicapomocnicza[r, j] = tablicapomocnicza[n - 1, j];
                    n--;
                }
            }

            //Liczenie odległości
            int a;
            int b;
            int odleglosc;

            for (int j = 0; j < losy; j++)
            {
                odleglosc = 0;
                for (int i = 0; i < iloscmiast; i++)
                {
                    a = tablicapopulacji[i, j];
                    if (i == iloscmiast - 1)
                    {
                        b = tablicapopulacji[0, j];
                    }
                    else
                    {
                        b = tablicapopulacji[i + 1, j];
                    }
                    odleglosc = odleglosc + tablicamiast[a - 1, b - 1];
                }
                tablicapopulacji[iloscmiast, j] = odleglosc;
            }

            for(int p = 0; p < petlowanie; p++)
            {
                //----------**********SELEKCJA**********----------
                switch (selekcja)
                {
                    case 1:
                        //Koło ruletki
                        int max = 0;
                        int sumaszans = 0;
                        for (int j = 0; j < losy; j++)
                        {
                            if (tablicapopulacji[iloscmiast, j] > max)
                            {
                                max = tablicapopulacji[iloscmiast, j];
                            }
                        }
                        max++;

                        int[,] tablicakolaruletki = new int[3, losy];

                        for (int j = 0; j < losy; j++)
                        {
                            tablicakolaruletki[0, j] = max - tablicapopulacji[iloscmiast, j];
                            sumaszans = sumaszans + tablicakolaruletki[0, j];
                            if (j == 0)
                            {
                                tablicakolaruletki[1, j] = 0;
                                tablicakolaruletki[2, j] = tablicakolaruletki[0, j];
                            }
                            else if (j == losy - 1)
                            {
                                tablicakolaruletki[1, j] = sumaszans - tablicakolaruletki[0, j];
                                tablicakolaruletki[2, j] = sumaszans;
                            }
                            else
                            {
                                tablicakolaruletki[1, j] = tablicakolaruletki[2, j - 1];
                                tablicakolaruletki[2, j] = tablicakolaruletki[1, j] + tablicakolaruletki[0, j];
                            }
                        }
                        for (int k = 0; k < losy; k++)
                        {
                            int r = rand.Next(0, sumaszans);
                            for (int j = 0; j < losy; j++)
                            {
                                if (r >= tablicakolaruletki[1, j] && r < tablicakolaruletki[2, j])
                                {
                                    for (int i = 0; i < iloscmiast + 1; i++)
                                    {
                                        tablicapomocnicza[i, k] = tablicapopulacji[i, j];
                                    }
                                }
                            }
                        }
                        for (int j = 0; j < losy; j++)
                        {
                            for (int i = 0; i < iloscmiast + 1; i++)
                            {
                                tablicapopulacji[i, j] = tablicapomocnicza[i, j];
                            }
                        }
                        break;
                    case 2:
                        //Selekcja turniejowa
                        int[,] tablicaturniejowa = new int[iloscmiast + 1, 3];
                        for (int j = 0; j < losy; j++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                int populacja1 = rand.Next(1, losy + 1);
                                int populacja2 = rand.Next(1, losy + 1);
                                int populacja3 = rand.Next(1, losy + 1);
                                if (tablicapopulacji[iloscmiast, populacja1 - 1] <= tablicapopulacji[iloscmiast, populacja2 - 1] && tablicapopulacji[iloscmiast, populacja1 - 1] <= tablicapopulacji[iloscmiast, populacja3 - 1])
                                {
                                    for (int k = 0; k < iloscmiast + 1; k++)
                                    {
                                        tablicaturniejowa[k, i] = tablicapopulacji[k, populacja1 - 1];
                                    }
                                }
                                else if (tablicapopulacji[iloscmiast, populacja2 - 1] < tablicapopulacji[iloscmiast, populacja1 - 1] && tablicapopulacji[iloscmiast, populacja2 - 1] < tablicapopulacji[iloscmiast, populacja3 - 1])
                                {
                                    for (int k = 0; k < iloscmiast + 1; k++)
                                    {
                                        tablicaturniejowa[k, i] = tablicapopulacji[k, populacja2 - 1];
                                    }
                                }
                                else
                                {
                                    for (int k = 0; k < iloscmiast + 1; k++)
                                    {
                                        tablicaturniejowa[k, i] = tablicapopulacji[k, populacja3 - 1];
                                    }
                                }
                            }
                            if (tablicaturniejowa[iloscmiast, 0] <= tablicaturniejowa[iloscmiast, 1] && tablicaturniejowa[iloscmiast, 0] <= tablicaturniejowa[iloscmiast, 2])
                            {
                                for (int m = 0; m < iloscmiast + 1; m++)
                                {
                                    tablicapomocnicza[m, j] = tablicaturniejowa[m, 0];
                                }
                            }
                            else if (tablicaturniejowa[iloscmiast, 1] < tablicaturniejowa[iloscmiast, 0] && tablicaturniejowa[iloscmiast, 1] < tablicaturniejowa[iloscmiast, 2])
                            {
                                for (int m = 0; m < iloscmiast + 1; m++)
                                {
                                    tablicapomocnicza[m, j] = tablicaturniejowa[m, 1];
                                }
                            }
                            else
                            {
                                for (int m = 0; m < iloscmiast + 1; m++)
                                {
                                    tablicapomocnicza[m, j] = tablicaturniejowa[m, 2];
                                }
                            }
                        }
                        for (int j = 0; j < losy; j++)
                        {
                            for (int i = 0; i < iloscmiast + 1; i++)
                            {
                                tablicapopulacji[i, j] = tablicapomocnicza[i, j];
                            }
                        }
                        break;
                }

                //----------**********KRZYŻOWANIE**********----------
                int[,] tablicakrzyzowania = new int[iloscmiast + 1, losy];
                int[] rodzic1 = new int[iloscmiast];
                int[] rodzic2 = new int[iloscmiast];
                int[] potomek1 = new int[iloscmiast];
                int[] potomek2 = new int[iloscmiast];
                switch (krzyzowanie)
                {
                    case 1:
                        //OX
                        int[] tablicapomocniczak = new int[iloscmiast + 1];
                        int ciecie1;
                        int ciecie2;
                        int k;
                        for (int j = 0; j < losy; j++)
                        {
                            for (int i = 0; i < iloscmiast + 1; i++)
                            {
                                tablicakrzyzowania[i, j] = tablicapopulacji[i, j];
                                tablicakrzyzowania[i, j + 1] = tablicapopulacji[i, j + 1];
                            }
                            ciecie1 = rand.Next(0, iloscmiast - (iloscmiast / 10));
                            ciecie2 = rand.Next(ciecie1 + 2, iloscmiast + 1);

                            //Wymiana środków
                            for (int i = 0; i < iloscmiast + 1; i++)
                            {
                                if (i >= ciecie1 && i < ciecie2)
                                {
                                    tablicapomocniczak[i] = tablicakrzyzowania[i, j];
                                    potomek1[i] = tablicakrzyzowania[i, j + 1];
                                    potomek2[i] = tablicapomocniczak[i];
                                }
                            }

                            //Rodzice(pomocniczo)
                            for (int i = 0; i < iloscmiast; i++)
                            {
                                rodzic1[i] = tablicakrzyzowania[i, j];
                                rodzic2[i] = tablicakrzyzowania[i, j + 1];
                            }
                            //Krzyżowanie
                            if (ciecie2 == iloscmiast)
                            {
                                k = 0;
                            }
                            else
                            {
                                k = ciecie2;
                            }
                            int l = k;
                            for (int i = k; i < iloscmiast; i++)
                            {
                                if (!(potomek1.Contains(rodzic1[i])))
                                {
                                    potomek1[l] = rodzic1[i];
                                    l++;
                                }
                                if (i == iloscmiast - 1)
                                {
                                    i = -1;
                                }
                                if (l == iloscmiast)
                                {
                                    l = 0;
                                }
                                if (l == ciecie1)
                                {
                                    break;
                                }
                            }
                            l = k;
                            for (int i = k; i < iloscmiast; i++)
                            {
                                if (!(potomek2.Contains(rodzic2[i])))
                                {
                                    potomek2[l] = rodzic2[i];
                                    l++;
                                }
                                if (i == iloscmiast - 1)
                                {
                                    i = -1;
                                }
                                if (l == iloscmiast)
                                {
                                    l = 0;
                                }
                                if (l == ciecie1)
                                {
                                    break;
                                }
                            }
                            for (int i = 0; i < iloscmiast; i++)
                            {
                                tablicapopulacji[i, j] = potomek1[i];
                                tablicapopulacji[i, j + 1] = potomek2[i];
                            }
                            Array.Clear(potomek1, 0, iloscmiast);
                            Array.Clear(potomek2, 0, iloscmiast);
                            j++;
                        }
                        break;
                    case 2:
                        //UDBX
                        int[] tablica01 = new int[iloscmiast];
                        for (int j = 0; j < losy; j++)
                        {
                            for (int i = 0; i < iloscmiast; i++)
                            {
                                tablicakrzyzowania[i, j] = tablicapopulacji[i, j];
                                tablicakrzyzowania[i, j + 1] = tablicapopulacji[i, j + 1];
                                tablica01[i] = rand.Next(0, 2);
                            }

                            //Rodzice
                            for (int i = 0; i < iloscmiast; i++)
                            {
                                rodzic1[i] = tablicakrzyzowania[i, j];
                                rodzic2[i] = tablicakrzyzowania[i, j + 1];
                            }

                            //Przepisanie odpowiadających wartości
                            for (int i = 0; i < iloscmiast; i++)
                            {
                                if (tablica01[i] == 0)
                                {
                                    potomek1[i] = rodzic1[i];
                                }
                                else
                                {
                                    potomek2[i] = rodzic2[i];
                                }
                            }
                            //Krzyżowanie
                            int l = 0;
                            for (int i = l; i < iloscmiast; i++)
                            {

                                if (potomek1[i] == 0)
                                {
                                    if (!(potomek1.Contains(rodzic2[l])))
                                    {
                                        potomek1[i] = rodzic2[l];
                                    }
                                    l++;
                                    i--;
                                }
                                if (l == iloscmiast)
                                {
                                    l = 0;
                                }
                            }
                            l = 0;
                            for (int i = l; i < iloscmiast; i++)
                            {

                                if (potomek2[i] == 0)
                                {
                                    if (!(potomek2.Contains(rodzic1[l])))
                                    {
                                        potomek2[i] = rodzic1[l];
                                    }
                                    l++;
                                    i--;
                                }
                                if (l == iloscmiast)
                                {
                                    l = 0;
                                }
                            }
                            for (int i = 0; i < iloscmiast; i++)
                            {
                                tablicapopulacji[i, j] = potomek1[i];
                                tablicapopulacji[i, j + 1] = potomek2[i];
                            }
                            Array.Clear(tablica01, 0, iloscmiast);
                            Array.Clear(potomek1, 0, iloscmiast);
                            Array.Clear(potomek2, 0, iloscmiast);
                            j++;
                        }
                        break;
                }
                //----------**********MUTACJA**********----------
                int[,] tablicamutacji = new int[iloscmiast + 1, losy];
                switch (mutacja)
                {
                    case 1:
                        //Mutacja przez inwersję
                        int[] tablicamutacji2 = new int[iloscmiast + 1];
                        int ciecie1;
                        int ciecie2;
                        for (int j = 0; j < losy; j++)
                        {
                            int r = rand.Next(0, 101);
                            if (r <= parametrmutacji)
                            {

                                ciecie1 = rand.Next(0, iloscmiast - (iloscmiast / 10));
                                ciecie2 = rand.Next(ciecie1 + 2, iloscmiast + 1);
                                for (int i = 0; i < iloscmiast + 1; i++)
                                {
                                    tablicamutacji[i, j] = tablicapopulacji[i, j];
                                    tablicamutacji2[i] = tablicamutacji[i, j];
                                }
                                Array.Reverse(tablicamutacji2, ciecie1, ciecie2 - ciecie1);
                                for (int i = 0; i < iloscmiast + 1; i++)
                                {
                                    tablicapopulacji[i, j] = tablicamutacji2[i];
                                }
                            }
                        }
                        break;
                    case 2:
                        //Mutacja przez zamianę
                        for (int j = 0; j < losy; j++)
                        {
                            int r = rand.Next(0, iloscmiast * 100);
                            if (r <= parametrmutacji)
                            {
                                for (int i = 0; i < iloscmiast + 1; i++)
                                {
                                    tablicamutacji[i, j] = tablicapopulacji[i, j];
                                }
                                int r2 = rand.Next(2, 4);
                                if (r2 == 2)
                                {
                                    int indeks1 = rand.Next(0, iloscmiast);
                                    int indeks2 = rand.Next(0, iloscmiast);
                                    if (indeks1 == indeks2)
                                    {
                                        do
                                        {
                                            indeks2 = rand.Next(0, iloscmiast);
                                        } while (indeks1 == indeks2);
                                    }
                                    int indeks3 = tablicamutacji[indeks1, j];
                                    tablicapopulacji[indeks1, j] = tablicamutacji[indeks2, j];
                                    tablicapopulacji[indeks2, j] = indeks3;
                                }
                                else if (r2 == 3)
                                {
                                    int indeks1 = rand.Next(0, iloscmiast);
                                    int indeks2 = rand.Next(0, iloscmiast);
                                    int indeks3 = rand.Next(0, iloscmiast);
                                    if (indeks1 == indeks2 || indeks1 == indeks3 || indeks2 == indeks3)
                                    {
                                        do
                                        {
                                            indeks2 = rand.Next(0, iloscmiast);
                                            indeks3 = rand.Next(0, iloscmiast);
                                        } while (indeks1 == indeks2 || indeks1 == indeks3 || indeks2 == indeks3);
                                    }
                                    int indeks4 = tablicamutacji[indeks1, j];
                                    tablicapopulacji[indeks1, j] = tablicamutacji[indeks2, j];
                                    tablicapopulacji[indeks2, j] = tablicamutacji[indeks3, j];
                                    tablicapopulacji[indeks3, j] = indeks4;
                                }
                            }
                        }
                        break;
                }
                //Liczenie odległości
                for (int j = 0; j < losy; j++)
                {
                    odleglosc = 0;
                    if (tablicapopulacji[0, j] != 0)
                    {
                        for (int i = 0; i < iloscmiast; i++)
                        {
                            a = tablicapopulacji[i, j];
                            if (i == iloscmiast - 1)
                            {
                                b = tablicapopulacji[0, j];
                            }
                            else
                            {
                                b = tablicapopulacji[i + 1, j];
                            }
                            odleglosc = odleglosc + tablicamiast[a - 1, b - 1];
                        }
                    }
                    tablicapopulacji[iloscmiast, j] = odleglosc;
                }
                Console.WriteLine(p);
            }
            //Sortowanie
            int min = 35000;
            int[] najlepszywynik = new int[iloscmiast + 1];
            for(int j=0; j < losy; j++)
            {
                if(tablicapopulacji[iloscmiast,j] < min)
                {
                    min = tablicapopulacji[iloscmiast, j];
                    for(int i=0; i<iloscmiast +1; i++)
                    {
                        najlepszywynik[i] = tablicapopulacji[i, j];
                    }
                }
            }
            ////Wyświetlenie po wszystkim
            for (int i = 0; i < iloscmiast + 1; i++)
            {
                Console.Write(najlepszywynik[i] + " ");
            }
            //Zapisywanie do pliku
            using (StreamWriter wynik = new StreamWriter(@"C:\\Users\dlugo\source\repos\ConsoleApp1\wynik.txt"))
            {
                for(int i=0; i < iloscmiast; i++)
                {
                    wynik.Write(najlepszywynik[i] - 1);
                    if (i != iloscmiast - 1) 
                    {
                        wynik.Write("-");
                    }
                }
                wynik.Write(" " + najlepszywynik[iloscmiast]);
            }
            Console.ReadLine();
        }
    }
}