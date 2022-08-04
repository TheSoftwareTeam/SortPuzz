using System;
using System.IO;

namespace SortPuzz
{
    class Program
    {


        static void Main(string[] args)
        {
            string adimAtla = "";
            string islem;
            string eldeki = "__";

            int cikmazAdimsayaci = 0;
            int cikmazSayaci = 0;
            int geriAdimSayaci = 0;
            int hareketSayisi = 0;
            int adimSayisi = 0;
            int a = 0, b = 0;
            int adim = 1;

            bool tupUygun = false;
            bool tekliGeriAdim = true;
            bool tupDolu = false;
            bool eldekiUygun = false;


            string[] hareketDetay = new string[1000];
            string[] adimListesi = new string[1000];
            int sonDurumStringSayisi = 0;
            string[] cikmazAdimKaydi = new string[10000];
            string[,] sortPuzz = new string[,] { };



            while (true)
            {
                Console.Write("Renkleri klavyeden giriniz veya dosyadan seçiniz : Dosya/D - Klavye/K --> D - K = ");
                islem = Console.ReadLine();
                if (islem == "K")
                {
                    Console.WriteLine("Tüp sayısı gir :");//Tüp sayısı belirleme

                    int tupSayisiKlavye = int.Parse(Console.ReadLine());
                    sortPuzz = new string[tupSayisiKlavye, 4];

                    for (int i = 0; i < tupSayisiKlavye; i++)
                    {
                        Console.WriteLine("{0}. tüpteki renkler ", i + 1);//Renkleri klavyeden girme işlemi
                        int k = 0;

                        for (int j = 0; j < 4; j++)
                        {
                            Console.Write("{0}. tüpteki {1}. rengini gir : ", i + 1, k + 1);
                            sortPuzz[i, j] = Console.ReadLine();
                            k++;
                        }
                    }
                    break;

                }
                else if (islem == "D")
                {
                    //Dosya yollari
                    ////Murat-> 
                    //Mahir->D:/Arcelik/C#/
                    Console.Write("Dosya numarasını gir: ");
                    string dosyaNo = Console.ReadLine();
                    String textFile = File.ReadAllText(@"D:/Arcelik/C#/SortPuzz/renkKlasor/tubelist" + dosyaNo + ".txt");//Dosyadan renkleri çekme


                    //Dosyadaki tüp sayısını bulma
                    int tupSayisiDosya = 0;

                    foreach (var row in textFile.Split('\n'))
                    {
                        tupSayisiDosya++;
                    }

                    sortPuzz = new string[tupSayisiDosya, 4];
                    Console.WriteLine("Dosyadaki Tüp Sayisi : " + tupSayisiDosya);

                    int i = 0, j = 0;
                    //Dosyadan renkleri getirme ve sortPuzz çok boyutlu diziye kaydetme
                    foreach (var row in textFile.Split('\n'))
                    {
                        j = 0;
                        foreach (var col in row.Trim().Split(' '))
                        {
                            sortPuzz[i, j] = Convert.ToString(col.Trim());
                            j++;
                        }
                        i++;
                    }
                    break;

                }
                else
                {
                    Console.WriteLine("Yanlış tuşlama yaptınız. Yeniden Deneyin.");

                }
                Console.Clear();
            }

            int tupsayisi = sortPuzz.GetLength(0) - 1;
            string[,] sonDurumString = new string[10000, sortPuzz.GetLength(0)];
            string[,] sonDurumHareketListesi = new string[sortPuzz.GetLength(0), 4];


            //kullanıcıya ekranda anlık çıktıyı gösterir
            void cikti()
            {

                for (int j = 3; j >= 0; j--)
                {

                    for (int i = 0; i <= tupsayisi; i++)
                    {

                        Console.Write(sortPuzz[i, j] + " ");

                    }
                    Console.WriteLine();
                }

            }

            // son durum hareket listesindeki renklerin ilk iki karakterini string tipinde saklıyoruz
            void sondurum()
            {
                for (int i = 0; i <= tupsayisi; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        sonDurumString[sonDurumStringSayisi, i] = sonDurumString[sonDurumStringSayisi, i] + sortPuzz[i, j];
                    }
                }
                sonDurumStringSayisi++;

            }

