namespace Middleware_Bridge_ASPNet;

// Sample code to map Job and Interviews
public class JobMapper
{
    // Method to map Job and Interviews from partner platform data
    public Job MapJobFromPartnerPlatform(dynamic partnerJobData)
    {
        // Create a new Job object
        Job job = new Job
        {
            JobId = partnerJobData.JobId,
            Title = partnerJobData.Title,
            Description = partnerJobData.Description,
            Interviews = new List<Interview>()
        };

        // Map Interviews related to the Job
        foreach (var partnerInterviewData in partnerJobData.Interviews)
        {
            Interview interview = new Interview
            {
                InterviewId = partnerInterviewData.InterviewId,
                Date = DateTime.Parse(partnerInterviewData.Date),
                Location = partnerInterviewData.Location
            };

            // Add the mapped Interview to the Job's Interviews list
            job.Interviews.Add(interview);
        }

        return job;
    }
}