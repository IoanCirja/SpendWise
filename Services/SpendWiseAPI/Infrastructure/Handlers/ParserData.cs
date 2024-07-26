using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Handlers
{
    public class ParserData:IParserData
    {
        public string GetUpdatedStringSpentOfCategoryWhenAddTransaction(string categoryBudgetPlan, string spentOfCategoryMonthlyPlan, string currentCategory, double amount)
        {
            if(!categoryBudgetPlan.Contains(currentCategory))
            {
                throw new Exception("the category name is wrong");
            }
            string[] categorii = categoryBudgetPlan.Split(',');
            string[] cheltuieli = spentOfCategoryMonthlyPlan.Split(',');
            string spentOfCategory = "";
            int i = 0;
            foreach (string categorie in categorii)
            {
                Console.WriteLine(categorie);

                if (categorie.Equals(currentCategory) || categorie.Equals(" "+currentCategory))
                {
                    double numar;
                    bool raspuns = double.TryParse(cheltuieli[i], out numar);
                    if(raspuns == false)
                    {
                        throw new Exception("error parsing the number");
                    }
                    double suma = numar + amount;
                    spentOfCategory += suma.ToString();
                }
                else
                {
                     spentOfCategory += cheltuieli[i];
                }
                if (i != categorii.Length - 1)
                {
                    spentOfCategory += ",";
                }
                i++;
            }
            return spentOfCategory;
        }
        public string[] GetCategory(string categoryName)
        {
            string[] categorii = categoryName.Split(',');
            return categorii;
        }
        public double[] GetPrice(string priceByCategory)
        {
            Console.WriteLine(priceByCategory);
            string[] cheltuieli = priceByCategory.Split(',');
            double[] result = new double[cheltuieli.Length+1];
            int i = 0;
            foreach( string chelt in cheltuieli)
            {
                double numar;
                bool raspuns = double.TryParse(chelt, out numar);
                if (raspuns == false)
                {
                    throw new Exception("error parsing the number");
                }
                result[i] = numar;
                i++;
            }
            Console.WriteLine(result[0]);
            return result;
        }
    }
}
