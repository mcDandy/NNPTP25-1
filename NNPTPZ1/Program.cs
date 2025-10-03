using System;
using System.Collections.Generic;
using System.Drawing;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1
{
    /// <summary>
    /// This program should produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int width = int.Parse(args[0]);
            int height = int.Parse(args[1]);
            double xmin = double.Parse(args[2]);
            double xmax = double.Parse(args[3]);
            double ymin = double.Parse(args[4]);
            double ymax = double.Parse(args[5]);
            string output = args[6];

            Size imageSize = new Size(width, height);

            // TODO: add parameters from args?
            Bitmap bmp = new Bitmap(imageSize.Width, imageSize.Height);


            double xstep = (xmax - xmin) / imageSize.Width;
            double ystep = (ymax - ymin) / imageSize.Height;

            List<Cplx> koreny = new List<Cplx>();
            // TODO: poly should be parameterised?
            Poly polynomial = new Poly();
            polynomial.Coefficients.Add(new Cplx() { Real = 1 });
            polynomial.Coefficients.Add(Cplx.Zero);
            polynomial.Coefficients.Add(Cplx.Zero);
            //p.Coe.Add(Cplx.Zero);
            polynomial.Coefficients.Add(new Cplx() { Real = 1 });
            Poly polyTmp = polynomial;
            Poly polyDerivative = polynomial.Derive();

            Console.WriteLine(polynomial);
            Console.WriteLine(polyDerivative);

            Color[] colorPalete = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };



            // TODO: cleanup!!!
            // for every pixel in image...
            for (int i = 0; i < imageSize.Height; i++)
            {
                for (int j = 0; j < imageSize.Width; j++)
                {
                    // find "world" coordinates of pixel
                    double y = ymin + i * ystep;
                    double x = xmin + j * xstep;

                    Cplx complexCoords = new Cplx()
                    {
                        Real = x,
                        Imaginary = y
                    };

                    complexCoords.Real = complexCoords.Real == 0 ? 0.0001 : complexCoords.Real;
                    complexCoords.Imaginary = complexCoords.Imaginary == 0 ? 0.0001 : complexCoords.Imaginary;

                    for (int q = 0; q < 30;)
                    {
                        Cplx diff = polynomial.Eval(complexCoords).Divide(polyDerivative.Eval(complexCoords));
                        complexCoords = complexCoords.Subtract(diff);

                        if (Math.Pow(diff.Real, 2) + Math.Pow(diff.Imaginary, 2) < 0.5)
                        {
                            q++;
                        }
                    }

                    // find solution root number
                    bool known = false;
                    int pocetKorenu = 0;
                    for (int w = 0; w < koreny.Count; w++)
                    {
                        if (Math.Pow(complexCoords.Real - koreny[w].Real, 2) + Math.Pow(complexCoords.Imaginary - koreny[w].Imaginary, 2) <= 0.01)
                        {
                            known = true;
                            pocetKorenu = w;
                        }
                    }
                    if (!known)
                    {
                        koreny.Add(complexCoords);
                        pocetKorenu = koreny.Count;
                    }

                    // colorize pixel according to root number
                    Color pixelColor = colorPalete[pocetKorenu % colorPalete.Length];
                    bmp.SetPixel(j, i, pixelColor);
                }
            }



            bmp.Save(output ?? "../../../out.png");
            //Console.ReadKey();
        }
    }

    namespace Mathematics
    {
        public class Poly
        {
            /// <summary>
            /// Coe
            /// </summary>
            public List<Cplx> Coefficients { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public Poly() => Coefficients = new List<Cplx>();

            public void Add(Cplx coe) =>
                Coefficients.Add(coe);

            /// <summary>
            /// Derives this polynomial and creates new one
            /// </summary>
            /// <returns>Derivated polynomial</returns>
            public Poly Derive()
            {
                Poly p = new Poly();
                for (int q = 1; q < Coefficients.Count; q++)
                {
                    p.Coefficients.Add(Coefficients[q].Multiply(new Cplx() { Real = q }));
                }

                return p;
            }

            /// <summary>
            /// Evaluates polynomial at given point
            /// </summary>
            /// <param name="x">point of evaluation</param>
            /// <returns>y</returns>
            public Cplx Eval(double x)
            {
                Cplx y = Eval(new Cplx() { Real = x, Imaginary = 0 });
                return y;
            }

            /// <summary>
            /// Evaluates polynomial at given point
            /// </summary>
            /// <param name="x">point of evaluation</param>
            /// <returns>y</returns>
            public Cplx Eval(Cplx x)
            {
                Cplx s = Cplx.Zero;
                for (int i = 0; i < Coefficients.Count; i++)
                {
                    Cplx coef = Coefficients[i];
                    Cplx bx = x;
                    int power = i;

                    if (i > 0)
                    {
                        for (int j = 0; j < power - 1; j++)
                            bx = bx.Multiply(x);

                        coef = coef.Multiply(bx);
                    }

                    s = s.Add(coef);
                }

                return s;
            }

            /// <summary>
            /// ToString
            /// </summary>
            /// <returns>String repr of polynomial</returns>
            public override string ToString()
            {
                string s = "";
                int i = 0;
                for (; i < Coefficients.Count; i++)
                {
                    s += Coefficients[i];
                    if (i > 0)
                    {
                        int j = 0;
                        for (; j < i; j++)
                        {
                            s += "x";
                        }
                    }
                    if (i + 1 < Coefficients.Count)
                        s += " + ";
                }
                return s;
            }
        }

        public class Cplx
        {
            public double Real { get; set; }
            public double Imaginary { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is Cplx)
                {
                    Cplx x = obj as Cplx;
                    return x.Real == Real && x.Imaginary == Imaginary;
                }
                return base.Equals(obj);
            }

            public readonly static Cplx Zero = new Cplx()
            {
                Real = 0,
                Imaginary = 0
            };

            public Cplx Multiply(Cplx b)
            {
                Cplx a = this;
                // aRe*bRe + aRe*bIm*i + aIm*bRe*i + aIm*bIm*i*i
                return new Cplx()
                {
                    Real = a.Real * b.Real - a.Imaginary * b.Imaginary,
                    Imaginary = (float)(a.Real * b.Imaginary + a.Imaginary * b.Real)
                };
            }
            public double GetAbS()
            {
                return Math.Sqrt(Real * Real + Imaginary * Imaginary);
            }

            public Cplx Add(Cplx b)
            {
                Cplx a = this;
                return new Cplx()
                {
                    Real = a.Real + b.Real,
                    Imaginary = a.Imaginary + b.Imaginary
                };
            }
            public double GetAngleInDegrees()
            {
                return Math.Atan(Imaginary / Real);
            }
            public Cplx Subtract(Cplx b)
            {
                Cplx a = this;
                return new Cplx()
                {
                    Real = a.Real - b.Real,
                    Imaginary = a.Imaginary - b.Imaginary
                };
            }

            public override string ToString()
            {
                return $"({Real} + {Imaginary}i)";
            }

            internal Cplx Divide(Cplx b)
            {
                // (aRe + aIm*i) / (bRe + bIm*i)
                // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
                //  bRe*bRe - bIm*bIm*i*i
                Cplx tmp = this.Multiply(new Cplx() { Real = b.Real, Imaginary = -b.Imaginary });
                double tmp2 = b.Real * b.Real + b.Imaginary * b.Imaginary;

                return new Cplx()
                {
                    Real = tmp.Real / tmp2,
                    Imaginary = (float)(tmp.Imaginary / tmp2)
                };
            }
        }
    }
}
