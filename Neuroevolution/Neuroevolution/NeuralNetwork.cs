using System;

/// <summary>
/// Neural Network
/// </summary>
public class NeuralNetwork
{
    //Each network has a fitness for the Genetic Algorithm
    public int fitness = 0;

    //Assume we're only working with networks with 1 hidden layer
    public const int LAYERNUM = 3;

    //Initialize the network with 3 neuron layers
    public NeuronLayer[] neuronLayers = new NeuronLayer[LAYERNUM];

    public NeuralNetwork(int inputs, int hidden, int outputs)
    {
        //Create Input layer
        neuronLayers[0] = CreateNeuronLayer(inputs);
        neuronLayers[1] = CreateNeuronLayer(hidden, inputs);
        neuronLayers[2] = CreateNeuronLayer(outputs, hidden);
    }

    void FeedForward()
    {
        //Start at hidden layer
        for (int i = 1; i < neuronLayers.Length; i++)
        {
            //For each neuron
            for (int j = 0; j < neuronLayers[i].neurons.Length; j++)
            {
                double value = 0;
                for (int k = 0; k < neuronLayers[i].neurons[j].inputNum; k++)
                {
                    value += neuronLayers[i - 1].neurons[k].value * neuronLayers[i].neurons[j].weights[k];
                }
                value += neuronLayers[i].neurons[j].bias;
                neuronLayers[i].neurons[j].value = value;
            }
        }
    }

    //Create a neuron input layer object
    static NeuronLayer CreateNeuronLayer(int neuronNum)
    {
        //Create new layer
        NeuronLayer neuronLayer = new NeuronLayer();
        //Set number of neurons
        neuronLayer.neuronNum = neuronNum;
        //Create each neuron object
        neuronLayer.neurons = new Neuron[neuronNum];
        for (int i = 0; i < neuronNum; i++)
        {
            neuronLayer.neurons[i] = CreateNeuron();
        }

        return neuronLayer;
    }

    //Create a neuron hidden or output layer object
    static NeuronLayer CreateNeuronLayer(int neuronNum, int inputNum)
    {
        //Create new layer
        NeuronLayer neuronLayer = new NeuronLayer();
        //Set number of neurons
        neuronLayer.neuronNum = neuronNum;
        //Create each neuron object
        neuronLayer.neurons = new Neuron[neuronNum];
        for (int i = 0; i < neuronNum; i++)
        {
            neuronLayer.neurons[i] = CreateNeuron(inputNum);
        }

        return neuronLayer;
    }

    //Create an input neuron object that doesn't have weights
    static Neuron CreateNeuron()
    {
        //Create new neuron
        Neuron neuron = new Neuron();
        neuron.inputNum = 0;
        neuron.value = 0;
        return neuron;
    }

    //Create a neuron object with the correct number of weights
    static Neuron CreateNeuron(int inputNum)
    {
        //Create new neuron
        Neuron neuron = new Neuron();
        neuron.inputNum = inputNum;
        neuron.value = 0;
        neuron.weights = new double[inputNum];
        neuron.bias = 0;
        return neuron;
    }

    //Turn neural network tree into weights array
    double[] ToArray()
    {
        int inputs = neuronLayers[0].neuronNum;
        int hidden = neuronLayers[1].neuronNum;
        int outputs = neuronLayers[2].neuronNum;

        double[] weights = new double[(inputs + 1) * hidden + (hidden + 1) * outputs];
        int ii = 0;

        for (int i = 0; i < neuronLayers.Length; i++)
        {
            for (int j = 0; j < neuronLayers[i].neurons.Length; j++)
            {
                for (int k = 0; k < neuronLayers[i].neurons[j].weights.Length; k++)
                {
                    weights[ii] = neuronLayers[i].neurons[j].weights[k];
                    ii++;
                }
                weights[ii] = neuronLayers[i].neurons[j].bias;
                ii++;
            }
        }

        return weights;
    }

    //Turn weights array into neural network tree
    static NeuralNetwork ConvertArray(double[] weights, int inputs, int hidden, int outputs)
    {
        NeuralNetwork network = new NeuralNetwork(inputs, hidden, outputs);
        int ii = 0;

        for (int i = 0; i < network.neuronLayers.Length; i++)
        {
            for (int j = 0; j < network.neuronLayers[i].neurons.Length; j++)
            {
                for (int k = 0; k < network.neuronLayers[i].neurons[j].weights.Length; k++)
                {
                    network.neuronLayers[i].neurons[j].weights[k] = weights[ii];
                    ii++;
                }
                network.neuronLayers[i].neurons[j].bias = weights[ii];
                ii++;
            }
        }

        return network;
    }
}

//Neuron layer object
public struct NeuronLayer
{
    public int neuronNum;
    public Neuron[] neurons;
}

//Neuron object
public struct Neuron
{
    public int inputNum;
    public double value;
    public double[] weights;
    public double bias;
}
