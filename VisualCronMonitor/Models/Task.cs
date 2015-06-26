using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VisualCron;
using VisualCronAPI;

namespace VisualCronMonitor.Models
{
    public class Task
    {
        [Key]
        public string Name { get; set; }

        public int TaskId { get; set; }
        public string Server { get; set; }
        public string Group { get; set; }
        public string JobName { get; set; }
        public DateTime CurrentRunTime { get; set; }
        public string RunTimeHistory { get; set; }
        public double RunTimeFrequencyMinutes { get; set; }
        public int CurrentSuccess { get; set; }
        public string SuccessHistory { get; set; }
        public double SuccessAverage { get; set; }
        public double CurrentExectutionTimeDuration { get; set; }
        public string ExecutionTimeHistory { get; set; }
        public double ExecutionTimeAverageSeconds { get; set; }
        public string CurrentStandardOutput { get; set; }
        public string StandardOutputHistory { get; set; }
        public int CurrentSubjectiveConcernLevel { get; set; }
        public string SubjectiveConcernHistory { get; set; }
        public string CommandLine { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }

        public Task()
        {
        }

        public Task(ref Server server, JobClass job, TaskClass task)
        {
            Name = task.Name;
            Group = job.Group;
            Server = server.IP.ToUpper().Replace(".AD.METASOURCE.COM", "").Replace("DRPR-", "");
            JobName = job.Name;
            CurrentSuccess = task.Stats.Result == "Success" ? 1 : 0;
            CurrentRunTime = task.Stats.DateLastExecution;
            CurrentExectutionTimeDuration = task.Stats.ExecutionTime;
            CurrentStandardOutput = GetStandardOutput(ref server, job, task);
            RunTimeFrequencyMinutes = Math.Ceiling((job.Stats.DateNextExecution - job.Stats.DateLastExecution).TotalMinutes);

            if (task.Execute != null)
            {
                CommandLine = task.Execute.CmdLine;
                Arguments = task.Execute.Arguments;
                WorkingDirectory = task.Execute.WorkingDirectory;
            }
            
        }

        private string GetStandardOutput(ref Server server, JobClass job, TaskClass task)
        {
            var stdOutput = "";

            try
            {
                if (task.Stats.StandardOutput != null)
                {

                    stdOutput = Encoding.Default.GetString(task.Stats.StandardOutput);



                    //try
                    //{
                    //    stdOutput = server.Jobs.Tasks.GetOutputString(OutputInfoClass.OutputT.StandardOutput, job.Id, task.Id, true);
                    //}
                    //catch (Exception)
                    //{
                    //    stdOutput = Encoding.Default.GetString(task.Stats.StandardOutput);
                    //}
                }

                return stdOutput;
            }
            catch (Exception ex)
            {
                var stdOutputErrorMessage = string.Format("Unable to retrieve Standard Message with message: {0}", ex.Message);
                return stdOutputErrorMessage;
            }
        }
    }
}