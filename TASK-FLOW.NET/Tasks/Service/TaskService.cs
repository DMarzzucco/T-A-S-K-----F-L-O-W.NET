﻿using AutoMapper;
using TASK_FLOW.NET.Project.Service.Interface;
using TASK_FLOW.NET.Tasks.DTOs;
using TASK_FLOW.NET.Tasks.Model;
using TASK_FLOW.NET.Tasks.Repository.Interface;
using TASK_FLOW.NET.Tasks.Service.Interface;

namespace TASK_FLOW.NET.Tasks.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public TaskService(ITaskRepository repository, IMapper mapper, IProjectService projectSerivice)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._projectService = projectSerivice;
        }

        public async Task<TaskModel> CreateTask(int ProjectId, CreateTaskDTO body)
        {
            var project = await this._projectService.GetProjectById(ProjectId);
            if (project == null)
                throw new KeyNotFoundException("Project not found");

            var mapperTask = this._mapper.Map<TaskModel>(body);

            mapperTask.Project = project;

            await this._repository.SaveTaskAsync(mapperTask);

            return mapperTask;
        }

        public async Task DeleteTask(int id)
        {
            var tasks = await this._repository.findByIdAsync(id);
            if (tasks == null)
                throw new KeyNotFoundException("Task not found");

            await this._repository.DeleteTaskAsync(tasks);
        }

        public async Task<TaskModel> GetTaskById(int id)
        {
            var tasks = await this._repository.findByIdAsync(id);
            if (tasks == null)
                throw new KeyNotFoundException("Task not found");
            return tasks;
        }

        public async Task<IEnumerable<TaskModel>> ListOfAllTask()
        {
            return await this._repository.ListAllTaskAsync();
        }

        public async Task<TaskModel> UpdateTask(int id, UpdateTaskDTO body)
        {
            var tasks = await this._repository.findByIdAsync(id);
            if (tasks == null)
                throw new KeyNotFoundException("Task not found");

            this._mapper.Map(body, tasks);

            await this._repository.UpdateTaskAsync(tasks);
            return tasks;

        }
    }
}