            //son durum hareket listesini geri alıyoruz
            void geriAdim(int adim)
            {
                int sayac = 0;
                for (int j = 0; j <= tupsayisi; j++)
                {
                    for (int k = 0; k <= 3; k++)
                    {
                        for (int i = 0; i <= tupsayisi; i++)
                        {
                            for (int s = 0; s <= 3; s++)
                            {
                                if (sayac <= 14 && sonDurumString[adim, j].Substring(sayac, 2) == sortPuzz[i, s].Substring(0, 2))
                                {
                                    sonDurumHareketListesi[j, k] = sortPuzz[i, s];
                                }
                            }
                        }
                        sayac = sayac + 2;
                    }
                    sayac = 0;
                }

                for (int j = 3; j >= 0; j--)
                {
                    for (int i = 0; i <= tupsayisi; i++)
                    {
                        sortPuzz[i, j] = sonDurumHareketListesi[i, j];
                    }
                }
            }

            //eldeki verinin taşınmaya uygunluğunu kontrol eder uygun ise true döner
            void eldekiKontrol(int i, int j)
            {
                if (j == 3 && sortPuzz[i, j] != "__" || (j < 3 && sortPuzz[i, j] != "__" && sortPuzz[i, j + 1] == "__"))
                {
                    if (adimAtla != i.ToString())
                    {
                        eldekiUygun = true;
                    }
                    else
                    {
                        eldekiUygun = false;

                    }

                }
                else
                {
                    eldekiUygun = false;

                }
            }

            //tüplerin doluluğunu kontrol eder uygun ise true döner
            void tupDoluKontrol(int i)
            {
                if (sortPuzz[i, 0] == eldeki && sortPuzz[i, 1] == eldeki && sortPuzz[i, 2] == eldeki && sortPuzz[i, 3] == eldeki)
                {
                    tupDolu = true;
                }
                else
                {
                    tupDolu = false;
                }
            }

            //tüplerin taşınmaya uygunluğunu kontrol eder uygun ise true döner
            void tupKontrol(int i, int j, int k, int l)
            {
                tupDoluKontrol(i);
                if ((sortPuzz[k, l] == "__" && i != k && eldeki != "__")//baktığın yer boş VE aynı konum değil VE eldeki değişkeni boş değil ise VE
                        &&
                        (
                                (l == 0 && sortPuzz[k, 1] == "__" && sortPuzz[k, 2] == "__" && sortPuzz[k, 3] == "__") //baktığın tupun en altındayken VE tüpün tümü boş ise VEYA
                                ||
                                (l != 0 && sortPuzz[k, l - 1] == eldeki)//tüpün en altında değilken VE baktığın yerin bir altı eldeki ile aynı ise
                         )
                    )
                {
                    if (tupDolu == true || (j < 3 && sortPuzz[i, 0] == eldeki && sortPuzz[i, 1] == eldeki && sortPuzz[i, 2] == eldeki))//taşınacak tüpün içinde 3 adet eldekinin aynısı var ise 
                    {
                        tupUygun = false;
                    }
                    else
                    {

                        if (j != 0 && l == 3 && sortPuzz[i, j - 1] == eldeki && sortPuzz[k, l - 1] == eldeki)
                        {

                            tupUygun = false;

                        }
                        else
                        {

                            if (adimAtla != "")
                            {
                                a = Convert.ToInt32(adimAtla.Substring(0, 2));
                                b = Convert.ToInt32(adimAtla.Substring(adimAtla.Length - 2, 2));

                            }
                            if (adimAtla != "" && a == i + 1 && b == k + 1)
                            {
                                tupUygun = false;
                            }
                            else
                            {
                                if (sortPuzz[k, 0] == "__" && sortPuzz[k, 1] == "__" && sortPuzz[k, 2] == "__" && sortPuzz[k, 3] == "__"
                                    &&
                                    (
                                    j == 0
                                    ||
                                    (j == 1 && sortPuzz[i, j - 1] == sortPuzz[i, j])
                                    ||
                                    (j == 2 && sortPuzz[i, j - 1] == sortPuzz[i, j] && sortPuzz[i, j - 2] == sortPuzz[i, j])
                                    ||
                                    (j == 3 && sortPuzz[i, j - 1] == sortPuzz[i, j] && sortPuzz[i, j - 2] == sortPuzz[i, j] && sortPuzz[i, j - 2] == sortPuzz[i, j])))
                                {
                                    tupUygun = false;

                                }
                                else
                                {
                                    tupUygun = true;

                                }
                            }


                        }
                    }
                }
                else
                {
                    tupUygun = false;
                }
            }

