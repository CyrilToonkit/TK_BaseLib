using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Math3D
{
    public static class Interpolate
    {
        public static double Linear(double y1,double y2,double mu)
        {
           return(y1*(1-mu)+y2*mu);
        }

        public static double Cosine(double y1,double y2,double mu)
        {
           double mu2;
           mu2 = (1-Math.Cos(mu*Math.PI))/2;
           return(y1*(1-mu2)+y2*mu2);
        }

        public static double Cubic(double y0,double y1,double y2,double y3,double mu)
        {
           double a0,a1,a2,a3,mu2;

           mu2 = mu*mu;
           a0 = y3 - y2 - y0 + y1;
           a1 = y0 - y1 - a0;
           a2 = y2 - y0;
           a3 = y1;

           return(a0*mu*mu2+a1*mu2+a2*mu+a3);
        }

        public static double Bezier(double y0, double y1, double y2, double y3, double t)
        {
                double ax, bx, cx;
                double tSquared, tCubed;
                double result;

                /* calculate the polynomial coefficients */

                cx = 3.0 * (y1 - y0);
                bx = 3.0 * (y2 - y1) - cx;
                ax = y3 - y0 - cx - bx;

                /* calculate the curve point at parameter value t */

                tSquared = t * t;
                tCubed = tSquared * t;

                result = (ax * tCubed) + (bx * tSquared) + (cx * t) + y0;

                return result;
        }


        /*
           Tension: 1 is high, 0 normal, -1 is low
           Bias: 0 is even,
                 positive is towards first segment,
                 negative towards the other
        */
        public static double Hermite(double y0,double y1,double y2,double y3,double mu, double tension, double bias)
        {
           double m0,m1,mu2,mu3;
           double a0,a1,a2,a3;

            mu2 = mu * mu;
            mu3 = mu2 * mu;
            m0  = (y1-y0)*(1+bias)*(1-tension)/2;
            m0 += (y2-y1)*(1-bias)*(1-tension)/2;
            m1  = (y2-y1)*(1+bias)*(1-tension)/2;
            m1 += (y3-y2)*(1-bias)*(1-tension)/2;
            a0 =  2*mu3 - 3*mu2 + 1;
            a1 =    mu3 - 2*mu2 + mu;
            a2 =    mu3 -   mu2;
            a3 = -2*mu3 + 3*mu2;

           return(a0*y1+a1*m0+a2*m1+a3*y2);
        }

        public static double Hermite(double y0, double y1, double y2, double y3, double mu)
        {
            return Hermite(y0, y1, y2, y3, mu, 0, 0);
        }
    }
}
