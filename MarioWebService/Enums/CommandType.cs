using System;
using System.ComponentModel;

namespace MarioWebService.Enums
{
    public enum CommandType
    {
        [Description("help")]
        Help,
        [Description("info")]
        Info,
        [Description("set project id")]
        SetProjectId,
        [Description("add tasks")]
        AddTasks,
        [Description("add story")]
        AddStory,
        [Description("add default task")]
        AddDefaultTask,
        [Description("clear default tasks")]
        ClearDefaultTasks,
        [Description("set default tasks from json")]
        SetDefaultTasksFromJson,
        [Description("random fractal")]
        RandomFractal,
        [Description("add cats")]
        AddCats,
        [Description("youtube")]
        YouTube,
        [Description("imgur")]
        Imgur,
        [Description("google books")]
        GoogleBooks,
        [Description("google vision")]
        GoogleVision,
        [Description("send text")]
        SendText,
        [Description("search repos")]
        SearchRepos
    }
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        public static string GetDescription(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }


    }
}
