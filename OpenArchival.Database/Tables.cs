using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenArchival.Database;

public static class Tables
{
    public const string CategoryTable = """
        DROP TABLE IF EXISTS Categories CASCADE;
        CREATE TABLE IF NOT EXISTS Categories (
            categoryid SERIAL PRIMARY KEY, 
            categoryname TEXT NOT NULL,
            fieldseparator TEXT NOT NULL,
            fieldnames TEXT [] NOT NULL,
            fielddescriptions TEXT [] NOT NULL
        );
        
        DROP TABLE IF EXISTS categoryfieldoptions;
        CREATE TABLE IF NOT EXISTS categoryfieldoptions (
            categoryid INT NOT NULL,
            fieldnumber INT NOT NULL,
            value TEXT NOT NULL,
            name TEXT NOT NULL,
            FOREIGN KEY (categoryid) REFERENCES Categories(categoryid)
        );
        """;

    public const string ArtifactTypesTable = """
        DROP TABLE IF EXISTS artifacttypes;
        CREATE TABLE IF NOT EXISTS artifacttypes (
            type TEXT NOT NULL
        );
        """;

    public const string ArtifactAssociatedNamesTable = """
        DROP TABLE IF EXISTS artifactassociatednames;
        CREATE TABLE IF NOT EXISTS artifactassociatednames(
            name TEXT NOT NULL
        );
        """;

    public const string ArchiveStorageLocationTable = """
        DROP TABLE IF EXISTS archivestoragelocations;
        CREATE TABLE IF NOT EXISTS archivestoragelocations(
            location TEXT NOT NULL
        );
        """;

    public const string TagsTable = """
        DROP TABLE IF EXISTS tags;
        CREATE TABLE IF NOT EXISTS tags (
            tag TEXT NOT NULL
        );
        """;

    public const string DefectsTable = """
        DROP TABLE IF EXISTS defects;
        CREATE TABLE IF NOT EXISTS defects (
            defect TEXT NOT NULL
        );
        """;

    public const string ArchiveFiles = """
        DROP TABLE IF EXISTS archivefiles;
        CREATE TABLE IF NOT EXISTS archivefiles (
            id SERIAL PRIMARY KEY,
            filename TEXT NOT NULL,
            path TEXT NOT NULL
        );
        """;

    static async Task<bool> InitTables(NpgsqlDataSource dataSource)
    {
        await using var connection = await dataSource.OpenConnectionAsync();


        return true;
    }
}
