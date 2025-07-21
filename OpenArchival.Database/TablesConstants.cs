namespace OpenArchival.Database;

public static class TablesConstants
{
    public static string CreateCategoriesTable = """
        CREATE TABLE IF NOT EXISTS Categories (
            categoryname TEXT NOT NULL PRIMARY KEY,
            fieldseperator TEXT NOT NULL,
            fieldnames TEXT [] NOT NULL
        );
        """;
}
