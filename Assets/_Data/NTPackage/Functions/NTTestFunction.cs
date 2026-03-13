using UnityEngine;


namespace NTPackage.Functions
{
    public class NTTestFunction : NTBehaviour
    {
        public double Num;
        public string Str;

        [NTButton]
        public void Test(){
            this.Str = NTFunction.FormatHigherNumber(Num);
        }
    }
}