﻿namespace MediaSolution.DAL.Options;

public record DALOptions
{
    public string DatabaseDirectory { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = string.Empty;
    public string DatabaseFilePath => Path.Combine(DatabaseDirectory, DatabaseName);

    /// <summary>
    /// Deletes database before application startup
    /// </summary>
    public bool RecreateDatabaseEachTime { get; init; } = false;

    /// <summary>
    /// Seeds DemoData from DbContext on database creation.
    /// </summary>
    public bool SeedDemoData { get; init; } = true;
}