            //tüp sayısının iki eksiği kadar aynı renkte tüp olunca oyunu bitir
            void bitisKontrol(int hareketSayisi)
            {
                string kntrl = "__";
                int doluSayac = 0;
                for (int x = 0; x <= tupsayisi; x++)
                {
                    kntrl = sortPuzz[x, 3];

                    if (sortPuzz[x, 0] == kntrl && sortPuzz[x, 1] == kntrl && sortPuzz[x, 2] == kntrl && sortPuzz[x, 3] == kntrl && kntrl != "__")
                    {
                        doluSayac++;
                    }
                }

                if (doluSayac == tupsayisi - 1)
                {

                    int sayac = 0;
                    for (int adimlar = 0; adimlar < sonDurumStringSayisi; adimlar++)
                    {


                        for (int j = 0; j <= tupsayisi; j++)
                        {
                            for (int k = 0; k <= 3; k++)
                            {
                                for (int i = 0; i <= tupsayisi; i++)
                                {
                                    for (int s = 0; s <= 3; s++)
                                    {
                                        if (sayac <= 14 && sonDurumString[adimlar, j].Substring(sayac, 2) == sortPuzz[i, s].Substring(0, 2))
                                        {
                                            sonDurumHareketListesi[j, k] = sortPuzz[i, s];
                                        }
                                    }
                                }
                                sayac = sayac + 2;
                            }
                            sayac = 0;
                        }
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (adimlar != 0)
                        {
                            Console.WriteLine(adimListesi[adimlar].Substring(0, 2) + " dök " + adimListesi[adimlar].Substring(adimListesi[adimlar].Length - 2, 2));

                        }
                        else
                        {
                            Console.WriteLine("ilk durum");
                        }

                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine("");
                        for (int j = 3; j >= 0; j--)
                        {
                            for (int i = 0; i <= tupsayisi; i++)
                            {
                                sortPuzz[i, j] = sonDurumHareketListesi[i, j];
                            }
                        }
                        cikti();
                    }




                    Console.WriteLine("");
                    for (int y = 1; y <= hareketSayisi; y++)
                    {
                        if (hareketDetay[y] != null)
                        {
                            //Console en son detay gösterme
                            Console.WriteLine(hareketDetay[y].ToString());
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }

                }
            }


            //dökme işlemi gerçekleşir
            void ilkAdimAl()
            {
                for (int i = 0; i <= tupsayisi; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        sonDurumString[sonDurumStringSayisi, i] += sortPuzz[i, j];
                    }
                }
                sonDurumStringSayisi++;
                hareketDetay[0] = "ilk durum";
                adimListesi[0] = "ilk durum";
                hareketSayisi++;
                adimSayisi++;
            }
            //dökme işlemi gerçekleşir
            void tupDok(int i, int j, int k, int l)
            {

                //birim sıvı sayısı bulma
                int birimSiviSayisi = 0;
                if (l < 2 && j > 1 && sortPuzz[i, j - 1] == eldeki && sortPuzz[i, j - 2] == eldeki)//3lü taşı
                {
                    birimSiviSayisi += 3;
                }
                else if (l < 3 && j > 0 && sortPuzz[i, j - 1] == eldeki)//2li taşı
                {
                    birimSiviSayisi += 2;
                }
                else
                {
                    birimSiviSayisi++;
                }

                //Hareket detayı en son kısımda göstermek için hareketDetay dizisine detay verilerini ekleme. 
                hareketDetay[hareketSayisi] = hareketSayisi + ". hareket " + (i + 1) + ". tüpten -> " + (k + 1) + ". nolu tüpe " + eldeki + " renginden " + birimSiviSayisi + " birim sıvı.";//ekrana atılması gereken adımları yazar





                hareketSayisi++;

                adimListesi[adimSayisi] = (i + 1) + " " + birimSiviSayisi + " " + j + " " + +(k + 1);//atılan adımların listesini tutar

                adimSayisi++;

                if (l < 2 && j > 1 && sortPuzz[i, j - 1] == eldeki && sortPuzz[i, j - 2] == eldeki)//3lü taşı
                {
                    sortPuzz[k, l] = eldeki;
                    sortPuzz[k, l + 1] = eldeki;
                    sortPuzz[k, l + 2] = eldeki;
                    eldeki = "__";
                    sortPuzz[i, j] = "__";
                    sortPuzz[i, j - 1] = "__";
                    sortPuzz[i, j - 2] = "__";
                }
                else if (l < 3 && j > 0 && sortPuzz[i, j - 1] == eldeki)//2li taşı
                {
                    sortPuzz[k, l] = eldeki;
                    sortPuzz[k, l + 1] = eldeki;
                    eldeki = "__";
                    sortPuzz[i, j] = "__";
                    sortPuzz[i, j - 1] = "__";
                }
                else//teki taşı
                {
                    sortPuzz[k, l] = eldeki;
                    eldeki = "__";
                    sortPuzz[i, j] = "__";
                }
                cikmazSayaci = 0;
                if (geriAdimSayaci > 0)
                {
                    cikmazAdimKaydi[cikmazAdimsayaci] += (i + 1) + "" + (k + 1);
                    adim++;
                }
            }
            ilkAdimAl();



