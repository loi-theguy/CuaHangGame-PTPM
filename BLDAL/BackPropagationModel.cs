using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;
using Accord.Neuro.Learning;

namespace BLDAL
{
    public class BackPropagationModel
    {
        private BLDAL_HoaDon hdHeper;

        private double[][] data;
        private double[] predict;
        private double[][] labels;
        private double range;
        private const double INF = 10000000000;
        public bool Success { get; set; }
        public List<TrainData> TrainDatas { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public BackPropagationModel()
        {
            hdHeper = new BLDAL_HoaDon();
        }

        private List<double> NormalizeData(List<TrainData> data)
        {
            List<double> result = new List<double>();
            range = 0;
            double max = -INF;
            foreach (TrainData d in data)
            {
                if (d.Revenue > max) max = d.Revenue;
            }
            range = max;
            for (int i = 0; i < data.Count; i++)
            {
                result.Add(data[i].Revenue / max);
            }
            return result;
        }

        public void Preprocessing()
        {
            TrainDatas = hdHeper.GetTrainDatas(Start, End);
            if (TrainDatas.Count < 5)
            {
                Success = false;
                return;
            }
            List<double> norData = NormalizeData(TrainDatas);
            data = new double[TrainDatas.Count - 5][];
            labels = new double[TrainDatas.Count - 5][];
            for (int i = 0; i < norData.Count - 5; i++)
            {
                labels[i] = new double[1];
                data[i] = new double[] {
                    norData[i], norData[i+1],norData[i+2],norData[i+3]
                };
                labels[i][0] = norData[i + 5];
            }
            int ii = norData.Count - 4;
            predict = new double[] { norData[ii], norData[ii + 1], norData[ii + 2], norData[ii + 3] };
        }

        public double Predict()
        {
            Success = true;
            Preprocessing();
            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), 4, 4, 1);
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            int d = 0;
            while (d <1000)
            {
                d++;
                teacher.RunEpoch(data, labels);
            }
            double[] result = network.Compute(predict);
            return result[0] * range;
        }

    }
}
