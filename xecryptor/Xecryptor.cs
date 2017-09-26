using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace xecryptor
{
    public class Xecryptor
    {
        public string Decrypt(string algorithm)
        {
            string result = "";

            string myAlgorithm = algorithm.Replace("rn", "");

            ArrayList mySplitAlgorithmArrayList = SplitAlgorithm(myAlgorithm);

            ArrayList mySumArrayList = AddAlgorithmParts(mySplitAlgorithmArrayList);

            int myPasswordMax = FindMaxPasswordValue(mySumArrayList);

            for (int i = 0; i <= myPasswordMax; i++)
            {
                ArrayList myPasswordDifference = SubtractPassword(mySumArrayList, i);

                int myPossibles = FindPossibleSolutions(myPasswordDifference);

                if (myPossibles < 5)
                {
                    string myDecryptedString = ConvertAsciiToString(myPasswordDifference);
                    result += "Password Value = " + i + Environment.NewLine + myDecryptedString;
                }
            }

            return result;
        }

        private ArrayList SplitAlgorithm(string algorithm)
        {
            ArrayList myArrayList = new ArrayList();
            Regex myRegex = new Regex(@"(.\d*.\d*.\d*)");
            string[] temp = myRegex.Split(algorithm);

            foreach (string s in temp)
            {
                if (s != "")
                {
                    myArrayList.Add(s);
                }
            }

            return myArrayList;
        }

        private ArrayList AddAlgorithmParts(ArrayList targetArrayList)
        {
            ArrayList myArrayList = new ArrayList();
            Regex myRegex = new Regex(@"(.\d*)");

            foreach (string s in targetArrayList)
            {
                string[] temp = myRegex.Split(s);
                int myNumber = 0;

                foreach (string s2 in temp)
                {
                    if (s2 != "")
                    {
                        string i = s2.Replace(".", "");
                        myNumber += Convert.ToInt32(i);
                    }
                }

                myArrayList.Add(myNumber);
            }

            return myArrayList;
        }

        private ArrayList SubtractPassword(ArrayList targetArrayList, int targetPassword)
        {
            ArrayList myArrayList = new ArrayList();

            foreach (int i in targetArrayList)
            {
                int myNewInt = i - targetPassword;
                myArrayList.Add(myNewInt);
            }

            return myArrayList;
        }

        private string ConvertAsciiToString(ArrayList targetArrayList)
        {
            string myString = "";

            foreach (int i in targetArrayList)
            {
                myString += char.ConvertFromUtf32(i);
            }

            return myString;
        }

        private int FindMaxPasswordValue(ArrayList targetArrayList)
        {
            int myMax = int.MaxValue;

            foreach (int i in targetArrayList)
            {
                if (i < myMax)
                {
                    myMax = i;
                }
            }

            if (myMax == int.MaxValue)
            {
                myMax = 0;
            }

            return myMax;
        }

        private int FindPossibleSolutions(ArrayList targetArrayList)
        {
            int myInt = 0;
            int myAsciiMinValue = 32;
            int myAsciiMaxValue = 127;
            int myAsciiCarriageReturn = 13;
            int myAsciiNewLine = 10;

            foreach (int i in targetArrayList)
            {
                if ((i != myAsciiNewLine) && (i != myAsciiCarriageReturn) && 
                    (i < myAsciiMinValue) || (i >= myAsciiMaxValue))
                {
                    myInt++;
                }
            }

            return myInt;
        }
    }
}
