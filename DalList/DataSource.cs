﻿
namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        internal const int startCourseId = 1000;
        private static int nextCourseId = startCourseId;
        internal static int NextCourseId { get => nextCourseId++; }
    }
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();


}
