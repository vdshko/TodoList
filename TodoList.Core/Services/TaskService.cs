﻿using SQLite;
using System.Collections.Generic;
using System.Linq;
using TodoList.Core.Interfaces;
using TodoList.Core.Models;

namespace TodoList.Core.Services
{
    public class TaskService : ITaskService
    {
        private SQLiteConnection _sqlConnection;

        public TaskService(IDataBaseConnectionService connection)
        {
            _sqlConnection = connection.GetDataBaseConnection();
            _sqlConnection.CreateTable<Goal>();
            _sqlConnection.CreateTable<User>();
        }

        public List<Goal> GetAllGoals()
        {
            return (from data in _sqlConnection.Table<Goal>() select data).ToList();
        }

        public void InsertGoal(Goal goal)
        {
            if (goal.Id != 0)
            {
                _sqlConnection.Update(goal);
            }

            if (goal.Id == 0)
            {
                _sqlConnection.Insert(goal);
            }
        }

        public void DeleteGoal(int goalId)
        {
            _sqlConnection.Delete<Goal>(goalId);
        }

        public List<User> GetAllUsers()
        {
            return (from data in _sqlConnection.Table<User>() select data).ToList();
        }

        public void InsertUser(User user)
        {
            if (user.Id != 0)
            {
                _sqlConnection.Update(user);
            }
            if (user.Id == 0)
            {
                _sqlConnection.Insert(user);
            }
        }

        public List<Goal> GetUserGoal(string currentUserId)
        {
            return (from data in _sqlConnection.Table<Goal>() where data.UserId == currentUserId select data).ToList();
        }

        //public Goal GetGoalData(int goalId)
        //{
        //    return _sqlConnection.Table<Goal>().FirstOrDefault(x => x.Id == goalId);
        //}

        //public void UpdateGoal(Goal goal)
        //{
        //    _sqlConnection.Update(goal);
        //}

        //public void DeleteAllGoals()
        //{
        //    _sqlConnection.DeleteAll<Goal>();
        //}
    }
}