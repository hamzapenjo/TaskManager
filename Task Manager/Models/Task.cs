using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool isComplete { get; set; }
        public TimeSpan Timer { get; set; }
        public TaskState TaskState { get; set; }
        public TaskImportance TaskImportance { get; set; }
        public TaskCategory TaskCategory { get; set; } 
        public ObservableCollection<TaskCheckList> TaskCheckList { get; set; }

    }

    public enum TaskState
    {
        /// <summary>
        /// The task is still in progress.
        /// </summary>
        InProgress,
        /// <summary>
        /// The task has been marked as completed.
        /// </summary>
        Complete,
        /// <summary>
        /// The task hasn't been started yet.
        /// </summary>
        NotStarted,
        /// <summary>
        /// The task is late.
        /// </summary>
        Late,
        /// <summary>
        /// The task has been archived.
        /// </summary>
        Archived,
        /// <summary>
        /// The task has been marked as deleted.
        /// </summary>
        Deleted
    }

    public enum TaskCategory
    {
        /// <summary>
        /// Tasks related to your job, like meetings, project deadlines, or professional development activities.
        /// </summary>
        Work,
        /// <summary>
        /// Daily routines, hobbies, or personal goals.
        /// </summary>
        Personal,
        /// <summary>
        /// Household chores, repairs, or home improvement projects.
        /// </summary>
        Home,
        /// <summary>
        /// Exercise routines, meal planning, doctor's appointments, or meditation.
        /// </summary>
        HealthWellbeing,
        /// <summary>
        /// Budgeting, bill payments, and financial planning.
        /// </summary>
        Finance,
        /// <summary>
        /// Grocery lists, clothing, or other shopping needs.
        /// </summary>
        Shopping,
        /// <summary>
        /// Family gatherings, social events, or activities with friends.
        /// </summary>
        SocialFamily,
        /// <summary>
        /// Study sessions, assignments, or educational goals.
        /// </summary>
        Education,
        /// <summary>
        /// Planning for trips, packing lists, or travel itineraries.
        /// </summary>
        Travel,
        /// <summary>
        /// Tasks like going to the post office, bank, or dry cleaners.
        /// </summary>
        Errands,
        /// <summary>
        /// Time set aside for hobbies or leisure activities.
        /// </summary>
        HobbiesLeisure,
        /// <summary>
        /// Community service or volunteering activities.
        /// </summary>
        VolunteeringCommunity,
        /// <summary>
        /// Special dates and celebrations.
        /// </summary>
        BirthdaysAnniversaries,
        /// <summary>
        /// Larger tasks that might span over multiple days or weeks.
        /// </summary>
        Projects,
        /// <summary>
        /// Objectives or goals that you're working towards over a longer period.
        /// </summary>
        LongTermGoals
    }

    public enum TaskImportance
    {
        /// <summary>
        /// Low importance. Suitable for tasks that are not urgent and can be deferred.
        /// </summary>
        Low,
        /// <summary>
        /// Medium importance. For tasks that are of regular priority.
        /// </summary>
        Medium,
        /// <summary>
        /// High importance. Tasks that need to be completed soon but are not critical.
        /// </summary>
        High,
        /// <summary>
        /// Critical importance. These tasks have the highest priority and often have tight deadlines or significant consequences if not completed in time.
        /// </summary>
        Critical
    }

}