            while (true)
            {
                for (int i = 0; i <= tupsayisi; i++)//sutunları gez soldan sağa
                {
                    for (int j = 3; j >= 0; j--)//satırları gez yukarıdan aşağıya
                    {
                        eldekiKontrol(i, j);
                        if (eldekiUygun == true)//içinde veri var ise veya (en üst hariç) bir üstü boş ise ve kendisi boş değil ise)
                        {

                            eldeki = sortPuzz[i, j];//eldeki değişkenine veriyi at

                            for (int l = 3; l >= 0; l--)//satırları yukarıdan aşağıya gezerek arama yap
                            {
                                for (int k = 0; k <= tupsayisi; k++)//sutunları soldan sağa gezerek arama yap
                                {
                                    tupKontrol(i, j, k, l);
                                    if (tupUygun == true)
                                    {
                                        tupDok(i, j, k, l);
                                        sondurum();
                                        bitisKontrol(hareketSayisi);
                                        cikmazSayaci = 0;
                                    }
                                    else
                                    {
                                        cikmazSayaci++;
                                        if (cikmazSayaci == 1000 || (adimSayisi > 4 && adimListesi[adimSayisi - 1] == adimListesi[adimSayisi - 3]))
                                        {
                                            geriAdimSayaci = adim;
                                            cikmazSayaci = 0;
                                            for (int e = 1; e < cikmazAdimsayaci; e++)
                                            {
                                                if ((adimSayisi - (geriAdimSayaci + 1) == 0) || cikmazAdimKaydi[cikmazAdimsayaci] == cikmazAdimKaydi[e])
                                                {

                                                    if (adimSayisi - (geriAdimSayaci + 1) <= 0)
                                                    {
                                                        if (adimSayisi - (geriAdimSayaci + 1) != 0)
                                                        {
                                                            tekliGeriAdim = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tekliGeriAdim = false;
                                                    }
                                                    if (tekliGeriAdim == false)
                                                    {
                                                        adimAtla = adimListesi[adimSayisi - geriAdimSayaci];

                                                        for (int h = 1; h <= (geriAdimSayaci); h++)
                                                        {
                                                            adimListesi[adimSayisi - h] = null;
                                                            hareketDetay[adimSayisi - h] = null;
                                                            for (int x = 0; x <= tupsayisi; x++)
                                                            {
                                                                sonDurumString[sonDurumStringSayisi - h, x] = null;
                                                            }
                                                        }
                                                        sonDurumStringSayisi = sonDurumStringSayisi - (geriAdimSayaci);
                                                        geriAdim(adimSayisi - (geriAdimSayaci + 1));
                                                        hareketSayisi = adimSayisi - (geriAdimSayaci);
                                                        adimSayisi = adimSayisi - (geriAdimSayaci);

                                                        if (tupsayisi + 1 != Convert.ToInt32(adimAtla.Substring(0, 2)))
                                                        {
                                                            i = Convert.ToInt32(adimAtla.Substring(0, 2));
                                                        }
                                                        else
                                                        {
                                                            i = 0;
                                                        }

                                                        j = 3;
                                                        eldeki = sortPuzz[i, j];

                                                        tekliGeriAdim = true;

                                                        break;
                                                    }
                                                }
                                            }
                                            if (tekliGeriAdim == true)
                                            {
                                                adimAtla = adimListesi[adimSayisi - 1];
                                                cikmazSayaci = 0;

                                                adimListesi[adimSayisi - 1] = null;
                                                hareketDetay[adimSayisi - 1] = null;

                                                for (int x = 0; x <= tupsayisi; x++)
                                                {

                                                    sonDurumString[sonDurumStringSayisi - 1, x] = null;


                                                }

                                                geriAdimSayaci = 1;
                                                sonDurumStringSayisi--;
                                                geriAdim(adimSayisi - (2));
                                                hareketSayisi = adimSayisi - (geriAdimSayaci);
                                                adimSayisi = adimSayisi - (geriAdimSayaci);
                                            }

                                            tekliGeriAdim = true;
                                            if (cikmazAdimKaydi[cikmazAdimsayaci] != null)
                                            {
                                                cikmazAdimsayaci++;
                                            }
                                            adim = 1;

                                            if (0 != Convert.ToInt32(adimAtla.Substring(0, 2)) && tupsayisi + 1 != Convert.ToInt32(adimAtla.Substring(0, 2)))
                                            {
                                                i = Convert.ToInt32(adimAtla.Substring(0, 2));
                                                j = 3;
                                                eldeki = sortPuzz[i, j];
                                            }
                                            k = tupsayisi + 1;
                                            l = -1;
                                            j++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}











