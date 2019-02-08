/* Alivia Houdek
 * 02.08.2019.
 * Gale-Shapley algorithm example
 * Demonstrates the concept of stable matching
 * Console application
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaleShapleyAlgorithm
{
    class Program
    {
        // Declaration
        private int ProgramRunCount { get; set; }
        private Dictionary<string, int[]> MaleIndividuals { get; set; }
        private Dictionary<string, int[]> FemaleIndividuals { get; set; }
        private int PopulationCount { get; set; }
        private List<int> Wife { get; set; }
        private List<int> Husband { get; set; }

        public Program(int[][] malePref, int[][] femalePref)
        {
            // initialize variables
            ProgramRunCount = 0;
            MaleIndividuals = new Dictionary<string, int[]>();
            FemaleIndividuals = new Dictionary<string, int[]>();
            Wife = new List<int>();
            Husband = new List<int>();

            // set population count
            PopulationCount = malePref.Count();
            if (PopulationCount != femalePref.Count())
            {
                throw new Exception("There needs to be the same number of males and females for stable matching.");
            }

            // initialize individual/preference lists with provided data
            for (int m = 0; m < PopulationCount; m++)
            {
                MaleIndividuals.Add((m + 1) + "M", (int[])malePref[m]);
            }
            for (int f = 0; f < PopulationCount; f++)
            {
                FemaleIndividuals.Add((f + 1) + "F", femalePref[f]);
            }
        }

        static void Main(string[] args)
        {
            // Create nested arrays containing each individual's preferences
            // Can change test data or change to user-input-based here:
            int[][] men = new int[][] { new int[] { 2, 3, 4, 1, 5 }, new int[] { 1, 2, 3, 5, 4 }, new int[] { 5, 2, 3, 4, 1 }, new int[] { 3, 2, 4, 5, 1 }, new int[] { 2, 3, 4, 1, 5 } };

            int[][] women = new int[][] { new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 4, 3, 2, 1 }, new int[] { 2, 3, 1, 4, 5 }, new int[] { 5, 3, 4, 1, 2 }, new int[] { 4, 1, 2, 3, 5 } };

            Program p = new Program(men, women);

            try
            {
                // attempts to match partners provided
                p.Matchmaker(p);
            }
            catch (Exception generalE)
            {
                if (p.ProgramRunCount <= 3)
                {
                    // try again
                    p.Matchmaker(p);
                }
            }
        }

        // Matching Methods
        private void Matchmaker(Program p)
        {
            // how many times the program has run the same code
            p.ProgramRunCount++;

            while (Husband.Count != PopulationCount)
            {
                // loop through each man
                foreach (KeyValuePair<string, int[]> maleIndividual in p.MaleIndividuals)
                {
                    if (IsFree(maleIndividual.Key))
                    {
                        // loop through man's preferences for a wife
                        foreach (int prospect in maleIndividual.Value)
                        {
                            if (ProposalAccepted(Convert.ToInt16(maleIndividual.Key.Substring(0, 1)), prospect) == true)
                            {
                                // break the prospect foreach 
                                break;
                            }
                        }
                    }
                }
            }

            // display final results
            PrintCouples();
        }

        private bool IsFree(string id)
        {
            try
            {
                int idNum = Convert.ToInt16(id.Substring(0,1));
                char sex = Convert.ToChar(id.Substring(1, 1));

                if (sex == 'M')
                {
                    if (Husband.Contains(idNum))
                    {
                        return false;
                    }
                }
                else
                {
                    if (Wife.Contains(idNum))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool WillTradeUp(string fId, string mId)
        {
            try
            {
                int mNumId = Convert.ToInt16(mId.Substring(0, 1));
                int fNumId = Convert.ToInt16(fId.Substring(0, 1));
                int pairPosition = Wife.IndexOf(fNumId);
                var female = FemaleIndividuals.ElementAt(FemaleIndividuals.Keys.ToArray().ToList().IndexOf(fId));

                if (Wife.Contains(fNumId))
                {
                    int mHusbandId = Husband.ElementAt(pairPosition);

                    for (int i = 0; i < PopulationCount; i++)
                    {
                        if (female.Value.ElementAt(i) == mNumId)
                        {
                            // remove original couple from married lists
                            Wife.Remove(fNumId);
                            Husband.RemoveAt(i);

                            //Console.WriteLine("This wife is going to trade up.");
                            return true;
                        }
                        else if (female.Value.ElementAt(i) == mHusbandId)
                        {
                            //Console.WriteLine("This wife would rather stay with her husband.");
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool ProposalAccepted(int maleId, int femaleId)
        {
            if (IsFree(femaleId + "F"))
            {
                // set couple to married
                Wife.Add(femaleId);
                Husband.Add(maleId);

                //Console.Write("Congrats to the new couple!");
                return true;
            }
            else if (WillTradeUp(femaleId + "F", maleId + "M") == true)
            {
                // set couple to married
                Wife.Add(femaleId);
                Husband.Add(maleId);

                //Console.Write("Congrats to the new couple!");
                return true;
            }
            else
            {
                //Console.Write("The male was rejected.");
                return false;
            }
        }

        private void PrintCouples()
        {
            Console.WriteLine("\n\n*PREFERENCES*");

            Console.WriteLine("\nMales\n");
            foreach (var male in MaleIndividuals)
            {
                Console.Write(male.Key + ":");
                foreach (int pref in male.Value)
                {
                    Console.Write(" " + pref);
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nFemales\n");
            foreach (var female in FemaleIndividuals)
            {
                Console.Write(female.Key + ":");
                foreach (int pref in female.Value)
                {
                    Console.Write(" " + pref);
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n\n*MATCHED COUPLES*");
            Console.WriteLine("(Husband -> Wife)");

            for (int h = 0; h < PopulationCount; h++)
            {
                int hubby = Husband.ElementAt(h);
                int wifey = Wife.ElementAt(h);

                Console.WriteLine(hubby + " -> " + wifey);
            }
        }

    }
}
