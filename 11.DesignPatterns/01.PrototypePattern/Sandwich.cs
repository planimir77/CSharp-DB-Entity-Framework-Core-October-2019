namespace _01.PrototypePattern
{
    using System;
    public class Sandwich : SandwichPrototype
    {
        private string _bread;
        private string _meat;
        private string _cheese;
        private string _veggies;

        public Sandwich(string bread, string meat, 
            string cheese, string veggies)
        {
            _bread = bread;
            _meat = meat;
            _cheese = cheese;
            _veggies = veggies;
        }

        public override SandwichPrototype Clone()
        {
            string ingredientList = GetIngredientList();
            Console.WriteLine
                ("Cloning sandwich with ingredients: {0}", ingredientList);
            return MemberwiseClone() as SandwichPrototype;
        }

        private string GetIngredientList()
        {
            return $"{_bread}, {_meat}, {_cheese}, {_veggies}";
        }
    }
}
