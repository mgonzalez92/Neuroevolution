using System;

/// <summary>
/// Genetic Algorithm
/// </summary>
public class GeneticAlgorithm
{
    /***************************
    * Genetic Algorithm Method *
    ***************************/

    static void Compute(double[][] genes, int[] fitness, Selection selection, Crossover crossover, Mutation mutation, double mutationRate)
    {
        int populationSize = genes[0].Length;
        double[][] parents;
        double[][] children;

        //Select parents
        if (selection == Selection.Roulette)
            parents = RouletteSelect(genes, fitness);
        else
            parents = genes;

        //Crossover
        if (crossover == Crossover.OnePoint)
            children = CrossoverOnePoint(parents);
        else if (crossover == Crossover.TwoPoint)
            children = CrossoverTwoPoint(parents);
        else
            children = parents;

        //Mutation
        if (mutation == Mutation.Random)
            children = MutateRandom(children, mutationRate);
        else if (mutation == Mutation.Uniform)
            children = MutateUniform(children, mutationRate);
    }

    /****************************
    * Initializing a population *
    ****************************/

    //Random values
    static double[][] Randomize(double[][] genes)
    {
        Random random = new Random();
        int populationSize = genes.Length;
        int geneLength = genes[0].Length;
        double[][] randomGenes = new double[populationSize][];

        for (int i = 0; i < populationSize; i++)
        {
            for (int j = 0; j < geneLength; j++)
            {
                randomGenes[i][j] = random.NextDouble();
            }
        }

        return randomGenes;
    }

    /********************
    * Selection Methods *
    ********************/

    //Roulette selection
    static double[][] RouletteSelect(double[][] genes, int[] fitness)
    {
        Random random = new Random();
        int totalFitness = 0;
        int accumulatedFitness = 0;
        int randomFitness = 0;
        int length = fitness.Length;
        int[] parentIndex = new int[length];

        //Find total fitness
        for (int i = 0; i < fitness.Length; i++)
        {
            totalFitness += fitness[i];
        }

        //Select all parents indices
        for (int i = 0; i < length; i++)
        {
            accumulatedFitness = 0;
            randomFitness = random.Next(0, totalFitness);
            for (int j = 0; j < fitness.Length; j++)
            {
                accumulatedFitness += fitness[i];
                if (randomFitness < accumulatedFitness)
                {
                    parentIndex[i] = j;
                    break;
                }
            }
        }

        //Return array of parents
        double[][] parents = new double[length][];
        for (int i = 0; i < length; i++)
        {
            parents[i] = genes[parentIndex[i]];
        }

        return parents;
    }

    /********************
    * Crossover Methods *
    ********************/

    //One point crossover
    static double[][] CrossoverOnePoint(double[][] parents)
    {
        Random random = new Random();
        int populationSize = parents.Length;
        int geneLength = parents[0].Length;

        double[][] children = new double[populationSize][];
        for (int i = 0; i < populationSize; i++)
        {
            children[i] = new double[geneLength];
        }

        for (int i = 0; i < populationSize; i += 2)
        {
            int division = random.Next(0, geneLength);
            for (int j = 0; j < geneLength; j++)
            {
                if (j < division)
                {
                    children[i][j]     = parents[i][j];
                    children[i + 1][i] = parents[i + 1][j];
                }
                else
                {
                    children[i][j]     = parents[i + 1][j];
                    children[i + 1][j] = parents[i][j];
                }
            }
        }

        return children;
    }

    //Two point crossover
    static double[][] CrossoverTwoPoint(double[][] parents)
    {
        Random random = new Random();
        int populationSize = parents.Length;
        int geneLength = parents[0].Length;

        double[][] children = new double[populationSize][];
        for (int i = 0; i < populationSize; i++)
        {
            children[i] = new double[geneLength];
        }

        for (int i = 0; i < populationSize; i += 2)
        {
            int division1 = random.Next(0, geneLength / 2);
            int division2 = division1 + geneLength / 2;
            for (int j = 0; j < geneLength; j++)
            {
                if (j < division1 || j >= division2)
                {
                    children[i][j] = parents[i][j];
                    children[i + 1][i] = parents[i + 1][j];
                }
                else
                {
                    children[i][j] = parents[i + 1][j];
                    children[i + 1][j] = parents[i][j];
                }
            }
        }

        return children;
    }

    /*******************
    * Mutation Methods *
    *******************/

    //Mutated gene is random value
    static double[][] MutateRandom(double[][] genes, double rate)
    {
        Random random = new Random();
        int populationSize = genes.Length;
        int geneLength = genes[0].Length;
        double[][] mutatedGenes = new double[populationSize][];

        for (int i = 0; i < populationSize; i++)
        {
            for (int j = 0; j < geneLength; j++)
            {
                if (random.NextDouble() < rate)
                    mutatedGenes[i][j] = random.NextDouble();
                else
                    mutatedGenes[i][j] = genes[i][j];
            }
        }

        return mutatedGenes;
    }

    //Mutated gene adds uniform random value
    static double[][] MutateUniform(double[][] genes, double rate)
    {
        Random random = new Random();
        int populationSize = genes.Length;
        int geneLength = genes[0].Length;
        double[][] mutatedGenes = new double[populationSize][];

        for (int i = 0; i < populationSize; i++)
        {
            for (int j = 0; j < geneLength; j++)
            {
                if (random.NextDouble() < rate)
                {
                    mutatedGenes[i][j] = genes[i][j] + random.NextDouble();
                    if (mutatedGenes[i][j] > 1)
                        mutatedGenes[i][j] -= 1;
                }
                else
                    mutatedGenes[i][j] = genes[i][j];
            }
        }

        return mutatedGenes;
    }
}

public enum Selection
{
    Roulette,
    Tournament,
    Truncate
}

public enum Crossover
{
    OnePoint,
    TwoPoint
}

public enum Mutation
{
    Random,
    Uniform,
    Gaussian
}