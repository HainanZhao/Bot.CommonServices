using Bot.CommonServices.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.CommonServices
{
    public class MultiQnAMakerClient : IQnAMakerClient
    {
        private List<QnAMaker> _QnAMakers;
        private QnAMaker _activeQnAMaker;
        public MultiQnAMakerClient(IList<QnAKnowledgeBase> kbs)
        {
            _QnAMakers = new List<QnAMaker>();
            foreach (var kb in kbs)
            {
                var qnaMaker = new QnAMaker(new QnAMakerEndpoint() { Host = kb.Host, EndpointKey = kb.EndpointKey, KnowledgeBaseId = kb.KnowledgeBaseId });
                _QnAMakers.Add(qnaMaker);
            }
        }

        public async Task CallTrainAsync(FeedbackRecords feedbackRecords)
        {
            if (_activeQnAMaker != null)
                await _activeQnAMaker.CallTrainAsync(feedbackRecords);
        }

        public Task<QueryResult[]> GetAnswersAsync(ITurnContext turnContext, QnAMakerOptions options, Dictionary<string, string> telemetryProperties, Dictionary<string, double> telemetryMetrics = null)
        {
            var tasks = new List<Task<QueryResult[]>>();
            foreach (var qnAMaker in _QnAMakers)
            {
                tasks.Add(qnAMaker.GetAnswersAsync(turnContext, options, telemetryProperties, telemetryMetrics));
            }

            Task.WaitAll(tasks.ToArray());

            float bestScore = 0;
            int bestIndex = 0;
            for (int i = 0; i < tasks.Count(); i++)
            {
                foreach (var result in tasks[i].Result)
                {
                    if (result.Score > bestScore)
                    {
                        bestScore = result.Score;
                        bestIndex = i;
                    }
                }
            }

            _activeQnAMaker = _QnAMakers[bestIndex];
            return tasks[bestIndex];
        }

        public Task<QueryResults> GetAnswersRawAsync(ITurnContext turnContext, QnAMakerOptions options, Dictionary<string, string> telemetryProperties = null, Dictionary<string, double> telemetryMetrics = null)
        {
            var tasks = new List<Task<QueryResults>>();
            foreach (var qnAMaker in _QnAMakers)
            {
                tasks.Add(qnAMaker.GetAnswersRawAsync(turnContext, options, telemetryProperties, telemetryMetrics));
            }

            Task.WaitAll(tasks.ToArray());

            float bestScore = 0;
            int bestIndex = 0;
            for (int i = 0; i < tasks.Count(); i++)
            {
                foreach (var result in tasks[i].Result.Answers)
                {
                    if (result.Score > bestScore)
                    {
                        bestScore = result.Score;
                        bestIndex = i;
                    }
                }
            }
            _activeQnAMaker = _QnAMakers[bestIndex];
            return tasks[bestIndex];
        }

        public QueryResult[] GetLowScoreVariation(QueryResult[] queryResult)
        {
            if (_activeQnAMaker != null)
                return _activeQnAMaker.GetLowScoreVariation(queryResult);

            return new QueryResult[] { };
        }
    }
}
