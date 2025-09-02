using Microsoft.EntityFrameworkCore;
using OpenArchival.DataAccess;

namespace OpenArchival.Blazor;

public class ArtifactEntrySharedHelpers
{
    IArtifactDefectProvider DefectsProvider { get; set; }

    IArtifactStorageLocationProvider StorageLocationProvider { get; set; }

    IArchiveEntryTagProvider TagsProvider { get; set; } 

    IArtifactTypeProvider TypesProvider { get; set; }

    IListedNameProvider ListedNameProvider { get; set; }
    
    IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; }


    public ArtifactEntrySharedHelpers(IArtifactDefectProvider defectsProvider, IArtifactStorageLocationProvider storageLocationProvider, IArchiveEntryTagProvider tagsProvider, IArtifactTypeProvider typesProvider, IListedNameProvider listedNamesProvider, IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        DefectsProvider = defectsProvider;
        StorageLocationProvider = storageLocationProvider;
        TagsProvider = tagsProvider;
        TypesProvider = typesProvider;
        ListedNameProvider = listedNamesProvider;
        DbContextFactory = contextFactory;
    }

    public async Task<IEnumerable<string>> SearchDefects(string value, CancellationToken cancellationToken)
    {
        List<string> defects;
        if (string.IsNullOrEmpty(value))
        {
            defects = new((await DefectsProvider.Top(25) ?? []).Select(prop => prop.Description));
        }
        else
        {
            defects = new((await DefectsProvider.Search(value) ?? []).Select(prop => prop.Description));
        }

        return defects;
    }

    public async Task<IEnumerable<string>> SearchStorageLocation(string value, CancellationToken cancellationToken)
    {
        List<string> storageLocations;
        if (string.IsNullOrEmpty(value))
        {
            storageLocations = new((await StorageLocationProvider.Top(25) ?? []).Select(prop => prop.Location));
        }
        else
        {
            storageLocations = new((await StorageLocationProvider.Search(value) ?? []).Select(prop => prop.Location));
        }

        return storageLocations;
    }

    public async Task<IEnumerable<string>> SearchTags(string value, CancellationToken cancellationToken)
    {
        List<string> tags;
        if (string.IsNullOrEmpty(value))
        {
            tags = new((await TagsProvider.Top(25) ?? []).Select(prop => prop.Name));
        }
        else
        {
            tags = new((await TagsProvider.Search(value) ?? []).Select(prop => prop.Name));
        }

        return tags;
    }

    public async Task<IEnumerable<string>> SearchItemTypes(string value, CancellationToken cancellationToken)
    {
        List<string> itemTypes;
        if (string.IsNullOrEmpty(value))
        {
            itemTypes = new((await TypesProvider.Top(25) ?? []).Select(prop => prop.Name));
        }
        else
        {
            itemTypes = new((await TypesProvider.Search(value) ?? []).Select(prop => prop.Name));
        }

        return itemTypes;
    }

    public async Task<IEnumerable<string>> SearchListedNames(string value, CancellationToken cancellationToken)
    {
        List<ListedName> names;
        if (string.IsNullOrEmpty(value))
        {
            names = new((await ListedNameProvider.Top(25) ?? []));
        }
        else
        {
            names = new((await ListedNameProvider.Search(value) ?? []));
        }

        return names.Select(p => p.Value);
    }

    public async Task OnGroupingPublished(ArtifactGroupingValidationModel model)
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();
        var grouping = model.ToArtifactGrouping();

        // Caches to track entities processed within this transaction
        var processedFilePaths = new Dictionary<string, FilePathListing>();
        var processedLocations = new Dictionary<string, ArtifactStorageLocation>();
        var processedTags = new Dictionary<string, ArtifactEntryTag>();
        var processedDefects = new Dictionary<string, ArtifactDefect>();
        var processedTypes = new Dictionary<string, ArtifactType>();
        var processedNames = new Dictionary<string, ListedName>();

        // Process File Paths for each entry first
        foreach (var entry in grouping.ChildArtifactEntries)
        {
            if (entry.Files is { Count: > 0 })
            {
                var correctedFileList = new List<FilePathListing>();
                foreach (var fileListing in entry.Files)
                {
                    var path = fileListing.Path;
                    if (string.IsNullOrWhiteSpace(path)) continue;

                    if (processedFilePaths.TryGetValue(path, out var trackedFile))
                    {
                        correctedFileList.Add(trackedFile);
                    }
                    else
                    {
                        var existingFile = await context.ArtifactFilePaths
                            .FirstOrDefaultAsync(f => f.Path == path);

                        if (existingFile != null)
                        {
                            correctedFileList.Add(existingFile);
                            processedFilePaths[path] = existingFile;
                        }
                        else
                        {
                            correctedFileList.Add(fileListing);
                            processedFilePaths[path] = fileListing;
                        }
                    }
                }
                entry.Files = correctedFileList;
            }
        }

        // Process all other related entities for each entry
        foreach (var entry in grouping.ChildArtifactEntries)
        {
            // Attach entry to its parent grouping
            entry.ArtifactGrouping = grouping;

            // --- Process Storage Location ---
            var locationName = entry.StorageLocation?.Location;
            if (!string.IsNullOrWhiteSpace(locationName))
            {
                if (processedLocations.TryGetValue(locationName, out var trackedLocation))
                {
                    entry.StorageLocation = trackedLocation;
                }
                else
                {
                    var existingLocation = await context.ArtifactStorageLocations
                        .FirstOrDefaultAsync(l => l.Location == locationName);

                    if (existingLocation != null)
                    {
                        entry.StorageLocation = existingLocation;
                        processedLocations[locationName] = existingLocation;
                    }
                    else
                    {
                        processedLocations[locationName] = entry.StorageLocation;
                    }
                }
            }

            // --- Process Tags ---
            if (entry.Tags is { Count: > 0 })
            {
                var correctedTagList = new List<ArtifactEntryTag>();
                foreach (var tag in entry.Tags)
                {
                    var tagName = tag.Name;
                    if (string.IsNullOrWhiteSpace(tagName)) continue;

                    if (processedTags.TryGetValue(tagName, out var trackedTag))
                    {
                        correctedTagList.Add(trackedTag);
                    }
                    else
                    {
                        var existingTag = await context.ArtifactEntryTags.FirstOrDefaultAsync(t => t.Name == tagName);
                        if (existingTag != null)
                        {
                            correctedTagList.Add(existingTag);
                            processedTags[tagName] = existingTag;
                        }
                        else
                        {
                            correctedTagList.Add(tag);
                            processedTags[tagName] = tag;
                        }
                    }
                }
                entry.Tags = correctedTagList;
            }

            // --- Process Defects ---
            if (entry.Defects is { Count: > 0 })
            {
                var correctedDefectList = new List<ArtifactDefect>();
                foreach (var defect in entry.Defects)
                {
                    var defectDesc = defect.Description;
                    if (string.IsNullOrWhiteSpace(defectDesc)) continue;

                    if (processedDefects.TryGetValue(defectDesc, out var trackedDefect))
                    {
                        correctedDefectList.Add(trackedDefect);
                    }
                    else
                    {
                        var existingDefect = await context.ArtifactDefects.FirstOrDefaultAsync(d => d.Description == defectDesc);
                        if (existingDefect != null)
                        {
                            correctedDefectList.Add(existingDefect);
                            processedDefects[defectDesc] = existingDefect;
                        }
                        else
                        {
                            correctedDefectList.Add(defect);
                            processedDefects[defectDesc] = defect;
                        }
                    }
                }
                entry.Defects = correctedDefectList;
            }

            // --- Process Types ---
            if (entry.Type is not null)
            {
                var typeName = entry.Type.Name;
                if (!string.IsNullOrWhiteSpace(typeName))
                {
                    if (processedTypes.TryGetValue(typeName, out var trackedType))
                    {
                        entry.Type = trackedType;
                    }
                    else
                    {
                        var existingType = await context.ArtifactTypes.FirstOrDefaultAsync(t => t.Name == typeName);
                        if (existingType != null)
                        {
                            entry.Type = existingType;
                            processedTypes[typeName] = existingType;
                        }
                        else
                        {
                            processedTypes[typeName] = entry.Type;
                        }
                    }
                }
            }

            // --- Process Listed Names ---
            if (entry.ListedNames is { Count: > 0 })
            {
                var correctedNameList = new List<ListedName>();
                foreach (var name in entry.ListedNames)
                {
                    var nameValue = name.Value;
                    if (string.IsNullOrWhiteSpace(nameValue)) continue;

                    if (processedNames.TryGetValue(nameValue, out var trackedName))
                    {
                        correctedNameList.Add(trackedName);
                    }
                    else
                    {
                        var existingName = await context.ArtifactAssociatedNames
                            .FirstOrDefaultAsync(n => n.Value == nameValue);

                        if (existingName != null)
                        {
                            correctedNameList.Add(existingName);
                            processedNames[nameValue] = existingName;
                        }
                        else
                        {
                            correctedNameList.Add(name);
                            processedNames[nameValue] = name;
                        }
                    }
                }
                entry.ListedNames = correctedNameList;
            }
        }

        if (grouping.Category != null && grouping.Category.Id > 0)
        {
            context.Attach(grouping.Category);
        }
        // Add the entire graph. EF Core will correctly handle new vs. existing entities.
        context.ArtifactGroupings.Add(grouping);
        await context.SaveChangesAsync();
    }
}
