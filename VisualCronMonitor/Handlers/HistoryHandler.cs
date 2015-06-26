using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using VisualCronMonitor.Models;

namespace VisualCronMonitor.Handlers
{
    public class HistoryHandler : IHistoryHandler
    {

        public void UpdateTaskHistoryProperties(Task task, Task matchingDbTask)
        {
            var runTimeHistory = matchingDbTask == null ? null : matchingDbTask.RunTimeHistory;
            var successHistory = matchingDbTask == null ? null : matchingDbTask.SuccessHistory;
            var executionTimeHistory = matchingDbTask == null ? null : matchingDbTask.ExecutionTimeHistory;
            var standardOutputHistory = matchingDbTask == null ? null : matchingDbTask.StandardOutputHistory;
            var subjectiveConcernHistory = matchingDbTask == null ? null : matchingDbTask.SubjectiveConcernHistory;

            task.RunTimeHistory = UpdateHistory(task.CurrentRunTime, runTimeHistory);

            task.SuccessHistory = UpdateHistory(task.CurrentSuccess, successHistory);
            task.SuccessAverage = GetAverage(task.SuccessHistory);

            task.ExecutionTimeHistory = UpdateHistory(task.CurrentExectutionTimeDuration, executionTimeHistory);
            task.ExecutionTimeAverageSeconds = GetAverage(task.ExecutionTimeHistory);

            task.StandardOutputHistory = UpdateHistory(task.CurrentStandardOutput, standardOutputHistory);
            task.CurrentSubjectiveConcernLevel = GetSubjectiveConcernLevel(task.CurrentStandardOutput);
            task.SubjectiveConcernHistory = UpdateHistory(task.CurrentSubjectiveConcernLevel, subjectiveConcernHistory);
        }

        private string UpdateHistory<T>(T taskValue, string previousHistory)
        {
            const int maxHistoryValues = 10;
            var history = new List<T> { taskValue };

            if (previousHistory != null)
            {
                var listHistory = Json.Decode<List<T>>(previousHistory);
                listHistory = listHistory.Take(maxHistoryValues - 1).ToList();
                history.AddRange(listHistory);
            }

            return Json.Encode(history);
        }

        private double GetAverage(string history)
        {
            var average = 0.0;

            if (history != null)
            {
                var listHistory = Json.Decode<List<double>>(history);
                average = listHistory.Average();
            }

            return average;
        }

        private int GetSubjectiveConcernLevel(string currentStandardOutput)
        {
            var possibleErrorCount = 0;
            var knownNonErrorCount = 0;

            string output = string.IsNullOrEmpty(currentStandardOutput) ? "" : currentStandardOutput;
            possibleErrorCount += Regex.Matches(output.ToUpper(), "ERROR").Count;
            possibleErrorCount += Regex.Matches(output.ToUpper(), "EXCEPTION").Count;
            knownNonErrorCount += Regex.Matches(output.ToUpper(), "NO ERROR").Count;
            knownNonErrorCount += Regex.Matches(output.ToUpper(), "NO EXCEPTION").Count;
            knownNonErrorCount += Regex.Matches(output.ToUpper(), "WITHOUT EXCEPTION").Count; 

            return possibleErrorCount - knownNonErrorCount;
        }
    }
}