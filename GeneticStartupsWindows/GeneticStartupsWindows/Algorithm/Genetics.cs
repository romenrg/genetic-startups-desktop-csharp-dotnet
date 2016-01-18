﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticStartupsWindows
{
    public class Genetics
    {
        public enum Actions { None=0, Advisor=1, Circus=2, Team=3, Product=4, Feedback=5, Investor=6, Doubts=7, Sales=8, BadNews=9}
        public enum States { Confusion=0, Success=1, Failure=2}

        public Actions [,] matrix { get; set; }
        public Dictionary<Actions, int>[] actionProbabilityPerQ { get; set; }
        public Dictionary<Actions, List<KeyValuePair<int, string>>> possibleScoresPerAction { get; set; }

        private Random generateRandomNum;
        private int numCols;
        private int numRows;
        private int numSteps;

        public int[][] population;
        public List<KeyValuePair<int, int>> individualsSortedByScore;
        public int individualLength;

        public int numOfBinaryDigitsForStartCells;
        public int numOfBinaryDigitsForSteps;
        public int possibleDirections = 4;

        public const int CELL_VALUE_OUT_OF_BOUNDS = -10;
        public const int NUM_GENERATIONS = 15;

        // -----------------------------
        //  Public methods
        // -----------------------------

        public Genetics(int numCols, int numRows, int numSteps)
        {
            this.numCols = numCols;
            this.numRows = numRows;
            this.numSteps = numSteps;
            this.generateRandomNum = new Random();
            this.generatePercentagesOfActionsPerQ();
            this.generateScoreProbabilitiesPerAction();
        }

        public void createBoard()
        {
            this.matrix = new Actions[this.numCols, this.numRows];
            for (int i = 0; i < this.numCols; i++)
            {
                for (int j = 0; j < this.numRows; j++)
                {
                    this.matrix[i, j] = generateCellContent(i,j);
                }
            }
        }

        public Image getIconForPos(int i, int j)
        {
            return this.transformActionEnumToImage(i, j);
        }

        public void generatePopulation(int populationSize)
        {
            this.numOfBinaryDigitsForStartCells = (int)Math.Ceiling(Math.Log(this.numRows, 2));
            this.numOfBinaryDigitsForSteps = (int)Math.Ceiling(Math.Log(this.possibleDirections, 2)) * this.numSteps;
            this.population = new int[populationSize][];
            for (int i = 0; i < populationSize; i++)
            {
                this.individualLength = numOfBinaryDigitsForStartCells + numOfBinaryDigitsForSteps;
                this.population[i] = new int[this.individualLength];
                for (int j=0; j< this.individualLength; j++)
                    this.population[i][j] = this.generateRandomNum.Next(2);
            }
        }

        public void newGeneration()
        {
            int elementsToMutate = this.population.Length / 3;
            int elementsToCross = (this.population.Length / 3) / 2 * 2;
            int elementsToSelect = this.population.Length - elementsToMutate - elementsToCross;
            int[][] selectedIndividuals = this.selection(elementsToSelect);
            int[][] crossedIndividuals = this.crossover(elementsToCross);
            int[][] mutatedIndividuals = this.mutation(elementsToMutate);
            this.population = selectedIndividuals.Concat(crossedIndividuals).Concat(mutatedIndividuals).ToArray(); ;
        }

        public void generateScores()
        {
            this.individualsSortedByScore = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < this.population.Length; i++)
                this.individualsSortedByScore.Add(new KeyValuePair<int, int>(i, this.fitness(this.population[i])));
            this.individualsSortedByScore.Sort((x, y) => -1 * x.Value.CompareTo(y.Value));
        }

        public Tuple<int, int>[] getBestIndividualCellsPath()
        {
            return calculatePathOfIndividual(this.population[this.individualsSortedByScore[0].Key]);
        }


        public bool isCellInsideMap(int x, int y)
        {
            return (x >= 0) && (x < this.numCols) && (y >= 0) && (y < this.numRows);
        }

        // TODO: Probably should be changed as a private method and the test for this is included in the test for the whole individual path
        public int calculateSquareValue(int x, int y)
        {
            if (this.isCellInsideMap(x, y))
            {
                Actions squareAction = this.matrix[x, y];
                int squareValue = 0;
                for (int i=0; i<this.possibleScoresPerAction[squareAction].Count; i++)
                {
                    squareValue += this.possibleScoresPerAction[squareAction][i].Key;
                }
                return squareValue;
            }
            else
            {
                return Genetics.CELL_VALUE_OUT_OF_BOUNDS;
            }
        }

        //TODO: Probably should also be a private method called inside "fitness"
        public Tuple<int, int>[] calculatePathOfIndividual(int[] individual)
        {
            Tuple<int, int>[] cellsPath = new Tuple<int, int>[this.numSteps + 1];
            cellsPath[0] = this.calculateCellFromBinaryFirstPosition(individual.Skip(0).Take(this.numOfBinaryDigitsForStartCells).ToArray());
            int startingPositionOfMovement = this.numOfBinaryDigitsForStartCells;
            int numOfCellsToSkip = this.numOfBinaryDigitsForStartCells;
            Tuple<int, int> previousCell = cellsPath[0];
            for (int i=1; i<=this.numSteps; i++)
            {
                cellsPath[i] = this.calculateCellFromPreviousAndMovement(previousCell, individual.Skip(numOfCellsToSkip).Take(2).ToArray());
                previousCell = cellsPath[i];
                numOfCellsToSkip += 2;
            }
            return cellsPath;
        }


        // -----------------------------
        //  Private methods
        // -----------------------------

        private int fitness(int[] individual)
        {
            int individualScore = 0;
            Tuple<int, int>[] individualCellsPath = this.calculatePathOfIndividual(individual);
            for (int i=0; i<individualCellsPath.Length; i++)
            {
                individualScore += this.calculateSquareValue(individualCellsPath[i].Item1, individualCellsPath[i].Item2);
            }
            // Now we should check the squares the individual is visiting and
            // must have a data structure with the possible values of each action
            // E.G. advisor is a random number between -15 and 5
            // Finish is -50 (failure) or 50 (success)
            // To sort multiple winning solutions we add the number of remaining squares to the score
            // E.G. If winning in the 5th square out of 20, the score will be 50 + 15 = 65;
            return individualScore;
        }

        private int[][] selection(int elementsToSelect) {
            int[][] selectedIndividuals = new int[elementsToSelect][];
            for (int i=0; i< elementsToSelect; i++)
            {
                //TODO: Array must be copied!!! This is a reference
                //selectedIndividuals[i] = this.population[this.populationIndividualScores[i].Key];
                selectedIndividuals[i] = (int[])(this.population[this.individualsSortedByScore[i].Key]).Clone();
            }
            return selectedIndividuals;
        }

        private int[][] crossover(int elementsToCross) {
            int[][] crossedIndividuals = new int[elementsToCross][];
            for (int i=0; i < elementsToCross; i+=2)
            {
                int randomIndexOfFirstIndividualToMutate = this.generateRandomNum.Next(this.population.Length);
                int randomIndexOfSecondIndividualToMutate = this.generateRandomNum.Next(this.population.Length);
                //int[] firstElementToCross = this.population[randomIndexOfFirstIndividualToMutate];
                //int[] secondElementToCross = this.population[randomIndexOfSecondIndividualToMutate];
                int[] firstElementToCross = (int[])this.population[randomIndexOfFirstIndividualToMutate].Clone();
                int[] secondElementToCross = (int[])this.population[randomIndexOfSecondIndividualToMutate].Clone();
                int[] newFirstHalf = firstElementToCross.Skip(0).Take(this.individualLength / 2).ToArray();
                int[] newSecondHalf = secondElementToCross.Skip(this.individualLength / 2).Take(this.individualLength).ToArray();
                crossedIndividuals[i] = new int[this.individualLength];
                newFirstHalf.CopyTo(crossedIndividuals[i], 0);
                newSecondHalf.CopyTo(crossedIndividuals[i], newFirstHalf.Length);
                int[] oppositeFirstHalf = secondElementToCross.Skip(0).Take(this.individualLength / 2).ToArray();
                int[] oppositeSecondHalf = firstElementToCross.Skip(this.individualLength / 2).Take(this.individualLength).ToArray();
                crossedIndividuals[i+1] = new int[this.individualLength];
                oppositeFirstHalf.CopyTo(crossedIndividuals[i+1], 0);
                oppositeSecondHalf.CopyTo(crossedIndividuals[i+1], newFirstHalf.Length);
            }
            return crossedIndividuals;
        }

        private int[][] mutation(int elementsToMutate) {
            int[][] mutatedIndividuals = new int[elementsToMutate][];
            for (int i = 0; i < elementsToMutate; i++)
            {
                int randomIndexOfIndividualToMutate = this.generateRandomNum.Next(this.population.Length);
                //mutatedIndividuals[i] = this.population[randomIndexOfIndividualToMutate];
                mutatedIndividuals[i] = (int[])this.population[randomIndexOfIndividualToMutate].Clone();
                int randomElementOfndividual = this.generateRandomNum.Next(mutatedIndividuals[i].Length - 1);
                if (mutatedIndividuals[i][randomElementOfndividual] == 1)
                    mutatedIndividuals[i][randomElementOfndividual] = 0;
                else
                    mutatedIndividuals[i][randomElementOfndividual] = 1;
            }
            return mutatedIndividuals;
        }

        private Tuple<int, int> calculateCellFromPreviousAndMovement(Tuple<int, int> previousCell, int[] movement)
        {
            // TODO: Something should be done if movements lead to a non-existing square...
            // Probably in the fitness function it should be considered like -10 (going out of the map)
            Tuple<int, int> newSquare = new Tuple<int, int>(-1, -1);
            if (this.movingRight(movement))
            {
                newSquare = new Tuple<int, int>(previousCell.Item1 + 1, previousCell.Item2);
            }
            else if (this.movingDown(movement))
            {
                newSquare = new Tuple<int, int>(previousCell.Item1, previousCell.Item2 - 1);
            }
            else if (this.movingLeft(movement))
            {
                newSquare = new Tuple<int, int>(previousCell.Item1 - 1, previousCell.Item2);
            }
            else if (this.movingUp(movement))
            {
                newSquare = new Tuple<int, int>(previousCell.Item1, previousCell.Item2 + 1);
            }
            return newSquare;
        }

        private bool movingRight(int[] movement)
        {
            return ((movement[0] == 0) && (movement[1] == 0)) || ((movement[0] == 1) && (movement[1] == 0));
        }

        private bool movingDown(int[] movement)
        {
            return (movement[0] == 0) && (movement[1] == 1);
        }

        private bool movingLeft(int[] movement)
        {
            return false;
        }

        private bool movingUp(int[] movement)
        {
            return (movement[0] == 1) && (movement[1] == 1);
        }

        private Tuple<int, int> calculateCellFromBinaryFirstPosition(int[] firstSquare)
        {
            int decimalValue = this.binaryArrayToDecimalInt(firstSquare);
            // Number of rows may not be a power of 2. If decimal value is too big, make it circular
            int decimalValueAdjustedToMax = decimalValue % this.numRows;
            return new Tuple<int, int>(0, decimalValueAdjustedToMax);
        }

        private int binaryArrayToDecimalInt(int[] binary)
        {
            int decimalValue = 0;
            int highestPower = binary.Length - 1;
            for (int i = highestPower; i >= 0; i--)
            {
                decimalValue += binary[highestPower - i] * (int)Math.Pow(2, i);
            }
            return decimalValue;
        }

        private void generateScoreProbabilitiesPerAction()
        {
            this.possibleScoresPerAction = new Dictionary<Actions, List<KeyValuePair<int, string>>>();
            this.possibleScoresPerAction[Actions.None] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(-1, "Feeling that things don't go as fast as expeceted..."));
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(-1, "Feeling that things don't go as fast as expeceted..."));
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(0, "Just one more day in the startup world!"));
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(0, "Just one more day in the startup world!"));
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(0, "Just one more day in the startup world!"));
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(0, "Just one more day in the startup world!"));
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(0, "Just one more day in the startup world!"));
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(0, "Just one more day in the startup world!"));
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(1, "One day closer to success!"));
            this.possibleScoresPerAction[Actions.None].Add(new KeyValuePair<int, string>(1, "One day closer to success!"));
            this.possibleScoresPerAction[Actions.Advisor] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(-15, "The 'advisor' turned out to be a liar, had no idea but took big shares"));
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(-10, "The 'advisor' had no idea, gave wrong advice and company suffered"));
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(-6, "You realized the 'advisor' won't help at all"));
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(-6, "You realized the 'advisor' won't help at all"));
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(-4, "The 'advisor' just wanted to sell his/her services"));
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(-4, "The 'advisor' just wanted to sell his/her services"));
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(-4, "The 'advisor' just wanted to sell his/her services"));
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(2, "The 'advisor' nows about the market and may be helful"));
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(5, "The 'advisor' is connected to investors in the field"));
            this.possibleScoresPerAction[Actions.Advisor].Add(new KeyValuePair<int, string>(10, "The 'advisor' will bring important customers"));
            this.possibleScoresPerAction[Actions.Circus] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(-5, "You should have been working instead; you wasted a lot of time"));
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(-5, "You should have been working instead; you wasted a lot of time"));
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(-5, "You should have been working instead; you wasted a lot of time"));
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(-5, "You should have been working instead; you wasted a lot of time"));
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(-5, "You should have been working instead; you wasted a lot of time"));
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(-2, "You just wasted some time going there, not that bad"));
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(-2, "You just wasted some time going there, not that bad"));
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(2, "Maybe someone you met today will help you in future"));
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(5, "You built useful connections (either partners or potential investors)"));
            this.possibleScoresPerAction[Actions.Circus].Add(new KeyValuePair<int, string>(7, "You built very useful connections (someone important or well connected)"));
            this.possibleScoresPerAction[Actions.Team] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(-15, "You picked a troublemaker as founder and gave him/her 50% of shares"));
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(-3, "The new team member has just left college and is inexperienced"));
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(-2, "Another person with the same profile joined"));
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(2, "Regular worker joined the company"));
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(2, "Regular worker joined the company"));
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(2, "Regular worker joined the company"));
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(5, "Talented employee joined the company"));
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(5, "Talented employee joined the company"));
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(7, "Talented employee with startup experience joined the company"));
            this.possibleScoresPerAction[Actions.Team].Add(new KeyValuePair<int, string>(7, "Talented person with startup experience and connections in the field joined as co-founder"));
            this.possibleScoresPerAction[Actions.Product] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(-3, "You invested too much (time and money) in your MVP"));
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(3, "You released an MVP or a very small increment to test the market"));
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(3, "You released an MVP or a very small increment to test the market"));
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(3, "You released an MVP or a very small increment to test the market"));
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(3, "You released an MVP or a very small increment to test the market"));
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(3, "You released an MVP or a very small increment to test the market"));
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(4, "You released new features after listening to customer feedback and testing the market"));
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(4, "You released new features after listening to customer feedback and testing the market"));
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(4, "You released new features after listening to customer feedback and testing the market"));
            this.possibleScoresPerAction[Actions.Product].Add(new KeyValuePair<int, string>(5, "You embraced Agile methodologies: product delivery is optimized and work environment improved significantly"));
            this.possibleScoresPerAction[Actions.Feedback] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(1, "You included polls in your product"));
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(2, "You read customer emails and comments"));
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(2, "You read customer emails and comments"));
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(3, "You did one usability test with a friend"));
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(3, "You did one usability test with a friend"));
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(5, "You did one usability test with a potential customer"));
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(5, "You did one usability test with a potential customer"));
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(6, "You are tracking user events and reviewing analytics to improve"));
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(6, "You are tracking user events and reviewing analytics to improve"));
            this.possibleScoresPerAction[Actions.Feedback].Add(new KeyValuePair<int, string>(8, "You performed an A/B test to be sure which change better"));
            this.possibleScoresPerAction[Actions.Investor] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(2, "You get money from FFF"));
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(3, "An investor with no tech experience nor startup experience joined"));
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(3, "An investor with no tech experience nor startup experience joined"));
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(5, "An investor with tech experience but no startup experience joined"));
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(7, "An investor with startup experience (in other fields) joined"));
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(7, "An investor with startup experience (in other fields) joined"));
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(10, "An investor with startup experience in your field joined"));
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(10, "An investor with startup experience in your field joined"));
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(15, "An investor with startup experience and contacts in your field joined, bringing customers"));
            this.possibleScoresPerAction[Actions.Investor].Add(new KeyValuePair<int, string>(15, "An investor with startup experience and contacts in your field joined, bringing customers"));
            this.possibleScoresPerAction[Actions.Doubts] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(-2, "You have doubts and feel lost"));
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(-1, "You have doubts and feel lost"));
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(-1, "You have doubts and feel lost"));
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(-1, "You have doubts and feel lost"));
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(-1, "You have doubts and feel lost"));
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(-1, "You have doubts and feel lost"));
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(1, "You have doubts, but that motivates you to try new things"));
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(1, "You have doubts, but that motivates you to try new things"));
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(1, "You have doubts, but that motivates you to try new things"));
            this.possibleScoresPerAction[Actions.Doubts].Add(new KeyValuePair<int, string>(2, "You have doubts, but that motivates you to try new things"));
            this.possibleScoresPerAction[Actions.Sales] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(7, "Sold the product to a small customer (or small group)"));
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(7, "Sold the product to a small customer (or small group)"));
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(7, "Sold the product to a small customer (or small group)"));
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(7, "Sold the product to a small customer (or small group)"));
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(7, "Sold the product to a small customer (or small group)"));
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(7, "Sold the product to a small customer (or small group)"));
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(7, "Sold the product to a small customer (or small group)"));
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(15, "Sold the product to a medium-size customer (or medium-size group)"));
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(15, "Sold the product to a medium-size customer (or medium-size group)"));
            this.possibleScoresPerAction[Actions.Sales].Add(new KeyValuePair<int, string>(25, "Sold the product to a big customer (or big group)"));
            this.possibleScoresPerAction[Actions.BadNews] = new List<KeyValuePair<int, string>>();
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-15, "Bad news..."));
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-10, "Bad news..."));
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-5, "Bad news..."));
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-5, "Bad news..."));
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-5, "Bad news..."));
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-5, "Bad news..."));
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-5, "Bad news..."));
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-2, "Bad news..."));
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-2, "Bad news..."));
            this.possibleScoresPerAction[Actions.BadNews].Add(new KeyValuePair<int, string>(-1, "Bad news..."));
        }

        private void generatePercentagesOfActionsPerQ()
        {
            this.actionProbabilityPerQ = new Dictionary<Actions, int>[4];
            this.actionProbabilityPerQ[0] = new Dictionary<Actions, int>();
            this.actionProbabilityPerQ[0][Actions.None]     = 65;
            this.actionProbabilityPerQ[0][Actions.Advisor]  = 11;
            this.actionProbabilityPerQ[0][Actions.Circus]   = 8;
            this.actionProbabilityPerQ[0][Actions.Team]     = 4;
            this.actionProbabilityPerQ[0][Actions.Product]  = 2;
            this.actionProbabilityPerQ[0][Actions.Feedback] = 2;
            this.actionProbabilityPerQ[0][Actions.Investor] = 1;
            this.actionProbabilityPerQ[0][Actions.Doubts] = 4;
            this.actionProbabilityPerQ[0][Actions.Sales] = 1;
            this.actionProbabilityPerQ[0][Actions.BadNews] = 2;
            this.actionProbabilityPerQ[1] = new Dictionary<Actions, int>();
            this.actionProbabilityPerQ[1][Actions.None] = 55;
            this.actionProbabilityPerQ[1][Actions.Advisor] = 8;
            this.actionProbabilityPerQ[1][Actions.Circus] = 13;
            this.actionProbabilityPerQ[1][Actions.Team] = 4;
            this.actionProbabilityPerQ[1][Actions.Product] = 6;
            this.actionProbabilityPerQ[1][Actions.Feedback] = 4;
            this.actionProbabilityPerQ[1][Actions.Investor] = 1;
            this.actionProbabilityPerQ[1][Actions.Doubts] = 3;
            this.actionProbabilityPerQ[1][Actions.Sales] = 1;
            this.actionProbabilityPerQ[1][Actions.BadNews] = 5;
            this.actionProbabilityPerQ[2] = new Dictionary<Actions, int>();
            this.actionProbabilityPerQ[2][Actions.None] = 55;
            this.actionProbabilityPerQ[2][Actions.Advisor] = 6;
            this.actionProbabilityPerQ[2][Actions.Circus] = 6;
            this.actionProbabilityPerQ[2][Actions.Team] = 4;
            this.actionProbabilityPerQ[2][Actions.Product] = 5;
            this.actionProbabilityPerQ[2][Actions.Feedback] = 7;
            this.actionProbabilityPerQ[2][Actions.Investor] = 3;
            this.actionProbabilityPerQ[2][Actions.Doubts] = 2;
            this.actionProbabilityPerQ[2][Actions.Sales] = 4;
            this.actionProbabilityPerQ[2][Actions.BadNews] = 8;
            this.actionProbabilityPerQ[3] = new Dictionary<Actions, int>();
            this.actionProbabilityPerQ[3][Actions.None] = 70;
            this.actionProbabilityPerQ[3][Actions.Advisor] = 2;
            this.actionProbabilityPerQ[3][Actions.Circus] = 4;
            this.actionProbabilityPerQ[3][Actions.Team] = 3;
            this.actionProbabilityPerQ[3][Actions.Product] = 3;
            this.actionProbabilityPerQ[3][Actions.Feedback] = 5;
            this.actionProbabilityPerQ[3][Actions.Investor] = 3;
            this.actionProbabilityPerQ[3][Actions.Doubts] = 0;
            this.actionProbabilityPerQ[3][Actions.Sales] = 7;
            this.actionProbabilityPerQ[3][Actions.BadNews] = 3;
        }

        private Actions generateCellContent(int i, int j)
        {
            Actions cellContent;
            int randomNumber = this.generateRandomNum.Next(100);
            if (i < (this.numCols / 4))
            {
                cellContent = generateCellBasedOnQAndRandomNumber(0, randomNumber);
            }
            else if (i < (2 * this.numCols / 4))
            {
                cellContent = generateCellBasedOnQAndRandomNumber(1, randomNumber);
            }
            else if (i < (3 * this.numCols / 4))
            {
                cellContent = generateCellBasedOnQAndRandomNumber(2, randomNumber);
            }
            else
            {
                cellContent = generateCellBasedOnQAndRandomNumber(3, randomNumber);
            }
            return cellContent;
        }

        private Actions generateCellBasedOnQAndRandomNumber(int quarter, int randomNumber)
        {
            int currentRange = 0, i = 0;
            Actions currentAction = Actions.None;
            while (currentRange < randomNumber)
            {
                currentRange += this.actionProbabilityPerQ[quarter][(Actions)i];
                currentAction = (Actions)i;
                i++;
            }
            return currentAction;
        }

        private Image transformActionEnumToImage(int i, int j)
        {
            switch (this.matrix[i, j]) {
                case Actions.None:
                    return null;
                    break;
                case Actions.Advisor:
                    return GeneticStartupsWindows.Properties.Resources.advisor_greedy;
                    break;
                case Actions.Circus:
                    return GeneticStartupsWindows.Properties.Resources.startups_circus;
                    break;
                case Actions.Team:
                    return GeneticStartupsWindows.Properties.Resources.entrepreneur_team;
                    break;
                case Actions.Product:
                    return GeneticStartupsWindows.Properties.Resources.product_release;
                    break;
                case Actions.Feedback:
                    return GeneticStartupsWindows.Properties.Resources.entrepreneur_customer_feedback;
                    break;
                case Actions.Investor:
                    return GeneticStartupsWindows.Properties.Resources.investor;
                    break;
                case Actions.Doubts:
                    return GeneticStartupsWindows.Properties.Resources.entrepreneur_starting;
                    break;
                case Actions.Sales:
                    return GeneticStartupsWindows.Properties.Resources.entrepreneur_success;
                    break;
                case Actions.BadNews:
                    return GeneticStartupsWindows.Properties.Resources.entrepreneur_failure;
                    break;
                default:
                    return null;
                    break;
            }
        }
    }
}
